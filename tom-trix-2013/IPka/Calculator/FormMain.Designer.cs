namespace Calculator
{
    partial class FormMain
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.dataGridViewMainAuto = new System.Windows.Forms.DataGridView();
            this.dataGridViewRight = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewMainManual = new System.Windows.Forms.DataGridView();
            this.groupBoxInfo = new System.Windows.Forms.GroupBox();
            this.buttonReason = new System.Windows.Forms.Button();
            this.labelStatus2 = new System.Windows.Forms.Label();
            this.textBoxCalendar = new System.Windows.Forms.TextBox();
            this.labelStatus1 = new System.Windows.Forms.Label();
            this.panelColour = new System.Windows.Forms.Panel();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.данныеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.новаяЗаписьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.редакторПравилToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.помощьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelResult = new System.Windows.Forms.Panel();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.splitContainerLeft = new System.Windows.Forms.SplitContainer();
            this.groupBoxMainAuto = new System.Windows.Forms.GroupBox();
            this.groupBoxMainManual = new System.Windows.Forms.GroupBox();
            this.contextMenuStripLeft = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.изменитьНачальнуюДатуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.удалитьЗаписьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.перместитьНаРучноеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMainAuto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMainManual)).BeginInit();
            this.groupBoxInfo.SuspendLayout();
            this.menuStripMain.SuspendLayout();
            this.panelResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLeft)).BeginInit();
            this.splitContainerLeft.Panel1.SuspendLayout();
            this.splitContainerLeft.Panel2.SuspendLayout();
            this.splitContainerLeft.SuspendLayout();
            this.groupBoxMainAuto.SuspendLayout();
            this.groupBoxMainManual.SuspendLayout();
            this.contextMenuStripLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewMainAuto
            // 
            this.dataGridViewMainAuto.AllowUserToAddRows = false;
            this.dataGridViewMainAuto.AllowUserToDeleteRows = false;
            this.dataGridViewMainAuto.AllowUserToResizeRows = false;
            this.dataGridViewMainAuto.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.dataGridViewMainAuto.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMainAuto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewMainAuto.Location = new System.Drawing.Point(3, 16);
            this.dataGridViewMainAuto.MultiSelect = false;
            this.dataGridViewMainAuto.Name = "dataGridViewMainAuto";
            this.dataGridViewMainAuto.ReadOnly = true;
            this.dataGridViewMainAuto.RowHeadersVisible = false;
            this.dataGridViewMainAuto.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewMainAuto.Size = new System.Drawing.Size(533, 160);
            this.dataGridViewMainAuto.TabIndex = 0;
            this.dataGridViewMainAuto.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridViewMainAutoCellMouseUp);
            this.dataGridViewMainAuto.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewMainAutoRowEnter);
            this.dataGridViewMainAuto.Enter += new System.EventHandler(this.DataGridViewMainAutoEnter);
            this.dataGridViewMainAuto.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DataGridViewMainAutoMouseDoubleClick);
            // 
            // dataGridViewRight
            // 
            this.dataGridViewRight.AllowUserToAddRows = false;
            this.dataGridViewRight.AllowUserToDeleteRows = false;
            this.dataGridViewRight.AllowUserToResizeRows = false;
            this.dataGridViewRight.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.dataGridViewRight.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewRight.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
            this.dataGridViewRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewRight.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewRight.MultiSelect = false;
            this.dataGridViewRight.Name = "dataGridViewRight";
            this.dataGridViewRight.ReadOnly = true;
            this.dataGridViewRight.RowHeadersVisible = false;
            this.dataGridViewRight.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewRight.Size = new System.Drawing.Size(441, 538);
            this.dataGridViewRight.TabIndex = 1;
            this.dataGridViewRight.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridViewRightCellMouseUp);
            this.dataGridViewRight.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.DataGridViewRightColumnWidthChanged);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Отметка";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // dataGridViewMainManual
            // 
            this.dataGridViewMainManual.AllowUserToAddRows = false;
            this.dataGridViewMainManual.AllowUserToDeleteRows = false;
            this.dataGridViewMainManual.AllowUserToResizeRows = false;
            this.dataGridViewMainManual.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.dataGridViewMainManual.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMainManual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewMainManual.Location = new System.Drawing.Point(3, 16);
            this.dataGridViewMainManual.MultiSelect = false;
            this.dataGridViewMainManual.Name = "dataGridViewMainManual";
            this.dataGridViewMainManual.ReadOnly = true;
            this.dataGridViewMainManual.RowHeadersVisible = false;
            this.dataGridViewMainManual.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewMainManual.Size = new System.Drawing.Size(533, 336);
            this.dataGridViewMainManual.TabIndex = 4;
            this.dataGridViewMainManual.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridViewMainManualCellMouseUp);
            this.dataGridViewMainManual.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewMainManualRowEnter);
            this.dataGridViewMainManual.Enter += new System.EventHandler(this.DataGridViewMainManualEnter);
            this.dataGridViewMainManual.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DataGridViewMainManualMouseDoubleClick);
            // 
            // groupBoxInfo
            // 
            this.groupBoxInfo.Controls.Add(this.buttonReason);
            this.groupBoxInfo.Controls.Add(this.labelStatus2);
            this.groupBoxInfo.Controls.Add(this.textBoxCalendar);
            this.groupBoxInfo.Controls.Add(this.labelStatus1);
            this.groupBoxInfo.Controls.Add(this.panelColour);
            this.groupBoxInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxInfo.Location = new System.Drawing.Point(0, 0);
            this.groupBoxInfo.Name = "groupBoxInfo";
            this.groupBoxInfo.Size = new System.Drawing.Size(441, 538);
            this.groupBoxInfo.TabIndex = 5;
            this.groupBoxInfo.TabStop = false;
            this.groupBoxInfo.Text = "Информация";
            // 
            // buttonReason
            // 
            this.buttonReason.Location = new System.Drawing.Point(10, 89);
            this.buttonReason.Name = "buttonReason";
            this.buttonReason.Size = new System.Drawing.Size(27, 23);
            this.buttonReason.TabIndex = 4;
            this.buttonReason.Text = "...";
            this.buttonReason.UseVisualStyleBackColor = true;
            this.buttonReason.Click += new System.EventHandler(this.ButtonReasonClick);
            // 
            // labelStatus2
            // 
            this.labelStatus2.AutoSize = true;
            this.labelStatus2.Location = new System.Drawing.Point(155, 63);
            this.labelStatus2.Name = "labelStatus2";
            this.labelStatus2.Size = new System.Drawing.Size(37, 13);
            this.labelStatus2.TabIndex = 3;
            this.labelStatus2.Text = "          ";
            // 
            // textBoxCalendar
            // 
            this.textBoxCalendar.Location = new System.Drawing.Point(69, 60);
            this.textBoxCalendar.Name = "textBoxCalendar";
            this.textBoxCalendar.Size = new System.Drawing.Size(68, 20);
            this.textBoxCalendar.TabIndex = 2;
            this.textBoxCalendar.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TextBoxCalendarRequest);
            this.textBoxCalendar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBoxCalendarRequest);
            // 
            // labelStatus1
            // 
            this.labelStatus1.AutoSize = true;
            this.labelStatus1.Location = new System.Drawing.Point(7, 63);
            this.labelStatus1.Name = "labelStatus1";
            this.labelStatus1.Size = new System.Drawing.Size(56, 13);
            this.labelStatus1.TabIndex = 1;
            this.labelStatus1.Text = "Статус на";
            // 
            // panelColour
            // 
            this.panelColour.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelColour.Location = new System.Drawing.Point(3, 16);
            this.panelColour.Name = "panelColour";
            this.panelColour.Size = new System.Drawing.Size(435, 38);
            this.panelColour.TabIndex = 0;
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.данныеToolStripMenuItem,
            this.справкаToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(984, 24);
            this.menuStripMain.TabIndex = 7;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // данныеToolStripMenuItem
            // 
            this.данныеToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.новаяЗаписьToolStripMenuItem,
            this.редакторПравилToolStripMenuItem});
            this.данныеToolStripMenuItem.Name = "данныеToolStripMenuItem";
            this.данныеToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.данныеToolStripMenuItem.Text = "Данные";
            // 
            // новаяЗаписьToolStripMenuItem
            // 
            this.новаяЗаписьToolStripMenuItem.Name = "новаяЗаписьToolStripMenuItem";
            this.новаяЗаписьToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.новаяЗаписьToolStripMenuItem.Text = "Новая запись";
            this.новаяЗаписьToolStripMenuItem.Click += new System.EventHandler(this.NewRecordToolStripMenuItemClick);
            // 
            // редакторПравилToolStripMenuItem
            // 
            this.редакторПравилToolStripMenuItem.Name = "редакторПравилToolStripMenuItem";
            this.редакторПравилToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.редакторПравилToolStripMenuItem.Text = "Редактор правил";
            this.редакторПравилToolStripMenuItem.Click += new System.EventHandler(this.RulesToolStripMenuItemClick);
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.помощьToolStripMenuItem,
            this.оПрограммеToolStripMenuItem});
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.справкаToolStripMenuItem.Text = "Справка";
            // 
            // помощьToolStripMenuItem
            // 
            this.помощьToolStripMenuItem.Name = "помощьToolStripMenuItem";
            this.помощьToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.помощьToolStripMenuItem.Text = "Помощь";
            // 
            // оПрограммеToolStripMenuItem
            // 
            this.оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            this.оПрограммеToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.оПрограммеToolStripMenuItem.Text = "О программе";
            // 
            // panelResult
            // 
            this.panelResult.Controls.Add(this.groupBoxInfo);
            this.panelResult.Controls.Add(this.dataGridViewRight);
            this.panelResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelResult.Location = new System.Drawing.Point(0, 0);
            this.panelResult.Name = "panelResult";
            this.panelResult.Size = new System.Drawing.Size(441, 538);
            this.panelResult.TabIndex = 9;
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 24);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.splitContainerLeft);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.panelResult);
            this.splitContainerMain.Size = new System.Drawing.Size(984, 538);
            this.splitContainerMain.SplitterDistance = 539;
            this.splitContainerMain.TabIndex = 10;
            // 
            // splitContainerLeft
            // 
            this.splitContainerLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerLeft.Location = new System.Drawing.Point(0, 0);
            this.splitContainerLeft.Name = "splitContainerLeft";
            this.splitContainerLeft.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerLeft.Panel1
            // 
            this.splitContainerLeft.Panel1.Controls.Add(this.groupBoxMainAuto);
            // 
            // splitContainerLeft.Panel2
            // 
            this.splitContainerLeft.Panel2.Controls.Add(this.groupBoxMainManual);
            this.splitContainerLeft.Size = new System.Drawing.Size(539, 538);
            this.splitContainerLeft.SplitterDistance = 179;
            this.splitContainerLeft.TabIndex = 0;
            // 
            // groupBoxMainAuto
            // 
            this.groupBoxMainAuto.Controls.Add(this.dataGridViewMainAuto);
            this.groupBoxMainAuto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxMainAuto.Location = new System.Drawing.Point(0, 0);
            this.groupBoxMainAuto.Name = "groupBoxMainAuto";
            this.groupBoxMainAuto.Size = new System.Drawing.Size(539, 179);
            this.groupBoxMainAuto.TabIndex = 1;
            this.groupBoxMainAuto.TabStop = false;
            this.groupBoxMainAuto.Text = "Автоматическое управление";
            // 
            // groupBoxMainManual
            // 
            this.groupBoxMainManual.Controls.Add(this.dataGridViewMainManual);
            this.groupBoxMainManual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxMainManual.Location = new System.Drawing.Point(0, 0);
            this.groupBoxMainManual.Name = "groupBoxMainManual";
            this.groupBoxMainManual.Size = new System.Drawing.Size(539, 355);
            this.groupBoxMainManual.TabIndex = 5;
            this.groupBoxMainManual.TabStop = false;
            this.groupBoxMainManual.Text = "Ручное управление";
            // 
            // contextMenuStripLeft
            // 
            this.contextMenuStripLeft.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.изменитьНачальнуюДатуToolStripMenuItem,
            this.удалитьЗаписьToolStripMenuItem,
            this.перместитьНаРучноеToolStripMenuItem});
            this.contextMenuStripLeft.Name = "contextMenuStripLeft";
            this.contextMenuStripLeft.Size = new System.Drawing.Size(220, 70);
            // 
            // изменитьНачальнуюДатуToolStripMenuItem
            // 
            this.изменитьНачальнуюДатуToolStripMenuItem.Name = "изменитьНачальнуюДатуToolStripMenuItem";
            this.изменитьНачальнуюДатуToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.изменитьНачальнуюДатуToolStripMenuItem.Text = "Изменить начальную дату";
            this.изменитьНачальнуюДатуToolStripMenuItem.Click += new System.EventHandler(this.ChangeDateToolStripMenuItemClick);
            // 
            // удалитьЗаписьToolStripMenuItem
            // 
            this.удалитьЗаписьToolStripMenuItem.Name = "удалитьЗаписьToolStripMenuItem";
            this.удалитьЗаписьToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.удалитьЗаписьToolStripMenuItem.Text = "Удалить запись";
            this.удалитьЗаписьToolStripMenuItem.Click += new System.EventHandler(this.DeleteToolStripMenuItemClick);
            // 
            // перместитьНаРучноеToolStripMenuItem
            // 
            this.перместитьНаРучноеToolStripMenuItem.Name = "перместитьНаРучноеToolStripMenuItem";
            this.перместитьНаРучноеToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.перместитьНаРучноеToolStripMenuItem.Text = "Переместить на ручное";
            this.перместитьНаРучноеToolStripMenuItem.Click += new System.EventHandler(this.ChangeTypeToolStripMenuItemClick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 562);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.menuStripMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripMain;
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IPka Calculator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMainFormClosing);
            this.Load += new System.EventHandler(this.FormMainLoad);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMainAuto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMainManual)).EndInit();
            this.groupBoxInfo.ResumeLayout(false);
            this.groupBoxInfo.PerformLayout();
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.panelResult.ResumeLayout(false);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerLeft.Panel1.ResumeLayout(false);
            this.splitContainerLeft.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLeft)).EndInit();
            this.splitContainerLeft.ResumeLayout(false);
            this.groupBoxMainAuto.ResumeLayout(false);
            this.groupBoxMainManual.ResumeLayout(false);
            this.contextMenuStripLeft.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewMainAuto;
        private System.Windows.Forms.DataGridView dataGridViewRight;
        private System.Windows.Forms.DataGridViewImageColumn Column1;
        private System.Windows.Forms.DataGridView dataGridViewMainManual;
        private System.Windows.Forms.GroupBox groupBoxInfo;
        private System.Windows.Forms.Label labelStatus2;
        private System.Windows.Forms.TextBox textBoxCalendar;
        private System.Windows.Forms.Label labelStatus1;
        private System.Windows.Forms.Panel panelColour;
        private System.Windows.Forms.Button buttonReason;
        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem данныеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem новаяЗаписьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem редакторПравилToolStripMenuItem;
        private System.Windows.Forms.Panel panelResult;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.SplitContainer splitContainerLeft;
        private System.Windows.Forms.GroupBox groupBoxMainAuto;
        private System.Windows.Forms.GroupBox groupBoxMainManual;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem помощьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripLeft;
        private System.Windows.Forms.ToolStripMenuItem изменитьНачальнуюДатуToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem удалитьЗаписьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem перместитьНаРучноеToolStripMenuItem;
    }
}

