import SwiftUI
import MarkdownUI // don't use "LiYanan2004/MarkdownView" due to performance issues: https://github.com/LiYanan2004/MarkdownView/issues/36

internal struct MainView: View {
    @EnvironmentObject var vm: MainViewModel

    // bindings
    @Binding internal var currentText: String         // binding for main text in add/edit mode
    @State private var currentTags = ""               // binding for comma-separated tags in the text field
    @State private var currentKeyword = ""            // binding for full-text search textfield
    @State private var showArchived = false           // binding for whether to show soft-deleted (archived) notes
    @State private var selection: [NSRange] = []      // binding for selection range in main editor
    @FocusState private var focusedField: FocusField? // binding for active focused textfield

    // vars
    @State private var currentNoteId: Int64?          // if present, it's an ID of the note in edit mode
    @State private var oldTags = ""                   // old comma-separated tags for edit mode (to calc tags diff)
    @State private var notes: [Note] = []             // in view mode, DB notes array for markdown view
    @State private var search = ""                    // search by tag name (SearchMode.tag), keyword (.keyword) or ID (.id)
    @State private var editorMode = EditorMode.edit   // edit or view mode
    @State private var searchMode = SearchMode.tag    // how to search notes (by clicking tag, by full-text search or by ID)

    var body: some View {
        HSplitView {
            // LEFT DRAWER
            VStack {
                HStack(alignment: .top) {
                    Button { setEditMode() } label: {
                        VStack {
                            Image(systemName: "plus.app")
                                .padding(4)
                                .font(.system(size: 26, weight: .bold))
                                .background(.white)
                                .cornerRadius(10)
                                .frame(width: 30, height: 30)
                                .shadow(color: .black.opacity(0.15), radius: 5, x: 5, y: 5)
                                .shadow(color: .black.opacity(0.15), radius: 5, x: -5, y: 5)
                            
                            Text("new")
                        }
                    }
                    .padding(8)
                    .buttonStyle(PlainButtonStyle())
                    .keyboardShortcut("n")
                    .help(String("⌘N"))
                    
                    VStack {
                        Textbox(stringValue: $currentKeyword, placeholder: ~"global-search", onEnter: {
                            setReadMode(search: currentKeyword, by: .keyword)
                        }) // don't use Textfield due to the bug: https://stackoverflow.com/q/74585499
                            .autocorrectionDisabled()
                            .clipShape(RoundedRectangle(cornerRadius: 6))
                            .overlay(RoundedRectangle(cornerRadius: 6).stroke(.black, lineWidth: 0.5).shadow(radius: 1))
                            .focused($focusedField, equals: .search)
                            .padding(.top, 10)
                            .padding(.trailing, 6)

                        Toggle("show-archive", isOn: $showArchived)
                            .toggleStyle(SwitchToggleStyle(tint: .red))
                            .onChange(of: showArchived) { value in
                                setReadMode(search: search, by: searchMode) // update
                            }
                            .padding(.top, 1)
                            .padding(.trailing, 4)
                    }
                }

                // LIST OF TAGS
                ScrollView {
                    ForEach(vm.getTags(), id: \.self) { tag in
                        HStack {
                            Button { setReadMode(search: tag, by: .tag) } label: {
                                Text(tag)
                                Spacer()
                            }
                            .padding(.horizontal, 3)
                            
                            Spacer()
                        }
                    }

                    // fake buttons to register shortcuts in SwiftUI
                    Button(String("")) { focusedField = .search }
                        .frame(width: 0, height: 0)
                        .buttonStyle(PlainButtonStyle())
                        .keyboardShortcut("f", modifiers: [.command, .shift])
                    Button(String("")) { vm.isSponsor = false } // for debug purposes
                        .frame(width: 0, height: 0)
                        .buttonStyle(PlainButtonStyle())
                        .keyboardShortcut(";", modifiers: [.command, .shift, .control, .option])
                }
            }
            .padding(.leading, 8)
            .frame(minWidth: 180, maxWidth: 255)
            .disabled(vm.currentPath == nil)

            // RIGHT MAIN AREA
            HStack {
                Spacer()
                VStack {
                    switch vm.currentPath {
                    case nil:
                        HStack {
                            Spacer()
                            VStack {
                                Spacer()
                                SplashScreen()
                                Spacer()
                            }
                            Spacer()
                        }
                    default:
                        switch editorMode {
                        case .read:
                            VStack {
                                if (searchMode == .random) {
                                    Text("digest")
                                        .font(Font.custom("SavoyeLetPlain", size: 40))
                                        .padding(.top, 10)
                                        .padding(.bottom, -16)
                                }
                                ScrollView {
                                    // LIST OF NOTES
                                    ForEach(notes) { note in
                                        ZStack(alignment: .topTrailing) {
                                            HStack {
                                                Markdown(note.data)
                                                    .markdownTheme(.docC)
                                                    .textSelection(.enabled)
                                                    .opacity(note.isDeleted ? 0.6 : 1)
                                                Spacer()
                                            }
                                            
                                            ContextMenu(isArchived: note.isDeleted, tags: note.tags.splitted())
                                                .onEdit {
                                                    setEditMode(noteId: note.id, text: note.data, tags: note.tags)
                                                }
                                                .onArchive {
                                                    vm.archiveNoteById(note.id)
                                                    setReadMode(search: search, by: searchMode) // update
                                                }
                                                .onRestore {
                                                    vm.restoreNoteById(note.id)
                                                    setReadMode(search: search, by: searchMode) // update
                                                }
                                                .onDelete {
                                                    vm.deleteNoteById(note.id)
                                                    setReadMode(search: search, by: searchMode) // update
                                                }
                                        }
                                        Divider()
                                    }
                                }.padding(.vertical, 5)
                            }

                        case .edit:
                            Toolbar(txt: $currentText, range: $selection, focus: $focusedField, like: vm.isSponsor)
                            HSplitView {
                                // LEFT EDITOR
                                LasNotesEditor(text: $currentText, selectedRanges: $selection) // in macOS 15+ use TextEditor(text:selection:)
                                    .font(.monospacedSystemFont(ofSize: 14, weight: .regular))
                                    .focused($focusedField, equals: .editor)

                                // RIGHT PREVIEW
                                ScrollView {
                                    HStack {
                                        Markdown(currentText)
                                            .markdownTheme(.docC)
                                            .textSelection(.enabled)
                                        Spacer()
                                    }
                                }
                                .padding(4)
                            }
                            Spacer()

                            // BOTTOM PANEL
                            HStack {
                                Text("tags")
                                
                                // don't use Textfield due to the bug: https://stackoverflow.com/q/74585499
                                Textbox(stringValue: $currentTags, placeholder: ~"tag1-tag2", onEnter: saveNote)
                                    .frame(maxWidth: 200)
                                    .autocorrectionDisabled()
                                    .clipShape(RoundedRectangle(cornerRadius: 6))
                                    .overlay(RoundedRectangle(cornerRadius: 6).stroke(.black, lineWidth: 0.5).shadow(radius: 1))
                                    .focused($focusedField, equals: .tags)

                                Button(action: saveNote, label: {
                                    Label { Text("save-note") } icon: {
                                        Image(systemName: currentNoteId == nil ? "plus.circle" : "checkmark.seal")
                                            .font(.system(size: 15, weight: .semibold))
                                    }
                                })
                                .buttonStyle(TallButtonStyle())
                                .disabled(currentText.isWhiteSpace())
                                .keyboardShortcut("s")
                                .help(String("⌘S"))
                                
                                Spacer()
                            }.padding(.bottom, 7)
                        }
                    }
                }
            }
        }
        .preferredColorScheme(.light)
        .navigationTitle(String(vm.currentPath != nil ? "Las Notes (\(vm.currentPath!))" : "Las Notes")) // "String()" to avoid l10n
        .onReceive(vm.$currentPath) { _ in
            if vm.currentPath != nil && vm.isSponsor && vm.showDigest {
                setReadMode(search: "", by: .random)
            } else {
                setEditMode()
            }
        }
        .onExitCommand { // on ESC
            if vm.currentPath != nil && editorMode == .edit {
                setReadMode(search: search, by: searchMode)
            }
        }
    }
    
    private func saveNote() {
        guard !currentText.isWhiteSpace() else {return}

        if let newId = vm.saveNote(currentNoteId, data: currentText, newTags: currentTags, oldTags: oldTags) {
            setReadMode(search: String(newId), by: .id)
        } else {focusedField = .tags}
    }
    
    private func setEditMode(noteId: Int64? = nil, text: String = "", tags: String = "") {
        self.currentText = text
        self.currentTags = tags
        self.currentKeyword = ""
        self.currentNoteId = noteId
        self.oldTags = tags
        self.notes = []
        self.editorMode = .edit
        /// self.search = search
        /// self.searchMode = searchMode

        focusedField = .editor
    }
    
    private func setReadMode(search: String, by: SearchMode) {
        self.currentText = ""
        self.currentTags = ""
        /// self.currentKeyword = currentKeyword
        self.currentNoteId = nil
        self.oldTags = ""
        self.notes = by == .tag     ? vm.getAllNotes(showArchive: showArchived) :
                     by == .tag     ? vm.searchByTag(search, showArchive: showArchived) :
                     by == .keyword ? vm.searchByKeyword(search, showArchive: showArchived) :
                     by == .id      ? vm.searchByID(Int64(search)!).map{[$0]} ?? [] :
                     by == .random  ? vm.getRandomNotes(showArchive: showArchived, max: 10) : []
        self.editorMode = .read
        self.search = search
        self.searchMode = by
    }
}

private enum EditorMode {
    case read, edit
}

private enum SearchMode {
    case all, tag, keyword, id, random
}

enum FocusField {
    case tags, editor, search
}

struct TallButtonStyle: ButtonStyle {
    @Environment(\.isEnabled) private var isEnabled: Bool

    func makeBody(configuration: Configuration) -> some View {
        configuration.label
            .frame(height: 24)
            .font(Font.system(size: 13, weight: .semibold))
            .padding(.horizontal, 8)
            .foregroundColor(!isEnabled ? .gray.opacity(0.6) : configuration.isPressed ? .white : .black)
            .background(!isEnabled ? .gray.opacity(0.1) : configuration.isPressed ? .blue : .white)
            .clipShape(RoundedRectangle(cornerRadius: 6))
            .overlay(RoundedRectangle(cornerRadius: 6).stroke(.black, lineWidth: !isEnabled ? 0 : 0.5).shadow(radius: 1))
    }
}

#Preview {
    MainView(currentText: .constant("")).environmentObject(MainViewModel())
}
