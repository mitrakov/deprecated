using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Calculator
{
    public partial class TrixConjunctPanel : Panel
    {
        public event TrixEventHandler NewPressed;
        public event TrixEventHandler DeletePressed;
        public event TrixEventHandler SomethingChanged;
        public delegate void TrixEventHandler(TrixConjunctPanel sender, string e);

        public string PkRule { get; set; }
        public string PkConjunct { get; set; }

        public TrixConjunctPanel(string pkRule, string pkConjunct)
        {
            InitializeComponent();
            MyInitializer(pkRule, pkConjunct);
        }

        public TrixConjunctPanel(IContainer container, string pkRule, string pkConjunct)
        {
            container.Add(this);
            InitializeComponent();
            MyInitializer(pkRule, pkConjunct);
        }

        private void MyInitializer(string pkRule, string pkConjunct)
        {
            PkRule = pkRule;
            PkConjunct = pkConjunct;
            var list = TrixOrm.GetInstance().GetArray("SELECT text FROM calc_t_operands").ToArray();
            comboBoxVariable.Items.AddRange(list);
            comboBoxOperation.Items.AddRange(new[] { "< ", "<=", "= ", ">=", "> " });
            comboBoxValue.Items.AddRange(list);
        }

        private void ButtonNewClick(object sender, EventArgs e)
        {
            if (NewPressed != null) NewPressed(this, null);
        }

        private void ButtonDeleteClick(object sender, EventArgs e)
        {
            if (DeletePressed != null) DeletePressed(this, null);
        }

        private void ComboBoxVariableTextChanged(object sender, EventArgs e)
        {
            if (SomethingChanged != null) SomethingChanged(this, null);
        }

        private void ComboBoxOperationTextChanged(object sender, EventArgs e)
        {
            if (SomethingChanged != null) SomethingChanged(this, null);
        }

        private void ComboBoxValueTextChanged(object sender, EventArgs e)
        {
            if (SomethingChanged != null) SomethingChanged(this, null);
        }

        private void TextBoxParameterTextChanged(object sender, EventArgs e)
        {
            if (SomethingChanged != null) SomethingChanged(this, null);
        }
    }
}
