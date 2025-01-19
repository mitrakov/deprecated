// ReSharper disable EmptyGeneralCatchClause
// ReSharper disable PossibleNullReferenceException
// =============== примечание ======================
//ComboText предназначен для хранения данных по типу Dictionary, т.е. пар "key-value". В случае корреспонденции это 
//не так, поэтому стандартный алгоритм для UPDATE не работает (лишь ключа недостаточно для выбора кортежа). Мы 
//вынуждены хранить первичный ключ (в Tag'e), т.к. естественного первичного ключа в таблице просто напросто нет
// =================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using Calculator;

namespace IPka
{
    public partial class FormDb : Form
    {
        #region //controls
        private readonly ProjectTypePanel _projectTypePanel;
        #endregion

        #region //constants
        private const string WarningInternal = @"Внутренний код пуст. Выберите тип проекта";
        private const string WarningWrongInternal = @"Внутренний код содержит недопустимые знаки. Возможно, не выбран корректный тип проекта";
        private const string WarningProjectName = @"Введите название проекта";
        private const string Error = "Ошибка БД. Операция отменена. \n\nВозможные причины ошибки: \n1) Повторяющиеся данные в уникальных полях \n2) Слишком длинное строковое имя";
        private const string HellError = "Ошибка при откате на начало транзакции. Путь к каталогу '{0}', вероятно, будет потерян...\nПереименуйте каталог вручную";
        private const string Caption = @"Свойства проекта '{0}' (№ {1})";
        private const string ColourAutoText = @"Авто";
        #endregion

        #region //variables
        private readonly ParametersDb _pars;
        private FormClients _frm = new FormClients(true);
        #endregion

        #region //constructor
        public FormDb(ParametersDb pars)
        {
            InitializeComponent();
            _pars = pars;
            _projectTypePanel = new ProjectTypePanel(_pars.Pk);
            Controls.Add(_projectTypePanel);
            MouseWheel += FormDbMouseWheel;
        }
        #endregion

        #region //static methods
        public static bool FillTheMainTable(bool insert, ParametersDb pars, string internalcode)
        {
            //заполнение таблицы (t_main)
            //в режиме изменения передавать _pars, в режиме добавления передавать новый объект класса
            var cmd = String.Format(insert
                        ? "INSERT INTO t_main (projectname, request_number, date_created, patent_number, is_legal, project_type, date_received, client_code) VALUES ('{0}', '{1}', {2}, '{3}', '{4}', '{5}', {6}, {7})"
                        : "UPDATE t_main SET projectname = '{0}', request_number = '{1}', date_created = {2}, patent_number = '{3}', is_legal = '{4}', project_type = '{5}', date_received = {6}, client_code = {7}, colour = {8} WHERE pk = {9}",
                    pars.ProjectName,
                    pars.RequestNumber,
                    String.IsNullOrWhiteSpace(pars.DateCreated) ? "NULL" : "'" + pars.DateCreated.Replace('.', '/') + "'",
                    pars.PatentNumber,
                    pars.IsLegal,
                    internalcode.Substring(0, 2),
                    String.IsNullOrWhiteSpace(pars.DateReceived) ? "NULL" : "'" + pars.DateReceived.Replace('.', '/') + "'",
                    String.IsNullOrWhiteSpace(pars.ClientCode) ? "NULL" : pars.ClientCode,
                    pars.ColourType,
                    pars.Pk);
            if (TrixOrm.GetInstance().Execute(cmd) < 0) { MessageBox.Show(Error, FormMain.Warning); return false; } //срабатывает в случае нарушения уникальности индексного ключа или длинных строк
            return true;
        }

        public static bool ChangingDirectoriesIsSuccessful(string oldinternalcode, string newinternalcode, string oldprojectname, string newprojname)
        {
            //переименование файлов и подкаталогов (если oldinternalcode <> NULL)
            if (!String.IsNullOrWhiteSpace(oldinternalcode))
                return FormMain.RenameDirectories(oldinternalcode.Trim(), newinternalcode.Trim(), oldprojectname.Trim(), newprojname.Trim());
            //создание каталогов (здесь oldinternalcode == NULL)
            var s = String.Format(@"{0}\{1}\{2}_{3}\", Directory.GetCurrentDirectory(), FormMain.FolderDocuments, newinternalcode.Trim(), newprojname.Trim());
            var incomingDir = s + FormMain.CorrIncoming;
            var outcomingDir = s + FormMain.CorrOutcoming;
            var matDir = s + FormMain.FolderMaterials;
            if (!Directory.Exists(incomingDir)) Directory.CreateDirectory(incomingDir);
            if (!Directory.Exists(outcomingDir)) Directory.CreateDirectory(outcomingDir);
            if (!Directory.Exists(matDir)) Directory.CreateDirectory(matDir);
            return true;
        }

        private static Dictionary<string, string> CreateRequisitesDictionary(string clientCode, bool isLegal)
        {
            //получение словаря реквизитов (key = название реквизита, value = значение реквизита)
            return TrixOrm.GetInstance().GetListOfCortages(String.Format("SELECT DISTINCT requisname, value FROM t_requisites INNER JOIN d_requisites ON ek_requis = d_requisites.pk WHERE client_code = {0} AND is_legal = '{1}' UNION SELECT requisname, '' AS trix FROM d_requisites WHERE is_legal = '{1}' AND requisname NOT IN (SELECT requisname FROM t_requisites INNER JOIN d_requisites ON ek_requis = d_requisites.pk WHERE client_code = {0} AND is_legal = '{1}')", !String.IsNullOrWhiteSpace(clientCode) ? clientCode : @"-1", isLegal)).ToDictionary(t => t[0].ToString(), t => t[1].ToString());
        }
        #endregion

        #region //private methods
        private void AddNewRequisite(string requisname, bool isLegal)
        {
            //добавление нового реквизита и обновление панели
            if (TrixOrm.GetInstance().Execute(String.Format("INSERT INTO d_requisites (requisname, is_legal) VALUES ('{0}', '{1}')", requisname, isLegal)) < 0)
                MessageBox.Show(Error);
            if (isLegal) panelRequisitesLegal.ResetDictionary(CreateRequisitesDictionary(_pars.ClientCode, true));
            else panelRequisitesPhysical.ResetDictionary(CreateRequisitesDictionary(_pars.ClientCode, false));
        }
        #endregion

        #region //my handlers
        private void PanelRequisitesPhysNewItemInput(object sender, string e)
        {
            AddNewRequisite(e, false);
        }

        private void PanelRequisitesLegalNewItemInput(object sender, string e)
        {
            AddNewRequisite(e, true);
        }
        #endregion

        #region //common handlers
        private void FormDbLoad(object sender, EventArgs e)
        {
            //изменение формы (_pars не м.б. = null)
            _projectTypePanel.SetToComboBox(_pars.ProjectTypeText);
            textBoxProjectName.Text = _pars.ProjectName;
            radioButtonLegal.Checked = _pars.IsLegal;
            textBoxRequestNumber.Text = _pars.RequestNumber;
            textBoxPatentNumber.Text = _pars.PatentNumber;
            textBoxDateCreated.Text = String.IsNullOrWhiteSpace(_pars.DateCreated) ? "" : DateTime.Parse(_pars.DateCreated).ToShortDateString();
            textBoxDateReceived.Text = String.IsNullOrWhiteSpace(_pars.DateReceived) ? "" : DateTime.Parse(_pars.DateReceived).ToShortDateString();
            Text = String.Format(Caption, _pars.ProjectName, FormMain.GetInternalCode(_pars.ProjectTypeText, _pars.Pk.ToString(), false, FormMain.CodeLength));
            buttonColour.BackColor = _pars.ColourType == 3 ? Color.Yellow : _pars.ColourType == 4 ? Color.Red : Color.Transparent;
            buttonColour.Text = _pars.ColourType < 3 ? ColourAutoText : @"";
            //показываем или скрываем кнопки "С"
            buttonCreatedClear.Visible = buttonReceivedClear.Visible = FormMain.ShowC;
            //заполнение комбобокса с типами проектов
            _projectTypePanel.RefreshCombobox();
            //обновление панелей реквизитов  
            panelRequisitesPhysical.ResetDictionary(CreateRequisitesDictionary(_pars.ClientCode, false));
            panelRequisitesLegal.ResetDictionary(CreateRequisitesDictionary(_pars.ClientCode, true));
            //делаем одну из них видимой
            RadioButtonPhysicalCheckedChanged(this, null);
            //подгоняем размер панелей и формы
            panelRequisitesPhysical.Width = panelRequisitesPhysical.Controls.Cast<Control>().Max(t => t.Right) + FormMain.CommonMargin * 2;
            panelRequisitesLegal.Width = panelRequisitesLegal.Controls.Cast<Control>().Max(t => t.Right) + FormMain.CommonMargin * 2;
            Width = Controls.Cast<Control>().Max(t => t.Right) + FormMain.CommonMargin;
        }

        private void RadioButtonPhysicalCheckedChanged(object sender, EventArgs e)
        {
            panelRequisitesPhysical.Visible = radioButtonPhysical.Checked;
            panelRequisitesLegal.Visible = radioButtonLegal.Checked;
        }

        void FormDbMouseWheel(object sender, MouseEventArgs e)
        {
            panelRequisitesPhysical.ScrollControlIntoView(e.Delta < 0 ? panelRequisitesPhysical.Controls[panelRequisitesPhysical.Controls.Count - 1] : panelRequisitesPhysical.Controls[0]);
            panelRequisitesLegal.ScrollControlIntoView(e.Delta < 0 ? panelRequisitesLegal.Controls[panelRequisitesLegal.Controls.Count - 1] : panelRequisitesLegal.Controls[0]);
        }
        #endregion

        #region //common handlers (buttons)
        private void ButtonOkClick(object sender, EventArgs e)
        {
            //проверки
            if (String.IsNullOrWhiteSpace(_projectTypePanel.GetInternalCodeTrim())) { MessageBox.Show(WarningInternal, FormMain.Warning); return; }
            if (String.IsNullOrWhiteSpace(textBoxProjectName.Text)) { MessageBox.Show(WarningProjectName, FormMain.Warning); textBoxProjectName.Focus(); return; }
            if (_projectTypePanel.GetInternalCodeTrim().Contains('?')) { MessageBox.Show(WarningWrongInternal, FormMain.Warning); return; }
            //создаём и/или переименовываем каталоги (ВЫЗВАТЬ ДО ИЗМЕНЕНИЯ В БАЗЕ ДАННЫХ !!! На случай, если каталог нельзя удалить ввиду заблокирования др. процессом)
            if (!ChangingDirectoriesIsSuccessful(FormMain.GetInternalCode(_pars.ProjectTypeText.Trim(), _pars.Pk.ToString(), false, FormMain.CodeLength), _projectTypePanel.GetInternalCodeTrim(), _pars.ProjectName, textBoxProjectName.Text.Trim())) return;
            //вычисляем код клиента
            var clientCode = _frm.Tag ?? (_pars != null ? _pars.ClientCode : @"");
            if (String.IsNullOrWhiteSpace(clientCode.ToString()))
            {
                var s = TrixOrm.GetInstance().GetScalar("SELECT MAX(client_code) FROM t_requisites");
                clientCode = s != null ? int.Parse(s.ToString()) + 1 : 1;
            }
            //заполнение таблицы Main (выполняет статический метод)
            if (!FillTheMainTable(false, new ParametersDb { ProjectName = textBoxProjectName.Text.Trim(), RequestNumber = textBoxRequestNumber.Text.Trim(), DateCreated = textBoxDateCreated.Text.Trim(), DateReceived = textBoxDateReceived.Text.Trim(), PatentNumber = textBoxPatentNumber.Text.Trim(), IsLegal = radioButtonLegal.Checked, Pk = _pars != null ? _pars.Pk : int.MinValue, ClientCode = clientCode.ToString(), ColourType = buttonColour.BackColor == Color.Red ? 4 : buttonColour.BackColor == Color.Yellow ? 3 : 0 }, _projectTypePanel.GetInternalCodeTrim()))
            {
                //если косяк, то переименовываем каталоги в прежнее состояние
                if (!ChangingDirectoriesIsSuccessful(_projectTypePanel.GetInternalCodeTrim(), FormMain.GetInternalCode(_pars.ProjectTypeText.Trim(), _pars.Pk.ToString(), false, FormMain.CodeLength), textBoxProjectName.Text.Trim(), _pars.ProjectName))
                    MessageBox.Show(String.Format(HellError, _pars.ProjectName));
                return;
            }
            //заполнение таблицы (t_requisites)
            var dictOfValues = new Dictionary<object, string>(); //требуется словарь Dictionary<ek_requis, value>, т.к. объединение 2 словарей Dictionary<requisname, value> может вызвать ошибку
            foreach (var t in panelRequisitesPhysical.GetDictionary())
            {
                if (String.IsNullOrWhiteSpace(t.Value)) continue;
                dictOfValues.Add(TrixOrm.GetInstance().GetScalar(String.Format("SELECT pk FROM d_requisites WHERE requisname = '{0}' AND is_legal = 'false'", t.Key)), t.Value);
            }
            foreach (var t in panelRequisitesLegal.GetDictionary())
            {
                if (String.IsNullOrWhiteSpace(t.Value)) continue;
                dictOfValues.Add(TrixOrm.GetInstance().GetScalar(String.Format("SELECT pk FROM d_requisites WHERE requisname = '{0}' AND is_legal = 'true'", t.Key)), t.Value);
            }
            foreach (var t in dictOfValues)
            {
                if (TrixOrm.GetInstance().Execute(String.Format("UPDATE t_requisites SET value = '{2}' WHERE client_code = {0} AND ek_requis = {1}", clientCode, t.Key, t.Value)) == 0)
                    TrixOrm.GetInstance().Execute(String.Format("INSERT INTO t_requisites (client_code, ek_requis, value) VALUES ({0}, {1}, '{2}')", clientCode, t.Key, t.Value));
            }
            //удаление из таблицы t_requisites
            if (_pars != null)
            {
                foreach (var t in panelRequisitesPhysical.GetDictionary().Where(p => String.IsNullOrWhiteSpace(p.Value)))
                    TrixOrm.GetInstance().Execute(String.Format("DELETE FROM t_requisites WHERE client_code = {0} AND ek_requis IN (SELECT pk FROM d_requisites WHERE requisname = '{1}' AND is_legal = 'false')", _pars.ClientCode, t.Key));
                foreach (var t in panelRequisitesLegal.GetDictionary().Where(p => String.IsNullOrWhiteSpace(p.Value)))
                    TrixOrm.GetInstance().Execute(String.Format("DELETE FROM t_requisites WHERE client_code = {0} AND ek_requis IN (SELECT pk FROM d_requisites WHERE requisname = '{1}' AND is_legal = 'true')", _pars.ClientCode, t.Key));
            }
            //close the form
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ButtonCreatedCalendarClick(object sender, EventArgs e)
        {
            var frm = new FormCalendar(String.IsNullOrWhiteSpace(textBoxDateCreated.Text) ? (DateTime?)null : DateTime.Parse(textBoxDateCreated.Text), null);
            if (frm.ShowDialog() == DialogResult.OK)
                textBoxDateCreated.Text = ((DateTime)frm.Tag).ToShortDateString();
        }

        private void ButtonReceivedCalendarClick(object sender, EventArgs e)
        {
            var frm = new FormCalendar(String.IsNullOrWhiteSpace(textBoxDateReceived.Text) ? (DateTime?)null : DateTime.Parse(textBoxDateReceived.Text), null);
            if (frm.ShowDialog() == DialogResult.OK)
                textBoxDateReceived.Text = ((DateTime)frm.Tag).ToShortDateString();
        }

        private void ButtonCreatedClearClick(object sender, EventArgs e)
        {
            textBoxDateCreated.Text = @"";
        }

        private void ButtonReceivedClearClick(object sender, EventArgs e)
        {
            textBoxDateReceived.Text = @"";
        }

        private void ButtonClientsClick(object sender, EventArgs e)
        {
            _frm = new FormClients(radioButtonLegal.Checked);
            if (_frm.ShowDialog() != DialogResult.OK) return;
            panelRequisitesPhysical.ResetDictionary(CreateRequisitesDictionary(_frm.Tag.ToString(), false));
            panelRequisitesLegal.ResetDictionary(CreateRequisitesDictionary(_frm.Tag.ToString(), true));
        }

        private void ButtonColourClick(object sender, EventArgs e)
        {
            buttonColour.BackColor = buttonColour.BackColor == Color.Yellow ? Color.Red : buttonColour.BackColor == Color.Red ? Color.Transparent : Color.Yellow;
            buttonColour.Text = buttonColour.BackColor == Color.Transparent ? ColourAutoText : @"";
        }
        #endregion
    }
}
