// ReSharper disable MemberCanBeMadeStatic.Local
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Calculator;

namespace IPka
{
    public partial class FormCorrespondence : Form
    {
        #region //constants
        private const string EmptyName = @"Введите имя типа корреспонденции";
        private const string EmptyShortName = @"Введите сокращение (требуется при генерации отчётов)";
        private const string DbError = "Ошибка БД. Операция прервана. Возможные ошибки:\n\n 1) Возможно, такое имя или сокращение уже существуют\n 2) Слишком длинное строковое имя";
        private const string UniqueError = "Операция прервана. Ошибка в параметре 'Является единственной для одной заявки'\n\nВ БД уже существует более 1 экземпляра данного типа на заявку, а значит, он не может быть уникальным.\n\nУдалите лишние экземпляры и повторите попытку";
        #endregion

        #region //variables
        private readonly bool _insertmode;
        private string _oldshortname;
        #endregion

        #region //constructor
        public FormCorrespondence(string startName, bool insert = false, bool incoming = true, bool blockRadiobuttons = false)
        {
            InitializeComponent();
            _insertmode = insert;
            textBoxName.Text = startName.Trim();
            radioButtonOutcoming.Checked = !incoming;
            radioButtonIncoming.Enabled = radioButtonOutcoming.Enabled = blockRadiobuttons;
        }
        #endregion

        #region //handlers (Form)
        private void FormCorrespondenceLoad(object sender, EventArgs e)
        {
            //добавление данных в комбобокс
            comboBoxType.Items.Add(" ");
            foreach (var t in TrixOrm.GetInstance().GetArray(@"SELECT RTRIM(corrname) FROM d_correspondence WHERE is_incoming = 'False'"))
                comboBoxType.Items.Add(t.ToString());
            //наполнение формы в режиме изменения
            if (!_insertmode)
            {
                var cortage = TrixOrm.GetInstance().GetCortage(String.Format("SELECT RTRIM(d_correspondence.corrshortname), d_correspondence.is_incoming, d_correspondence.is_only, d_correspondence.months_deadline, d_correspondence.template, RTRIM(trix.corrname) FROM d_correspondence LEFT OUTER JOIN d_correspondence AS trix ON trix.pk = d_correspondence.ik_deactivator WHERE d_correspondence.corrname = '{0}'", textBoxName.Text));
                textBoxShortName.Text = cortage[0].ToString();
                _oldshortname = cortage[0].ToString();
                radioButtonOutcoming.Checked = !(bool)cortage[1];
                checkBoxOnly.Checked = (bool)cortage[2];
                Activate();
                comboBoxTemplates.Text = cortage[4].ToString().Trim();
                checkBoxResponse.Checked = false;
                if (!String.IsNullOrWhiteSpace(cortage[3].ToString()))
                {
                    checkBoxResponse.Checked = true;
                    numericUpDownResponse.Value = (int)cortage[3];
                    comboBoxType.SelectedItem = cortage[5];
                }
            }
            //активация/деактивация элементов
            RadioButtonIncomingCheckedChanged(this, null);
        }

        private void FormCorrespondenceActivated(object sender, EventArgs e)
        {
            //часто требуется обновление списков => use FormActivated event
            comboBoxTemplates.Items.Clear();
            //загрузка документов и шаблонов (загружаем по отдельности, т.к. searchPattern не поддерживает более 1 типа файла)
            foreach (var t in Directory.GetFiles(String.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), FormMain.FolderTemplates), @"*.dot").Select(Path.GetFileName).Where(w => w != null))
                comboBoxTemplates.Items.Add(t);
            foreach (var t in Directory.GetFiles(String.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), FormMain.FolderTemplates), @"*.doc").Select(Path.GetFileName).Where(w => w != null))
                comboBoxTemplates.Items.Add(t);
            //выбор первого
            if (comboBoxTemplates.Items.Count > 0)
                comboBoxTemplates.SelectedIndex = 0;
        }
        #endregion

        #region //handlers (Buttons)
        private void ButtonOkClick(object sender, EventArgs e)
        {
            //проверки
            if (String.IsNullOrWhiteSpace(textBoxName.Text)) { MessageBox.Show(EmptyName, FormMain.Warning); textBoxName.Focus(); return; }
            if (String.IsNullOrWhiteSpace(textBoxShortName.Text)) { MessageBox.Show(EmptyShortName, FormMain.Warning); textBoxShortName.Focus(); return; }
            //проверка на уникальность писем
            if (!_insertmode && checkBoxOnly.Checked)
            {
                var x = TrixOrm.GetInstance().GetScalar(String.Format("SELECT COUNT(ek_main) FROM t_correspondence INNER JOIN d_correspondence ON ek_corr = d_correspondence.pk WHERE corrshortname = '{0}'", _oldshortname));
                var y = TrixOrm.GetInstance().GetScalar(String.Format("SELECT COUNT(DISTINCT ek_main) FROM t_correspondence INNER JOIN d_correspondence ON ek_corr = d_correspondence.pk WHERE corrshortname = '{0}'", _oldshortname));
                if (int.Parse(x.ToString()) > int.Parse(y.ToString())) { MessageBox.Show(UniqueError, FormMain.Warning); return; }
            }
            //получение первичного ключа деактиватора
            object ik = null;
            if (groupBoxResponse.Enabled && checkBoxResponse.Checked && !String.IsNullOrWhiteSpace(comboBoxType.Text))
                ik = TrixOrm.GetInstance().GetScalar(String.Format("SELECT pk FROM d_correspondence WHERE corrname = '{0}'", comboBoxType.Text));
            //получение кол-ва месяцев
            var months = groupBoxResponse.Enabled ? numericUpDownResponse.Value.ToString() : "NULL";
            //вставка/изменение таблицы (вы не поверите, но в роли ключа выступает corrshortname!)
            if (TrixOrm.GetInstance().Execute(String.Format(_insertmode
                                    ? "INSERT INTO d_correspondence (corrname, corrshortname, months_deadline, ik_deactivator, is_only, is_incoming, template) VALUES ('{0}', '{1}', {2}, {3}, '{4}', '{5}', '{6}')"
                                    : "UPDATE d_correspondence SET corrname = '{0}', corrshortname = '{1}', months_deadline = {2}, ik_deactivator = {3}, is_only = '{4}', is_incoming = '{5}', template = '{6}' WHERE corrshortname = '{7}'",
                                textBoxName.Text.Trim(), textBoxShortName.Text.Trim(), months, ik ?? @"NULL", checkBoxOnly.Checked, radioButtonIncoming.Checked, comboBoxTemplates.Text.Trim(), _oldshortname)) < 0) { MessageBox.Show(DbError, FormMain.Warning); return; }
            //запоминаем старое и новое сокращение в Tag (пригодится при переименовании файлов)
            Tag = new[] { _oldshortname, textBoxShortName.Text.Trim() };
            //закрываем всё
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ButtonExploreClick(object sender, EventArgs e)
        {
            var s = String.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), FormMain.FolderTemplates);
            if (Directory.Exists(s))
                Process.Start(s);
        }
        #endregion

        #region //handlers (Other)
        private void RadioButtonIncomingCheckedChanged(object sender, EventArgs e)
        {
            checkBoxResponse.Enabled =  radioButtonIncoming.Checked;
            groupBoxTemplates.Enabled = radioButtonOutcoming.Checked;
            if (!radioButtonIncoming.Checked)
                checkBoxResponse.Checked = false;
        }

        private void CheckBoxResponseCheckedChanged(object sender, EventArgs e)
        {
            groupBoxResponse.Enabled = checkBoxResponse.Checked;
        }
        #endregion
    }
}
