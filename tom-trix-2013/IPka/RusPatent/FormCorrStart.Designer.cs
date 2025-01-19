namespace IPka
{
    partial class FormCorrStart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCorrStart));
            this.listBoxCorrespondence = new System.Windows.Forms.ListBox();
            this.buttonChoose = new System.Windows.Forms.Button();
            this.buttonQuit = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBoxCorrespondence
            // 
            this.listBoxCorrespondence.Dock = System.Windows.Forms.DockStyle.Left;
            this.listBoxCorrespondence.FormattingEnabled = true;
            this.listBoxCorrespondence.Location = new System.Drawing.Point(0, 0);
            this.listBoxCorrespondence.Margin = new System.Windows.Forms.Padding(2);
            this.listBoxCorrespondence.Name = "listBoxCorrespondence";
            this.listBoxCorrespondence.Size = new System.Drawing.Size(185, 222);
            this.listBoxCorrespondence.TabIndex = 0;
            this.listBoxCorrespondence.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListBoxCorrespondenceMouseDoubleClick);
            // 
            // buttonChoose
            // 
            this.buttonChoose.Location = new System.Drawing.Point(201, 93);
            this.buttonChoose.Margin = new System.Windows.Forms.Padding(2);
            this.buttonChoose.Name = "buttonChoose";
            this.buttonChoose.Size = new System.Drawing.Size(82, 28);
            this.buttonChoose.TabIndex = 1;
            this.buttonChoose.Text = "Изменить";
            this.buttonChoose.UseVisualStyleBackColor = true;
            this.buttonChoose.Click += new System.EventHandler(this.ButtonChooseClick);
            // 
            // buttonQuit
            // 
            this.buttonQuit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonQuit.Location = new System.Drawing.Point(201, 183);
            this.buttonQuit.Margin = new System.Windows.Forms.Padding(2);
            this.buttonQuit.Name = "buttonQuit";
            this.buttonQuit.Size = new System.Drawing.Size(82, 28);
            this.buttonQuit.TabIndex = 3;
            this.buttonQuit.Text = "Выход";
            this.buttonQuit.UseVisualStyleBackColor = true;
            this.buttonQuit.Click += new System.EventHandler(this.ButtonQuitClick);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(202, 127);
            this.buttonRemove.Margin = new System.Windows.Forms.Padding(2);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(82, 28);
            this.buttonRemove.TabIndex = 2;
            this.buttonRemove.Text = "Удалить";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.ButtonRemoveClick);
            // 
            // FormCorrStart
            // 
            this.AcceptButton = this.buttonChoose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonQuit;
            this.ClientSize = new System.Drawing.Size(294, 222);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.buttonQuit);
            this.Controls.Add(this.buttonChoose);
            this.Controls.Add(this.listBoxCorrespondence);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCorrStart";
            this.Opacity = 0.85D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Выберите тип";
            this.Load += new System.EventHandler(this.FormCorrStartLoad);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxCorrespondence;
        private System.Windows.Forms.Button buttonChoose;
        private System.Windows.Forms.Button buttonQuit;
        private System.Windows.Forms.Button buttonRemove;
    }
}