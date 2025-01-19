// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable EmptyGeneralCatchClause
// ReSharper disable AssignNullToNotNullAttribute
using System;
using System.Windows.Forms;

namespace Calculator
{
    public partial class TrixInputBox : Form
    {
        public TrixInputBox(string label = null, string formCaption = null, string text = null, bool isNumeric = false)
        {
            InitializeComponent();
            labelCaption.Text = label;
            Text = formCaption;
            textBoxInput.Text = text;
            try { numericUpDownInput.Value = int.Parse(text); }
            catch {}
            textBoxInput.SelectAll();
            textBoxInput.Visible = !isNumeric;
            numericUpDownInput.Visible = isNumeric;
            buttonOK.Enabled = isNumeric;
        }

        private void TextBoxInputTextChanged(object sender, EventArgs e)
        {
            buttonOK.Enabled = !String.IsNullOrWhiteSpace(textBoxInput.Text);
        }

        private void ButtonOkClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Tag = textBoxInput.Visible ? textBoxInput.Text.Trim() : numericUpDownInput.Value.ToString().Trim();
            Close();
        }

        private void ButtonCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
