namespace IPka
{
    partial class FormProjects
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProjects));
            this.listBoxMain = new System.Windows.Forms.ListBox();
            this.textBoxType = new System.Windows.Forms.TextBox();
            this.buttonRename = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonQuit = new System.Windows.Forms.Button();
            this.textBoxCode = new System.Windows.Forms.TextBox();
            this.labelType = new System.Windows.Forms.Label();
            this.labelCode = new System.Windows.Forms.Label();
            this.listBoxHelpUserCodes = new System.Windows.Forms.ListBox();
            this.labelShortname = new System.Windows.Forms.Label();
            this.textBoxShortname = new System.Windows.Forms.TextBox();
            this.listBoxHelpShortnames = new System.Windows.Forms.ListBox();
            this.checkBoxShowInCalc = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // listBoxMain
            // 
            this.listBoxMain.Dock = System.Windows.Forms.DockStyle.Left;
            this.listBoxMain.FormattingEnabled = true;
            this.listBoxMain.Location = new System.Drawing.Point(0, 0);
            this.listBoxMain.Margin = new System.Windows.Forms.Padding(2);
            this.listBoxMain.Name = "listBoxMain";
            this.listBoxMain.Size = new System.Drawing.Size(152, 305);
            this.listBoxMain.TabIndex = 0;
            this.listBoxMain.SelectedIndexChanged += new System.EventHandler(this.ListBoxMainSelectedIndexChanged);
            // 
            // textBoxType
            // 
            this.textBoxType.Location = new System.Drawing.Point(167, 23);
            this.textBoxType.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxType.Name = "textBoxType";
            this.textBoxType.Size = new System.Drawing.Size(108, 20);
            this.textBoxType.TabIndex = 2;
            // 
            // buttonRename
            // 
            this.buttonRename.Location = new System.Drawing.Point(167, 183);
            this.buttonRename.Margin = new System.Windows.Forms.Padding(2);
            this.buttonRename.Name = "buttonRename";
            this.buttonRename.Size = new System.Drawing.Size(107, 24);
            this.buttonRename.TabIndex = 5;
            this.buttonRename.Text = "Переименовать";
            this.buttonRename.UseVisualStyleBackColor = true;
            this.buttonRename.Click += new System.EventHandler(this.ButtonRenameClick);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(167, 220);
            this.buttonRemove.Margin = new System.Windows.Forms.Padding(2);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(107, 24);
            this.buttonRemove.TabIndex = 6;
            this.buttonRemove.Text = "Удалить";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.ButtonRemoveClick);
            // 
            // buttonQuit
            // 
            this.buttonQuit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonQuit.Location = new System.Drawing.Point(189, 266);
            this.buttonQuit.Margin = new System.Windows.Forms.Padding(2);
            this.buttonQuit.Name = "buttonQuit";
            this.buttonQuit.Size = new System.Drawing.Size(64, 26);
            this.buttonQuit.TabIndex = 7;
            this.buttonQuit.Text = "Выход";
            this.buttonQuit.UseVisualStyleBackColor = true;
            this.buttonQuit.Click += new System.EventHandler(this.ButtonQuitClick);
            // 
            // textBoxCode
            // 
            this.textBoxCode.Location = new System.Drawing.Point(189, 67);
            this.textBoxCode.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxCode.Name = "textBoxCode";
            this.textBoxCode.Size = new System.Drawing.Size(65, 20);
            this.textBoxCode.TabIndex = 4;
            // 
            // labelType
            // 
            this.labelType.AutoSize = true;
            this.labelType.Location = new System.Drawing.Point(184, 7);
            this.labelType.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(70, 13);
            this.labelType.TabIndex = 1;
            this.labelType.Text = "Тип проекта";
            // 
            // labelCode
            // 
            this.labelCode.AutoSize = true;
            this.labelCode.Location = new System.Drawing.Point(173, 50);
            this.labelCode.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(96, 13);
            this.labelCode.TabIndex = 3;
            this.labelCode.Text = "Код типа проекта";
            // 
            // listBoxHelpUserCodes
            // 
            this.listBoxHelpUserCodes.FormattingEnabled = true;
            this.listBoxHelpUserCodes.Location = new System.Drawing.Point(259, 248);
            this.listBoxHelpUserCodes.Margin = new System.Windows.Forms.Padding(2);
            this.listBoxHelpUserCodes.Name = "listBoxHelpUserCodes";
            this.listBoxHelpUserCodes.Size = new System.Drawing.Size(16, 17);
            this.listBoxHelpUserCodes.TabIndex = 8;
            this.listBoxHelpUserCodes.Visible = false;
            // 
            // labelShortname
            // 
            this.labelShortname.AutoSize = true;
            this.labelShortname.Location = new System.Drawing.Point(186, 93);
            this.labelShortname.Name = "labelShortname";
            this.labelShortname.Size = new System.Drawing.Size(71, 13);
            this.labelShortname.TabIndex = 9;
            this.labelShortname.Text = "Сокращение";
            // 
            // textBoxShortname
            // 
            this.textBoxShortname.Location = new System.Drawing.Point(189, 109);
            this.textBoxShortname.Name = "textBoxShortname";
            this.textBoxShortname.Size = new System.Drawing.Size(65, 20);
            this.textBoxShortname.TabIndex = 10;
            // 
            // listBoxHelpShortnames
            // 
            this.listBoxHelpShortnames.FormattingEnabled = true;
            this.listBoxHelpShortnames.Location = new System.Drawing.Point(259, 270);
            this.listBoxHelpShortnames.Name = "listBoxHelpShortnames";
            this.listBoxHelpShortnames.Size = new System.Drawing.Size(15, 17);
            this.listBoxHelpShortnames.TabIndex = 11;
            this.listBoxHelpShortnames.Visible = false;
            // 
            // checkBoxShowInCalc
            // 
            this.checkBoxShowInCalc.AutoSize = true;
            this.checkBoxShowInCalc.Location = new System.Drawing.Point(177, 149);
            this.checkBoxShowInCalc.Name = "checkBoxShowInCalc";
            this.checkBoxShowInCalc.Size = new System.Drawing.Size(108, 17);
            this.checkBoxShowInCalc.TabIndex = 12;
            this.checkBoxShowInCalc.Text = "Показать в Calc";
            this.checkBoxShowInCalc.UseVisualStyleBackColor = true;
            this.checkBoxShowInCalc.CheckedChanged += new System.EventHandler(this.CheckBoxShowInCalcCheckedChanged);
            // 
            // FormProjects
            // 
            this.AcceptButton = this.buttonRename;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonQuit;
            this.ClientSize = new System.Drawing.Size(294, 305);
            this.Controls.Add(this.checkBoxShowInCalc);
            this.Controls.Add(this.listBoxHelpShortnames);
            this.Controls.Add(this.textBoxShortname);
            this.Controls.Add(this.labelShortname);
            this.Controls.Add(this.listBoxHelpUserCodes);
            this.Controls.Add(this.labelCode);
            this.Controls.Add(this.labelType);
            this.Controls.Add(this.textBoxCode);
            this.Controls.Add(this.buttonQuit);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.buttonRename);
            this.Controls.Add(this.textBoxType);
            this.Controls.Add(this.listBoxMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormProjects";
            this.Opacity = 0.85D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Изменение словаря типов проекта";
            this.Load += new System.EventHandler(this.FormDictionariesLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxMain;
        private System.Windows.Forms.TextBox textBoxType;
        private System.Windows.Forms.Button buttonRename;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonQuit;
        private System.Windows.Forms.TextBox textBoxCode;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.Label labelCode;
        private System.Windows.Forms.ListBox listBoxHelpUserCodes;
        private System.Windows.Forms.Label labelShortname;
        private System.Windows.Forms.TextBox textBoxShortname;
        private System.Windows.Forms.ListBox listBoxHelpShortnames;
        private System.Windows.Forms.CheckBox checkBoxShowInCalc;
    }
}