namespace IPka
{
    partial class FormDb
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDb));
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelProjectName = new System.Windows.Forms.Label();
            this.textBoxProjectName = new System.Windows.Forms.TextBox();
            this.buttonCreatedClear = new System.Windows.Forms.Button();
            this.buttonCreatedCalendar = new System.Windows.Forms.Button();
            this.labelRequestNumber = new System.Windows.Forms.Label();
            this.labelPatentNumber = new System.Windows.Forms.Label();
            this.textBoxPatentNumber = new System.Windows.Forms.TextBox();
            this.textBoxRequestNumber = new System.Windows.Forms.TextBox();
            this.textBoxDateCreated = new System.Windows.Forms.TextBox();
            this.labelDateCreated = new System.Windows.Forms.Label();
            this.radioButtonPhysical = new System.Windows.Forms.RadioButton();
            this.radioButtonLegal = new System.Windows.Forms.RadioButton();
            this.buttonReceivedClear = new System.Windows.Forms.Button();
            this.buttonReceivedCalendar = new System.Windows.Forms.Button();
            this.textBoxDateReceived = new System.Windows.Forms.TextBox();
            this.labelDateReceived = new System.Windows.Forms.Label();
            this.buttonClients = new System.Windows.Forms.Button();
            this.buttonColour = new System.Windows.Forms.Button();
            this.panelRequisitesLegal = new IPka.RequisPanel();
            this.panelRequisitesPhysical = new IPka.RequisPanel();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(65, 389);
            this.buttonOk.Margin = new System.Windows.Forms.Padding(2);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(67, 33);
            this.buttonOk.TabIndex = 21;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.ButtonOkClick);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(159, 389);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(67, 33);
            this.buttonCancel.TabIndex = 22;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.ButtonCancelClick);
            // 
            // labelProjectName
            // 
            this.labelProjectName.AutoSize = true;
            this.labelProjectName.Location = new System.Drawing.Point(11, 107);
            this.labelProjectName.Margin = new System.Windows.Forms.Padding(10);
            this.labelProjectName.Name = "labelProjectName";
            this.labelProjectName.Size = new System.Drawing.Size(57, 13);
            this.labelProjectName.TabIndex = 4;
            this.labelProjectName.Text = "Название";
            // 
            // textBoxProjectName
            // 
            this.textBoxProjectName.Location = new System.Drawing.Point(207, 104);
            this.textBoxProjectName.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxProjectName.Name = "textBoxProjectName";
            this.textBoxProjectName.Size = new System.Drawing.Size(130, 20);
            this.textBoxProjectName.TabIndex = 5;
            // 
            // buttonCreatedClear
            // 
            this.buttonCreatedClear.Location = new System.Drawing.Point(320, 169);
            this.buttonCreatedClear.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCreatedClear.Name = "buttonCreatedClear";
            this.buttonCreatedClear.Size = new System.Drawing.Size(20, 19);
            this.buttonCreatedClear.TabIndex = 29;
            this.buttonCreatedClear.Text = "C";
            this.buttonCreatedClear.UseVisualStyleBackColor = true;
            this.buttonCreatedClear.Click += new System.EventHandler(this.ButtonCreatedClearClick);
            // 
            // buttonCreatedCalendar
            // 
            this.buttonCreatedCalendar.Location = new System.Drawing.Point(291, 169);
            this.buttonCreatedCalendar.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCreatedCalendar.Name = "buttonCreatedCalendar";
            this.buttonCreatedCalendar.Size = new System.Drawing.Size(25, 19);
            this.buttonCreatedCalendar.TabIndex = 28;
            this.buttonCreatedCalendar.Text = "...";
            this.buttonCreatedCalendar.UseVisualStyleBackColor = true;
            this.buttonCreatedCalendar.Click += new System.EventHandler(this.ButtonCreatedCalendarClick);
            // 
            // labelRequestNumber
            // 
            this.labelRequestNumber.AutoSize = true;
            this.labelRequestNumber.Location = new System.Drawing.Point(13, 139);
            this.labelRequestNumber.Margin = new System.Windows.Forms.Padding(10);
            this.labelRequestNumber.Name = "labelRequestNumber";
            this.labelRequestNumber.Size = new System.Drawing.Size(119, 13);
            this.labelRequestNumber.TabIndex = 24;
            this.labelRequestNumber.Text = "№ заявки Роспатента";
            // 
            // labelPatentNumber
            // 
            this.labelPatentNumber.AutoSize = true;
            this.labelPatentNumber.Location = new System.Drawing.Point(11, 205);
            this.labelPatentNumber.Margin = new System.Windows.Forms.Padding(10);
            this.labelPatentNumber.Name = "labelPatentNumber";
            this.labelPatentNumber.Size = new System.Drawing.Size(142, 13);
            this.labelPatentNumber.TabIndex = 30;
            this.labelPatentNumber.Text = "№ патента/свидетельства";
            // 
            // textBoxPatentNumber
            // 
            this.textBoxPatentNumber.Location = new System.Drawing.Point(207, 202);
            this.textBoxPatentNumber.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxPatentNumber.Name = "textBoxPatentNumber";
            this.textBoxPatentNumber.Size = new System.Drawing.Size(130, 20);
            this.textBoxPatentNumber.TabIndex = 31;
            // 
            // textBoxRequestNumber
            // 
            this.textBoxRequestNumber.Location = new System.Drawing.Point(207, 136);
            this.textBoxRequestNumber.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxRequestNumber.Name = "textBoxRequestNumber";
            this.textBoxRequestNumber.Size = new System.Drawing.Size(130, 20);
            this.textBoxRequestNumber.TabIndex = 25;
            // 
            // textBoxDateCreated
            // 
            this.textBoxDateCreated.Enabled = false;
            this.textBoxDateCreated.Location = new System.Drawing.Point(207, 168);
            this.textBoxDateCreated.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxDateCreated.Name = "textBoxDateCreated";
            this.textBoxDateCreated.Size = new System.Drawing.Size(80, 20);
            this.textBoxDateCreated.TabIndex = 27;
            // 
            // labelDateCreated
            // 
            this.labelDateCreated.AutoSize = true;
            this.labelDateCreated.Location = new System.Drawing.Point(11, 172);
            this.labelDateCreated.Margin = new System.Windows.Forms.Padding(10);
            this.labelDateCreated.Name = "labelDateCreated";
            this.labelDateCreated.Size = new System.Drawing.Size(110, 13);
            this.labelDateCreated.TabIndex = 26;
            this.labelDateCreated.Text = "Дата подачи заявки";
            // 
            // radioButtonPhysical
            // 
            this.radioButtonPhysical.AutoSize = true;
            this.radioButtonPhysical.Checked = true;
            this.radioButtonPhysical.Location = new System.Drawing.Point(358, 11);
            this.radioButtonPhysical.Name = "radioButtonPhysical";
            this.radioButtonPhysical.Size = new System.Drawing.Size(116, 17);
            this.radioButtonPhysical.TabIndex = 32;
            this.radioButtonPhysical.TabStop = true;
            this.radioButtonPhysical.Text = "Физическое лицо";
            this.radioButtonPhysical.UseVisualStyleBackColor = true;
            this.radioButtonPhysical.CheckedChanged += new System.EventHandler(this.RadioButtonPhysicalCheckedChanged);
            // 
            // radioButtonLegal
            // 
            this.radioButtonLegal.AutoSize = true;
            this.radioButtonLegal.Location = new System.Drawing.Point(358, 32);
            this.radioButtonLegal.Name = "radioButtonLegal";
            this.radioButtonLegal.Size = new System.Drawing.Size(120, 17);
            this.radioButtonLegal.TabIndex = 33;
            this.radioButtonLegal.TabStop = true;
            this.radioButtonLegal.Text = "Юридическое лицо";
            this.radioButtonLegal.UseVisualStyleBackColor = true;
            // 
            // buttonReceivedClear
            // 
            this.buttonReceivedClear.Location = new System.Drawing.Point(320, 235);
            this.buttonReceivedClear.Margin = new System.Windows.Forms.Padding(2);
            this.buttonReceivedClear.Name = "buttonReceivedClear";
            this.buttonReceivedClear.Size = new System.Drawing.Size(20, 19);
            this.buttonReceivedClear.TabIndex = 37;
            this.buttonReceivedClear.Text = "C";
            this.buttonReceivedClear.UseVisualStyleBackColor = true;
            this.buttonReceivedClear.Click += new System.EventHandler(this.ButtonReceivedClearClick);
            // 
            // buttonReceivedCalendar
            // 
            this.buttonReceivedCalendar.Location = new System.Drawing.Point(291, 235);
            this.buttonReceivedCalendar.Margin = new System.Windows.Forms.Padding(2);
            this.buttonReceivedCalendar.Name = "buttonReceivedCalendar";
            this.buttonReceivedCalendar.Size = new System.Drawing.Size(25, 19);
            this.buttonReceivedCalendar.TabIndex = 36;
            this.buttonReceivedCalendar.Text = "...";
            this.buttonReceivedCalendar.UseVisualStyleBackColor = true;
            this.buttonReceivedCalendar.Click += new System.EventHandler(this.ButtonReceivedCalendarClick);
            // 
            // textBoxDateReceived
            // 
            this.textBoxDateReceived.Enabled = false;
            this.textBoxDateReceived.Location = new System.Drawing.Point(207, 234);
            this.textBoxDateReceived.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxDateReceived.Name = "textBoxDateReceived";
            this.textBoxDateReceived.Size = new System.Drawing.Size(80, 20);
            this.textBoxDateReceived.TabIndex = 35;
            // 
            // labelDateReceived
            // 
            this.labelDateReceived.AutoSize = true;
            this.labelDateReceived.Location = new System.Drawing.Point(11, 237);
            this.labelDateReceived.Margin = new System.Windows.Forms.Padding(10);
            this.labelDateReceived.Name = "labelDateReceived";
            this.labelDateReceived.Size = new System.Drawing.Size(197, 13);
            this.labelDateReceived.TabIndex = 34;
            this.labelDateReceived.Text = "Дата выдачи патента/свидетельства";
            // 
            // buttonClients
            // 
            this.buttonClients.Location = new System.Drawing.Point(714, 20);
            this.buttonClients.Name = "buttonClients";
            this.buttonClients.Size = new System.Drawing.Size(37, 23);
            this.buttonClients.TabIndex = 38;
            this.buttonClients.Text = "...";
            this.buttonClients.UseVisualStyleBackColor = true;
            this.buttonClients.Click += new System.EventHandler(this.ButtonClientsClick);
            // 
            // buttonColour
            // 
            this.buttonColour.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonColour.Location = new System.Drawing.Point(14, 280);
            this.buttonColour.Name = "buttonColour";
            this.buttonColour.Size = new System.Drawing.Size(67, 33);
            this.buttonColour.TabIndex = 39;
            this.buttonColour.UseVisualStyleBackColor = true;
            this.buttonColour.Click += new System.EventHandler(this.ButtonColourClick);
            // 
            // panelRequisitesLegal
            // 
            this.panelRequisitesLegal.AutoScroll = true;
            this.panelRequisitesLegal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelRequisitesLegal.Location = new System.Drawing.Point(358, 54);
            this.panelRequisitesLegal.Margin = new System.Windows.Forms.Padding(2);
            this.panelRequisitesLegal.Name = "panelRequisitesLegal";
            this.panelRequisitesLegal.Size = new System.Drawing.Size(393, 368);
            this.panelRequisitesLegal.TabIndex = 24;
            this.panelRequisitesLegal.NewItemInput += new IPka.RequisPanel.TrixEventHandler(this.PanelRequisitesLegalNewItemInput);
            // 
            // panelRequisitesPhysical
            // 
            this.panelRequisitesPhysical.AutoScroll = true;
            this.panelRequisitesPhysical.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelRequisitesPhysical.Location = new System.Drawing.Point(358, 54);
            this.panelRequisitesPhysical.Margin = new System.Windows.Forms.Padding(2);
            this.panelRequisitesPhysical.Name = "panelRequisitesPhysical";
            this.panelRequisitesPhysical.Size = new System.Drawing.Size(393, 368);
            this.panelRequisitesPhysical.TabIndex = 23;
            this.panelRequisitesPhysical.NewItemInput += new IPka.RequisPanel.TrixEventHandler(this.PanelRequisitesPhysNewItemInput);
            // 
            // FormDb
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(771, 435);
            this.Controls.Add(this.buttonColour);
            this.Controls.Add(this.buttonClients);
            this.Controls.Add(this.panelRequisitesLegal);
            this.Controls.Add(this.buttonReceivedClear);
            this.Controls.Add(this.buttonReceivedCalendar);
            this.Controls.Add(this.textBoxDateReceived);
            this.Controls.Add(this.labelDateReceived);
            this.Controls.Add(this.radioButtonLegal);
            this.Controls.Add(this.radioButtonPhysical);
            this.Controls.Add(this.buttonCreatedClear);
            this.Controls.Add(this.buttonCreatedCalendar);
            this.Controls.Add(this.labelRequestNumber);
            this.Controls.Add(this.labelPatentNumber);
            this.Controls.Add(this.textBoxPatentNumber);
            this.Controls.Add(this.textBoxRequestNumber);
            this.Controls.Add(this.textBoxDateCreated);
            this.Controls.Add(this.labelDateCreated);
            this.Controls.Add(this.panelRequisitesPhysical);
            this.Controls.Add(this.textBoxProjectName);
            this.Controls.Add(this.labelProjectName);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormDb";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Изменить заявку";
            this.Load += new System.EventHandler(this.FormDbLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelProjectName;
        private System.Windows.Forms.TextBox textBoxProjectName;
        private RequisPanel panelRequisitesPhysical;
        private System.Windows.Forms.Button buttonCreatedClear;
        private System.Windows.Forms.Button buttonCreatedCalendar;
        private System.Windows.Forms.Label labelRequestNumber;
        private System.Windows.Forms.Label labelPatentNumber;
        private System.Windows.Forms.TextBox textBoxPatentNumber;
        private System.Windows.Forms.TextBox textBoxRequestNumber;
        private System.Windows.Forms.TextBox textBoxDateCreated;
        private System.Windows.Forms.Label labelDateCreated;
        private System.Windows.Forms.RadioButton radioButtonPhysical;
        private System.Windows.Forms.RadioButton radioButtonLegal;
        private System.Windows.Forms.Button buttonReceivedClear;
        private System.Windows.Forms.Button buttonReceivedCalendar;
        private System.Windows.Forms.TextBox textBoxDateReceived;
        private System.Windows.Forms.Label labelDateReceived;
        private RequisPanel panelRequisitesLegal;
        private System.Windows.Forms.Button buttonClients;
        private System.Windows.Forms.Button buttonColour;
    }
}