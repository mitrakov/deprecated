using System.ComponentModel;

namespace LasNotes;

internal class MainViewModel : INotifyPropertyChanged {
    public event PropertyChangedEventHandler? PropertyChanged; // event to notify subscribers about changes in CurrentPath

    private readonly SQLiteDatabase db = new(); // reference to the DB
    private static readonly ComponentResourceManager resources = new (typeof(MainViewModel));

    /// <summary>
    /// Currently opened DB file
    /// </summary>
    internal string? CurrentPath { get; set; }

    /// <summary>
    /// PIN-code for a current DB
    /// </summary>
    internal string? PinCode {
        get {
            if (!db.IsConnected) return null;
            return db.GetMetadata("pincode");
        }
        set {
            if (!db.IsConnected) return;
            db.SetMetadata("pincode", value);
        }
    }

    /// <summary>
    /// Shows if a user is a Las Notes sponsor
    /// </summary>
    internal bool IsSponsor {
        get {
            return User.Default.sponsor;
        }
        set {
            User.Default.sponsor = value;
            User.Default.Save();
        }
    }

    /// <summary>
    /// Whether to show digest on startup
    /// </summary>
    internal bool ShowDigest {
        get {
            return User.Default.showDigest;
        }
        set {
            User.Default.showDigest = value;
            User.Default.Save();
        }
    }

    /// <summary>
    /// Opens a DB file
    /// </summary>
    /// <param name="path">Full path to DB file</param>
    internal void OpenFile(string path) {
        if (File.Exists(path)) {
            Console.WriteLine($"Opening file {path}");
            db.OpenDb(path);
            if (!IsPinCodeValid()) {
                db.CloseDb();
                return;
            }
            CurrentPath = path;
            AddToRecentFilesList(path);
            FirePropertyChanged();
        } else {
            MessageBox.Show($"{resources.GetString("file-not-found")}\n{path}", resources.GetString("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            RemoveFromRecentFilesList(path);
        }
    }

    /// <summary>
    /// Shows a User Dialog to open a DB file
    /// </summary>
    internal void OpenFile() {
        var dialog = new OpenFileDialog() { Title = resources.GetString("dlg-select-file"), Filter = resources.GetString("dlg-filter-open") };
        if (dialog.ShowDialog() == DialogResult.OK)
            OpenFile(dialog.FileName);
    }

    /// <summary>
    /// Shows a User Dialog to create a new DB file
    /// </summary>
    internal void NewFile() {
        var dialog = new SaveFileDialog() { Title = resources.GetString("dlg-new-file"), FileName = "mydb", DefaultExt = "db", Filter = resources.GetString("dlg-filter-save") };
        if (dialog.ShowDialog() == DialogResult.OK) {
            var path = dialog.FileName;
            if (File.Exists(path)) {
                // no need to show YesNoDialog: OS will show it itself
                db.CloseDb();
                try {
                    File.Delete(path); // may throw "Cannot access file because it is being used by another process"
                } catch(Exception e) { Console.Error.WriteLine(e); }
            }

            Console.WriteLine($"Creating file {path}");
            db.CreateDb(path);
            CurrentPath = path;
            AddToRecentFilesList(path);
            FirePropertyChanged();
        }
    }

    /// <summary>
    /// Closes the DB file. If there are no opened DB files, does nothing
    /// </summary>
    internal void CloseFile() {
        if (!db.IsConnected) return;

        db.CloseDb();
        CurrentPath = null;
        FirePropertyChanged();
    }

    /// <summary>
    /// Receives a list of recent files
    /// </summary>
    internal IEnumerable<string> RecentFiles => User.Default.recentFiles.Cast<string>();

    /// <summary>
    /// Receives all tags from the current DB
    /// </summary>
    /// <returns>a list of tags</returns>
    internal IEnumerable<string> GetTags() {
        if (!db.IsConnected) return [];
        return db.GetTags();
    }

    /// <summary>
    /// Receives all notes from the current DB
    /// </summary>
    /// <param name="showArchive">whether to fetch archived notes</param>
    /// <returns>a list of notes</returns>
    internal IEnumerable<Note> GetAllNotes(bool showArchive) {
        if (!db.IsConnected) return [];
        return db.GetAllNotes(showArchive);
    }

    /// <summary>
    /// Receives N random notes from the current DB
    /// </summary>
    /// <param name="showArchive">whether to fetch archived notes</param>
    /// <param name="max">max N notes</param>
    /// <returns>a list of notes</returns>
    internal IEnumerable<Note> GetRandomNotes(bool showArchive, uint max) {
        if (!db.IsConnected) return [];
        return db.GetRandomNotes(showArchive, max);
    }

    /// <summary>
    /// Receives a single note by its ID, or NULL if a note not found
    /// </summary>
    /// <param name="noteId">note ID</param>
    /// <returns>a single note, or NULL</returns>
    internal Note? SearchByID(long noteId) {
        if (!db.IsConnected) return null;
        return db.SearchByID(noteId);
    }

    /// <summary>
    /// Receives all notes from the current DB by a given tag
    /// </summary>
    /// <param name="tag">tag to search</param>
    /// <param name="showArchive">whether to fetch archived notes</param>
    /// <returns>a list of notes</returns>
    internal IEnumerable<Note> SearchByTag(string tag, bool showArchive) {
        if (string.IsNullOrWhiteSpace(tag)) return [];
        if (!db.IsConnected) return [];
        return db.SearchByTag(tag, showArchive);
    }

    /// <summary>
    /// Receives all notes from the current DB by a given keyword (full-text search)
    /// </summary>
    /// <param name="word">keyword to search</param>
    /// <param name="showArchive">whether to fetch archived notes</param>
    /// <returns>a list of notes</returns>
    internal IEnumerable<Note> SearchByKeyword(string word, bool showArchive) {
        if (string.IsNullOrWhiteSpace(word)) return [];
        if (!db.IsConnected) return [];
        return db.SearchByKeyword(word, showArchive);
    }

    /// <summary>
    /// Soft-deletes a note from the current DB by a given ID
    /// </summary>
    /// <param name="noteId">note ID</param>
    internal void ArchiveNoteById(long noteId) {
        if (!db.IsConnected) return;
        if (MessageBox.Show(resources.GetString("msg-archive-txt"), resources.GetString("msg-archive-hdr"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            db.SoftDeleteNote(noteId, true);
    }

    /// <summary>
    /// Undo soft-delete for a given note
    /// </summary>
    /// <param name="noteId">note ID</param>
    internal void RestoreNoteById(long noteId) {
        if (!db.IsConnected) return;
        db.SoftDeleteNote(noteId, false);
    }

    /// <summary>
    /// Deletes (completely) a note from the current DB. You can also use ArchiveNoteById() to perform soft-delete.
    /// </summary>
    /// <param name="noteId">note ID</param>
    internal void DeleteNoteById(long noteId) {
        if (!db.IsConnected) return;
        if (MessageBox.Show(resources.GetString("msg-delete-txt"), resources.GetString("msg-delete-hdr"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            db.DeleteNote(noteId);
    }

    /// <summary>
    /// Upserts a note to the current DB
    /// </summary>
    /// <param name="noteId">note ID</param>
    /// <param name="data">markdown string</param>
    /// <param name="newTags">comma-separated tags of a note, should not be empty</param>
    /// <param name="oldTags">comma-separated old tags (only for UPDATE case); for INSERT case leave it empty</param>
    /// <returns>a new generated note ID for INSERT, and the same note ID for UPDATE</returns>
    internal long? SaveNote(long? noteId, string data, string newTags, string oldTags) {
        var tags = newTags.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (!db.IsConnected) return null;
        if (string.IsNullOrWhiteSpace(data)) return null;
        if (tags.Length == 0) {
            MessageBox.Show(resources.GetString("msg-tag-needed-txt"), resources.GetString("msg-tag-needed-hdr"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }
        if (noteId is long id) {
            // UPDATE
            db.UpdateNote(id, data);
            UpdateTags(id, newTags, oldTags);
            MessageBox.Show(resources.GetString("msg-note-updated"), resources.GetString("done"));
            return noteId;
        } else {
            // INSERT
            var newNoteId = db.InsertNote(data);
            db.LinkTagsToNote(newNoteId, tags);
            MessageBox.Show(resources.GetString("msg-note-added"), resources.GetString("done"));
            return newNoteId;
        }
    }

    /// <summary>
    /// Helper method to update tags
    /// </summary>
    /// <param name="noteId">note ID</param>
    /// <param name="newTags">comma-separated new tags</param>
    /// <param name="oldTags">comma-separated old tags (for UPDATE only)</param>
    private void UpdateTags(long noteId, string newTags, string oldTags) {
        var oldtags = oldTags.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToHashSet();
        var newtags = newTags.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToHashSet();
        var rmTags = oldtags.Except(newtags);
        var addTags = newtags.Except(oldtags);

        db.UnlinkTagsFromNote(noteId, rmTags);
        db.LinkTagsToNote(noteId, addTags);
    }

    /// <summary>
    /// Shows a User Dialog to ask for a PIN-code, and checks if it is valid
    /// </summary>
    /// <returns>TRUE, if PIN code is valid (or no PIN code required), and FALSE otherwise</returns>
    private bool IsPinCodeValid() {
        var dbPinCode = PinCode; // DB call here
        if (dbPinCode == null) return true; // no pinCode protection

        var userPinCode = InputBox.Show(resources.GetString("dlg-input-pin-hdr"), resources.GetString("dlg-input-pin-txt"), placeholder: "PIN");
        if (userPinCode != null) {
            if (dbPinCode == userPinCode) return true; // match!
            MessageBox.Show(resources.GetString("msg-invalid-pin"), resources.GetString("error"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }
        return false;
    }

    /// <summary>
    /// Updates the recent files list with a new item (which is inserted to the top of the list)
    /// </summary>
    /// <param name="item">item to store</param>
    private static void AddToRecentFilesList(string item) {
        User.Default.recentFiles.Remove(item);
        User.Default.recentFiles.Insert(0, item);
        User.Default.Save();
    }

    /// <summary>
    /// Deletes an entry from the recent files list
    /// </summary>
    /// <param name="item">item to remove</param>
    private static void RemoveFromRecentFilesList(string item) {
        User.Default.recentFiles.Remove(item);
        User.Default.Save();
    }

    /// <summary>
    /// Helper method to notify subscribers that the CurrentPath has been changed (a user opened/created a DB file)
    /// </summary>
    protected void FirePropertyChanged() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(CurrentPath));
}
