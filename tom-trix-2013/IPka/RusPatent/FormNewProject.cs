using System;
using System.Windows.Forms;
using Calculator;

namespace IPka
{
    public partial class FormNewProject : Form
    {
        private readonly ProjectTypePanel _projectTypePanel;

        private const string InputName = @"Введите имя проекта";
        private const string InputType = @"Введите тип проекта";
        private const string WarningWrongInternal = @"Внутренний код содержит недопустимые знаки. Возможно, не выбран корректный тип проекта";

        public FormNewProject()
        {
            InitializeComponent();
            _projectTypePanel = new ProjectTypePanel(null);
            Controls.Add(_projectTypePanel);
        }

        private void FormNewProjectLoad(object sender, EventArgs e)
        {
            _projectTypePanel.RefreshCombobox();
        }

        private void ButtonOkClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxProjectName.Text)) { MessageBox.Show(InputName, FormMain.Warning); textBoxProjectName.Focus(); return; }
            if (string.IsNullOrWhiteSpace(_projectTypePanel.GetInternalCodeTrim())) { MessageBox.Show(InputType, FormMain.Warning); return; }
            if (_projectTypePanel.GetInternalCodeTrim().Contains("?")) { MessageBox.Show(WarningWrongInternal, FormMain.Warning); return; }
            if (!FormDb.FillTheMainTable(true, new ParametersDb { IsLegal = false, ProjectName = textBoxProjectName.Text.Trim() }, _projectTypePanel.GetInternalCodeTrim()))
            {
                DialogResult = DialogResult.Retry; 
                Close();
                return;
            }
            FormDb.ChangingDirectoriesIsSuccessful(null, _projectTypePanel.GetInternalCodeTrim(), null, textBoxProjectName.Text.Trim());
            //добавление корреспонденции "Материалы"
            FormReportStart.AddNewCorrespondence(FormMain.FirstCorrespondence, DateTime.Now.Subtract(TimeSpan.FromDays(1)), false, null, int.Parse(TrixOrm.GetInstance().GetScalar("SELECT IDENT_CURRENT('t_main')").ToString()), null, null, _projectTypePanel.GetInternalCodeTrim(), textBoxProjectName.Text.Trim(), null);
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
