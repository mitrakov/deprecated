using System.ComponentModel;
using SorexUI.model;

namespace SorexUI.viewmodel;

internal class MainViewModel : INotifyPropertyChanged {
    public event PropertyChangedEventHandler? PropertyChanged;

    private readonly SQLiteDatabase db = new();
    internal string? CurrentPath { get; set; }

    internal void OpenFile(string path) {
        if (File.Exists(path)) {
            Console.WriteLine($"Opening file {path}");
            db.OpenDb(path);
            CurrentPath = path;
            AddToRecentFilesList(path);
            FirePropertyChanged();
        } else {
            MessageBox.Show($"File not found:\n{path}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            RemoveFromRecentFilesList(path);
        }
    }

    internal void OpenFile() {
        var dialog = new OpenFileDialog() { Title = "Select a DB file", Filter = "Sorex DB files (*.db)|*.db|All Files (*.*)|*.*" };
        if (dialog.ShowDialog() == DialogResult.OK)
            OpenFile(dialog.FileName);
    }

    internal void NewFile() {
        var dialog = new SaveFileDialog() { Title = "New DB file", FileName = "mydb", DefaultExt = "db", Filter = "Sorex DB files (*.db)|*.db" };
        if (dialog.ShowDialog() == DialogResult.OK) {
            var path = dialog.FileName;
            if (File.Exists(path)) {
                if (MessageBox.Show($"File already exists:\n{path}\n\nDo you want to erase it?\nIt will remove all data", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation) == DialogResult.Yes) {
                    db.CloseDb();
                    File.Delete(path);
                } else return;
            }

            Console.WriteLine($"Creating file {path}");
            db.CreateDb(path);
            CurrentPath = path;
            AddToRecentFilesList(path);
            FirePropertyChanged();
        }
    }

    internal void CloseFile() {
        db.CloseDb();
        CurrentPath = null;
        FirePropertyChanged();
    }

    internal static IEnumerable<string> RecentFiles => User.Default.recentFiles.Cast<string>();

    internal IEnumerable<string> GetTags() {
        if (!db.IsConnected) return [];
        return db.GetTags();
    }

    internal IEnumerable<Note> GetNotes(bool showArchive) {
        if (!db.IsConnected) return [];
        return db.GetNotes(showArchive);
    }

    internal Note? SearchByID(long noteId) {
        if (!db.IsConnected) return null;
        return db.SearchByID(noteId);
    }

    internal IEnumerable<Note> SearchByTag(string tag, bool showArchive) {
        if (tag.Length == 0) return [];
        if (!db.IsConnected) return [];
        return db.SearchByTag(tag, showArchive);
    }

    internal IEnumerable<Note> SearchByKeyword(string word, bool showArchive) {
        if (word.Length == 0) return [];
        if (!db.IsConnected) return [];
        return db.SearchByKeyword(word, showArchive);
    }

    internal void ArchiveNoteById(long noteId) {
        if (!db.IsConnected) return;
        if (MessageBox.Show("Are you sure you want to archive this note?\nIt can be restored later", "Archive note", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            db.SoftDeleteNote(noteId, true);
    }

    internal void RestoreNoteById(long noteId) {
        if (!db.IsConnected) return;
        db.SoftDeleteNote(noteId, false);
    }

    internal void DeleteNoteById(long noteId) {
        if (!db.IsConnected) return;
        if (MessageBox.Show("Are you sure you want to delete this note?", "Delete note", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            db.DeleteNote(noteId);
    }

    internal long? SaveNote(long? noteId, string data, string newTags, string oldTags) {
        var tags = newTags.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (!db.IsConnected) return null;
        if (data.Length == 0) return null;
        if (tags.Length == 0) {
            MessageBox.Show("Please add at least 1 tag\ne.g. \"Work\" or \"TODO\"", "Tag required", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }
        if (noteId is long id) {
            // UPDATE
            db.UpdateNote(id, data);
            UpdateTags(id, newTags, oldTags);
            MessageBox.Show("Note updated", "Done");
            return noteId;
        } else {
            // INSERT
            var newNoteId = db.InsertNote(data);
            db.LinkTagsToNote(newNoteId, tags);
            MessageBox.Show("Note added", "Done");
            return newNoteId;
        }
    }

    private void UpdateTags(long noteId, string newTags, string oldTags) {
        var oldtags = oldTags.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToHashSet();
        var newtags = newTags.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToHashSet();
        var rmTags = oldtags.Except(newtags);
        var addTags = newtags.Except(oldtags);

        db.UnlinkTagsFromNote(noteId, rmTags);
        db.LinkTagsToNote(noteId, addTags);
    }

    private static void AddToRecentFilesList(string item) {
        User.Default.recentFiles.Remove(item);
        User.Default.recentFiles.Insert(0, item);
        User.Default.Save();
    }

    private static void RemoveFromRecentFilesList(string item) {
        User.Default.recentFiles.Remove(item);
        User.Default.Save();
    }

    protected void FirePropertyChanged() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(CurrentPath));
}
