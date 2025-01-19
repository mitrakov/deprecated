// ReSharper disable EmptyGeneralCatchClause
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Calculator;

namespace IPka
{
    public partial class FormReportStart : Form
    {
        #region //constants
        private const string ErrorAlreadyExists = "Для данной заявки уже существует '{0}'\n\n  1) задайте другой тип\n  2) укажите, что тип допускает более 1 экземпляра (Данные -> Редактировать словари -> Корреспонденция)";
        private const string ErrorNoDcorr = "Не найден тип корреспонденции '{0}'";
        private const string ErrorNoIncoming = "Ошибка: нет корреспонденции, на которую можно было бы ответить письмом '{0}'";
        private const string WarnDelete = @"Сгенерирован новый документ. Удалить старый (в корзину)?";
        private const string InviteCreated = @"Введите номер заявки Роспатента";
        private const string InviteCreatedHeader = @"Номер заявки Роспатента";
        private const string InviteReceived = @"Введите номер патента/свидетельства";
        private const string InviteReceivedHeader = @"Номер патента/свидетельства";
        #endregion

        #region //variables
        private readonly int _ekMain;                               //первичный ключ таблицы t_main
        private readonly int? _pkTcorr;                             //первичный ключ таблицы t_correspondence
        private readonly string _internalcode, _projectname;        //параметры из таблицы t_main
        private string _oldcorrname, _oldCorrShortname;             //старые имя корреспонденции и сокращение (в результате конт. пополнения могут появиться и новые)
        private readonly bool _isIncoming;                          //входящая или исходящая корреспонденция
        #endregion

        #region //static methods
        public static void AddNewCorrespondence(string kindOfCorrespondence, DateTime dateOfCorrespondence, bool isIncoming, int? pkTcorr, int ekMain, string oldcorrname, string comment, string internalcode, string projectname, string oldCorrShortname)
        {
            //проверки и обработка параметров
            if (String.IsNullOrWhiteSpace(kindOfCorrespondence)) return;
            var kind = kindOfCorrespondence.Trim();
            var comm = comment != null ? comment.Trim().Substring(0, Math.Min(255, comment.Trim().Length)) : "";
            //берём параметры корреспонденции
            var cortage = TrixOrm.GetInstance().GetCortage(String.Format("SELECT pk, RTRIM(template), RTRIM(corrshortname), is_only FROM d_correspondence WHERE corrname = '{0}'", kind));
            object ekCorr;
            try { ekCorr = cortage[0]; }
            catch { MessageBox.Show(String.Format(ErrorNoDcorr, kind)); return; }
            var template = cortage[1];
            var corrShortname = cortage[2];
            var isOnly = (bool)cortage[3];
            //нельзя добавлять "ответ на запрос" без наличия "запроса" и т.д.
            if (int.Parse(TrixOrm.GetInstance().GetScalar(String.Format("SELECT COUNT(*) FROM d_correspondence WHERE pk = {0} AND pk IN (SELECT ik_deactivator FROM d_correspondence)", ekCorr)).ToString()) > 0)         //косячная корреспонденция... надо убедиться, что имеется экземпляр входящей, прежде чем создавать исходящую
                if (int.Parse(TrixOrm.GetInstance().GetScalar(String.Format("SELECT COUNT(*) FROM t_correspondence INNER JOIN d_correspondence ON t_correspondence.ek_corr = d_correspondence.pk WHERE ek_main = {0} AND ik_deactivator = {1}", ekMain, ekCorr)).ToString()) == 0) { MessageBox.Show(String.Format(ErrorNoIncoming, kind)); return; }
            //вычисление даты (либо тупо берём из календаря (для входящей и ИЗМЕНЕНИЯ исходящей), либо по-хитрому вычисляем (для ДОБАВЛЕНИЯ исходящей))
            object date;
            if (isIncoming || pkTcorr != null)
                date = dateOfCorrespondence.ToShortDateString();
            else
                date = TrixOrm.GetInstance().GetScalar(String.Format("SELECT value FROM t_correspondence WHERE ek_main = {0} AND ek_corr IN (SELECT pk FROM d_correspondence WHERE ik_deactivator = {1}) ORDER BY value DESC", ekMain, ekCorr)) ?? dateOfCorrespondence.ToShortDateString();
            //проверка на наличие документа (а вдруг он д.б. единственным?)
            if (kind != (oldcorrname ?? "").Trim() && isOnly)
                if (int.Parse(TrixOrm.GetInstance().GetScalar(String.Format("SELECT COUNT(*) FROM t_correspondence WHERE ek_main = {0} AND ek_corr = {1}", ekMain, ekCorr)).ToString()) > 0) { MessageBox.Show(String.Format(ErrorAlreadyExists, kind)); return; }
            //добавляем запись в БД
            TrixOrm.GetInstance().Execute(String.Format(pkTcorr == null ? "INSERT INTO t_correspondence (ek_main, ek_corr, is_sent, value, comment) VALUES ({0}, {1}, 'False', '{2}', '{3}')" : "UPDATE t_correspondence SET ek_corr = {1}, value = '{2}', comment = '{3}' WHERE pk = {4}", ekMain, ekCorr, date.ToString().Replace('.', '/'), comm, pkTcorr));
            //получаем первичный ключ в t_correspondence
            var pk = pkTcorr ?? TrixOrm.GetInstance().GetScalar(@"SELECT IDENT_CURRENT('t_correspondence')");
            //генерируем отчёт
            if (!isIncoming && kind != oldcorrname)             //либо поменялся тип корреспонденции, либо _oldcorrname == null (новая корреспонденция)
            {
                //создаём папку в любом случае
                var destination = String.Format(@"{0}\{1}\{2}_{3}\{4}\{2}_{5}_{6}",
                                                Directory.GetCurrentDirectory(),
                                                FormMain.FolderDocuments,
                                                internalcode,
                                                projectname,
                                                FormMain.CorrOutcoming,
                                                corrShortname,
                                                pk);
                if (!Directory.Exists(destination)) Directory.CreateDirectory(destination);
                //проверяем, определён ли шаблон
                if (!String.IsNullOrWhiteSpace(template.ToString()))
                {
                    //создаём новый
                    FormMain.GenerateReportStarter(ekMain, template.ToString(), destination, String.Format(@"{0}_{1}_{2}", internalcode, corrShortname, pk));
                    //удаляем старый
                    if (pkTcorr != null && MessageBox.Show(WarnDelete, FormMain.Warning, MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                        try { FormMain.RemoveTheCorrItem(pk.ToString(), internalcode, projectname, oldCorrShortname, false); }
                        catch { }
                }
            }
            //были добавлены специальные типы корреспонденции - надо обновить таблицу t_main
            if (kind == FormMain.RequestCorrespondence)
                SpecialCorrespondence(InviteCreated, InviteCreatedHeader, "request_number", "date_created", dateOfCorrespondence, ekMain);
            if (kind == FormMain.PatentCorrespondence)
                SpecialCorrespondence(InviteReceived, InviteReceivedHeader, "patent_number", "date_received", dateOfCorrespondence, ekMain);
        }

        private static void SpecialCorrespondence(string label, string header, string paramText, string paramDate, DateTime datevalue, int ekMain)
        {
            var frm = FormMain.CreateInputBox(label, header);
            frm.ShowDialog();
            if (String.IsNullOrWhiteSpace(frm.Tag.ToString())) return;
            TrixOrm.GetInstance().Execute(String.Format("UPDATE t_main SET {0} = '{2}', {1} = '{3}' WHERE pk = {4}", paramText, paramDate, frm.Tag.ToString().Trim(), datevalue.ToShortDateString().Replace('.', '/'), ekMain));
        }
        #endregion

        #region //constructor
        public FormReportStart(int ekMain, string internalCode, string projectName, bool isIncoming, DateTime? minDate = null, string oldCorrname = null, string oldCorrShortname = null, int? pkTcorr = null, DateTime? oldDate = null, string comment = null)
        {
            InitializeComponent();
            _ekMain = ekMain;
            _internalcode = internalCode.Trim();
            _projectname = projectName.Trim();
            _isIncoming = isIncoming;
            _oldcorrname = oldCorrname;
            _oldCorrShortname = oldCorrShortname;
            _pkTcorr = pkTcorr;
            textBoxComment.Text = comment;
            monthCalendarDate.Visible = _isIncoming || pkTcorr != null;
            monthCalendarDate.SelectionStart = oldDate ?? DateTime.Now;
            monthCalendarDate.MinDate = minDate ?? DateTime.MinValue;
        }
        #endregion

        #region // handlers
        private void FormReportStartLoad(object sender, EventArgs e)
        {
            //заполнение данными
            comboBoxCorrespondence.Items.Clear();   //нужно обязательно
            foreach (var t in TrixOrm.GetInstance().GetArray(String.Format("SELECT RTRIM(corrname) FROM d_correspondence WHERE is_incoming = '{0}'", _isIncoming)))
                comboBoxCorrespondence.Items.Add(t);
            //установка первого элемента
            if (comboBoxCorrespondence.Items.Count > 0)
                if (_oldcorrname != null)
                    comboBoxCorrespondence.Text = _oldcorrname.Trim();
                else
                    comboBoxCorrespondence.SelectedIndex = 0;
        }

        private void ComboBoxCorrespondenceValidated(object sender, EventArgs e)
        {
            //проверки
            if (String.IsNullOrWhiteSpace(comboBoxCorrespondence.Text)) { if (comboBoxCorrespondence.Items.Count > 0) comboBoxCorrespondence.SelectedIndex = 0; return; }
            ////if (comboBoxCorrespondence.Text.Trim() == _oldcorrname) { return; }
            //////пора добавить request_number + date_created
            ////if (comboBoxCorrespondence.Text.Trim() == FormMain.RequestCorrespondence)
            ////    SpecialCorrespondenceComes(InviteCreated, InviteCreatedHeader, "request_number", "date_created");
            //////пора добавить patent_number + date_received
            ////if (comboBoxCorrespondence.Text.Trim() == FormMain.PatentCorrespondence)
            ////    SpecialCorrespondenceComes(InviteReceived, InviteReceivedHeader, "patent_number", "date_received");
            //отмена каскадки, если там приемлемое значение
            if (comboBoxCorrespondence.Items.Cast<string>().Any(t => t.ToUpper().Trim() == comboBoxCorrespondence.Text.ToUpper().Trim())) return;
            //выполнение контекстного пополнения словарей
            var frm = new FormCorrespondence(comboBoxCorrespondence.Text.Trim(), true, _isIncoming);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                _oldcorrname = comboBoxCorrespondence.Text.Trim();
                _oldCorrShortname = ((string[])frm.Tag)[1];
                FormReportStartLoad(this, null);
            }
        }
        #endregion

        #region // handlers (buttons)
        private void ButtonLetsGoClick(object sender, EventArgs e)
        {
            //проверки
            if (String.IsNullOrWhiteSpace(comboBoxCorrespondence.Text)) return;
            //ниже не извращенство - просто надо, чтобы метод добавления заявки был static (для вызова из другого места)
            AddNewCorrespondence(comboBoxCorrespondence.Text, monthCalendarDate.SelectionStart, _isIncoming, _pkTcorr, _ekMain, _oldcorrname, textBoxComment.Text, _internalcode, _projectname, _oldCorrShortname);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonBackClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
        #endregion
    }
}
