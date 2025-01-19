namespace IPka
{
    partial class FormCorrespondence
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCorrespondence));
            this.labelType = new System.Windows.Forms.Label();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.checkBoxResponse = new System.Windows.Forms.CheckBox();
            this.numericUpDownResponse = new System.Windows.Forms.NumericUpDown();
            this.labelShortName = new System.Windows.Forms.Label();
            this.textBoxShortName = new System.Windows.Forms.TextBox();
            this.checkBoxOnly = new System.Windows.Forms.CheckBox();
            this.radioButtonIncoming = new System.Windows.Forms.RadioButton();
            this.radioButtonOutcoming = new System.Windows.Forms.RadioButton();
            this.buttonExplore = new System.Windows.Forms.Button();
            this.comboBoxTemplates = new System.Windows.Forms.ComboBox();
            this.labelTemplate = new System.Windows.Forms.Label();
            this.groupBoxResponse = new System.Windows.Forms.GroupBox();
            this.groupBoxTemplates = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelResponse = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownResponse)).BeginInit();
            this.groupBoxResponse.SuspendLayout();
            this.groupBoxTemplates.SuspendLayout();
            this.tableLayoutPanelResponse.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelType
            // 
            this.labelType.AutoSize = true;
            this.labelType.Location = new System.Drawing.Point(43, 55);
            this.labelType.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(63, 13);
            this.labelType.TabIndex = 12;
            this.labelType.Text = "Тип ответа";
            // 
            // comboBoxType
            // 
            this.comboBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Location = new System.Drawing.Point(10, 70);
            this.comboBoxType.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(123, 21);
            this.comboBoxType.TabIndex = 13;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(234, 270);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(2);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(72, 27);
            this.buttonOK.TabIndex = 20;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.ButtonOkClick);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(330, 270);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(72, 27);
            this.buttonCancel.TabIndex = 21;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.ButtonCancelClick);
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(25, 17);
            this.labelName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(148, 13);
            this.labelName.TabIndex = 1;
            this.labelName.Text = "Имя типа корреспонденции";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(25, 36);
            this.textBoxName.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(148, 20);
            this.textBoxName.TabIndex = 3;
            // 
            // checkBoxResponse
            // 
            this.checkBoxResponse.AutoSize = true;
            this.checkBoxResponse.Checked = true;
            this.checkBoxResponse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxResponse.Location = new System.Drawing.Point(88, 97);
            this.checkBoxResponse.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxResponse.Name = "checkBoxResponse";
            this.checkBoxResponse.Size = new System.Drawing.Size(195, 17);
            this.checkBoxResponse.TabIndex = 8;
            this.checkBoxResponse.Text = "Установить крайний срок ответа";
            this.checkBoxResponse.UseVisualStyleBackColor = true;
            this.checkBoxResponse.CheckedChanged += new System.EventHandler(this.CheckBoxResponseCheckedChanged);
            // 
            // numericUpDownResponse
            // 
            this.numericUpDownResponse.Location = new System.Drawing.Point(46, 21);
            this.numericUpDownResponse.Margin = new System.Windows.Forms.Padding(2);
            this.numericUpDownResponse.Maximum = new decimal(new int[] {
            36,
            0,
            0,
            0});
            this.numericUpDownResponse.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownResponse.Name = "numericUpDownResponse";
            this.numericUpDownResponse.Size = new System.Drawing.Size(55, 20);
            this.numericUpDownResponse.TabIndex = 11;
            this.numericUpDownResponse.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelShortName
            // 
            this.labelShortName.AutoSize = true;
            this.labelShortName.Location = new System.Drawing.Point(220, 17);
            this.labelShortName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelShortName.Name = "labelShortName";
            this.labelShortName.Size = new System.Drawing.Size(71, 13);
            this.labelShortName.TabIndex = 2;
            this.labelShortName.Text = "Сокращение";
            // 
            // textBoxShortName
            // 
            this.textBoxShortName.Location = new System.Drawing.Point(223, 36);
            this.textBoxShortName.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxShortName.Name = "textBoxShortName";
            this.textBoxShortName.Size = new System.Drawing.Size(60, 20);
            this.textBoxShortName.TabIndex = 4;
            // 
            // checkBoxOnly
            // 
            this.checkBoxOnly.AutoSize = true;
            this.checkBoxOnly.Checked = true;
            this.checkBoxOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxOnly.Location = new System.Drawing.Point(88, 75);
            this.checkBoxOnly.Name = "checkBoxOnly";
            this.checkBoxOnly.Size = new System.Drawing.Size(242, 17);
            this.checkBoxOnly.TabIndex = 7;
            this.checkBoxOnly.Text = "Является единственной для одной заявки";
            this.checkBoxOnly.UseVisualStyleBackColor = true;
            // 
            // radioButtonIncoming
            // 
            this.radioButtonIncoming.AutoSize = true;
            this.radioButtonIncoming.Checked = true;
            this.radioButtonIncoming.Location = new System.Drawing.Point(320, 16);
            this.radioButtonIncoming.Name = "radioButtonIncoming";
            this.radioButtonIncoming.Size = new System.Drawing.Size(76, 17);
            this.radioButtonIncoming.TabIndex = 5;
            this.radioButtonIncoming.TabStop = true;
            this.radioButtonIncoming.Text = "Входящая";
            this.radioButtonIncoming.UseVisualStyleBackColor = true;
            this.radioButtonIncoming.CheckedChanged += new System.EventHandler(this.RadioButtonIncomingCheckedChanged);
            // 
            // radioButtonOutcoming
            // 
            this.radioButtonOutcoming.AutoSize = true;
            this.radioButtonOutcoming.Location = new System.Drawing.Point(320, 39);
            this.radioButtonOutcoming.Name = "radioButtonOutcoming";
            this.radioButtonOutcoming.Size = new System.Drawing.Size(83, 17);
            this.radioButtonOutcoming.TabIndex = 6;
            this.radioButtonOutcoming.Text = "Исходящая";
            this.radioButtonOutcoming.UseVisualStyleBackColor = true;
            // 
            // buttonExplore
            // 
            this.buttonExplore.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonExplore.Location = new System.Drawing.Point(177, 55);
            this.buttonExplore.Margin = new System.Windows.Forms.Padding(2);
            this.buttonExplore.Name = "buttonExplore";
            this.buttonExplore.Size = new System.Drawing.Size(28, 27);
            this.buttonExplore.TabIndex = 19;
            this.buttonExplore.Text = "...";
            this.buttonExplore.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonExplore.UseVisualStyleBackColor = true;
            this.buttonExplore.Click += new System.EventHandler(this.ButtonExploreClick);
            // 
            // comboBoxTemplates
            // 
            this.comboBoxTemplates.FormattingEnabled = true;
            this.comboBoxTemplates.Location = new System.Drawing.Point(8, 61);
            this.comboBoxTemplates.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxTemplates.Name = "comboBoxTemplates";
            this.comboBoxTemplates.Size = new System.Drawing.Size(150, 21);
            this.comboBoxTemplates.TabIndex = 18;
            // 
            // labelTemplate
            // 
            this.labelTemplate.AutoSize = true;
            this.labelTemplate.Location = new System.Drawing.Point(8, 40);
            this.labelTemplate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelTemplate.Name = "labelTemplate";
            this.labelTemplate.Size = new System.Drawing.Size(182, 13);
            this.labelTemplate.TabIndex = 17;
            this.labelTemplate.Text = "Имя шаблона (из папки Templates)";
            // 
            // groupBoxResponse
            // 
            this.groupBoxResponse.Controls.Add(this.numericUpDownResponse);
            this.groupBoxResponse.Controls.Add(this.labelType);
            this.groupBoxResponse.Controls.Add(this.comboBoxType);
            this.groupBoxResponse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxResponse.Location = new System.Drawing.Point(3, 3);
            this.groupBoxResponse.Name = "groupBoxResponse";
            this.groupBoxResponse.Size = new System.Drawing.Size(169, 111);
            this.groupBoxResponse.TabIndex = 10;
            this.groupBoxResponse.TabStop = false;
            this.groupBoxResponse.Text = "Введите срок на ответ (мес.)";
            // 
            // groupBoxTemplates
            // 
            this.groupBoxTemplates.Controls.Add(this.buttonExplore);
            this.groupBoxTemplates.Controls.Add(this.labelTemplate);
            this.groupBoxTemplates.Controls.Add(this.comboBoxTemplates);
            this.groupBoxTemplates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxTemplates.Location = new System.Drawing.Point(178, 3);
            this.groupBoxTemplates.Name = "groupBoxTemplates";
            this.groupBoxTemplates.Size = new System.Drawing.Size(210, 111);
            this.groupBoxTemplates.TabIndex = 14;
            this.groupBoxTemplates.TabStop = false;
            this.groupBoxTemplates.Text = "Введите имя шаблона";
            // 
            // tableLayoutPanelResponse
            // 
            this.tableLayoutPanelResponse.ColumnCount = 2;
            this.tableLayoutPanelResponse.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanelResponse.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tableLayoutPanelResponse.Controls.Add(this.groupBoxResponse, 0, 0);
            this.tableLayoutPanelResponse.Controls.Add(this.groupBoxTemplates, 1, 0);
            this.tableLayoutPanelResponse.Location = new System.Drawing.Point(11, 130);
            this.tableLayoutPanelResponse.Name = "tableLayoutPanelResponse";
            this.tableLayoutPanelResponse.RowCount = 1;
            this.tableLayoutPanelResponse.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelResponse.Size = new System.Drawing.Size(391, 117);
            this.tableLayoutPanelResponse.TabIndex = 9;
            // 
            // FormCorrespondence
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(418, 311);
            this.Controls.Add(this.tableLayoutPanelResponse);
            this.Controls.Add(this.radioButtonOutcoming);
            this.Controls.Add(this.radioButtonIncoming);
            this.Controls.Add(this.checkBoxOnly);
            this.Controls.Add(this.textBoxShortName);
            this.Controls.Add(this.labelShortName);
            this.Controls.Add(this.checkBoxResponse);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCorrespondence";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Новый тип корреспонденции";
            this.Activated += new System.EventHandler(this.FormCorrespondenceActivated);
            this.Load += new System.EventHandler(this.FormCorrespondenceLoad);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownResponse)).EndInit();
            this.groupBoxResponse.ResumeLayout(false);
            this.groupBoxResponse.PerformLayout();
            this.groupBoxTemplates.ResumeLayout(false);
            this.groupBoxTemplates.PerformLayout();
            this.tableLayoutPanelResponse.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.CheckBox checkBoxResponse;
        private System.Windows.Forms.NumericUpDown numericUpDownResponse;
        private System.Windows.Forms.Label labelShortName;
        private System.Windows.Forms.TextBox textBoxShortName;
        private System.Windows.Forms.CheckBox checkBoxOnly;
        private System.Windows.Forms.RadioButton radioButtonIncoming;
        private System.Windows.Forms.RadioButton radioButtonOutcoming;
        private System.Windows.Forms.Button buttonExplore;
        private System.Windows.Forms.ComboBox comboBoxTemplates;
        private System.Windows.Forms.Label labelTemplate;
        private System.Windows.Forms.GroupBox groupBoxResponse;
        private System.Windows.Forms.GroupBox groupBoxTemplates;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelResponse;
    }
}