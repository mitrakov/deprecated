using System.ComponentModel;
using System.Windows.Forms.Integration;
using MdControl;

namespace LasNotes {
    internal partial class MainForm {
        private IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent() {
            components = new Container();
            resources = new ComponentResourceManager(typeof(MainForm));
            panelLeft = new Panel();
            tagsPanel = new TagsView();
            panelTop = new Panel();
            checkShowArchive = new CheckBox();
            textboxSearch = new TextBox();
            buttonNew = new Button();
            imagesNew = new ImageList(components);
            contentPanel = new Panel();
            splashScreen = new SplashScreen();
            editModePanel = new Panel();
            readModePanel = new Panel();
            editSplitPanel = new SplitContainer();
            textboxEdit = new RichTextBox();
            toolbar = new Toolbar(textboxEdit);
            panelBottom = new FlowLayoutPanel();
            labelTags = new Label();
            labelDigest = new Label();
            textboxTags = new TextBox();
            buttonSave = new Button();
            imagesSave = new ImageList(components);
            hintNew = new ToolTip();
            mainMenu = new MenuStrip();
            fileMenuItem = new ToolStripMenuItem();
            openRecentMenuItem = new ToolStripMenuItem();
            newFileMenuItem = new ToolStripMenuItem();
            openMenuItem = new ToolStripMenuItem();
            setPinMenuItem = new ToolStripMenuItem();
            closeFileMenuItem = new ToolStripMenuItem();
            quitMenuItem = new ToolStripMenuItem();
            helpMenuItem = new ToolStripMenuItem();
            aboutMenuItem = new ToolStripMenuItem();
            donateMenuItem = new ToolStripMenuItem();
            donateDoneMenuItem = new ToolStripMenuItem();
            donateMenuTextbox = new ToolStripTextBox();
            showDigestMenuCheckbox = new ToolStripCheckBox();
            panelLeft.SuspendLayout();
            panelTop.SuspendLayout();
            contentPanel.SuspendLayout();
            splashScreen.SuspendLayout();
            editModePanel.SuspendLayout();
            readModePanel.SuspendLayout();
            ((ISupportInitialize)editSplitPanel).BeginInit();
            editSplitPanel.Panel1.SuspendLayout();
            editSplitPanel.Panel2.SuspendLayout();
            editSplitPanel.SuspendLayout();
            toolbar.SuspendLayout();
            panelBottom.SuspendLayout();
            mainMenu.SuspendLayout();
            SuspendLayout();
            // 
            // panelLeft
            // 
            panelLeft.Controls.Add(tagsPanel);
            panelLeft.Controls.Add(panelTop);
            panelLeft.Dock = DockStyle.Left;
            panelLeft.Name = "panelLeft";
            panelLeft.Size = new Size(300, 0);
            // 
            // tagsPanel
            // 
            tagsPanel.Dock = DockStyle.Fill;
            tagsPanel.Name = "tagsPanel";
            // 
            // panelTop
            // 
            panelTop.AutoSize = true;
            panelTop.Controls.Add(checkShowArchive);
            panelTop.Controls.Add(textboxSearch);
            panelTop.Controls.Add(buttonNew);
            panelTop.Dock = DockStyle.Top;
            panelTop.Name = "panelTop";
            // 
            // checkShowArchive
            // 
            checkShowArchive.AutoSize = true;
            checkShowArchive.Location = new Point(80, 40);
            checkShowArchive.Name = "checkShowArchive";
            checkShowArchive.Text = resources.GetString("show-archive");
            checkShowArchive.UseVisualStyleBackColor = true;
            checkShowArchive.CheckedChanged += OnCheckboxShowArchiveChange;
            // 
            // textboxSearch
            // 
            textboxSearch.BorderStyle = BorderStyle.FixedSingle;
            textboxSearch.Location = new Point(80, 3);
            textboxSearch.Name = "textboxSearch";
            textboxSearch.PlaceholderText = resources.GetString("global-search");
            textboxSearch.Size = new Size(210, 28);
            textboxSearch.WordWrap = false;
            textboxSearch.KeyDown += OnTextboxSearchKeyDown;
            // 
            // buttonNew
            // 
            buttonNew.ImageList = imagesNew;
            buttonNew.Location = new Point(3, 2);
            buttonNew.Name = "buttonNew";
            buttonNew.Size = new Size(67, 64);
            buttonNew.Text = resources.GetString("new");
            buttonNew.TextImageRelation = TextImageRelation.ImageAboveText;
            buttonNew.UseVisualStyleBackColor = true;
            buttonNew.Click += OnNewButtonClick;
            // 
            // imagesNew
            // 
            imagesNew.ColorDepth = ColorDepth.Depth32Bit;
            imagesNew.ImageSize = new Size(28, 32);
            imagesNew.TransparentColor = Color.Transparent;
            // 
            // contentPanel
            // 
            contentPanel.Controls.Add(splashScreen);
            contentPanel.Controls.Add(editModePanel);
            contentPanel.Controls.Add(readModePanel);
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.Name = "contentPanel";
            // 
            // splashScreen
            // 
            splashScreen.Name = "splashScreen";
            // 
            // editModePanel
            // 
            editModePanel.Controls.Add(editSplitPanel);
            editModePanel.Controls.Add(panelBottom);
            editModePanel.Controls.Add(toolbar);
            editModePanel.Dock = DockStyle.Fill;
            editModePanel.Name = "editModePanel";
            editModePanel.Visible = false;
            // 
            // readModePanel
            // 
            readModePanel.Controls.Add(wpfHostMulti);
            readModePanel.Controls.Add(labelDigest);
            readModePanel.Dock = DockStyle.Fill;
            readModePanel.Name = "readModePanel";
            readModePanel.Visible = false;
            // 
            // editSplitPanel
            // 
            editSplitPanel.Dock = DockStyle.Fill;
            editSplitPanel.Name = "editSplitPanel";
            editSplitPanel.Panel1.Controls.Add(textboxEdit);
            editSplitPanel.Panel2.Controls.Add(wpfHostSingle);
            editSplitPanel.Size = new Size(777, 653);
            editSplitPanel.SplitterDistance = 388;
            // 
            // textboxEdit
            // 
            textboxEdit.Dock = DockStyle.Fill;
            textboxEdit.Font = new Font("Consolas", 11);
            textboxEdit.Name = "textboxEdit";
            textboxEdit.Text = "";
            textboxEdit.TextChanged += OnTextboxEditChange;
            // 
            // toolbar
            // 
            toolbar.Dock = DockStyle.Top;
            toolbar.Name = "toolbar";
            // 
            // panelBottom
            // 
            panelBottom.AutoSize = true;
            panelBottom.Controls.Add(labelTags);
            panelBottom.Controls.Add(textboxTags);
            panelBottom.Controls.Add(buttonSave);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Name = "panelBottom";
            // 
            // labelTags
            // 
            labelTags.AutoSize = true;
            labelTags.Margin = new(0, 2, 10, 4);
            labelTags.MinimumSize = new(30, 30);
            labelTags.Name = "labelTags";
            labelTags.Text = resources.GetString("tags");
            labelTags.TextAlign = ContentAlignment.MiddleRight;
            // 
            // labelDigest
            // 
            labelDigest.AutoSize = true;
            labelDigest.Dock = DockStyle.Top;
            labelDigest.Font = new Font("Arial", 20);
            labelDigest.ImageAlign = ContentAlignment.MiddleLeft;
            labelDigest.Name = "labelDigest";
            labelDigest.TextAlign = ContentAlignment.MiddleCenter;
            labelDigest.Text = "    " + resources.GetString("digest");
            // 
            // textboxTags
            // 
            textboxTags.AutoSize = true;
            textboxTags.BorderStyle = BorderStyle.FixedSingle;
            textboxTags.Name = "textboxTags";
            textboxTags.PlaceholderText = resources.GetString("tag1-tag2");
            textboxTags.Margin = new(10, 4, 10, 4);
            textboxTags.MinimumSize = new Size(250, 26);
            textboxTags.KeyDown += OnTextboxTagsKeyDown;
            // 
            // buttonSave
            // 
            buttonSave.AutoSize = true;
            buttonSave.ImageList = imagesSave;
            buttonSave.Margin = new(10, 2, 10, 4);
            buttonSave.MinimumSize = new(190, 30);
            buttonSave.Name = "buttonSave";
            buttonSave.Text = resources.GetString("save-note");
            buttonSave.TextAlign = ContentAlignment.MiddleRight;
            buttonSave.TextImageRelation = TextImageRelation.ImageBeforeText;
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Enabled = false;
            buttonSave.Click += SaveNote;
            // 
            // imagesSave
            // 
            imagesSave.ColorDepth = ColorDepth.Depth32Bit;
            imagesSave.ImageSize = new Size(18, 18);
            imagesSave.TransparentColor = Color.Transparent;
            // 
            // hintNew
            // 
            hintNew.AutomaticDelay = 200;
            hintNew.ShowAlways = true;
            hintNew.SetToolTip(buttonNew, "Ctrl+Shift+N");
            // 
            // mainMenu
            // 
            mainMenu.ImageScalingSize = new Size(20, 20);
            mainMenu.Items.AddRange(new ToolStripItem[] { fileMenuItem, helpMenuItem });
            mainMenu.Name = "mainMenu";
            mainMenu.Size = new Size(1006, 28);
            // 
            // fileMenuItem
            // 
            fileMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                openRecentMenuItem, new ToolStripSeparator(),
                newFileMenuItem, openMenuItem, setPinMenuItem, new ToolStripSeparator(),
                closeFileMenuItem, new ToolStripSeparator(),
                quitMenuItem
            });
            fileMenuItem.Name = "fileMenuItem";
            fileMenuItem.Text = resources.GetString("menu-file");
            // 
            // openRecentMenuItem
            // 
            openRecentMenuItem.Name = "openRecentMenuItem";
            openRecentMenuItem.Text = resources.GetString("menu-open-recent");
            // 
            // newFileMenuItem
            // 
            newFileMenuItem.Name = "newFileMenuItem";
            newFileMenuItem.Text = resources.GetString("menu-new");
            newFileMenuItem.ShortcutKeys = Keys.Control | Keys.N;
            newFileMenuItem.Click += OnNewFileClick;
            // 
            // openMenuItem
            // 
            openMenuItem.Name = "openMenuItem";
            openMenuItem.Text = resources.GetString("menu-open");
            openMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            openMenuItem.Click += OnOpenFileClick;
            // 
            // setPinMenuItem
            // 
            setPinMenuItem.Name = "setPinMenuItem";
            setPinMenuItem.Text = resources.GetString("menu-set-pin");
            setPinMenuItem.Click += OnSetPinClick;
            // 
            // closeFileMenuItem
            // 
            closeFileMenuItem.Name = "closeFileMenuItem";
            closeFileMenuItem.Text = resources.GetString("menu-close");
            closeFileMenuItem.ShortcutKeys = Keys.Control | Keys.W;
            closeFileMenuItem.Click += OnCloseFileClick;
            // 
            // quitMenuItem
            // 
            quitMenuItem.Name = "quitMenuItem";
            quitMenuItem.Text = resources.GetString("menu-exit");
            quitMenuItem.ShortcutKeys = Keys.Alt | Keys.F4;
            quitMenuItem.Click += OnQuitClick;
            // 
            // helpMenuItem
            // 
            helpMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                aboutMenuItem, new ToolStripSeparator(),
                donateMenuItem, donateDoneMenuItem, showDigestMenuCheckbox
            });
            helpMenuItem.Name = "helpMenuItem";
            helpMenuItem.Text = resources.GetString("menu-help");
            // 
            // aboutMenuItem
            // 
            aboutMenuItem.Name = "aboutMenuItem";
            aboutMenuItem.Text = resources.GetString("menu-about");
            aboutMenuItem.ShortcutKeys = Keys.F1;
            aboutMenuItem.Click += OnAboutClick;
            // 
            // donateMenuItem
            // 
            donateMenuItem.Name = "donateMenuItem";
            donateMenuItem.Text = resources.GetString("menu-donate");
            donateMenuItem.ShortcutKeys = Keys.F2;
            donateMenuItem.Click += OnDonateClick;
            // 
            // donateDoneMenuItem
            // 
            donateDoneMenuItem.DropDownItems.AddRange(new ToolStripItem[] { donateMenuTextbox });
            donateDoneMenuItem.Name = "donateDoneMenuItem";
            donateDoneMenuItem.Text = resources.GetString("menu-donate-done");
            donateDoneMenuItem.Visible = false;
            // 
            // donateMenuTextbox
            // 
            donateMenuTextbox.Name = "donateMenuTextbox";
            donateMenuTextbox.TextBox.PlaceholderText = resources.GetString("menu-donate-placeholder");
            donateMenuTextbox.Size = new Size(200, 0);
            donateMenuTextbox.Control.KeyPress += OnDonationTextBoxKeyPress;
            // 
            // showDigestMenuCheckbox
            // 
            showDigestMenuCheckbox.Name = "showDigestMenuCheckbox";
            showDigestMenuCheckbox.Text = resources.GetString("menu-show-digest");
            showDigestMenuCheckbox.Visible = false;
            showDigestMenuCheckbox.Click += OnShowDigestClick;
            //
            // MainForm
            // 
            ClientSize = new Size(1000, 700);
            Controls.Add(contentPanel);
            Controls.Add(panelLeft);
            Controls.Add(mainMenu);
            MainMenuStrip = mainMenu;
            Name = "MainForm";
            Text = "Las Notes";
            WindowState = FormWindowState.Maximized;
            Load += OnMainFormLoad;

            panelLeft.ResumeLayout(false);
            panelLeft.PerformLayout();
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            contentPanel.ResumeLayout(false);
            contentPanel.PerformLayout();
            splashScreen.ResumeLayout(false);
            splashScreen.PerformLayout();
            editModePanel.ResumeLayout(false);
            editModePanel.PerformLayout();
            readModePanel.ResumeLayout(false);
            readModePanel.PerformLayout();
            editSplitPanel.Panel2.ResumeLayout(false);
            editSplitPanel.Panel1.ResumeLayout(false);
            ((ISupportInitialize)editSplitPanel).EndInit();
            editSplitPanel.ResumeLayout(false);
            editSplitPanel.PerformLayout();
            toolbar.ResumeLayout(false);
            toolbar.PerformLayout();
            panelBottom.ResumeLayout(false);
            panelBottom.PerformLayout();
            mainMenu.ResumeLayout(false);
            mainMenu.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComponentResourceManager resources;
        private ElementHost wpfHostSingle;
        private ElementHost wpfHostMulti;
        private MarkdownMulti markdownMulti;
        private MarkdownSingle markdownSingle;
        private Panel panelLeft;
        private Panel contentPanel;
        private Panel panelTop;
        private MenuStrip mainMenu;
        private ToolStripMenuItem fileMenuItem;
        private ToolStripMenuItem openRecentMenuItem;
        private ToolStripMenuItem newFileMenuItem;
        private ToolStripMenuItem openMenuItem;
        private ToolStripMenuItem setPinMenuItem;
        private ToolStripMenuItem closeFileMenuItem;
        private ToolStripMenuItem quitMenuItem;
        private ToolStripMenuItem helpMenuItem;
        private ToolStripMenuItem aboutMenuItem;
        private ToolStripMenuItem donateMenuItem;
        private ToolStripMenuItem donateDoneMenuItem;
        private ToolStripTextBox donateMenuTextbox;
        private ToolStripCheckBox showDigestMenuCheckbox;
        private Button buttonNew;
        private CheckBox checkShowArchive;
        private TextBox textboxSearch;
        private ImageList imagesNew;
        private ImageList imagesSave;
        private SplitContainer editSplitPanel;
        private Toolbar toolbar;
        private FlowLayoutPanel panelBottom;
        private TextBox textboxTags;
        private Button buttonSave;
        private RichTextBox textboxEdit;
        private SplashScreen splashScreen;
        private Panel editModePanel;
        private Panel readModePanel;
        private TagsView tagsPanel;
        private Label labelTags;
        private Label labelDigest;
        private ToolTip hintNew;
    }
}
