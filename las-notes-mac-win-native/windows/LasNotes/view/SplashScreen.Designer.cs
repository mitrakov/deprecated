using System.ComponentModel;

namespace LasNotes {
    partial class SplashScreen {
        private IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent() {
            resources = new ComponentResourceManager(typeof(SplashScreen));
            panelFull = new Panel();
            panelTop = new Panel();
            buttonNew = new Button();
            buttonOpen = new Button();
            listbox = new SplashListbox();
            panelTop.SuspendLayout();
            panelFull.SuspendLayout();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.Controls.Add(buttonNew);
            panelTop.Controls.Add(buttonOpen);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(0, 130);
            panelTop.TabIndex = 0;
            // 
            // buttonNew
            // 
            buttonNew.BackColor = Color.GhostWhite;
            buttonNew.FlatStyle = FlatStyle.Flat;
            buttonNew.FlatAppearance.BorderColor = Color.SlateBlue;
            buttonNew.FlatAppearance.BorderSize = 2;
            buttonNew.FlatAppearance.MouseOverBackColor = Color.FromArgb(207, 227, 246);
            buttonNew.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            buttonNew.Location = new Point(58, 22);
            buttonNew.Name = "buttonNew";
            buttonNew.Size = new Size(220, 90);
            buttonNew.Text = resources.GetString("splash-new");
            // 
            // buttonOpen
            // 
            buttonOpen.BackColor = Color.GhostWhite;
            buttonOpen.FlatStyle = FlatStyle.Flat;
            buttonOpen.FlatAppearance.BorderColor = Color.SlateBlue;
            buttonOpen.FlatAppearance.BorderSize = 2;
            buttonOpen.FlatAppearance.MouseOverBackColor = Color.FromArgb(207, 227, 246);
            buttonOpen.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            buttonOpen.Location = new Point(320, 22);
            buttonOpen.Name = "buttonOpen";
            buttonOpen.Size = new Size(220, 90);
            buttonOpen.Text = resources.GetString("splash-open");
            // 
            // listbox
            // 
            listbox.AutoScroll = true;
            listbox.Dock = DockStyle.Fill;
            listbox.Name = "listbox";
            listbox.TabIndex = 1;
            // 
            // panelFull
            // 
            panelFull.BackColor = Color.AliceBlue;
            panelFull.BorderStyle = BorderStyle.FixedSingle;
            panelFull.Controls.Add(listbox);
            panelFull.Controls.Add(panelTop);
            panelFull.Name = "panelFull";
            panelFull.Size = new Size(596, 796);
            // 
            // SplashScreen
            // 
            BackColor = Color.FromArgb(50, 0, 0, 0);
            Controls.Add(panelFull);
            Name = "SplashScreen";
            Size = new Size(600, 800);
            panelTop.ResumeLayout(false);
            panelFull.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ComponentResourceManager resources;
        private Panel panelTop;
        private Button buttonOpen;
        private Button buttonNew;
        private SplashListbox listbox;
        private Panel panelFull;
    }
}
