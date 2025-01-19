// ============== ПРИМЕЧАНИЕ ==============
// Настройка "Разрешать отправлять в любом случае"
// невидима ПО ПРОСЬБЕ ЗАКАЗЧИКА!
// Поэтому через интерфейс настройка недоступна
// (возможно только через ini-файл)
//=========================================

using System;
using System.Windows.Forms;
using Calculator;

namespace IPka
{
    public partial class FormSettings : Form
    {
        #region //constants
        private const string Days1 = @"день";
        private const string Days234 = @"дня";
        private const string Days567890 = @"дней";
        #endregion

        #region //constructor
        public FormSettings()
        {
            InitializeComponent();
            //номер символа, с которого начинается сортировка по внутреннему коду
            checkBoxSort.Checked = FormMain.SortThirdSymbol;
            //число дней до deadline, с которого следует выдавать предупреждения
            try { numericUpDownWarn.Value = FormMain.WarnDeadline; }
            catch { numericUpDownWarn.Value = 4; }
            //суммарная длина внутреннего кода
            try { numericUpDownCodeLength.Value = FormMain.CodeLength + 2; }
            catch { numericUpDownCodeLength.Value = 6; }
            //надо ли показывать кнопки Удалить и Отчёт?
            checkBoxHideButtons.Checked = FormMain.ShowDeleteAndReport;
            //принудительно сохранять в Word2003
            checkBoxUseWord2003.Checked = FormMain.UseWord2003;
            //следует ли показывать кнопку "С" (очистка дат)?
            checkBoxShowC.Checked = FormMain.ShowC;
            //следует ли показывать архивные записи?
            checkBoxShowArchive.Checked = FormMain.ShowArchive;
            //тип корреспонденции, после появления которой следует запросить {внутр. код}
            //тип корреспонденции, после появления которой следует запросить {№ патента}
            foreach (var t in TrixOrm.GetInstance().GetArray("SELECT RTRIM(corrname) FROM d_correspondence WHERE is_incoming = 'True'"))
            {
                comboBoxRequestCorr.Items.Add(t);
                comboBoxPatentCorr.Items.Add(t);
            }
            //тип корреспонденции, который создаётся по умолчанию при добавлении нового проекта
            //тип корреспонденции, которая может отправляться ВСЕГДА
            foreach (var t in TrixOrm.GetInstance().GetArray("SELECT RTRIM(corrname) FROM d_correspondence WHERE is_incoming = 'False'"))
            {
                comboBoxFirstCorr.Items.Add(t);
                comboBoxSuperCorr.Items.Add(t);
            }
            //выбор либо первого, либо текущего
            comboBoxRequestCorr.SelectedItem = @"...";
            comboBoxRequestCorr.SelectedItem = FormMain.RequestCorrespondence;
            comboBoxPatentCorr.SelectedItem = @"...";
            comboBoxPatentCorr.SelectedItem = FormMain.PatentCorrespondence;
            comboBoxFirstCorr.SelectedItem = @" ";
            comboBoxFirstCorr.SelectedItem = FormMain.FirstCorrespondence;
            comboBoxSuperCorr.SelectedItem = @" ";
            comboBoxSuperCorr.SelectedItem = FormMain.SuperCorrespondence;
        }
        #endregion

        #region //common handlers
        private void NumericUpDownWarnValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownWarn.Value % 100 >= 11 && numericUpDownWarn.Value % 100 <= 14) labelDays.Text = Days567890;
            else if (numericUpDownWarn.Value % 10 == 1) labelDays.Text = Days1;
            else if (numericUpDownWarn.Value % 10 >= 2 && numericUpDownWarn.Value % 10 <= 4) labelDays.Text = Days234;
            else labelDays.Text = Days567890;
        }

        private void ButtonOkClick(object sender, EventArgs e)
        {
            FormMain.SortThirdSymbol = checkBoxSort.Checked;
            FormMain.WarnDeadline = int.Parse(numericUpDownWarn.Value.ToString());
            FormMain.ShowDeleteAndReport = checkBoxHideButtons.Checked;
            FormMain.UseWord2003 = checkBoxUseWord2003.Checked;
            FormMain.ShowC = checkBoxShowC.Checked;
            FormMain.ShowArchive = checkBoxShowArchive.Checked;
            FormMain.CodeLength = int.Parse(numericUpDownCodeLength.Value.ToString()) - 2;
            FormMain.RequestCorrespondence = comboBoxRequestCorr.Text.Trim();
            FormMain.PatentCorrespondence = comboBoxPatentCorr.Text.Trim();
            FormMain.FirstCorrespondence = comboBoxFirstCorr.Text.Trim();
            FormMain.SuperCorrespondence = comboBoxSuperCorr.Text.Trim();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
        #endregion
    }
}
