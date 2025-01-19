// ReSharper disable EmptyGeneralCatchClause
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Calculator
{
    public partial class FormNewRecordStart : Form
    {
        #region //Private consts
        private const string DbaseError = @"Обнаружена ошибка при доступе к БД ({0})";
        private const string DbaseAddNew = @"Возникла ошибка при добавлении записи в БД. Возможно, данное имя проекта уже существует";
        private const string DbaseEditMain = "Произошла ошибка при работе с БД.\nВозможно, заявка будет присутствовать и в главном модуле, и в калькуляторе.\nНЕ УДАЛЯЙТЕ ни одну из записей! Обратитесь к админу за поддержкой";
        private const string CaptionNext = @"Далее";
        private const string CaptionOk = @"OK";
        private const string CaptionStep1 = @"Шаг1";
        private const string CaptionStep2 = @"Шаг2";
        private const string CaptionStep3 = @"Шаг3";
        #endregion

        #region //Variables
        private int _ekMain = -1;
        private readonly string _initInternal;
        #endregion

        #region //Constructor
        public FormNewRecordStart(string initInternalCode = "")
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
            _initInternal = initInternalCode;
        }
        #endregion

        #region //Handlers
        private void FormNewRecordStartLoad(object sender, EventArgs e)
        {
            try
            {
                comboBoxProjectType.Items.AddRange(TrixOrm.GetInstance().GetArray(@"SELECT RTRIM(codeshortname) FROM d_icodes WHERE show_in_calc = 'True'").ToArray());
            }
            catch { MessageBox.Show(String.Format(DbaseError, @"объект d_icodes")); Close(); }
            textBoxInternalCode.Text = _initInternal;
        }

        private void TextBoxInternalCodeTextChanged(object sender, EventArgs e)
        {
            try
            {
                var z = TrixOrm.GetInstance().GetCortage(String.Format("SELECT t_main.pk, RTRIM(projectname), RTRIM(codeshortname) FROM t_main JOIN d_icodes ON project_type = usercode WHERE t_main.pk = {0}", int.Parse(textBoxInternalCode.Text.Substring(2))));
                _ekMain = int.Parse(z[0].ToString());
                textBoxProjectName.Text = z[1].ToString();
                comboBoxProjectType.Text = z[2].ToString();
            }
            catch
            {
                textBoxProjectName.Text = @"";
                comboBoxProjectType.SelectedIndex = _ekMain = -1;
            }
        }

        private void RadioButtonManualAutoCheckedChanged(object sender, EventArgs e)
        {
            buttonNext.Enabled = true;
        }

        private void RadioButtonNewImportChanged(object sender, EventArgs e)
        {
            groupBoxData.Enabled = radioButtonNew.Checked;
            textBoxInternalCode.Enabled = radioButtonImport.Checked;
        }

        private void ParametersChanged(object sender, EventArgs e)
        {
            buttonNext.Enabled = !(String.IsNullOrWhiteSpace(textBoxProjectName.Text) || comboBoxProjectType.SelectedIndex < 0);
        }
        #endregion

        #region //Handlers (button click)
        private void ButtonBackClick(object sender, EventArgs e)
        {
            if (panel2.Visible)
            {
                textBoxInternalCode.SelectAll();
                textBoxInternalCode.Focus();
                buttonNext.Enabled = true;
                panel1.Visible = true;
                panel2.Visible = false;
                buttonBack.Visible = false;
                groupBoxTop.Text = CaptionStep1;
            }
            else if (panel3.Visible || panel4.Visible)
            {
                panel2.Visible = true;
                panel3.Visible = false;
                panel4.Visible = false;
                buttonNext.Text = CaptionNext;
                groupBoxTop.Text = CaptionStep2;
            }
        }

        private void ButtonNextClick(object sender, EventArgs e)
        {
            if (panel1.Visible)
            {
                buttonNext.Enabled = radioButtonAuto.Checked || radioButtonManual.Checked;
                panel1.Visible = false;
                panel2.Visible = true;
                buttonBack.Visible = true;
                groupBoxTop.Text = CaptionStep2;
            }
            else if (panel2.Visible)
            {
                try
                {
                    //удаление элементов
                    foreach (var p in new List<Control>(panel3.Controls.Cast<Control>()))
                        panel3.Controls.Remove(p);
                    //добавление элементов
                    var lst = TrixOrm.GetInstance().GetListOfCortages(@"SELECT pk, text, days_before_deadline FROM calc_d_events");
                    var top = 0;
                    foreach (var p in lst)
                        panel3.Controls.AddRange(new Control[] { new Label { Left = 20, Top = top += 40, Text = p[1].ToString() }, new NumericUpDown { Left = 160, Top = top, Tag = p[0].ToString(), Value = decimal.Parse(p[2].ToString()) } });
                }
                catch { MessageBox.Show(String.Format(DbaseError, @"формирование списка событий")); Close(); }
                panel2.Visible = false;
                if (radioButtonAuto.Checked)
                    panel3.Visible = true;
                else if (radioButtonManual.Checked)
                    panel4.Visible = true;
                buttonNext.Text = CaptionOk;
                groupBoxTop.Text = CaptionStep3;
            }
            else if (panel3.Visible || panel4.Visible)
            {
                try
                {
                    var projType = TrixOrm.GetInstance().GetScalar(String.Format("SELECT usercode FROM d_icodes WHERE codeshortname = '{0}'", comboBoxProjectType.Text)).ToString();
                    if (radioButtonNew.Checked)         //нет записи в t_main => надо добавить новую запись с атрибутом is_archive=False ПРЕЖДЕ, чем заполнять calc_t_main
                    {
                        if (TrixOrm.GetInstance().Execute(String.Format("INSERT INTO t_main (project_type, projectname, client_code, is_legal, is_archive) VALUES ('{0}', '{1}', -1, 'False', 'False')", projType, textBoxProjectName.Text.Trim())) < 0) { MessageBox.Show(DbaseAddNew); return; }
                        _ekMain = int.Parse(TrixOrm.GetInstance().GetScalar("SELECT IDENT_CURRENT('t_main')").ToString());
                    }
                    else                                //есть запись в t_main => надо её изменить на is_archive=False
                        if (TrixOrm.GetInstance().Execute(String.Format("UPDATE t_main SET is_archive = 'False' WHERE pk = {0}", _ekMain)) < 0) { MessageBox.Show(DbaseEditMain); return; }
                    //заполнение calc_t_main
                    TrixOrm.GetInstance().Execute(String.Format("INSERT INTO calc_t_main (ek_main, start_date, paydate, year_number) VALUES ({0}, '{1}', '{2}', {3})", _ekMain, monthCalendarMain.SelectionStart.ToShortDateString().Replace(".", "/"), DateTime.Now.ToShortDateString().Replace(".", "/"), radioButtonAuto.Checked ? -1 : numericUpDownYear.Value));
                    //заполнение calc_t_events (только для авторежима)
                    if (radioButtonAuto.Checked)
                    {
                        var ekCalcMain = int.Parse(TrixOrm.GetInstance().GetScalar("SELECT IDENT_CURRENT('calc_t_main')").ToString());
                        foreach (NumericUpDown w in panel3.Controls.Cast<Control>().Where(t => t is NumericUpDown))
                            TrixOrm.GetInstance().Execute(String.Format("INSERT INTO calc_t_events (ek_calc_main, ek_calc_events, value, is_done) VALUES ({0}, {1}, {2}, 'False')", ekCalcMain, w.Tag, w.Value));
                    }
                    //создание каталогов (Копипаст! Оригинал см. в FormDb.ChangingDirectoriesIsSuccessful)
                    var s = String.Format(@"{0}\{1}\{2}_{3}\", Directory.GetCurrentDirectory(), FormMain.Parameters.FolderDocuments, FormMain.GetInternalCode(projType, _ekMain.ToString()), textBoxProjectName.Text.Trim());
                    var incomingDir = s + FormMain.Parameters.FolderIncoming;
                    var outcomingDir = s + FormMain.Parameters.FolderOutcoming;
                    var matDir = s + FormMain.Parameters.FolderMaterials;
                    if (!Directory.Exists(incomingDir)) Directory.CreateDirectory(incomingDir);
                    if (!Directory.Exists(outcomingDir)) Directory.CreateDirectory(outcomingDir);
                    if (!Directory.Exists(matDir)) Directory.CreateDirectory(matDir);
                }
                catch { MessageBox.Show(String.Format(DbaseError, @"добавление новой записи")); Close(); }
                DialogResult = DialogResult.OK;
                Close();
            }
        }
        #endregion
    }
}
