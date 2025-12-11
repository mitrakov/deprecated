import SQLite

final internal class SQLiteDatabase {
    private let GLOBAL_SCHEMA_VERSION: Int32 = 5;
    private var db: Connection?
    
    /// Establishes a new connection to database. Closes the old database connection, if required
    /// - Parameters:
    ///   - path: The location of the database
    func openDb(_ path: String) {
        closeDb()
        do {
            db = try Connection(path)
        } catch {print(error)}
        db?.foreignKeys = true // PRAGMA foreign_keys=ON; (in SQLite it's disabled by default)
        updateSchemaIfRequired()
    }
    
    /// Closes the database. If no databases open, does nothing
    func closeDb() {
        db = nil // internal DB connection will be closed by deinit()
    }
    
    /// Checks if a DB is open
    /// - Returns: true if DB is connected
    func isConnected() -> Bool {
        return db != nil
    }
    
    /// Creates a new database
    /// - Parameters:
    ///   - path: The location of the database
    func createDb(_ path: String) {
        openDb(path)
        do {
            try db?.transaction {
                try db?.execute("""
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
                    """)
                db?.userVersion = GLOBAL_SCHEMA_VERSION
            }
        } catch {print(error)}
    }
    
    /// Adds a new note to the database
    /// - Parameters:
    ///   - data: markdown string
    /// - Returns: a new generated note ID
    func insertNote(_ data: String) -> Int64 {
        var noteId: Int64 = 0
        do {
            try db?.transaction {
                noteId = try db?.scalar("INSERT INTO note DEFAULT VALUES RETURNING note_id;") as! Int64
                try db?.run("INSERT INTO notedata (rowid, data) VALUES (?, ?);", noteId, data)
            }
        } catch {print(error)}
        return noteId
    }
    
    /// Updates the given note in the database
    /// - Parameters:
    ///   - noteId: note ID
    ///   - data: markdown string
    func updateNote(_ noteId: Int64, _ data: String) {
        do {
            try db?.transaction {
                try db?.run("UPDATE notedata SET data = ? WHERE rowid = ?;", data, noteId)
                try db?.run("UPDATE note SET updated_at = CURRENT_TIMESTAMP WHERE note_id = ?;", noteId)
            }
        } catch {print(error)}
    }

    /// Soft-deletes (or restores) a given note from the database
    /// - Parameters:
    ///   - noteId: note ID
    ///   - deleted: whether to delete or restore
    func softDeleteNote(_ noteId: Int64, deleted: Bool) {
        do {
            try db?.run("UPDATE note SET is_deleted = ?, updated_at = CURRENT_TIMESTAMP WHERE note_id = ?;", deleted, noteId);
        } catch {print(error)}
    }
    
    /// Removes a given note from the database
    /// - Parameters:
    ///   - noteId: note ID
    func deleteNote(_ noteId: Int64) {
        do {
            try db?.transaction {
                try db?.run("DELETE FROM note     WHERE note_id = ?;", noteId);
                try db?.run("DELETE FROM notedata WHERE rowid = ?;", noteId);
                try db?.run("DELETE FROM tag      WHERE tag_id NOT IN (SELECT DISTINCT tag_id FROM note_to_tag);");
            }
        } catch {print(error)}
    }
    
    /// Fetches all notes from the database
    /// - Parameters:
    ///   - fetchDeleted: whether to fetch soft-deleted notes
    /// - Returns: a list of notes
    func getAllNotes(fetchDeleted: Bool) -> [Note] {
        let sql = """
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
        """
        do {
            return try db?.run(sql).map {Note(id: $0[0] as! Int64, data: $0[1] as! String, tags: $0[2] as! String, isDeleted: $0[3] as! Int64 > 0)} ?? []
        } catch {print(error)}
        
        return []
    }
    
    /// Fetches N random notes from the database
    /// - Parameters:
    ///   - fetchDeleted: whether to fetch soft-deleted notes
    ///   - limit: max count of notes
    /// - Returns: a list on notes
    func getRandomNotes(fetchDeleted: Bool, limit: Int) -> [Note] {
        let sql = """
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
          LIMIT ?
          ;
        """
        do {
            return try db?.run(sql, limit).map {Note(id: $0[0] as! Int64, data: $0[1] as! String, tags: $0[2] as! String, isDeleted: $0[3] as! Int64 > 0)} ?? []
        } catch {print(error)}

        return []
    }

    /// Fetches all tags from the database
    /// - Returns: a list of tags
    func getTags() -> [String] {
        do {
            return try db?.run("SELECT name FROM tag ORDER BY name;").map {$0[0] as! String} ?? [] // TODO don't show archived
        } catch {print(error)}
        return []
    }
    
    /// Searches for a single note by ID
    /// - Parameters:
    ///   - id: note ID
    /// - Returns: an optional note
    func searchByID(_ id: Int64) -> Note? {
        let sql = """
          SELECT note_id, data, GROUP_CONCAT(name, ', ') AS tags, is_deleted
          FROM note
          INNER JOIN notedata ON note_id = notedata.rowid
          INNER JOIN note_to_tag USING (note_id)
          INNER JOIN tag         USING (tag_id)
          WHERE note_id = ?
          GROUP BY note_id
          ;
        """
        do {
            return try db?.run(sql, id).map {Note(id: $0[0] as! Int64, data: $0[1] as! String, tags: $0[2] as! String, isDeleted: $0[3] as! Int64 > 0)}.first
        } catch {print(error)}

        return nil
    }

    /// Searches for multiple notes by a given tag
    /// - Parameters:
    ///   - tag: tag to search
    ///   - fetchDeleted: whether to fetch soft-deleted notes
    /// - Returns: a list of notes
    func searchByTag(_ tag: String, fetchDeleted: Bool) -> [Note] {
        let sql = """
          SELECT note_id, data, GROUP_CONCAT(name, ', ') AS tags, is_deleted
          FROM note
          INNER JOIN notedata ON note_id = notedata.rowid
          INNER JOIN note_to_tag USING (note_id)
          INNER JOIN tag         USING (tag_id)
          WHERE note_id IN (SELECT note_id FROM tag INNER JOIN note_to_tag USING (tag_id) WHERE name = ?)
        """
        + (fetchDeleted ? "" : " AND NOT is_deleted ") +
        """
          GROUP BY note_id
          ORDER BY note.updated_at DESC
          ;
        """
        do {
            return try db?.run(sql, tag).map {Note(id: $0[0] as! Int64, data: $0[1] as! String, tags: $0[2] as! String, isDeleted: $0[3] as! Int64 > 0)} ?? []
        } catch {print(error)}
        
        return []
    }
    
    /// Searches for multiple notes by a given keyword (full text search)
    /// - Parameters:
    ///   - word: keyword to search
    ///   - fetchDeleted: whether to fetch soft-deleted notes
    /// - Returns: a list of notes
    func searchByKeyword(_ word: String, fetchDeleted: Bool) -> [Note] {
        guard !word.isWhiteSpace() else {return []}

        let sql = """
          SELECT note_id, data, GROUP_CONCAT(name, ', ') AS tags, is_deleted
          FROM note
          INNER JOIN notedata ON note_id = notedata.rowid
          INNER JOIN note_to_tag USING (note_id)
          INNER JOIN tag         USING (tag_id)
          WHERE data MATCH ?
        """
        + (fetchDeleted ? "" : " AND NOT is_deleted ") +
        """
          GROUP BY note_id
          ORDER BY notedata.rank ASC, note.updated_at DESC
          ;
        """
        do {
            return try db?.run(sql, word).map {Note(id: $0[0] as! Int64, data: $0[1] as! String, tags: $0[2] as! String, isDeleted: $0[3] as! Int64 > 0)} ?? []
        } catch {print(error)}
        
        return []
    }
    
    /// Attaches given tags to a given note
    /// - Parameters:
    ///   - noteId: note ID
    ///   - tags: list of tags to attach to the note (empty array does nothing)
    func linkTagsToNote(_ noteId: Int64, _ tags: [String]) {
        guard !tags.isEmpty else {return}
        
        do {
            try db?.transaction {
                try tags.forEach { tag in
                    let tagIdOpt = try db?.scalar("SELECT tag_id FROM tag WHERE name = ?;", tag) as? Int64;
                    let tagId = tagIdOpt ?? {
                        do {
                            return try db?.scalar("INSERT INTO tag (name) VALUES (?) RETURNING tag_id;", tag) as! Int64;
                        } catch {print(error)}
                        return -1
                    }();

                    try db?.run("INSERT INTO note_to_tag (note_id, tag_id) VALUES (?, ?);", noteId, tagId);
                }
            }
        } catch {print(error)}
    }

    /// Detaches given tags from a given note
    /// - Parameters:
    ///   - noteId: note ID
    ///   - tags: list of tags to detach from the note (empty array does nothing)
    func unlinkTagsFromNote(_ noteId: Int64, _ tags: [String]) {
        guard !tags.isEmpty else {return}
        
        let IN = [String](repeating: "?", count: tags.count).joined(separator: ",") // "?,?,?,?"
        do {
            try db?.transaction {
                try db?.run("DELETE FROM note_to_tag WHERE note_id = ? AND tag_id IN (SELECT tag_id FROM tag WHERE name IN (\(IN)));", [noteId] + tags);
                try db?.run("DELETE FROM tag WHERE tag_id NOT IN (SELECT DISTINCT tag_id FROM note_to_tag);");
            }
        } catch {print(error)}
    }

    /// Receives metadata by a given key
    /// - Parameters:
    ///   - key: key to search
    /// - Returns: optional value
    func getMetadata(key: String) -> String? {
        do {
            return try db?.scalar("SELECT value FROM metadata WHERE key = ?;", key) as? String
        } catch {print(error)}
        return nil
    }

    /// Updates metadata with a given key-value pair; NIL value deletes the key-value pair
    /// - Parameters:
    ///   - key: key
    ///   - value: value (NIL means to delete the key-value entry from the DB)
    func setMetadata(key: String, _ value: String?) {
        do {
            if (value != nil) {
                try db?.run("INSERT INTO metadata (key, value) VALUES (@0, @1) ON CONFLICT (key) DO UPDATE SET value = @1;", key, value)
            } else {
                try db?.run("DELETE FROM metadata WHERE key = ?;", key)
            }
        } catch {print(error)}
    }

    /// Helper for migrations
    private func updateSchemaIfRequired() {
        let dbVersion = db?.userVersion ?? 0 // for real users, min = 3

        if (dbVersion < GLOBAL_SCHEMA_VERSION) {
            do {
                try db?.transaction {
                    if (dbVersion < 4) { // new "metadata" table
                        try db?.execute("""
                            CREATE TABLE IF NOT EXISTS metadata (
                              key VARCHAR(64) PRIMARY KEY NOT NULL,
                              value VARCHAR(255) NULL,
                              created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                              updated_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
                            );
                            """)
                        print("Migration 3 -> 4 done")
                    }
                    if (dbVersion < 5) { // bug fix to trim " tagName "
                        try db?.execute("UPDATE tag SET name = (trim(name) || '_bugfix_tag_id_' || tag_id) WHERE name != trim(name);")
                        print("Migration 4 -> 5 done")
                    }
                    db?.userVersion = GLOBAL_SCHEMA_VERSION
                }
            } catch {print(error)}
        }
    }
}
