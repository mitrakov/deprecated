namespace IPka
{
    partial class FormRequisites
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRequisites));
            this.listBoxPhysical = new System.Windows.Forms.ListBox();
            this.textBoxRename = new System.Windows.Forms.TextBox();
            this.buttonRename = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonQuit = new System.Windows.Forms.Button();
            this.listBoxLegal = new System.Windows.Forms.ListBox();
            this.groupBoxPhysical = new System.Windows.Forms.GroupBox();
            this.groupBoxLegal = new System.Windows.Forms.GroupBox();
            this.groupBoxPhysical.SuspendLayout();
            this.groupBoxLegal.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxPhysical
            // 
            this.listBoxPhysical.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxPhysical.FormattingEnabled = true;
            this.listBoxPhysical.Location = new System.Drawing.Point(2, 15);
            this.listBoxPhysical.Margin = new System.Windows.Forms.Padding(2);
            this.listBoxPhysical.Name = "listBoxPhysical";
            this.listBoxPhysical.Size = new System.Drawing.Size(146, 309);
            this.listBoxPhysical.TabIndex = 1;
            this.listBoxPhysical.SelectedIndexChanged += new System.EventHandler(this.ListBoxPhysicalSelectedIndexChanged);
            this.listBoxPhysical.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ListBoxPhysicalMouseDown);
            this.listBoxPhysical.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ListBoxPhysicalMouseMove);
            this.listBoxPhysical.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ListBoxPhysicalMouseUp);
            // 
            // textBoxRename
            // 
            this.textBoxRename.Location = new System.Drawing.Point(362, 41);
            this.textBoxRename.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxRename.Name = "textBoxRename";
            this.textBoxRename.Size = new System.Drawing.Size(108, 20);
            this.textBoxRename.TabIndex = 4;
            // 
            // buttonRename
            // 
            this.buttonRename.Location = new System.Drawing.Point(362, 63);
            this.buttonRename.Margin = new System.Windows.Forms.Padding(2);
            this.buttonRename.Name = "buttonRename";
            this.buttonRename.Size = new System.Drawing.Size(108, 24);
            this.buttonRename.TabIndex = 5;
            this.buttonRename.Text = "Переименовать";
            this.buttonRename.UseVisualStyleBackColor = true;
            this.buttonRename.Click += new System.EventHandler(this.ButtonRenameClick);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(362, 105);
            this.buttonRemove.Margin = new System.Windows.Forms.Padding(2);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(108, 24);
            this.buttonRemove.TabIndex = 6;
            this.buttonRemove.Text = "Удалить";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.ButtonRemoveClick);
            // 
            // buttonQuit
            // 
            this.buttonQuit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonQuit.Location = new System.Drawing.Point(383, 295);
            this.buttonQuit.Margin = new System.Windows.Forms.Padding(2);
            this.buttonQuit.Name = "buttonQuit";
            this.buttonQuit.Size = new System.Drawing.Size(64, 26);
            this.buttonQuit.TabIndex = 7;
            this.buttonQuit.Text = "Выход";
            this.buttonQuit.UseVisualStyleBackColor = true;
            this.buttonQuit.Click += new System.EventHandler(this.ButtonQuitClick);
            // 
            // listBoxLegal
            // 
            this.listBoxLegal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxLegal.FormattingEnabled = true;
            this.listBoxLegal.Location = new System.Drawing.Point(2, 15);
            this.listBoxLegal.Margin = new System.Windows.Forms.Padding(2);
            this.listBoxLegal.Name = "listBoxLegal";
            this.listBoxLegal.Size = new System.Drawing.Size(146, 309);
            this.listBoxLegal.TabIndex = 3;
            this.listBoxLegal.SelectedIndexChanged += new System.EventHandler(this.ListBoxLegalSelectedIndexChanged);
            this.listBoxLegal.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ListBoxLegalMouseDown);
            this.listBoxLegal.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ListBoxLegalMouseMove);
            this.listBoxLegal.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ListBoxLegalMouseUp);
            // 
            // groupBoxPhysical
            // 
            this.groupBoxPhysical.Controls.Add(this.listBoxPhysical);
            this.groupBoxPhysical.Location = new System.Drawing.Point(9, 10);
            this.groupBoxPhysical.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxPhysical.Name = "groupBoxPhysical";
            this.groupBoxPhysical.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxPhysical.Size = new System.Drawing.Size(150, 325);
            this.groupBoxPhysical.TabIndex = 0;
            this.groupBoxPhysical.TabStop = false;
            this.groupBoxPhysical.Text = "Для физических лиц";
            // 
            // groupBoxLegal
            // 
            this.groupBoxLegal.Controls.Add(this.listBoxLegal);
            this.groupBoxLegal.Location = new System.Drawing.Point(194, 10);
            this.groupBoxLegal.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxLegal.Name = "groupBoxLegal";
            this.groupBoxLegal.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxLegal.Size = new System.Drawing.Size(150, 325);
            this.groupBoxLegal.TabIndex = 2;
            this.groupBoxLegal.TabStop = false;
            this.groupBoxLegal.Text = "Для юридических лиц";
            // 
            // FormRequisites
            // 
            this.AcceptButton = this.buttonRename;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonQuit;
            this.ClientSize = new System.Drawing.Size(490, 339);
            this.Controls.Add(this.groupBoxLegal);
            this.Controls.Add(this.groupBoxPhysical);
            this.Controls.Add(this.buttonQuit);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.buttonRename);
            this.Controls.Add(this.textBoxRename);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRequisites";
            this.Opacity = 0.85D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Изменение словаря реквизитов";
            this.Load += new System.EventHandler(this.FormDictionariesLoad);
            this.groupBoxPhysical.ResumeLayout(false);
            this.groupBoxLegal.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxPhysical;
        private System.Windows.Forms.TextBox textBoxRename;
        private System.Windows.Forms.Button buttonRename;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonQuit;
        private System.Windows.Forms.ListBox listBoxLegal;
        private System.Windows.Forms.GroupBox groupBoxPhysical;
        private System.Windows.Forms.GroupBox groupBoxLegal;
    }
}