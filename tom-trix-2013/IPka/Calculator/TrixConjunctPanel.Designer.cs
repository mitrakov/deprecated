namespace Calculator
{
    partial class TrixConjunctPanel
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

        #region Код, автоматически созданный конструктором компонентов

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBoxVariable = new System.Windows.Forms.ComboBox();
            this.comboBoxOperation = new System.Windows.Forms.ComboBox();
            this.comboBoxValue = new System.Windows.Forms.ComboBox();
            this.textBoxParameter = new System.Windows.Forms.TextBox();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonNew = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBoxVariable
            // 
            this.comboBoxVariable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxVariable.FormattingEnabled = true;
            this.comboBoxVariable.Location = new System.Drawing.Point(10, 10);
            this.comboBoxVariable.Name = "comboBoxVariable";
            this.comboBoxVariable.Size = new System.Drawing.Size(150, 21);
            this.comboBoxVariable.TabIndex = 0;
            this.comboBoxVariable.TextChanged += new System.EventHandler(this.ComboBoxVariableTextChanged);
            // 
            // comboBoxOperation
            // 
            this.comboBoxOperation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOperation.FormattingEnabled = true;
            this.comboBoxOperation.Location = new System.Drawing.Point(170, 10);
            this.comboBoxOperation.Name = "comboBoxOperation";
            this.comboBoxOperation.Size = new System.Drawing.Size(40, 21);
            this.comboBoxOperation.TabIndex = 0;
            this.comboBoxOperation.TextChanged += new System.EventHandler(this.ComboBoxOperationTextChanged);
            // 
            // comboBoxValue
            // 
            this.comboBoxValue.FormattingEnabled = true;
            this.comboBoxValue.Location = new System.Drawing.Point(220, 10);
            this.comboBoxValue.Name = "comboBoxValue";
            this.comboBoxValue.Size = new System.Drawing.Size(150, 21);
            this.comboBoxValue.TabIndex = 0;
            this.comboBoxValue.TextChanged += new System.EventHandler(this.ComboBoxValueTextChanged);
            // 
            // textBoxParameter
            // 
            this.textBoxParameter.Location = new System.Drawing.Point(380, 10);
            this.textBoxParameter.Name = "textBoxParameter";
            this.textBoxParameter.Size = new System.Drawing.Size(50, 20);
            this.textBoxParameter.TabIndex = 0;
            this.textBoxParameter.TextChanged += new System.EventHandler(this.TextBoxParameterTextChanged);
            // 
            // buttonDelete
            // 
            this.buttonDelete.BackColor = System.Drawing.Color.Crimson;
            this.buttonDelete.Location = new System.Drawing.Point(450, 10);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(20, 20);
            this.buttonDelete.TabIndex = 0;
            this.buttonDelete.Text = "X";
            this.buttonDelete.UseVisualStyleBackColor = false;
            this.buttonDelete.Click += new System.EventHandler(this.ButtonDeleteClick);
            // 
            // buttonNew
            // 
            this.buttonNew.Location = new System.Drawing.Point(480, 10);
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(20, 20);
            this.buttonNew.TabIndex = 0;
            this.buttonNew.UseVisualStyleBackColor = true;
            this.buttonNew.Click += new System.EventHandler(this.ButtonNewClick);
            // 
            // TrixConjunctPanel
            // 
            this.Controls.Add(this.comboBoxVariable);
            this.Controls.Add(this.comboBoxOperation);
            this.Controls.Add(this.comboBoxValue);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonNew);
            this.Controls.Add(this.textBoxParameter);
            this.Size = new System.Drawing.Size(510, 40);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ComboBox comboBoxVariable;
        public System.Windows.Forms.ComboBox comboBoxOperation;
        public System.Windows.Forms.ComboBox comboBoxValue;
        public System.Windows.Forms.TextBox textBoxParameter;
        public System.Windows.Forms.Button buttonDelete;
        public System.Windows.Forms.Button buttonNew;

    }
}
