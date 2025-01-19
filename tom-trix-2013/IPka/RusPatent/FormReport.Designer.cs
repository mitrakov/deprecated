namespace IPka
{
    partial class FormReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReport));
            this.panelMain = new System.Windows.Forms.Panel();
            this.buttonBuild = new System.Windows.Forms.Button();
            this.buttonQuit = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.buttonOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.AutoScroll = true;
            this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Margin = new System.Windows.Forms.Padding(2);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(542, 472);
            this.panelMain.TabIndex = 0;
            // 
            // buttonBuild
            // 
            this.buttonBuild.Location = new System.Drawing.Point(560, 302);
            this.buttonBuild.Margin = new System.Windows.Forms.Padding(2);
            this.buttonBuild.Name = "buttonBuild";
            this.buttonBuild.Size = new System.Drawing.Size(94, 34);
            this.buttonBuild.TabIndex = 6;
            this.buttonBuild.Text = "Предпросмотр";
            this.buttonBuild.UseVisualStyleBackColor = true;
            this.buttonBuild.Click += new System.EventHandler(this.ButtonBuildClick);
            // 
            // buttonQuit
            // 
            this.buttonQuit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonQuit.Location = new System.Drawing.Point(560, 427);
            this.buttonQuit.Margin = new System.Windows.Forms.Padding(2);
            this.buttonQuit.Name = "buttonQuit";
            this.buttonQuit.Size = new System.Drawing.Size(94, 34);
            this.buttonQuit.TabIndex = 7;
            this.buttonQuit.Text = "Выход";
            this.buttonQuit.UseVisualStyleBackColor = true;
            this.buttonQuit.Click += new System.EventHandler(this.ButtonQuitClick);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Документы Word 2003-2010|*.doc; *.docx; *.docm; *.dot; *.dotx; *.dotm|Все файлы| " +
                "*.*";
            this.openFileDialog1.Title = "Открытие документа или шаблона Word";
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(560, 365);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(94, 34);
            this.buttonOk.TabIndex = 8;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.ButtonOkClick);
            // 
            // FormReport
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonQuit;
            this.ClientSize = new System.Drawing.Size(674, 472);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonQuit);
            this.Controls.Add(this.buttonBuild);
            this.Controls.Add(this.panelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Генератор отчётов";
            this.Load += new System.EventHandler(this.FormReportLoad);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Button buttonBuild;
        private System.Windows.Forms.Button buttonQuit;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button buttonOk;
    }
}