namespace Calculator
{
    partial class FormNewRecordStart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNewRecordStart));
            this.radioButtonImport = new System.Windows.Forms.RadioButton();
            this.radioButtonNew = new System.Windows.Forms.RadioButton();
            this.groupBoxData = new System.Windows.Forms.GroupBox();
            this.comboBoxProjectType = new System.Windows.Forms.ComboBox();
            this.labelProjectType = new System.Windows.Forms.Label();
            this.textBoxProjectName = new System.Windows.Forms.TextBox();
            this.labelProjectName = new System.Windows.Forms.Label();
            this.textBoxInternalCode = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.radioButtonManual = new System.Windows.Forms.RadioButton();
            this.radioButtonAuto = new System.Windows.Forms.RadioButton();
            this.monthCalendarMain = new System.Windows.Forms.MonthCalendar();
            this.panel3 = new System.Windows.Forms.Panel();
            this.buttonBack = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxTop = new System.Windows.Forms.GroupBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.labelYear = new System.Windows.Forms.Label();
            this.numericUpDownYear = new System.Windows.Forms.NumericUpDown();
            this.groupBoxData.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.tableLayoutPanelMain.SuspendLayout();
            this.groupBoxTop.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownYear)).BeginInit();
            this.SuspendLayout();
            // 
            // radioButtonImport
            // 
            this.radioButtonImport.AutoSize = true;
            this.radioButtonImport.Checked = true;
            this.radioButtonImport.Location = new System.Drawing.Point(19, 14);
            this.radioButtonImport.Name = "radioButtonImport";
            this.radioButtonImport.Size = new System.Drawing.Size(246, 17);
            this.radioButtonImport.TabIndex = 2;
            this.radioButtonImport.TabStop = true;
            this.radioButtonImport.Text = "Импортировать, используя внутренний код";
            this.radioButtonImport.UseVisualStyleBackColor = true;
            this.radioButtonImport.CheckedChanged += new System.EventHandler(this.RadioButtonNewImportChanged);
            // 
            // radioButtonNew
            // 
            this.radioButtonNew.AutoSize = true;
            this.radioButtonNew.Location = new System.Drawing.Point(19, 38);
            this.radioButtonNew.Name = "radioButtonNew";
            this.radioButtonNew.Size = new System.Drawing.Size(148, 17);
            this.radioButtonNew.TabIndex = 3;
            this.radioButtonNew.Text = "Добавить новую запись";
            this.radioButtonNew.UseVisualStyleBackColor = true;
            this.radioButtonNew.CheckedChanged += new System.EventHandler(this.RadioButtonNewImportChanged);
            // 
            // groupBoxData
            // 
            this.groupBoxData.Controls.Add(this.comboBoxProjectType);
            this.groupBoxData.Controls.Add(this.labelProjectType);
            this.groupBoxData.Controls.Add(this.textBoxProjectName);
            this.groupBoxData.Controls.Add(this.labelProjectName);
            this.groupBoxData.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBoxData.Enabled = false;
            this.groupBoxData.Location = new System.Drawing.Point(0, 116);
            this.groupBoxData.Name = "groupBoxData";
            this.groupBoxData.Size = new System.Drawing.Size(422, 111);
            this.groupBoxData.TabIndex = 5;
            this.groupBoxData.TabStop = false;
            this.groupBoxData.Text = "Данные о заявке";
            // 
            // comboBoxProjectType
            // 
            this.comboBoxProjectType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxProjectType.FormattingEnabled = true;
            this.comboBoxProjectType.Location = new System.Drawing.Point(131, 74);
            this.comboBoxProjectType.Name = "comboBoxProjectType";
            this.comboBoxProjectType.Size = new System.Drawing.Size(61, 21);
            this.comboBoxProjectType.TabIndex = 9;
            this.comboBoxProjectType.SelectedIndexChanged += new System.EventHandler(this.ParametersChanged);
            // 
            // labelProjectType
            // 
            this.labelProjectType.AutoSize = true;
            this.labelProjectType.Location = new System.Drawing.Point(25, 77);
            this.labelProjectType.Name = "labelProjectType";
            this.labelProjectType.Size = new System.Drawing.Size(70, 13);
            this.labelProjectType.TabIndex = 8;
            this.labelProjectType.Text = "Тип проекта";
            // 
            // textBoxProjectName
            // 
            this.textBoxProjectName.Location = new System.Drawing.Point(131, 39);
            this.textBoxProjectName.Name = "textBoxProjectName";
            this.textBoxProjectName.Size = new System.Drawing.Size(151, 20);
            this.textBoxProjectName.TabIndex = 7;
            this.textBoxProjectName.TextChanged += new System.EventHandler(this.ParametersChanged);
            // 
            // labelProjectName
            // 
            this.labelProjectName.AutoSize = true;
            this.labelProjectName.Location = new System.Drawing.Point(25, 42);
            this.labelProjectName.Name = "labelProjectName";
            this.labelProjectName.Size = new System.Drawing.Size(101, 13);
            this.labelProjectName.TabIndex = 6;
            this.labelProjectName.Text = "Название проекта";
            // 
            // textBoxInternalCode
            // 
            this.textBoxInternalCode.Location = new System.Drawing.Point(286, 14);
            this.textBoxInternalCode.Name = "textBoxInternalCode";
            this.textBoxInternalCode.Size = new System.Drawing.Size(100, 20);
            this.textBoxInternalCode.TabIndex = 1;
            this.textBoxInternalCode.TextChanged += new System.EventHandler(this.TextBoxInternalCodeTextChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBoxInternalCode);
            this.panel1.Controls.Add(this.radioButtonNew);
            this.panel1.Controls.Add(this.radioButtonImport);
            this.panel1.Controls.Add(this.groupBoxData);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(422, 227);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.radioButtonManual);
            this.panel2.Controls.Add(this.radioButtonAuto);
            this.panel2.Controls.Add(this.monthCalendarMain);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 16);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(422, 227);
            this.panel2.TabIndex = 10;
            this.panel2.Visible = false;
            // 
            // radioButtonManual
            // 
            this.radioButtonManual.AutoSize = true;
            this.radioButtonManual.Location = new System.Drawing.Point(211, 126);
            this.radioButtonManual.Name = "radioButtonManual";
            this.radioButtonManual.Size = new System.Drawing.Size(110, 17);
            this.radioButtonManual.TabIndex = 13;
            this.radioButtonManual.TabStop = true;
            this.radioButtonManual.Text = "Ручной контроль";
            this.radioButtonManual.UseVisualStyleBackColor = true;
            this.radioButtonManual.CheckedChanged += new System.EventHandler(this.RadioButtonManualAutoCheckedChanged);
            // 
            // radioButtonAuto
            // 
            this.radioButtonAuto.AutoSize = true;
            this.radioButtonAuto.Location = new System.Drawing.Point(211, 102);
            this.radioButtonAuto.Name = "radioButtonAuto";
            this.radioButtonAuto.Size = new System.Drawing.Size(159, 17);
            this.radioButtonAuto.TabIndex = 12;
            this.radioButtonAuto.TabStop = true;
            this.radioButtonAuto.Text = "Автоматический контроль";
            this.radioButtonAuto.UseVisualStyleBackColor = true;
            this.radioButtonAuto.CheckedChanged += new System.EventHandler(this.RadioButtonManualAutoCheckedChanged);
            // 
            // monthCalendarMain
            // 
            this.monthCalendarMain.Location = new System.Drawing.Point(19, -1);
            this.monthCalendarMain.MaxSelectionCount = 1;
            this.monthCalendarMain.Name = "monthCalendarMain";
            this.monthCalendarMain.TabIndex = 11;
            // 
            // panel3
            // 
            this.panel3.AutoScroll = true;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 16);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(422, 227);
            this.panel3.TabIndex = 11;
            this.panel3.Visible = false;
            // 
            // buttonBack
            // 
            this.buttonBack.Location = new System.Drawing.Point(214, 12);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(75, 23);
            this.buttonBack.TabIndex = 1;
            this.buttonBack.Text = "Назад";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Visible = false;
            this.buttonBack.Click += new System.EventHandler(this.ButtonBackClick);
            // 
            // buttonNext
            // 
            this.buttonNext.Enabled = false;
            this.buttonNext.Location = new System.Drawing.Point(311, 12);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(75, 23);
            this.buttonNext.TabIndex = 0;
            this.buttonNext.Text = "Далее";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.ButtonNextClick);
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.buttonNext);
            this.panelBottom.Controls.Add(this.buttonBack);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottom.Location = new System.Drawing.Point(3, 255);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(428, 54);
            this.panelBottom.TabIndex = 12;
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.panelBottom, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.groupBoxTop, 0, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 2;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(434, 312);
            this.tableLayoutPanelMain.TabIndex = 13;
            // 
            // groupBoxTop
            // 
            this.groupBoxTop.Controls.Add(this.panel1);
            this.groupBoxTop.Controls.Add(this.panel2);
            this.groupBoxTop.Controls.Add(this.panel3);
            this.groupBoxTop.Controls.Add(this.panel4);
            this.groupBoxTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxTop.Location = new System.Drawing.Point(3, 3);
            this.groupBoxTop.Name = "groupBoxTop";
            this.groupBoxTop.Size = new System.Drawing.Size(428, 246);
            this.groupBoxTop.TabIndex = 13;
            this.groupBoxTop.TabStop = false;
            this.groupBoxTop.Text = "Шаг 1";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.labelYear);
            this.panel4.Controls.Add(this.numericUpDownYear);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 16);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(422, 227);
            this.panel4.TabIndex = 22;
            // 
            // labelYear
            // 
            this.labelYear.AutoSize = true;
            this.labelYear.Location = new System.Drawing.Point(69, 106);
            this.labelYear.Name = "labelYear";
            this.labelYear.Size = new System.Drawing.Size(301, 13);
            this.labelYear.TabIndex = 25;
            this.labelYear.Text = "Год, за который пошлина была уплачена в последний раз";
            // 
            // numericUpDownYear
            // 
            this.numericUpDownYear.Location = new System.Drawing.Point(162, 126);
            this.numericUpDownYear.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numericUpDownYear.Name = "numericUpDownYear";
            this.numericUpDownYear.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownYear.TabIndex = 50;
            // 
            // FormNewRecordStart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 312);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormNewRecordStart";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Добавить новую запись";
            this.Load += new System.EventHandler(this.FormNewRecordStartLoad);
            this.groupBoxData.ResumeLayout(false);
            this.groupBoxData.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.groupBoxTop.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownYear)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonImport;
        private System.Windows.Forms.RadioButton radioButtonNew;
        private System.Windows.Forms.GroupBox groupBoxData;
        private System.Windows.Forms.RadioButton radioButtonManual;
        private System.Windows.Forms.RadioButton radioButtonAuto;
        private System.Windows.Forms.MonthCalendar monthCalendarMain;
        private System.Windows.Forms.ComboBox comboBoxProjectType;
        private System.Windows.Forms.Label labelProjectType;
        private System.Windows.Forms.TextBox textBoxProjectName;
        private System.Windows.Forms.Label labelProjectName;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.GroupBox groupBoxTop;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label labelYear;
        private System.Windows.Forms.NumericUpDown numericUpDownYear;
        private System.Windows.Forms.TextBox textBoxInternalCode;
    }
}