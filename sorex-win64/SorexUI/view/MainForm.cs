using System.Reflection;
using System.Resources;
using SorexMarkdownLibrary;
using SorexUI.model;
using SorexUI.viewmodel;

namespace SorexUI.view;

internal partial class MainForm : Form {
    private readonly MainViewModel vm;

    private long? currentNoteId;                      // if present, it's an ID of the note in edit mode
    private string oldTags = "";                      // old comma-separated tags for edit mode (to calc tags diff)
    private IEnumerable<Note> notes = [];             // in view mode, DB notes array for markdown view
    private string search = "";                       // search by tag name (SearchMode.tag), keyword (.keyword) or ID (.id)
    private EditorMode editorMode = EditorMode.edit;  // edit or view mode
    private SearchMode searchMode = SearchMode.tag;   // how to search notes (by clicking tag, by full-text search or by ID)

    internal MainForm(MainViewModel vm) {
        this.vm = vm;
        vm.PropertyChanged += (s, e) => UpdateUI(updateTagBtns: true);

        InitializeComponent();
        // due to a bug in UI Designer, these ElementHosts should be handled manually
        wpfHostSingle = new() { Child = sorexMarkdownSingle = new(), Dock = DockStyle.Fill };
        wpfHostMulti = new() { Child = sorexMarkdownMulti = new(), Dock = DockStyle.Fill };
        editSplitPanel.Panel2.Controls.Add(wpfHostSingle);
        // manually add images to avoid warning: "Resource "imagesNew.ImageStream" of type "System.String" may be deserialized via BinaryFormatter at runtime. BinaryFormatter is deprecated..."
        imagesNew.Images.Add(Extensions.BytesToImage(new ResourceManager(GetType()).GetObject("plus") as byte[] ?? []));
        imagesSave.Images.Add(Extensions.BytesToImage(new ResourceManager(GetType()).GetObject("plus-circle") as byte[] ?? []));
        imagesSave.Images.Add(Extensions.BytesToImage(new ResourceManager(GetType()).GetObject("mark-circle") as byte[] ?? []));
        buttonSave.ImageIndex = buttonNew.ImageIndex = 0;

        UpdateMenu();
        UpdateUI();
    }

    private void UpdateUI(bool updateTagBtns = false) {
        // tagsPanel
        if (updateTagBtns) {
            tagsPanel.Controls.Clear();
            tagsPanel.Controls.AddRange(vm.GetTags().Select(tag => {
                var btn = new Button { Text = tag, Size = new(170, 30), TextAlign = ContentAlignment.MiddleLeft };
                btn.Click += (s, e) => SetReadMode(tag, SearchMode.tag);
                return btn;
            }).ToArray());
        }

        // contentPanel
        contentPanel.Controls.Clear();
        contentPanel.Controls.Add(editorMode == EditorMode.edit ? editModePanel : wpfHostMulti);

        // notes markdown
        var ctx = vm.CurrentPath != null ? notes.Select(note => new ContextMenu(
            note.Data,
            note.IsDeleted,
            note.Tags.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
            // if a button gets deleted after click, form will lose focus => need to Activate() it manually
            () => { SetEditMode(note.Id, note.Data, note.Tags); Task.Delay(0).ContinueWith(t => Invoke(() => Activate())); },
            () => { vm.ArchiveNoteById(note.Id); SetReadMode(search, searchMode); },
            () => { vm.RestoreNoteById(note.Id); SetReadMode(search, searchMode); },
            () => { vm.DeleteNoteById(note.Id); SetReadMode(search, searchMode, updateTagBtns: true); }
        )) : [];
        sorexMarkdownMulti.SetMarkdown(ctx);

        // bottom button
        buttonSave.Text = currentNoteId == null ? " Add Note" : "Update Note";
        buttonSave.ImageIndex = currentNoteId == null ? 0 : 1;

        // form
        Text = vm.CurrentPath != null ? $"Sorex ({vm.CurrentPath})" : "Sorex";
        panelLeft.Enabled = contentPanel.Enabled = vm.CurrentPath != null;
        if (editorMode == EditorMode.edit) textboxEdit.Focus();
    }

    private void UpdateMenu() {
        openRecentMenuItem.DropDownItems.Clear();
        openRecentMenuItem.DropDownItems.AddRange(User.Default.recentFiles.Cast<string>().Select(file => new ToolStripMenuItem(file, null, OnRecentFileClick)).ToArray());
    }

    private void OnTextboxEditChange(object sender, EventArgs e) => sorexMarkdownSingle.Markdown = textboxEdit.Text;

    private void OnCheckboxShowArchiveChange(object sender, EventArgs e) => SetReadMode(search, searchMode);

    private void OnNewButtonClick(object sender, EventArgs e) => SetEditMode();

    private void OnTextboxSearchKeyDown(object sender, KeyEventArgs e) {
        if (e.KeyCode == Keys.Enter) {
            SetReadMode(textboxSearch.Text, SearchMode.keyword);
            e.SuppressKeyPress = true; // avoid beep sound
        }
    }

    private void OnTextboxTagsKeyDown(object sender, KeyEventArgs e) {
        if (e.KeyCode == Keys.Enter) {
            buttonSave.PerformClick();
            e.SuppressKeyPress = true; // avoid beep sound
        }
    }

    private void OnRecentFileClick(object? sender, EventArgs e) {
        if (sender is ToolStripMenuItem item) {
            vm.OpenFile(item.Text ?? "");
            UpdateMenu();
        }
    }

    private void OnNewFileClick(object sender, EventArgs e) {
        vm.NewFile();
        UpdateMenu();
    }

    private void OnOpenFileClick(object sender, EventArgs e) {
        vm.OpenFile();
        UpdateMenu();
    }

    private void OnCloseFileClick(object sender, EventArgs e) => vm.CloseFile();

    private void OnQuitSorexClick(object sender, EventArgs e) {
        vm.CloseFile();
        Application.Exit();
    }

    private void OnAboutSorexClick(object sender, EventArgs e) {
        var info = Assembly.GetExecutingAssembly().GetName();
        var msg = $"{info.Name} v{info.Version}\nAuthor: Artem Mitrakov (mitrakov-artem@yandex.ru)\nLicensed under MIT © All rights reserved";
        MessageBox.Show(msg, "Sorex", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void SaveNote(object sender, EventArgs e) {
        var newId = vm.SaveNote(currentNoteId, textboxEdit.Text.ReplaceLineEndings("\n"), textboxTags.Text.Trim(), oldTags);
        if (newId != null)
            SetReadMode($"{newId}", SearchMode.id, updateTagBtns: true);
        else textboxTags.Focus();
    }

    protected void SetEditMode(long? noteId = null, string text = "", string tags = "") {
        textboxEdit.Text = text;
        textboxTags.Text = tags;
        textboxSearch.Text = "";
        currentNoteId = noteId;
        oldTags = tags;
        notes = [];
        search = "";
        editorMode = EditorMode.edit;
        searchMode = SearchMode.tag;

        UpdateUI();
    }

    protected void SetReadMode(string search, SearchMode by, bool updateTagBtns = false) {
        textboxEdit.Text = "";
        textboxTags.Text = "";
        textboxSearch.Text = textboxSearch.Text;
        currentNoteId = null;
        oldTags = "";
        notes = by == SearchMode.tag ? vm.SearchByTag(search, checkShowArchive.Checked) :
                by == SearchMode.keyword ? vm.SearchByKeyword(search, checkShowArchive.Checked) :
                by == SearchMode.id ? new[] { vm.SearchByID(long.Parse(search)) }.OfType<Note>() : [];
        this.search = search;
        editorMode = EditorMode.read;
        searchMode = by;

        UpdateUI(updateTagBtns);
    }
}

enum EditorMode { read, edit }

enum SearchMode { tag, keyword, id }
