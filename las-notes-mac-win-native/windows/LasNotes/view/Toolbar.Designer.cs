using System.ComponentModel;

namespace LasNotes {
    partial class Toolbar {
        private IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent() {
            resources = new ComponentResourceManager(typeof(Toolbar));
            panelLeft = new FlowLayoutPanel();
            panelRight = new FlowLayoutPanel();

            btnH1 = new Button();
            btnH2 = new Button();
            btnH3 = new Button();
            btnBold = new Button();
            btnItalic = new Button();
            btnUnderline = new Button();
            btnStrike = new Button();
            btnTicks = new Button();
            btnBulletList = new Button();
            btnNumList = new Button();
            btnQuote = new Button();
            btnLink = new Button();
            btnCode = new Button();
            btnRule = new Button();
            btnTable = new Button();
            btnImage = new Button();
            btnMore = new Button();
            btnDonate = new Button();

            hintH1 = new ToolTip();
            hintH2 = new ToolTip();
            hintH3 = new ToolTip();
            hintBold = new ToolTip();
            hintItalic = new ToolTip();
            hintUnderline = new ToolTip();
            hintStrike = new ToolTip();
            hintTicks = new ToolTip();
            hintBulletList = new ToolTip();
            hintNumList = new ToolTip();
            hintQuote = new ToolTip();
            hintLink = new ToolTip();
            hintCode = new ToolTip();
            hintRule = new ToolTip();
            hintTable = new ToolTip();
            hintImage = new ToolTip();
            hintMore = new ToolTip();
            hintDonate = new ToolTip();

            panelLeft.SuspendLayout();
            panelRight.SuspendLayout();
            SuspendLayout();
            // 
            // btnH1
            // 
            btnH1.FlatStyle = FlatStyle.Flat;
            btnH1.Name = "btnH1";
            btnH1.Size = new Size(38, 32);
            btnH1.Text = "H1";
            btnH1.Click += BtnH1_Click;
            // 
            // btnH2
            // 
            btnH2.FlatStyle = FlatStyle.Flat;
            btnH2.Name = "btnH2";
            btnH2.Size = new Size(38, 32);
            btnH2.Text = "h2";
            btnH2.Click += BtnH2_Click;
            // 
            // btnH3
            // 
            btnH3.FlatStyle = FlatStyle.Flat;
            btnH3.Font = new Font("Segoe UI", 7);
            btnH3.Name = "btnH3";
            btnH3.Padding = new Padding(0, 4, 0, 0);
            btnH3.Size = new Size(38, 32);
            btnH3.Text = "h3";
            btnH3.Click += BtnH3_Click;
            // 
            // btnBold
            // 
            btnBold.FlatStyle = FlatStyle.Flat;
            btnBold.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnBold.Name = "btnBold";
            btnBold.Size = new Size(38, 32);
            btnBold.Text = "B";
            btnBold.Click += BtnBold_Click;
            // 
            // btnItalic
            // 
            btnItalic.FlatStyle = FlatStyle.Flat;
            btnItalic.Font = new Font("Segoe UI", 9, FontStyle.Italic);
            btnItalic.Name = "btnItalic";
            btnItalic.Size = new Size(38, 32);
            btnItalic.Text = "I";
            btnItalic.Click += BtnItalic_Click;
            // 
            // btnUnderline
            // 
            btnUnderline.FlatStyle = FlatStyle.Flat;
            btnUnderline.Name = "btnUnderline";
            btnUnderline.Size = new Size(38, 32);
            btnUnderline.Text = "U̲";
            btnUnderline.Click += BtnUnderline_Click;
            // 
            // btnStrike
            // 
            btnStrike.FlatStyle = FlatStyle.Flat;
            btnStrike.Name = "btnStrike";
            btnStrike.Size = new Size(38, 32);
            btnStrike.Text = "Ꞩ";
            btnStrike.Click += BtnStrike_Click;
            // 
            // btnTicks
            // 
            btnTicks.FlatStyle = FlatStyle.Flat;
            btnTicks.Name = "btnTicks";
            btnTicks.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnTicks.Size = new Size(38, 32);
            btnTicks.Text = "``";
            btnTicks.Click += BtnTicks_Click;
            // 
            // btnBulletList
            // 
            btnBulletList.FlatStyle = FlatStyle.Flat;
            btnBulletList.Font = new Font("Segoe UI", 10);
            btnBulletList.Name = "btnBulletList";
            btnBulletList.Size = new Size(38, 32);
            btnBulletList.Text = "⋮";
            btnBulletList.Click += BtnBulletList_Click;
            // 
            // btnNumList
            // 
            btnNumList.FlatStyle = FlatStyle.Flat;
            btnNumList.Name = "btnNumList";
            btnNumList.Size = new Size(38, 32);
            btnNumList.Text = "1.";
            btnNumList.Click += BtnNumList_Click;
            // 
            // btnQuote
            // 
            btnQuote.FlatStyle = FlatStyle.Flat;
            btnQuote.Name = "btnQuote";
            btnQuote.Size = new Size(38, 32);
            btnQuote.Text = "❝❞";
            btnQuote.Click += BtnQuote_Click;
            // 
            // btnLink
            // 
            btnLink.FlatStyle = FlatStyle.Flat;
            btnLink.Name = "btnLink";
            btnLink.Size = new Size(38, 32);
            btnLink.Text = "🔗";
            btnLink.Click += BtnLink_Click;
            // 
            // btnCode
            // 
            btnCode.FlatStyle = FlatStyle.Flat;
            btnCode.Font = new Font("Segoe UI", 7, FontStyle.Bold);
            btnCode.Name = "btnCode";
            btnCode.Size = new Size(38, 32);
            btnCode.Text = "</>";
            btnCode.Click += BtnCode_Click;
            // 
            // btnRule
            // 
            btnRule.FlatStyle = FlatStyle.Flat;
            btnRule.Name = "btnRule";
            btnRule.Size = new Size(38, 32);
            btnRule.Text = "⸺";
            btnRule.Click += BtnRule_Click;
            // 
            // btnTable
            // 
            btnTable.FlatStyle = FlatStyle.Flat;
            btnTable.Font = new Font("Segoe UI", 11);
            btnTable.Name = "btnTable";
            btnTable.Size = new Size(38, 32);
            btnTable.Text = "▦"; // differs from MacOS version
            btnTable.Click += BtnTable_Click;
            // 
            // btnImage
            // 
            btnImage.FlatStyle = FlatStyle.Flat;
            btnImage.Name = "btnImage";
            btnImage.Size = new Size(38, 32);
            btnImage.Text = "🖼";
            btnImage.Click += BtnImage_Click;
            // 
            // btnMore
            // 
            btnMore.FlatStyle = FlatStyle.Flat;
            btnMore.Name = "btnMore";
            btnMore.Size = new Size(38, 32);
            btnMore.Text = "❓";
            btnMore.Click += BtnMore_Click;
            // 
            // btnDonate
            // 
            btnDonate.FlatAppearance.BorderSize = 0;
            btnDonate.FlatStyle = FlatStyle.Flat;
            btnDonate.Name = "btnDonate";
            btnDonate.Size = new Size(38, 32);
            btnDonate.Click += BtnDonate_Click;
            // 
            // hintH1
            // 
            hintH1.SetToolTip(btnH1, resources.GetString("toolbar-h1"));
            hintH1.AutomaticDelay = 200;
            hintH1.ShowAlways = true;
            // 
            // hintH2
            // 
            hintH2.SetToolTip(btnH2, resources.GetString("toolbar-h2"));
            hintH2.AutomaticDelay = 200;
            hintH2.ShowAlways = true;
            // 
            // hintH3
            // 
            hintH3.SetToolTip(btnH3, resources.GetString("toolbar-h3"));
            hintH3.AutomaticDelay = 200;
            hintH3.ShowAlways = true;
            // 
            // hintBold
            // 
            hintBold.SetToolTip(btnBold, resources.GetString("toolbar-bold"));
            hintBold.AutomaticDelay = 200;
            hintBold.ShowAlways = true;
            // 
            // hintItalic
            // 
            hintItalic.SetToolTip(btnItalic, resources.GetString("toolbar-italic"));
            hintItalic.AutomaticDelay = 200;
            hintItalic.ShowAlways = true;
            // 
            // hintUnderline
            // 
            hintUnderline.SetToolTip(btnUnderline, resources.GetString("toolbar-underline"));
            hintUnderline.AutomaticDelay = 200;
            hintUnderline.ShowAlways = true;
            // 
            // hintStrike
            // 
            hintStrike.SetToolTip(btnStrike, resources.GetString("toolbar-strike"));
            hintStrike.AutomaticDelay = 200;
            hintStrike.ShowAlways = true;
            // 
            // hintTicks
            // 
            hintTicks.SetToolTip(btnTicks, resources.GetString("toolbar-ticks"));
            hintTicks.AutomaticDelay = 200;
            hintTicks.ShowAlways = true;
            // 
            // hintBulletList
            // 
            hintBulletList.SetToolTip(btnBulletList, resources.GetString("toolbar-bullet-list"));
            hintBulletList.AutomaticDelay = 200;
            hintBulletList.ShowAlways = true;
            // 
            // hintNumList
            // 
            hintNumList.SetToolTip(btnNumList, resources.GetString("toolbar-num-list"));
            hintNumList.AutomaticDelay = 200;
            hintNumList.ShowAlways = true;
            // 
            // hintQuote
            // 
            hintQuote.SetToolTip(btnQuote, resources.GetString("toolbar-quote"));
            hintQuote.AutomaticDelay = 200;
            hintQuote.ShowAlways = true;
            // 
            // hintLink
            // 
            hintLink.SetToolTip(btnLink, resources.GetString("toolbar-link"));
            hintLink.AutomaticDelay = 200;
            hintLink.ShowAlways = true;
            // 
            // hintCode
            // 
            hintCode.SetToolTip(btnCode, resources.GetString("toolbar-code"));
            hintCode.AutomaticDelay = 200;
            hintCode.ShowAlways = true;
            // 
            // hintRule
            // 
            hintRule.SetToolTip(btnRule, resources.GetString("toolbar-rule"));
            hintRule.AutomaticDelay = 200;
            hintRule.ShowAlways = true;
            // 
            // hintTable
            // 
            hintTable.SetToolTip(btnTable, resources.GetString("toolbar-table"));
            hintTable.AutomaticDelay = 200;
            hintTable.ShowAlways = true;
            // 
            // hintImage
            // 
            hintImage.SetToolTip(btnImage, resources.GetString("toolbar-image"));
            hintImage.AutomaticDelay = 200;
            hintImage.ShowAlways = true;
            // 
            // hintMore
            // 
            hintMore.SetToolTip(btnMore, resources.GetString("toolbar-more"));
            hintMore.AutomaticDelay = 200;
            hintMore.ShowAlways = true;
            // 
            // hintDonate
            // 
            hintDonate.SetToolTip(btnDonate, resources.GetString("toolbar-donate"));
            hintDonate.AutomaticDelay = 200;
            hintDonate.ShowAlways = true;
            // 
            // panelLeft
            // 
            panelLeft.AutoSize = true;
            panelLeft.Dock = DockStyle.Fill;
            panelLeft.Controls.Add(btnH1);
            panelLeft.Controls.Add(btnH2);
            panelLeft.Controls.Add(btnH3);
            panelLeft.Controls.Add(btnBold);
            panelLeft.Controls.Add(btnItalic);
            panelLeft.Controls.Add(btnUnderline);
            panelLeft.Controls.Add(btnStrike);
            panelLeft.Controls.Add(btnTicks);
            panelLeft.Controls.Add(btnBulletList);
            panelLeft.Controls.Add(btnNumList);
            panelLeft.Controls.Add(btnQuote);
            panelLeft.Controls.Add(btnLink);
            panelLeft.Controls.Add(btnCode);
            panelLeft.Controls.Add(btnRule);
            panelLeft.Controls.Add(btnTable);
            panelLeft.Controls.Add(btnImage);
            panelLeft.Controls.Add(btnMore);
            panelLeft.Name = "panelLeft";
            // 
            // panelRight
            // 
            panelRight.AutoSize = true;
            panelRight.Dock = DockStyle.Right;
            panelRight.Controls.Add(btnDonate);
            panelRight.Name = "panelRight";
            // 
            // Toolbar
            // 
            Controls.Add(panelRight);
            Controls.Add(panelLeft);
            AutoSize = true;
            BackColor = Color.LightGray;
            BorderStyle = BorderStyle.FixedSingle;
            Name = "Toolbar";
            panelLeft.ResumeLayout(true);
            panelRight.ResumeLayout(true);
            ResumeLayout(true);
        }

        #endregion

        private ComponentResourceManager resources;
        private FlowLayoutPanel panelLeft;
        private FlowLayoutPanel panelRight;

        private Button btnH1;
        private Button btnH2;
        private Button btnH3;
        private Button btnBold;
        private Button btnItalic;
        private Button btnUnderline;
        private Button btnStrike;
        private Button btnTicks;
        private Button btnBulletList;
        private Button btnNumList;
        private Button btnQuote;
        private Button btnLink;
        private Button btnCode;
        private Button btnRule;
        private Button btnTable;
        private Button btnImage;
        private Button btnMore;
        private Button btnDonate;

        private ToolTip hintH1;
        private ToolTip hintH2;
        private ToolTip hintH3;
        private ToolTip hintBold;
        private ToolTip hintItalic;
        private ToolTip hintUnderline;
        private ToolTip hintStrike;
        private ToolTip hintTicks;
        private ToolTip hintBulletList;
        private ToolTip hintNumList;
        private ToolTip hintQuote;
        private ToolTip hintLink;
        private ToolTip hintCode;
        private ToolTip hintRule;
        private ToolTip hintTable;
        private ToolTip hintImage;
        private ToolTip hintMore;
        private ToolTip hintDonate;
    }
}
