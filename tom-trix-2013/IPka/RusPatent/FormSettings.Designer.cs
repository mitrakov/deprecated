namespace IPka
{
    partial class FormSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            this.checkBoxSort = new System.Windows.Forms.CheckBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.numericUpDownWarn = new System.Windows.Forms.NumericUpDown();
            this.groupBoxWarn = new System.Windows.Forms.GroupBox();
            this.labelDays = new System.Windows.Forms.Label();
            this.groupBoxRequestCorr = new System.Windows.Forms.GroupBox();
            this.comboBoxRequestCorr = new System.Windows.Forms.ComboBox();
            this.checkBoxHideButtons = new System.Windows.Forms.CheckBox();
            this.groupBoxPatentCorr = new System.Windows.Forms.GroupBox();
            this.comboBoxPatentCorr = new System.Windows.Forms.ComboBox();
            this.checkBoxUseWord2003 = new System.Windows.Forms.CheckBox();
            this.groupBoxCodeLength = new System.Windows.Forms.GroupBox();
            this.numericUpDownCodeLength = new System.Windows.Forms.NumericUpDown();
            this.checkBoxShowC = new System.Windows.Forms.CheckBox();
            this.groupBoxFirstCorr = new System.Windows.Forms.GroupBox();
            this.comboBoxFirstCorr = new System.Windows.Forms.ComboBox();
            this.groupBoxSuperCorr = new System.Windows.Forms.GroupBox();
            this.comboBoxSuperCorr = new System.Windows.Forms.ComboBox();
            this.checkBoxShowArchive = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWarn)).BeginInit();
            this.groupBoxWarn.SuspendLayout();
            this.groupBoxRequestCorr.SuspendLayout();
            this.groupBoxPatentCorr.SuspendLayout();
            this.groupBoxCodeLength.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCodeLength)).BeginInit();
            this.groupBoxFirstCorr.SuspendLayout();
            this.groupBoxSuperCorr.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxSort
            // 
            this.checkBoxSort.AutoSize = true;
            this.checkBoxSort.Checked = true;
            this.checkBoxSort.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSort.Location = new System.Drawing.Point(11, 148);
            this.checkBoxSort.Name = "checkBoxSort";
            this.checkBoxSort.Size = new System.Drawing.Size(259, 17);
            this.checkBoxSort.TabIndex = 4;
            this.checkBoxSort.Text = "Сортировать внутренний код по 3-му символу";
            this.checkBoxSort.UseVisualStyleBackColor = true;
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(430, 285);
            this.buttonOk.Margin = new System.Windows.Forms.Padding(2);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(62, 28);
            this.buttonOk.TabIndex = 11;
            this.buttonOk.Text = "ОК";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.ButtonOkClick);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(527, 285);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(62, 28);
            this.buttonCancel.TabIndex = 12;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.ButtonCancelClick);
            // 
            // numericUpDownWarn
            // 
            this.numericUpDownWarn.Location = new System.Drawing.Point(31, 17);
            this.numericUpDownWarn.Margin = new System.Windows.Forms.Padding(2);
            this.numericUpDownWarn.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDownWarn.Name = "numericUpDownWarn";
            this.numericUpDownWarn.Size = new System.Drawing.Size(33, 20);
            this.numericUpDownWarn.TabIndex = 1;
            this.numericUpDownWarn.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numericUpDownWarn.ValueChanged += new System.EventHandler(this.NumericUpDownWarnValueChanged);
            // 
            // groupBoxWarn
            // 
            this.groupBoxWarn.Controls.Add(this.labelDays);
            this.groupBoxWarn.Controls.Add(this.numericUpDownWarn);
            this.groupBoxWarn.Location = new System.Drawing.Point(11, 11);
            this.groupBoxWarn.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxWarn.Name = "groupBoxWarn";
            this.groupBoxWarn.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxWarn.Size = new System.Drawing.Size(232, 46);
            this.groupBoxWarn.TabIndex = 0;
            this.groupBoxWarn.TabStop = false;
            this.groupBoxWarn.Text = "Предупреждать о крайних сроках за:";
            // 
            // labelDays
            // 
            this.labelDays.AutoSize = true;
            this.labelDays.Location = new System.Drawing.Point(76, 19);
            this.labelDays.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelDays.Name = "labelDays";
            this.labelDays.Size = new System.Drawing.Size(25, 13);
            this.labelDays.TabIndex = 7;
            this.labelDays.Text = "дня";
            // 
            // groupBoxRequestCorr
            // 
            this.groupBoxRequestCorr.Controls.Add(this.comboBoxRequestCorr);
            this.groupBoxRequestCorr.Location = new System.Drawing.Point(317, 14);
            this.groupBoxRequestCorr.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxRequestCorr.Name = "groupBoxRequestCorr";
            this.groupBoxRequestCorr.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxRequestCorr.Size = new System.Drawing.Size(301, 43);
            this.groupBoxRequestCorr.TabIndex = 7;
            this.groupBoxRequestCorr.TabStop = false;
            this.groupBoxRequestCorr.Text = "Запросить № заявки роспатента при получении:";
            // 
            // comboBoxRequestCorr
            // 
            this.comboBoxRequestCorr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRequestCorr.FormattingEnabled = true;
            this.comboBoxRequestCorr.Items.AddRange(new object[] {
            "..."});
            this.comboBoxRequestCorr.Location = new System.Drawing.Point(31, 19);
            this.comboBoxRequestCorr.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxRequestCorr.Name = "comboBoxRequestCorr";
            this.comboBoxRequestCorr.Size = new System.Drawing.Size(152, 21);
            this.comboBoxRequestCorr.TabIndex = 8;
            // 
            // checkBoxHideButtons
            // 
            this.checkBoxHideButtons.AutoSize = true;
            this.checkBoxHideButtons.Location = new System.Drawing.Point(11, 171);
            this.checkBoxHideButtons.Name = "checkBoxHideButtons";
            this.checkBoxHideButtons.Size = new System.Drawing.Size(299, 17);
            this.checkBoxHideButtons.TabIndex = 5;
            this.checkBoxHideButtons.Text = "Показывать кнопки \"Удалить\" и \"Создать документ\"";
            this.checkBoxHideButtons.UseVisualStyleBackColor = true;
            // 
            // groupBoxPatentCorr
            // 
            this.groupBoxPatentCorr.Controls.Add(this.comboBoxPatentCorr);
            this.groupBoxPatentCorr.Location = new System.Drawing.Point(317, 72);
            this.groupBoxPatentCorr.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxPatentCorr.Name = "groupBoxPatentCorr";
            this.groupBoxPatentCorr.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxPatentCorr.Size = new System.Drawing.Size(301, 43);
            this.groupBoxPatentCorr.TabIndex = 9;
            this.groupBoxPatentCorr.TabStop = false;
            this.groupBoxPatentCorr.Text = "Запросить № свидетельства (патента) при получении:";
            // 
            // comboBoxPatentCorr
            // 
            this.comboBoxPatentCorr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPatentCorr.FormattingEnabled = true;
            this.comboBoxPatentCorr.Items.AddRange(new object[] {
            "..."});
            this.comboBoxPatentCorr.Location = new System.Drawing.Point(31, 19);
            this.comboBoxPatentCorr.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxPatentCorr.Name = "comboBoxPatentCorr";
            this.comboBoxPatentCorr.Size = new System.Drawing.Size(152, 21);
            this.comboBoxPatentCorr.TabIndex = 10;
            // 
            // checkBoxUseWord2003
            // 
            this.checkBoxUseWord2003.AutoSize = true;
            this.checkBoxUseWord2003.Location = new System.Drawing.Point(11, 194);
            this.checkBoxUseWord2003.Name = "checkBoxUseWord2003";
            this.checkBoxUseWord2003.Size = new System.Drawing.Size(259, 17);
            this.checkBoxUseWord2003.TabIndex = 6;
            this.checkBoxUseWord2003.Text = "Использовать формат \".doc\" для документов";
            this.checkBoxUseWord2003.UseVisualStyleBackColor = true;
            // 
            // groupBoxCodeLength
            // 
            this.groupBoxCodeLength.Controls.Add(this.numericUpDownCodeLength);
            this.groupBoxCodeLength.Location = new System.Drawing.Point(11, 62);
            this.groupBoxCodeLength.Name = "groupBoxCodeLength";
            this.groupBoxCodeLength.Size = new System.Drawing.Size(232, 46);
            this.groupBoxCodeLength.TabIndex = 2;
            this.groupBoxCodeLength.TabStop = false;
            this.groupBoxCodeLength.Text = "Суммарная длина внутреннего кода:";
            // 
            // numericUpDownCodeLength
            // 
            this.numericUpDownCodeLength.Location = new System.Drawing.Point(31, 18);
            this.numericUpDownCodeLength.Margin = new System.Windows.Forms.Padding(2);
            this.numericUpDownCodeLength.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownCodeLength.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numericUpDownCodeLength.Name = "numericUpDownCodeLength";
            this.numericUpDownCodeLength.Size = new System.Drawing.Size(33, 20);
            this.numericUpDownCodeLength.TabIndex = 3;
            this.numericUpDownCodeLength.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // checkBoxShowC
            // 
            this.checkBoxShowC.AutoSize = true;
            this.checkBoxShowC.Location = new System.Drawing.Point(11, 217);
            this.checkBoxShowC.Name = "checkBoxShowC";
            this.checkBoxShowC.Size = new System.Drawing.Size(258, 17);
            this.checkBoxShowC.TabIndex = 13;
            this.checkBoxShowC.Text = "Показывать кнопку \"С\" для очистки поля дат";
            this.checkBoxShowC.UseVisualStyleBackColor = true;
            // 
            // groupBoxFirstCorr
            // 
            this.groupBoxFirstCorr.Controls.Add(this.comboBoxFirstCorr);
            this.groupBoxFirstCorr.Location = new System.Drawing.Point(317, 129);
            this.groupBoxFirstCorr.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxFirstCorr.Name = "groupBoxFirstCorr";
            this.groupBoxFirstCorr.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxFirstCorr.Size = new System.Drawing.Size(301, 43);
            this.groupBoxFirstCorr.TabIndex = 11;
            this.groupBoxFirstCorr.TabStop = false;
            this.groupBoxFirstCorr.Text = "При добавлении заявки по умолчанию создавать:";
            // 
            // comboBoxFirstCorr
            // 
            this.comboBoxFirstCorr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFirstCorr.FormattingEnabled = true;
            this.comboBoxFirstCorr.Items.AddRange(new object[] {
            " "});
            this.comboBoxFirstCorr.Location = new System.Drawing.Point(31, 19);
            this.comboBoxFirstCorr.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxFirstCorr.Name = "comboBoxFirstCorr";
            this.comboBoxFirstCorr.Size = new System.Drawing.Size(152, 21);
            this.comboBoxFirstCorr.TabIndex = 10;
            // 
            // groupBoxSuperCorr
            // 
            this.groupBoxSuperCorr.Controls.Add(this.comboBoxSuperCorr);
            this.groupBoxSuperCorr.Location = new System.Drawing.Point(317, 192);
            this.groupBoxSuperCorr.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxSuperCorr.Name = "groupBoxSuperCorr";
            this.groupBoxSuperCorr.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxSuperCorr.Size = new System.Drawing.Size(301, 43);
            this.groupBoxSuperCorr.TabIndex = 12;
            this.groupBoxSuperCorr.TabStop = false;
            this.groupBoxSuperCorr.Text = "Разрешать отправлять в любых случаях:";
            this.groupBoxSuperCorr.Visible = false;
            // 
            // comboBoxSuperCorr
            // 
            this.comboBoxSuperCorr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSuperCorr.FormattingEnabled = true;
            this.comboBoxSuperCorr.Items.AddRange(new object[] {
            " "});
            this.comboBoxSuperCorr.Location = new System.Drawing.Point(31, 19);
            this.comboBoxSuperCorr.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxSuperCorr.Name = "comboBoxSuperCorr";
            this.comboBoxSuperCorr.Size = new System.Drawing.Size(152, 21);
            this.comboBoxSuperCorr.TabIndex = 10;
            // 
            // checkBoxShowArchive
            // 
            this.checkBoxShowArchive.AutoSize = true;
            this.checkBoxShowArchive.Location = new System.Drawing.Point(11, 240);
            this.checkBoxShowArchive.Name = "checkBoxShowArchive";
            this.checkBoxShowArchive.Size = new System.Drawing.Size(180, 17);
            this.checkBoxShowArchive.TabIndex = 14;
            this.checkBoxShowArchive.Text = "Показывать архивные записи";
            this.checkBoxShowArchive.UseVisualStyleBackColor = true;
            // 
            // FormSettings
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(646, 324);
            this.Controls.Add(this.checkBoxShowArchive);
            this.Controls.Add(this.groupBoxSuperCorr);
            this.Controls.Add(this.groupBoxFirstCorr);
            this.Controls.Add(this.checkBoxShowC);
            this.Controls.Add(this.groupBoxCodeLength);
            this.Controls.Add(this.checkBoxSort);
            this.Controls.Add(this.checkBoxUseWord2003);
            this.Controls.Add(this.groupBoxPatentCorr);
            this.Controls.Add(this.checkBoxHideButtons);
            this.Controls.Add(this.groupBoxRequestCorr);
            this.Controls.Add(this.groupBoxWarn);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSettings";
            this.Opacity = 0.85D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройки программы";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWarn)).EndInit();
            this.groupBoxWarn.ResumeLayout(false);
            this.groupBoxWarn.PerformLayout();
            this.groupBoxRequestCorr.ResumeLayout(false);
            this.groupBoxPatentCorr.ResumeLayout(false);
            this.groupBoxCodeLength.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCodeLength)).EndInit();
            this.groupBoxFirstCorr.ResumeLayout(false);
            this.groupBoxSuperCorr.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.NumericUpDown numericUpDownWarn;
        private System.Windows.Forms.GroupBox groupBoxWarn;
        private System.Windows.Forms.Label labelDays;
        private System.Windows.Forms.GroupBox groupBoxRequestCorr;
        private System.Windows.Forms.ComboBox comboBoxRequestCorr;
        private System.Windows.Forms.CheckBox checkBoxSort;
        private System.Windows.Forms.CheckBox checkBoxHideButtons;
        private System.Windows.Forms.GroupBox groupBoxPatentCorr;
        private System.Windows.Forms.ComboBox comboBoxPatentCorr;
        private System.Windows.Forms.CheckBox checkBoxUseWord2003;
        private System.Windows.Forms.GroupBox groupBoxCodeLength;
        private System.Windows.Forms.NumericUpDown numericUpDownCodeLength;
        private System.Windows.Forms.CheckBox checkBoxShowC;
        private System.Windows.Forms.GroupBox groupBoxFirstCorr;
        private System.Windows.Forms.ComboBox comboBoxFirstCorr;
        private System.Windows.Forms.GroupBox groupBoxSuperCorr;
        private System.Windows.Forms.ComboBox comboBoxSuperCorr;
        private System.Windows.Forms.CheckBox checkBoxShowArchive;

    }
}