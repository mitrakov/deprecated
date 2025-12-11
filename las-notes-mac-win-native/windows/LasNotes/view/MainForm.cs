using System.Diagnostics;
using System.Reflection;
using MdControl;

namespace LasNotes;

internal partial class MainForm : Form {
    private readonly MainViewModel vm;
    private readonly CollectionHash<string> hash = new();

    private long? currentNoteId;                      // if present, it's an ID of the note in edit mode
    private string oldTags = "";                      // old comma-separated tags for edit mode (to calc tags diff)
    private IEnumerable<Note> notes = [];             // in view mode, DB notes array for markdown view
    private string search = "";                       // search by tag name (SearchMode.tag), keyword (.keyword) or ID (.id)
    private EditorMode editorMode = EditorMode.edit;  // edit or view mode
    private SearchMode searchMode = SearchMode.tag;   // how to search notes (by clicking tag, by full-text search or by ID)

    internal MainForm(MainViewModel vm) {
        this.vm = vm;

        // when a user selects a DB file, "vm.PropertyChanged" is raised
        vm.PropertyChanged += (s, e) => {
            if (vm.CurrentPath != null && vm.IsSponsor && vm.ShowDigest)
                SetReadMode("", SearchMode.random);
            else SetEditMode();
        };

        // due to a bug in UI Designer, these ElementHosts should be handled manually
        wpfHostSingle = new() { Child = markdownSingle = new(), Dock = DockStyle.Fill };
        wpfHostMulti = new() { Child = markdownMulti = new(), Dock = DockStyle.Fill };

        InitializeComponent();

        // getting images from byte arrays
        imagesNew .Images.Add(Utils.BytesToImage(resources.GetObject("plus") as byte[] ?? []));
        imagesSave.Images.Add(Utils.BytesToImage(resources.GetObject("plus-circle") as byte[] ?? []));
        imagesSave.Images.Add(Utils.BytesToImage(resources.GetObject("mark-circle") as byte[] ?? []));
        labelDigest.Image = donateDoneMenuItem.Image = Utils.BytesToImage(resources.GetObject("heart") as byte[] ?? []);
        Icon = Utils.BytesToIcon(resources.GetObject("icon") as byte[] ?? []);
        buttonSave.ImageIndex = buttonNew.ImageIndex = 0;

        // splash screen
        splashScreen.AddHandlers(OnNewFileClick, OnOpenFileClick);

        // *)
        SetEditMode();
    }

    private void UpdateUI() {
        // contentPanel
        splashScreen.Visible  = vm.CurrentPath == null;
        editModePanel.Visible = vm.CurrentPath != null && editorMode == EditorMode.edit;
        readModePanel.Visible = vm.CurrentPath != null && editorMode == EditorMode.read;

        // tagsPanel
        tagsPanel.AddButtons(vm.GetTags().Select(tag => {
            var btn = new Button { Text = tag };
            btn.Click += (s, e) => SetReadMode(tag, SearchMode.tag);
            return btn;
        }));

        // notes markdown
        var ctx = vm.CurrentPath != null ? notes.Select(note => new ContextMenu(
            note.Data,
            note.IsDeleted,
            note.Tags.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
            // if a button gets deleted after click, form will lose focus => need to Activate() it manually
            () => { SetEditMode(note.Id, note.Data, note.Tags); Task.Delay(0).ContinueWith(t => Invoke(() => Activate())); },
            () => { vm.ArchiveNoteById(note.Id); SetReadMode(search, searchMode); },
            () => { vm.RestoreNoteById(note.Id); SetReadMode(search, searchMode); },
            () => { vm.DeleteNoteById(note.Id); SetReadMode(search, searchMode); }
        )) : [];
        markdownMulti.SetMarkdown(ctx);

        // menu
        setPinMenuItem.Enabled = vm.CurrentPath != null;
        donateDoneMenuItem.Visible = !vm.IsSponsor;
        showDigestMenuCheckbox.Visible = vm.IsSponsor;
        showDigestMenuCheckbox.IsChecked = vm.ShowDigest;
        UpdateRecentFilesMenu();

        // other components
        buttonSave.ImageIndex = currentNoteId == null ? 0 : 1; // + or ✔
        labelDigest.Visible = searchMode == SearchMode.random; // show "❤️ DIGEST" label for sponsors
        panelLeft.Enabled = vm.CurrentPath != null;            // disable left bar when no DB file opened
        toolbar.SetLike(vm.IsSponsor);                         // ♡ or ❤️
        splashScreen.AddCards(vm.RecentFiles.Select(file => new CardWidget(Path.GetFileName(file), file, OnRecentFileClick)));

        // form
        Text = vm.CurrentPath != null ? $"Las Notes ({vm.CurrentPath})" : "Las Notes";
    }

    private void UpdateRecentFilesMenu() {
        if (!hash.NeedUpdate(vm.RecentFiles)) return;

        openRecentMenuItem.DropDownItems.Clear();
        openRecentMenuItem.DropDownItems.AddRange(
            vm.RecentFiles.Select((file, i) => {
                return i < 9
                    ? new ToolStripMenuItem(file, null, OnRecentFileClick, Keys.Control | Keys.D1 + i)
                    : new ToolStripMenuItem(file, null, OnRecentFileClick);
            }).ToArray()
        );
    }

    private void OnMainFormLoad(object sender, EventArgs e) {
        // this should be done on form load, to get actual sizes
        splashScreen.Location = new Point((contentPanel.Size.Width - splashScreen.Width - contentPanel.Left) / 2, 50);
        labelDigest.Padding = new Padding((contentPanel.Size.Width - labelDigest.Width) / 2, 0, 0, 3);
    }

    private void OnTextboxEditChange(object sender, EventArgs e) {
        markdownSingle.Markdown = textboxEdit.Text;
        buttonSave.Enabled = !string.IsNullOrWhiteSpace(textboxEdit.Text);
    }

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
        } else if (sender is UserControl uc) {
            vm.OpenFile(uc.Text);
        }
    }

    private void OnNewFileClick(object? sender, EventArgs e) {
        vm.NewFile();
    }

    private void OnOpenFileClick(object? sender, EventArgs e) {
        vm.OpenFile();
    }

    private void OnSetPinClick(object? sender, EventArgs e) {
        if (vm.CurrentPath == null) return;

        var curPin = vm.PinCode ?? ""; // DB call here
        var pin = InputBox.Show(resources.GetString("dlg-set-pin-hdr"), resources.GetString("dlg-set-pin-txt"), curPin, "PIN");
        if (pin != null) {
            if (string.IsNullOrWhiteSpace(pin)) {
                vm.PinCode = null;
                MessageBox.Show(resources.GetString("msg-pin-unset"), resources.GetString("done"));
            } else if (pin != curPin) {
                if (!string.IsNullOrWhiteSpace(textboxEdit.Text))
                    Clipboard.SetText(textboxEdit.Text); // DB file will be closed => let's CTRL+C user's text, if any
                vm.PinCode = pin;
                MessageBox.Show(resources.GetString("msg-pin-set"), resources.GetString("done"));
                vm.CloseFile();
            }
        }
    }

    private void OnCloseFileClick(object sender, EventArgs e) => vm.CloseFile();

    private void OnQuitClick(object sender, EventArgs e) {
        vm.CloseFile();
        Application.Exit();
    }

    private void OnAboutClick(object sender, EventArgs e) {
        var info = Assembly.GetExecutingAssembly().GetName();
        MessageBox.Show(resources.GetString("msg-about"), $"{info.Name} v{info.Version?.ToString(3)}", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void OnDonateClick(object sender, EventArgs e) => Process.Start("explorer", "https://lasnotes.com/donate");

    private void OnDonationTextBoxKeyPress(object sender, KeyPressEventArgs e) {
         if (e.KeyChar == (char)Keys.Return && sender is TextBox txtBox) {
            if (txtBox.Text.Trim() == "I am a Las Notes sponsor") {
                vm.IsSponsor = vm.ShowDigest = true;
                UpdateUI();
                MessageBox.Show(resources.GetString("msg-donate-valid"), resources.GetString("success"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            } else MessageBox.Show(resources.GetString("msg-donate-invalid"), resources.GetString("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void OnShowDigestClick(object sender, EventArgs e) => vm.ShowDigest = showDigestMenuCheckbox.IsChecked;

    private void SaveNote(object sender, EventArgs e) {
        if (string.IsNullOrWhiteSpace(textboxEdit.Text)) return;

        var newId = vm.SaveNote(currentNoteId, textboxEdit.Text.ReplaceLineEndings("\n"), textboxTags.Text.Trim(), oldTags);
        if (newId != null)
            SetReadMode($"{newId}", SearchMode.id);
        else textboxTags.Focus();
    }

    private void SetEditMode(long? noteId = null, string text = "", string tags = "") {
        textboxEdit.Text = text;
        textboxTags.Text = tags;
        textboxSearch.Text = "";
        currentNoteId = noteId;
        oldTags = tags;
        notes = [];
        editorMode = EditorMode.edit;
        /// search = search;
        /// searchMode = searchMode;

        UpdateUI();
        textboxEdit.SelectionStart = text.Length; // move cursor at the end
        Task.Delay(0).ContinueWith(t => Invoke(() => textboxEdit.Focus())); // Focus() should be called in a next loop cycle
    }

    private void SetReadMode(string search, SearchMode by) {
        textboxEdit.Text = "";
        textboxTags.Text = "";
        /// textboxSearch.Text = textboxSearch.Text;
        currentNoteId = null;
        oldTags = "";
        notes = by == SearchMode.all     ? vm.GetNotes(checkShowArchive.Checked) :
                by == SearchMode.tag     ? vm.SearchByTag(search, checkShowArchive.Checked) :
                by == SearchMode.keyword ? vm.SearchByKeyword(search, checkShowArchive.Checked) :
                by == SearchMode.id      ? new[] { vm.SearchByID(long.Parse(search)) }.OfType<Note>() :
                by == SearchMode.random  ? vm.GetRandomNotes(checkShowArchive.Checked, 10) : [];
        editorMode = EditorMode.read;
        this.search = search;
        searchMode = by;

        UpdateUI();
    }

    protected override bool ProcessCmdKey(ref Message message, Keys keys) {
        switch (keys) {
            case Keys.Control | Keys.S:
                SaveNote(this, EventArgs.Empty);
                return true; // processed
            case Keys.Escape:
                if (vm.CurrentPath != null && editorMode == EditorMode.edit)
                    SetReadMode(search, searchMode);
                return true;
            case Keys.Control | Keys.Shift | Keys.N:
                OnNewButtonClick(this, EventArgs.Empty);
                return true;
            case Keys.Control | Keys.Shift | Keys.S:
                textboxSearch.Focus();
                return true;
            case Keys.Control | Keys.Shift | Keys.Alt | Keys.OemSemicolon: // ";" key, for debug purposes
                vm.IsSponsor = false;
                UpdateUI();
                return true;
        }
        return base.ProcessCmdKey(ref message, keys);
    }

    private enum EditorMode { read, edit }

    private enum SearchMode { all, tag, keyword, id, random }
}

// *) There is a bug in RichTextBox: the "TextChanged" event is NOT fired until RichTextBox gets rendered in the widget tree, at least once.
// Probably the handler is not being registered on OS side (https://stackoverflow.com/a/12330915/2212849).
// So we have to get it visible as soon as possible to register all handlers properly (note that usual Textbox works normally).
