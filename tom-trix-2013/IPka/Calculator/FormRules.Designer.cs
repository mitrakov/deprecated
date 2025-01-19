namespace Calculator
{
    partial class FormRules
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRules));
            this.listBoxRules = new System.Windows.Forms.ListBox();
            this.labelCaption = new System.Windows.Forms.Label();
            this.textBoxCaption = new System.Windows.Forms.TextBox();
            this.labelColour = new System.Windows.Forms.Label();
            this.buttonColour = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonAddNew = new System.Windows.Forms.Button();
            this.listBoxRulesMapper = new System.Windows.Forms.ListBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.panelUp = new System.Windows.Forms.Panel();
            this.panelCenter = new System.Windows.Forms.Panel();
            this.buttonNewRule = new System.Windows.Forms.Button();
            this.buttonDeleteRule = new System.Windows.Forms.Button();
            this.panelUp.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxRules
            // 
            this.listBoxRules.Dock = System.Windows.Forms.DockStyle.Left;
            this.listBoxRules.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listBoxRules.FormattingEnabled = true;
            this.listBoxRules.ItemHeight = 18;
            this.listBoxRules.Location = new System.Drawing.Point(0, 0);
            this.listBoxRules.Name = "listBoxRules";
            this.listBoxRules.Size = new System.Drawing.Size(285, 372);
            this.listBoxRules.TabIndex = 0;
            this.listBoxRules.SelectedIndexChanged += new System.EventHandler(this.ListBoxRulesSelectedIndexChanged);
            this.listBoxRules.DoubleClick += new System.EventHandler(this.ListBoxRulesDoubleClick);
            this.listBoxRules.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ListBoxRulesMouseDown);
            this.listBoxRules.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ListBoxRulesMouseMove);
            this.listBoxRules.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ListBoxRulesMouseUp);
            // 
            // labelCaption
            // 
            this.labelCaption.AutoSize = true;
            this.labelCaption.Location = new System.Drawing.Point(25, 15);
            this.labelCaption.Name = "labelCaption";
            this.labelCaption.Size = new System.Drawing.Size(41, 13);
            this.labelCaption.TabIndex = 1;
            this.labelCaption.Text = "Статус";
            // 
            // textBoxCaption
            // 
            this.textBoxCaption.Location = new System.Drawing.Point(75, 12);
            this.textBoxCaption.Name = "textBoxCaption";
            this.textBoxCaption.Size = new System.Drawing.Size(278, 20);
            this.textBoxCaption.TabIndex = 2;
            this.textBoxCaption.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxCaptionValidating);
            // 
            // labelColour
            // 
            this.labelColour.AutoSize = true;
            this.labelColour.Location = new System.Drawing.Point(359, 17);
            this.labelColour.Name = "labelColour";
            this.labelColour.Size = new System.Drawing.Size(32, 13);
            this.labelColour.TabIndex = 3;
            this.labelColour.Text = "Цвет";
            // 
            // buttonColour
            // 
            this.buttonColour.BackColor = System.Drawing.Color.Black;
            this.buttonColour.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonColour.Location = new System.Drawing.Point(397, 12);
            this.buttonColour.Name = "buttonColour";
            this.buttonColour.Size = new System.Drawing.Size(28, 23);
            this.buttonColour.TabIndex = 5;
            this.buttonColour.Text = "...";
            this.buttonColour.UseVisualStyleBackColor = false;
            this.buttonColour.Click += new System.EventHandler(this.ButtonColourClick);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(604, 308);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 33);
            this.buttonOK.TabIndex = 6;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.ButtonOkClick);
            // 
            // buttonAddNew
            // 
            this.buttonAddNew.Location = new System.Drawing.Point(447, 6);
            this.buttonAddNew.Name = "buttonAddNew";
            this.buttonAddNew.Size = new System.Drawing.Size(81, 30);
            this.buttonAddNew.TabIndex = 7;
            this.buttonAddNew.Text = "Добавить";
            this.buttonAddNew.UseVisualStyleBackColor = true;
            this.buttonAddNew.Click += new System.EventHandler(this.ButtonAddNewClick);
            // 
            // listBoxRulesMapper
            // 
            this.listBoxRulesMapper.FormattingEnabled = true;
            this.listBoxRulesMapper.Location = new System.Drawing.Point(12, 31);
            this.listBoxRulesMapper.Name = "listBoxRulesMapper";
            this.listBoxRulesMapper.Size = new System.Drawing.Size(26, 17);
            this.listBoxRulesMapper.TabIndex = 8;
            this.listBoxRulesMapper.Visible = false;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(697, 308);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 33);
            this.buttonCancel.TabIndex = 9;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.ButtonCancelClick);
            // 
            // panelUp
            // 
            this.panelUp.Controls.Add(this.buttonAddNew);
            this.panelUp.Controls.Add(this.labelCaption);
            this.panelUp.Controls.Add(this.textBoxCaption);
            this.panelUp.Controls.Add(this.labelColour);
            this.panelUp.Controls.Add(this.buttonColour);
            this.panelUp.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelUp.Location = new System.Drawing.Point(285, 0);
            this.panelUp.Name = "panelUp";
            this.panelUp.Size = new System.Drawing.Size(559, 48);
            this.panelUp.TabIndex = 10;
            // 
            // panelCenter
            // 
            this.panelCenter.AutoScroll = true;
            this.panelCenter.Location = new System.Drawing.Point(285, 12);
            this.panelCenter.Name = "panelCenter";
            this.panelCenter.Size = new System.Drawing.Size(559, 295);
            this.panelCenter.TabIndex = 13;
            // 
            // buttonNewRule
            // 
            this.buttonNewRule.Location = new System.Drawing.Point(234, 313);
            this.buttonNewRule.Name = "buttonNewRule";
            this.buttonNewRule.Size = new System.Drawing.Size(23, 23);
            this.buttonNewRule.TabIndex = 11;
            this.buttonNewRule.Text = "+";
            this.buttonNewRule.UseVisualStyleBackColor = true;
            this.buttonNewRule.Click += new System.EventHandler(this.ButtonNewRuleClick);
            // 
            // buttonDeleteRule
            // 
            this.buttonDeleteRule.Location = new System.Drawing.Point(262, 313);
            this.buttonDeleteRule.Name = "buttonDeleteRule";
            this.buttonDeleteRule.Size = new System.Drawing.Size(23, 23);
            this.buttonDeleteRule.TabIndex = 12;
            this.buttonDeleteRule.Text = "–";
            this.buttonDeleteRule.UseVisualStyleBackColor = true;
            this.buttonDeleteRule.Click += new System.EventHandler(this.ButtonDeleteRuleClick);
            // 
            // FormRules
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(844, 372);
            this.ControlBox = false;
            this.Controls.Add(this.buttonDeleteRule);
            this.Controls.Add(this.buttonNewRule);
            this.Controls.Add(this.panelUp);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.listBoxRules);
            this.Controls.Add(this.listBoxRulesMapper);
            this.Controls.Add(this.panelCenter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 199);
            this.Name = "FormRules";
            this.Opacity = 0.95D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Редактор правил";
            this.Load += new System.EventHandler(this.FormRulesLoad);
            this.panelUp.ResumeLayout(false);
            this.panelUp.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxRules;
        private System.Windows.Forms.Label labelCaption;
        private System.Windows.Forms.TextBox textBoxCaption;
        private System.Windows.Forms.Label labelColour;
        private System.Windows.Forms.Button buttonColour;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonAddNew;
        private System.Windows.Forms.ListBox listBoxRulesMapper;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Panel panelUp;
        private System.Windows.Forms.Button buttonNewRule;
        private System.Windows.Forms.Button buttonDeleteRule;
        private System.Windows.Forms.Panel panelCenter;
    }
}