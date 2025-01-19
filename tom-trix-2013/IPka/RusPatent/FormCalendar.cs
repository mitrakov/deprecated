using System;
using System.Windows.Forms;

namespace IPka
{
    public partial class FormCalendar : Form
    {
        public FormCalendar(DateTime? startValue, DateTime? minValue)
        {
            InitializeComponent();
            monthCalendar1.SelectionStart = startValue ?? DateTime.Now;
            monthCalendar1.MinDate = minValue ?? DateTime.MinValue;
        }

        private void MonthCalendar1DateSelected(object sender, DateRangeEventArgs e)
        {
            Tag = monthCalendar1.SelectionStart;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
