namespace IPka
{
    partial class FormReportStart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReportStart));
            this.buttonLetsGo = new System.Windows.Forms.Button();
            this.comboBoxCorrespondence = new System.Windows.Forms.ComboBox();
            this.monthCalendarDate = new System.Windows.Forms.MonthCalendar();
            this.textBoxComment = new System.Windows.Forms.TextBox();
            this.labelCorrType = new System.Windows.Forms.Label();
            this.labelComment = new System.Windows.Forms.Label();
            this.buttonBack = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonLetsGo
            // 
            this.buttonLetsGo.Location = new System.Drawing.Point(191, 192);
            this.buttonLetsGo.Name = "buttonLetsGo";
            this.buttonLetsGo.Size = new System.Drawing.Size(75, 38);
            this.buttonLetsGo.TabIndex = 5;
            this.buttonLetsGo.Text = "OK";
            this.buttonLetsGo.UseVisualStyleBackColor = true;
            this.buttonLetsGo.Click += new System.EventHandler(this.ButtonLetsGoClick);
            // 
            // comboBoxCorrespondence
            // 
            this.comboBoxCorrespondence.FormattingEnabled = true;
            this.comboBoxCorrespondence.Location = new System.Drawing.Point(12, 30);
            this.comboBoxCorrespondence.Name = "comboBoxCorrespondence";
            this.comboBoxCorrespondence.Size = new System.Drawing.Size(167, 21);
            this.comboBoxCorrespondence.TabIndex = 1;
            this.comboBoxCorrespondence.Validated += new System.EventHandler(this.ComboBoxCorrespondenceValidated);
            // 
            // monthCalendarDate
            // 
            this.monthCalendarDate.Location = new System.Drawing.Point(191, 9);
            this.monthCalendarDate.MaxSelectionCount = 1;
            this.monthCalendarDate.Name = "monthCalendarDate";
            this.monthCalendarDate.ShowToday = false;
            this.monthCalendarDate.TabIndex = 2;
            // 
            // textBoxComment
            // 
            this.textBoxComment.Location = new System.Drawing.Point(12, 79);
            this.textBoxComment.Multiline = true;
            this.textBoxComment.Name = "textBoxComment";
            this.textBoxComment.Size = new System.Drawing.Size(167, 151);
            this.textBoxComment.TabIndex = 4;
            // 
            // labelCorrType
            // 
            this.labelCorrType.AutoSize = true;
            this.labelCorrType.Location = new System.Drawing.Point(9, 9);
            this.labelCorrType.Name = "labelCorrType";
            this.labelCorrType.Size = new System.Drawing.Size(170, 13);
            this.labelCorrType.TabIndex = 0;
            this.labelCorrType.Text = "Выберите тип корреспонденции";
            // 
            // labelComment
            // 
            this.labelComment.AutoSize = true;
            this.labelComment.Location = new System.Drawing.Point(12, 63);
            this.labelComment.Name = "labelComment";
            this.labelComment.Size = new System.Drawing.Size(77, 13);
            this.labelComment.TabIndex = 3;
            this.labelComment.Text = "Комментарий";
            // 
            // buttonBack
            // 
            this.buttonBack.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonBack.Location = new System.Drawing.Point(280, 192);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(75, 38);
            this.buttonBack.TabIndex = 6;
            this.buttonBack.Text = "Отмена";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.ButtonBackClick);
            // 
            // FormReportStart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonBack;
            this.ClientSize = new System.Drawing.Size(364, 242);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.labelComment);
            this.Controls.Add(this.labelCorrType);
            this.Controls.Add(this.textBoxComment);
            this.Controls.Add(this.monthCalendarDate);
            this.Controls.Add(this.comboBoxCorrespondence);
            this.Controls.Add(this.buttonLetsGo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormReportStart";
            this.Opacity = 0.85D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Добавление/изменение корреспонденции";
            this.Load += new System.EventHandler(this.FormReportStartLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonLetsGo;
        private System.Windows.Forms.ComboBox comboBoxCorrespondence;
        private System.Windows.Forms.MonthCalendar monthCalendarDate;
        private System.Windows.Forms.TextBox textBoxComment;
        private System.Windows.Forms.Label labelCorrType;
        private System.Windows.Forms.Label labelComment;
        private System.Windows.Forms.Button buttonBack;
    }
}