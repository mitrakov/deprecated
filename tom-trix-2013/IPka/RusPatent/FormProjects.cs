// ReSharper disable EmptyGeneralCatchClause
using System;
using System.Windows.Forms;
using Calculator;

namespace IPka
{
    public partial class FormProjects : Form
    {
        #region //constants
        private const string Error = "Переименование не выполнено. Возможные причины ошибки:\n\n 1) Возможно, данное значение уже существует\n 2) Слишком длинное строковое имя";
        private const string Warning = "Внимание! В БД существуют проекты данного типа (всего {0})!\nОни будут также удалены!\nВы уверены, что хотите удалить тип проекта вместе со всеми экземплярами?";
        private const string WarningIo = "Внимание! Вы собираетесь изменить числовой код типа проекта, что вызовет переименование связанных файлов и каталогов.\n\nОБЯЗАТЕЛЬНО закройте все программы, использующие данные файлы и каталоги!";
        #endregion

        #region //constructor
        public FormProjects()
        {
            InitializeComponent();
        }
        #endregion

        #region //private methods
        private void RefreshListbox()
        {
            listBoxMain.Items.Clear();
            listBoxHelpUserCodes.Items.Clear();
            listBoxHelpShortnames.Items.Clear();
            foreach (var t in TrixOrm.GetInstance().GetListOfCortages("SELECT codename, usercode, codeshortname FROM d_icodes"))
            {
                listBoxMain.Items.Add(t[0]);
                listBoxHelpUserCodes.Items.Add(t[1]);         //второй листбокс невидимый
                listBoxHelpShortnames.Items.Add(t[2]);        //третий листбокс невидимый
            }
        }
        #endregion

        #region //handlers
        private void FormDictionariesLoad(object sender, EventArgs e)
        {
            RefreshListbox();
        }

        private void ListBoxMainSelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxMain.SelectedItem == null) return;
            textBoxType.Text = listBoxMain.SelectedItem.ToString();
            textBoxCode.Text = listBoxHelpUserCodes.Items[listBoxMain.SelectedIndex].ToString();
            textBoxShortname.Text = listBoxHelpShortnames.Items[listBoxMain.SelectedIndex].ToString();
            checkBoxShowInCalc.Checked = bool.Parse(TrixOrm.GetInstance().GetScalar(String.Format("SELECT show_in_calc FROM d_icodes WHERE codename = '{0}'", listBoxMain.SelectedItem)).ToString());
        }

        private void ButtonRenameClick(object sender, EventArgs e)
        {
            //проверки
            if (String.IsNullOrWhiteSpace(textBoxType.Text)) return;
            if (String.IsNullOrWhiteSpace(textBoxCode.Text)) return;
            if (String.IsNullOrWhiteSpace(textBoxShortname.Text)) return;
            if (listBoxMain.SelectedItem == null) return;
            //изменение типа проекта
            if (TrixOrm.GetInstance().Execute(String.Format("UPDATE d_icodes SET codename = '{0}', usercode = '{1}', codeshortname = '{2}' WHERE codename = '{3}'", textBoxType.Text.Trim(), textBoxCode.Text.Trim(), textBoxShortname.Text.Trim(), listBoxMain.SelectedItem)) < 0) { MessageBox.Show(Error); return; }
            //в случае изменения номера требуется обновить таблицу t_main и каталоги для файлов
            if (textBoxCode.Text.Trim() != listBoxHelpUserCodes.Items[listBoxMain.SelectedIndex].ToString().Trim())
            {
                MessageBox.Show(WarningIo, FormMain.Warning);
                //получаем старый и новый код (это КОД, а не имя!)
                var oldcode = listBoxHelpUserCodes.Items[listBoxMain.SelectedIndex].ToString().Trim();
                var newcode = textBoxCode.Text.Trim().Substring(0, Math.Min(2, textBoxCode.Text.Trim().Length));
                //каскадное изменение таблицы t_main
                TrixOrm.GetInstance().Execute(String.Format("UPDATE t_main SET project_type = '{0}' WHERE project_type = '{1}'", newcode, oldcode));
                //переименование каталогов
                foreach (var t in TrixOrm.GetInstance().GetListOfCortages(String.Format("SELECT pk, RTRIM(projectname) FROM t_main WHERE project_type = '{0}'", newcode)))
                    FormMain.RenameDirectories(FormMain.GetInternalCode(oldcode, t[0].ToString(), true, FormMain.CodeLength), FormMain.GetInternalCode(newcode, t[0].ToString(), true, FormMain.CodeLength), t[1].ToString(), t[1].ToString());
                //FormDb.ChangingDirectoriesIsSuccessful(new ParametersDb { Pk = int.Parse(reader[0].ToString()), ProjectName = reader[1].ToString(), ProjectTypeText = listBoxHelpShortnames.Items[listBoxMain.SelectedIndex].ToString() }, FormMain.GetInternalCode(newcode, reader[0].ToString(), true, FormMain.CodeLength), reader[1].ToString());
            }
            //перезагрузка
            RefreshListbox();
        }

        private void ButtonRemoveClick(object sender, EventArgs e)
        {
            //проверки
            if (listBoxMain.SelectedItem == null) return;
            //получаем количество кортежей, использующий данный тип проекта
            var a = int.Parse(TrixOrm.GetInstance().GetScalar(String.Format("SELECT COUNT(*) FROM t_main WHERE project_type = '{0}'", listBoxHelpUserCodes.Items[listBoxMain.SelectedIndex].ToString().Trim())).ToString());
            //каскадное удаление
            if (a > 0)
                if (MessageBox.Show(String.Format(Warning, a), FormMain.Warning, MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                    foreach (var t in TrixOrm.GetInstance().GetListOfCortages(String.Format("SELECT pk, RTRIM(project_type), RTRIM(projectname) FROM t_main WHERE project_type = '{0}'", listBoxHelpUserCodes.Items[listBoxMain.SelectedIndex].ToString().Trim())))
                        FormMain.RemoveTheItem(t[0].ToString(), FormMain.GetInternalCode(t[1].ToString(), t[0].ToString(), true, FormMain.CodeLength), t[2].ToString());
                else return;
            //обычное удаление 
            TrixOrm.GetInstance().Execute(String.Format("DELETE FROM d_icodes WHERE codename = '{0}'", listBoxMain.SelectedItem));
            RefreshListbox();
        }

        private void ButtonQuitClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CheckBoxShowInCalcCheckedChanged(object sender, EventArgs e)
        {
            TrixOrm.GetInstance().Execute(String.Format("UPDATE d_icodes SET show_in_calc = '{0}' WHERE codename = '{1}'", checkBoxShowInCalc.Checked ? "True" : "False", listBoxMain.SelectedItem));
        }
        #endregion
    }
}
