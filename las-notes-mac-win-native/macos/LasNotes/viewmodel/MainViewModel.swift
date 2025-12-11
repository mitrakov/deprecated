import Foundation

final internal class MainViewModel: ObservableObject {
    private let db = SQLiteDatabase()                                      // reference to the DB
    @Published var currentPath: String?                                    // currently opened DB file
    @Published var like = UserDefaults.standard.bool(forKey: isSponsorKey) // published copy of "isSponsor" to update Toolbar and MainViewModel

    /// PIN-code for a current DB
    var pinCode: String? {
        get {
            guard db.isConnected() else {return nil}
            return db.getMetadata(key: "pincode")
        }
        set {
            guard db.isConnected() else {return}
            db.setMetadata(key: "pincode", newValue)
        }
    }
    
    /// Shows if a user is a Las Notes sponsor
    var isSponsor: Bool {
        get {
            return UserDefaults.standard.bool(forKey: isSponsorKey)
        }
        set {
            like = newValue
            UserDefaults.standard.set(newValue, forKey: isSponsorKey)
            UserDefaults.standard.synchronize()
        }
    }

    /// Whether to show digest on startup
    var showDigest: Bool {
        get {
            return UserDefaults.standard.bool(forKey: showDigestKey)
        }
        set {
            UserDefaults.standard.set(newValue, forKey: showDigestKey)
            UserDefaults.standard.synchronize()
        }
    }

    /// Opens a DB file
    /// - Parameters:
    ///   - path: Full path to DB file
    func openFile(_ path: String) {
        if FileManager.default.fileExists(atPath: path) {
            print("Opening file \(path)")
            db.openDb(path)
            if (!isPinCodeValid()) {
                db.closeDb()
                return
            }
            currentPath = path // MainView and LasNotesApp will be updated
            addToRecentFilesList(path)
        } else {
            Utils.showAlert(title: ~"error", text: String(format: ~"file-not-found", path), .critical)
            removeFromRecentFilesList(path)
        }
    }
    
    /// Shows a User Dialog to open a DB file
    func openFile() {
        if let path = Utils.showOpenFileDialog(message: ~"dlg-select-file", ["db"]) {
            self.openFile(path)
        }
    }
    
    /// Shows a User Dialog to create a new DB file
    func newFile() {
        if let path = Utils.showSaveFileDialog(title: ~"dlg-new-file-hdr", message: ~"dlg-new-file-txt", nameLabel: ~"dlg-new-file-lbl", defaultName: "mydb", ["db"]) {
            if FileManager.default.fileExists(atPath: path) {
                // no need to show YesNoDialog: OS will show it itself
                db.closeDb()
                do {
                    try FileManager.default.removeItem(atPath: path)
                } catch {print(error)}
            }
            
            print("Creating file \(path)")
            db.createDb(path)
            currentPath = path // MainView and LasNotesApp will be updated
            addToRecentFilesList(path)
        }
    }
    
    /// Closes the DB file. If there are no opened DB files, does nothing
    func closeFile() {
        guard db.isConnected() else {return}
        db.closeDb()
        currentPath = nil // MainView will be updated
    }

    /// Receives a list of recent files
    /// - Returns: a list of recent files
    func getRecentFiles() -> [String] {
        UserDefaults.standard.stringArray(forKey: recentFilesKey) ?? []
    }

    /// Receives all tags from the current DB
    /// - Returns: a list of tags
    func getTags() -> [String] {
        guard db.isConnected() else {return []}
        return db.getTags()
    }
    
    /// Receives all notes from the current DB
    /// - Parameters:
    ///   - showArchive: wheter to fetch archived notes
    /// - Returns: a list of notes
    func getAllNotes(showArchive: Bool) -> [Note] {
        guard db.isConnected() else {return []}
        return db.getAllNotes(fetchDeleted: showArchive)
    }
    
    /// Receives N random notes from the current DB
    /// - Parameters:
    ///   - showArchive: wheter to fetch archived notes
    ///   - max: max N notes
    /// - Returns: a list of notes
    func getRandomNotes(showArchive: Bool, max: Int) -> [Note] {
        guard db.isConnected() else {return []}
        return db.getRandomNotes(fetchDeleted: showArchive, limit: max)
    }

    /// Receives a single note by its ID, or NULL if a note not found
    /// - Parameters:
    ///   - noteId: note ID
    /// - Returns: a single note, or NIL
    func searchByID(_ noteId: Int64) -> Note? {
        guard db.isConnected() else {return nil}
        return db.searchByID(noteId)
    }

    /// Receives all notes from the current DB by a given tag
    /// - Parameters:
    ///   - tag: tag to search
    ///   - showArchive: whether to fetch archived notes
    /// - Returns: a list of notes
    func searchByTag(_ tag: String, showArchive: Bool) -> [Note] {
        guard !tag.isWhiteSpace() else {return []}
        guard db.isConnected() else {return []}
        return db.searchByTag(tag, fetchDeleted: showArchive)
    }
    
    /// Receives all notes from the current DB by a given keyword (full-text search)
    /// - Parameters:
    ///   - word: keyword to search
    ///   - showArchive: whether to fetch archived notes
    /// - Returns: a list of notes
    func searchByKeyword(_ word: String, showArchive: Bool) -> [Note] {
        guard !word.isWhiteSpace() else {return []}
        guard db.isConnected() else {return []}
        return db.searchByKeyword(word, fetchDeleted: showArchive)
    }

    /// Soft-deletes a note from the current DB by a given ID
    /// - Parameters:
    ///   - noteId: note ID
    func archiveNoteById(_ noteId: Int64) {
        guard db.isConnected() else {return}
        if Utils.showYesNoDialog(title: ~"msg-archive-hdr", text: ~"msg-archive-txt") {
            db.softDeleteNote(noteId, deleted: true)
        }
    }

    /// Undo soft-delete for a given note
    /// - Parameters:
    ///   - noteId: note ID
    func restoreNoteById(_ noteId: Int64) {
        guard db.isConnected() else {return}
        db.softDeleteNote(noteId, deleted: false)
    }
    
    /// Deletes (completely) a note from the current DB. You can also use ArchiveNoteById() to perform soft-delete.
    /// - Parameters:
    ///   - noteId: note ID
    func deleteNoteById(_ noteId: Int64) {
        guard db.isConnected() else {return}
        if Utils.showYesNoDialog(title: ~"msg-delete-hdr", text: ~"msg-delete-txt") {
            db.deleteNote(noteId)
        }
    }
    
    /// Upserts a note to the current DB
    /// - Parameters:
    ///   - noteId: note ID
    ///   - data: markdown string
    ///   - newTags: comma-separated tags of a note, should not be empty
    ///   - oldTags: comma-separated old tags (only for UPDATE case); for INSERT case leave it empty
    /// - Returns: a new generated note ID for INSERT, and the same note ID for UPDATE
    func saveNote(_ noteId: Int64?, data: String, newTags: String, oldTags: String) -> Int64? {
        let tags = newTags.splitted()
        guard db.isConnected() else {return nil}
        guard !data.isWhiteSpace() else {return nil}
        guard !tags.isEmpty else {
            Utils.showAlert(title: ~"msg-tag-needed-hdr", text: ~"msg-tag-needed-txt", .warning)
            return nil
        }
        
        if let noteId = noteId {
            // UPDATE
            db.updateNote(noteId, data)
            updateTags(noteId, newTags: newTags, oldTags: oldTags)
            Utils.showAlert(title: ~"done", text: ~"msg-note-updated")
            return noteId
        } else {
            // INSERT
            let newNoteId = db.insertNote(data)
            db.linkTagsToNote(newNoteId, tags)
            Utils.showAlert(title: ~"done", text: ~"msg-note-added")
            return newNoteId
        }
    }
    
    /// Helper method to update tags
    /// - Parameters:
    ///   - noteId: note ID
    ///   - newTags: comma-separated new tags
    ///   - oldTags: comma-separated old tags (for UPDATE only)
    private func updateTags(_ noteId: Int64, newTags: String, oldTags: String) {
        let oldTags = Set(oldTags.splitted())
        let newTags = Set(newTags.splitted())
        let rmTags  = Array(oldTags.subtracting(newTags))
        let addTags = Array(newTags.subtracting(oldTags))
        
        db.unlinkTagsFromNote(noteId, rmTags)
        db.linkTagsToNote(noteId, addTags)
    }
    
    /// Shows a User Dialog to ask for a PIN-code, and checks if it is valid
    /// - Returns: TRUE, if PIN code is valid (or no PIN code required), and FALSE otherwise
    private func isPinCodeValid() -> Bool {
        let dbPinCode = pinCode // DB call here
        if dbPinCode == nil {return true} // no pinCode protection

        if let userPinCode = Utils.showInputBox(title: ~"dlg-input-pin-hdr", text: ~"dlg-input-pin-txt", placeholder: "PIN") {
            if dbPinCode == userPinCode {return true} // match!
            Utils.showAlert(title: ~"error", text: ~"msg-invalid-pin", .critical)
        }
        return false
    }

    /// Updates the recent files list with a new item (which is inserted to the top of the list)
    /// - Parameters:
    ///   - item: item to store
    private func addToRecentFilesList(_ item: String) {
        UserDefaults.standard.set([item] + getRecentFiles().filter {$0 != item}, forKey: recentFilesKey)
        UserDefaults.standard.synchronize()
    }
    
    /// Deletes an entry from the recent files list
    /// - Parameters:
    ///   - item: item to remove
    private func removeFromRecentFilesList(_ item: String) {
        UserDefaults.standard.set(getRecentFiles().filter {$0 != item}, forKey: recentFilesKey)
        UserDefaults.standard.synchronize()
    }
}
