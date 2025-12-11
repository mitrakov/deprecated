using Microsoft.Data.Sqlite;

namespace LasNotes;

internal class SQLiteDatabase {
    private static readonly int GLOBAL_SCHEMA_VERSION = 5;
    private SqliteConnection db = new();

    /// <summary>
    /// Establishes a new connection to database. Closes the old database connection, if required
    /// </summary>
    /// <param name="path">The location of the database</param>
    internal void OpenDb(string path) {
        CloseDb();
        db = new($"Filename={path}; Foreign Keys=true");
        db.Open();
        UpdateSchemaIfRequired();
    }

    /// <summary>
    /// Closes the database. If no databases open, does nothing
    /// </summary>
    internal void CloseDb() => db.Close();

    /// <summary>
    /// Checks if a DB is open
    /// </summary>
    internal bool IsConnected => db.State == System.Data.ConnectionState.Open;

    /// <summary>
    /// Creates a new database
    /// </summary>
    /// <param name="path">The location of the database</param>
    internal void CreateDb(string path) {
        OpenDb(path);
        using var tx = db.BeginTransaction();
        var sql = """
            CREATE TABLE IF NOT EXISTS note (
                note_id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                author VARCHAR(64) NOT NULL DEFAULT '',
                client VARCHAR(255) NOT NULL DEFAULT '',
                user_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                colour INTEGER NOT NULL DEFAULT 16777215,
                rank TINYINT NOT NULL DEFAULT 0,
                is_visible BOOLEAN NOT NULL DEFAULT true,
                is_favourite BOOLEAN NOT NULL DEFAULT false,
                is_deleted BOOLEAN NOT NULL DEFAULT false,
                created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                updated_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
            );
            CREATE VIRTUAL TABLE IF NOT EXISTS notedata USING FTS5(data);
            CREATE TABLE IF NOT EXISTS tag (
                tag_id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                name VARCHAR(64) UNIQUE NOT NULL,
                created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                updated_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
            );
            CREATE TABLE IF NOT EXISTS image (
                guid UUID PRIMARY KEY NOT NULL,
                data BLOB NOT NULL,
                created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                updated_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
            );
            CREATE TABLE IF NOT EXISTS note_to_tag (
                note_id INTEGER NOT NULL REFERENCES note (note_id) ON UPDATE RESTRICT ON DELETE CASCADE,
                tag_id  INTEGER NOT NULL REFERENCES tag (tag_id) ON UPDATE RESTRICT ON DELETE CASCADE,
                created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                updated_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                PRIMARY KEY (note_id, tag_id)
            );
            CREATE TABLE IF NOT EXISTS metadata (
                key VARCHAR(64) PRIMARY KEY NOT NULL,
                value VARCHAR(255) NULL,
                created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                updated_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
            );
            """;

        new List<SqliteCommand>([
            SqlCmd(sql, tx),
            SqlCmd($"PRAGMA user_version={GLOBAL_SCHEMA_VERSION}", tx)
        ]).ForEach(cmd => cmd.ExecuteNonQuery());

        tx.Commit();
    }

    /// <summary>
    /// Adds a new note to the database
    /// </summary>
    /// <param name="data">markdown string</param>
    /// <returns>a new generated note ID</returns>
    internal long InsertNote(string data) {
        using var tx = db.BeginTransaction();
        var noteId = SqlCmd("INSERT INTO note DEFAULT VALUES RETURNING note_id;", tx).ExecuteScalar() as long? ?? -1;
        SqlCmd("INSERT INTO notedata (rowid, data) VALUES (@0, @1);", tx, noteId, data).ExecuteScalar();

        tx.Commit();
        return noteId;
    }

    /// <summary>
    /// Updates the given note in the database
    /// </summary>
    /// <param name="noteId">note ID</param>
    /// <param name="data">markdown string</param>
    internal void UpdateNote(long noteId, string data) {
        using var tx = db.BeginTransaction();

        new List<SqliteCommand>([
            SqlCmd("UPDATE notedata SET data = @0 WHERE rowid = @1;", tx, data, noteId),
            SqlCmd("UPDATE note SET updated_at = CURRENT_TIMESTAMP WHERE note_id = @0;", tx, noteId)
        ]).ForEach(cmd => cmd.ExecuteNonQuery());

        tx.Commit();
    }

    /// <summary>
    /// Soft-deletes (or restores) a given note from the database
    /// </summary>
    /// <param name="noteId">note ID</param>
    /// <param name="deleted">whether to delete or restore</param>
    internal void SoftDeleteNote(long noteId, bool deleted) {
        SqlCmd("UPDATE note SET is_deleted = @0, updated_at = CURRENT_TIMESTAMP WHERE note_id = @1;", deleted, noteId).ExecuteNonQuery();
    }

    /// <summary>
    /// Removes a given note from the database
    /// </summary>
    /// <param name="noteId">note ID</param>
    internal void DeleteNote(long noteId) {
        using var tx = db.BeginTransaction();

        new List<SqliteCommand>([
            SqlCmd("DELETE FROM note     WHERE note_id = @0;", tx, noteId),
            SqlCmd("DELETE FROM notedata WHERE rowid = @0;", tx, noteId),
            SqlCmd("DELETE FROM tag      WHERE tag_id NOT IN (SELECT DISTINCT tag_id FROM note_to_tag);", tx)
        ]).ForEach(cmd => cmd.ExecuteNonQuery());

        tx.Commit();
    }

    /// <summary>
    /// Fetches all notes from the database
    /// </summary>
    /// <param name="fetchDeleted">whether to fetch soft-deleted notes</param>
    /// <returns>a list of notes</returns>
    internal IEnumerable<Note> GetAllNotes(bool fetchDeleted) {
        var result = new List<Note>();

        var sql = """
          SELECT note_id, data, GROUP_CONCAT(name, ', ') AS tags, is_deleted
          FROM note
          INNER JOIN notedata ON note_id = notedata.rowid
          INNER JOIN note_to_tag USING (note_id)
          INNER JOIN tag         USING (tag_id)
        """
        + (fetchDeleted ? "" : "WHERE NOT is_deleted ") +
        """
          GROUP BY note_id
          ORDER BY note.updated_at DESC
          ;
        """;

        using var reader = SqlCmd(sql).ExecuteReader();
        while (reader.Read())
            result.Add(new Note(reader.GetInt64(0), reader.GetString(1), reader.GetString(2), reader.GetBoolean(3)));

        return result;
    }

    /// <summary>
    /// Fetches N random notes from the database
    /// </summary>
    /// <param name="fetchDeleted">whether to fetch soft-deleted notes</param>
    /// <param name="limit">max count of notes</param>
    /// <returns>a list on notes</returns>
    internal IEnumerable<Note> GetRandomNotes(bool fetchDeleted, uint limit) {
        var result = new List<Note>();

        var sql = """
          SELECT note_id, data, GROUP_CONCAT(name, ', ') AS tags, is_deleted
          FROM note
          INNER JOIN notedata ON note_id = notedata.rowid
          INNER JOIN note_to_tag USING (note_id)
          INNER JOIN tag         USING (tag_id)
        """
        + (fetchDeleted ? "" : "WHERE NOT is_deleted ") +
        """
          GROUP BY note_id
          ORDER BY RANDOM()
          LIMIT @0
          ;
        """;

        using var reader = SqlCmd(sql, limit).ExecuteReader();
        while (reader.Read())
            result.Add(new Note(reader.GetInt64(0), reader.GetString(1), reader.GetString(2), reader.GetBoolean(3)));

        return result;
    }

    /// <summary>
    /// Fetches all tags from the database
    /// </summary>
    /// <returns>a list of tags</returns>
    internal IEnumerable<string> GetTags() {
        var result = new List<string>();

        using var reader = SqlCmd("SELECT name FROM tag ORDER BY name;").ExecuteReader(); // TODO don't show archived
        while (reader.Read())
            result.Add(reader.GetString(0));

        return result;
    }

    /// <summary>
    /// Searches for a single note by ID
    /// </summary>
    /// <param name="id">note ID</param>
    /// <returns>an optional note</returns>
    internal Note? SearchByID(long id) {
        Note? result = null;

        var sql = """
          SELECT note_id, data, GROUP_CONCAT(name, ', ') AS tags, is_deleted
          FROM note
          INNER JOIN notedata ON note_id = notedata.rowid
          INNER JOIN note_to_tag USING (note_id)
          INNER JOIN tag         USING (tag_id)
          WHERE note_id = @0
          GROUP BY note_id
          ;
        """;
        using var reader = SqlCmd(sql, id).ExecuteReader();
        while (reader.Read())
            result = new Note(reader.GetInt64(0), reader.GetString(1), reader.GetString(2), reader.GetBoolean(3));

        return result;
    }

    /// <summary>
    /// Searches for multiple notes by a given tag
    /// </summary>
    /// <param name="tag">tag to search</param>
    /// <param name="fetchDeleted">whether to fetch soft-deleted notes</param>
    /// <returns>a list of notes</returns>
    internal IEnumerable<Note> SearchByTag(string tag, bool fetchDeleted) {
        var result = new List<Note>();

        var sql = """
          SELECT note_id, data, GROUP_CONCAT(name, ', ') AS tags, is_deleted
          FROM note
          INNER JOIN notedata ON note_id = notedata.rowid
          INNER JOIN note_to_tag USING (note_id)
          INNER JOIN tag         USING (tag_id)
          WHERE note_id IN (SELECT note_id FROM tag INNER JOIN note_to_tag USING (tag_id) WHERE name = @0)
        """
        + (fetchDeleted ? "" : " AND NOT is_deleted ") +
        """
          GROUP BY note_id
          ORDER BY note.updated_at DESC
          ;
        """;
        using var reader = SqlCmd(sql, tag).ExecuteReader();
        while (reader.Read())
            result.Add(new Note(reader.GetInt64(0), reader.GetString(1), reader.GetString(2), reader.GetBoolean(3)));

        return result;
    }

    /// <summary>
    /// Searches for multiple notes by a given keyword (full text search)
    /// </summary>
    /// <param name="word">keyword to search</param>
    /// <param name="fetchDeleted">whether to fetch soft-deleted notes</param>
    /// <returns>a list of notes</returns>
    internal IEnumerable<Note> SearchByKeyword(string word, bool fetchDeleted) {
        if (word == "") return [];
        var result = new List<Note>();

        var sql = """
          SELECT note_id, data, GROUP_CONCAT(name, ', ') AS tags, is_deleted
          FROM note
          INNER JOIN notedata ON note_id = notedata.rowid
          INNER JOIN note_to_tag USING (note_id)
          INNER JOIN tag         USING (tag_id)
          WHERE data MATCH @0
        """
        + (fetchDeleted ? "" : " AND NOT is_deleted ") +
        """
          GROUP BY note_id
          ORDER BY notedata.rank ASC, note.updated_at DESC
          ;
        """;
        using var reader = SqlCmd(sql, word).ExecuteReader();
        while (reader.Read())
            result.Add(new Note(reader.GetInt64(0), reader.GetString(1), reader.GetString(2), reader.GetBoolean(3)));

        return result;
    }

    /// <summary>
    /// Attaches given tags to a given note
    /// </summary>
    /// <param name="noteId">note ID</param>
    /// <param name="tags">list of tags to attach to the note (empty array does nothing)</param>
    internal void LinkTagsToNote(long noteId, IEnumerable<string> tags) {
        if (!tags.Any()) return;

        using var tx = db.BeginTransaction();
        foreach (var tag in tags) {
            var tagIdOpt = SqlCmd("SELECT tag_id FROM tag WHERE name = @0;", tx, tag).ExecuteScalar() as long?;
            var tagId = tagIdOpt.IfNull(() => SqlCmd("INSERT INTO tag (name) VALUES (@0) RETURNING tag_id;", tx, tag).ExecuteScalar() as long? ?? -1) ?? -1;
            SqlCmd("INSERT INTO note_to_tag (note_id, tag_id) VALUES (@0, @1);", tx, noteId, tagId).ExecuteNonQuery();
        }

        tx.Commit();
    }

    /// <summary>
    /// Detaches given tags from a given note
    /// </summary>
    /// <param name="noteId">note ID</param>
    /// <param name="tags">list of tags to detach from the note (empty array does nothing)</param>
    internal void UnlinkTagsFromNote(long noteId, IEnumerable<string> tags) {
        if (!tags.Any()) return;

        using var tx = db.BeginTransaction();
        var IN = string.Join(",", Enumerable.Range(1, tags.Count()).Select(i => $"@{i}")); // "@1,@2,@3,@4"
        object[] varargs = tags.ToArray<object>().Prepend(noteId).ToArray();               // [noteId, tag0, tag1, ...]
        new List<SqliteCommand>([
            SqlCmd($"DELETE FROM note_to_tag WHERE note_id = @0 AND tag_id IN (SELECT tag_id FROM tag WHERE name IN ({IN}));", tx, varargs),
            SqlCmd( "DELETE FROM tag WHERE tag_id NOT IN (SELECT DISTINCT tag_id FROM note_to_tag);", tx)
        ]).ForEach(cmd => cmd.ExecuteNonQuery());

        tx.Commit();
    }

    /// <summary>
    /// Receives metadata by a given key
    /// </summary>
    /// <param name="key">key to search</param>
    /// <returns>optional value</returns>
    internal string? GetMetadata(string key) {
        using var reader = SqlCmd("SELECT value FROM metadata WHERE key = @0", key).ExecuteReader();
        if (reader.Read()) return reader.GetString(0);
        return null;
    }

    /// <summary>
    /// Updates metadata with a given key-value pair; NULL value deletes the key-value pair
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="value">value (NULL means to delete the key-value entry from the DB)</param>
    internal void SetMetadata(string key, string? value) {
        var cmd = value != null
            ? SqlCmd("INSERT INTO metadata (key, value) VALUES (@0, @1) ON CONFLICT (key) DO UPDATE SET value = @1;", key, value)
            : SqlCmd("DELETE FROM metadata WHERE key = @0;", key);
        cmd.ExecuteNonQuery();
    }

    /// <summary>
    /// Helper to create an SqliteCommand with given arguments (use @0, @1, etc...)
    /// </summary>
    /// <param name="query">SQL command</param>
    /// <param name="args">array of arguments</param>
    /// <returns>SqliteCommand with prepared statement</returns>
    protected SqliteCommand SqlCmd(string query, params object[] args) {
        var cmd = new SqliteCommand(query, db);
        for (int i = 0; i < args.Length; i++)
            cmd.Parameters.AddWithValue($"@{i}", args[i]);
        return cmd;
    }

    /// <summary>
    /// Helper to create an SqliteCommand with a new transaction and given arguments (use @0, @1, etc...)
    /// </summary>
    /// <param name="query">SQL command</param>
    /// <param name="tx">transaction object</param>
    /// <param name="args">array of arguments</param>
    /// <returns>SqliteCommand with prepared statement</returns>
    protected SqliteCommand SqlCmd(string query, SqliteTransaction tx, params object[] args) {
        var cmd = new SqliteCommand(query, db, tx);
        for (int i = 0; i < args.Length; i++)
            cmd.Parameters.AddWithValue($"@{i}", args[i]);
        return cmd;
    }

    /// <summary>
    /// Retreives the current schema version of the DB
    /// </summary>
    /// <returns>integer value with schema version</returns>
    private long GetSchemaVersion() {
        using var reader = SqlCmd("PRAGMA user_version").ExecuteReader();
        if (reader.Read()) return reader.GetInt64(0);
        return -1;
    }

    /// <summary>
    /// Helper for migrations
    /// </summary>
    private void UpdateSchemaIfRequired() {
        var dbVersion = GetSchemaVersion();

        if (dbVersion < GLOBAL_SCHEMA_VERSION) {
            using var tx = db.BeginTransaction();
            if (dbVersion < 4) {  // new "metadata" table
                var sql = """
                    CREATE TABLE IF NOT EXISTS metadata (
                        key VARCHAR(64) PRIMARY KEY NOT NULL,
                        value VARCHAR(255) NULL,
                        created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                        updated_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
                    );
                """;
                SqlCmd(sql, tx).ExecuteNonQuery();
                Console.println("Migration 3 -> 4 done");
            }
            if (dbVersion < 5) { // bug fix to trim " tagName "
                SqlCmd("UPDATE tag SET name = (trim(name) || '_bugfix_tag_id_' || tag_id) WHERE name != trim(name);", tx).ExecuteNonQuery();
                Console.println("Migration 4 -> 5 done");
            }

            SqlCmd($"PRAGMA user_version={GLOBAL_SCHEMA_VERSION}", tx).ExecuteNonQuery();
            tx.Commit();
        }
    }
}
