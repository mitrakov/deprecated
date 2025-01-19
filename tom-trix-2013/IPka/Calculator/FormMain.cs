// ReSharper disable PossibleNullReferenceException
// ReSharper disable EmptyGeneralCatchClause
// ReSharper disable AccessToModifiedClosure
// =====        п р и м е ч а н и я         =====
//
//===============================================
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;
using Application = Microsoft.Office.Interop.Word.Application;

namespace Calculator
{
    public partial class FormMain : Form
    {
        #region //Public consts
        public const string HeaderCalcMainProjectName = "Название";
        public const string HeaderCalcMainDate = "Дата";
        public const string HeaderManualEvent = "Событие";
        public const string HeaderManualDeadline = "Deadline";
        public const string HeaderManualDays = "Осталось дней";
        public const string HeaderManualDone = "Done";
        #endregion

        #region //Private consts
        private const string MenuGoToAuto = "Переместить на авто";
        private const string MenuGoToManual = "Переместить на ручное";
        private const string MsgWarning = @"Предупреждение";
        private const string MsgDoYouWannaDelete = "Вы действительно хотите удалить запись?\n\nПримечание: запись удаляется из компонента \"Калькулятор\", но остаётся доступной из главного модуля\nДля полного удаления используйте главный модуль программы";
        private const string MsgError = @"Ошибка";
        private const string MsgTitle = @"Кулькулятор предупреждает";
        private const string MsgWarnUnchecking = @"Шаг уже отмечен как выполненный. Снять отметку?";
        private const string MsgYearLabel = @"За какой год посл. раз уплачена пошлина?";
        private const string MsgYearCaption = @"Год последней пошлины";
        private const string MsgCurse = "Внимание! Истекают сроки для следующих записей: \n";
        private const string MsgNoRule = @"<Статус не выведен>";
        private const string MsgReason = "Сработало правило №{0}: {1}";
        private const string ErrorGenerating = @"Ошибка при генерации документов в MS Word";
        private const string ErrorSize = @"Ошибка MS Word. Слишком большой размер файла";
        private const string ErrorNoMsWord = @"Ошибка. Не обнаружены компоненты MS Word";
        private const string ErrorFolder = "Ошибка. Не удалось найти каталог:\n\n";
        #endregion

        #region //Static variables
        private static readonly string SqlMain = String.Format(@"SELECT calc_t_main.pk, ek_main, paydate, project_type, RTRIM(projectname) AS '{0}', start_date AS '{1}' FROM calc_t_main JOIN t_main ON ek_main = t_main.pk WHERE year_number {2}", HeaderCalcMainProjectName, HeaderCalcMainDate, "{0}");
        public static string FolderDocs { get; set; }
        public static int CommonMargin { get; set; }
        public static Trix.CalcParameters Parameters;
        #endregion

        #region //Static variables
        private bool _wasChanged;
        #endregion

        #region //Constructor
        public FormMain(Trix.CalcParameters parameters, string addNewRecordInternal = null)
        {
            InitializeComponent();
            Parameters = parameters;
            //вызов новой записи прям из главного модуля (контекстное меню Перенести -> Калькулятор)
            if (!String.IsNullOrWhiteSpace(addNewRecordInternal))
                (new FormNewRecordStart(addNewRecordInternal)).ShowDialog();
        }
        #endregion

        #region //Static methods
        public static DateTime GetDateFromCalendar(DateTime startDate)
        {
            var frm = new Form { StartPosition = FormStartPosition.CenterScreen, DialogResult = DialogResult.Cancel, ShowIcon = false };
            var calendar = new MonthCalendar { MaxSelectionCount = 1, Top = 30, SelectionStart = startDate };
            calendar.Left = frm.Width / 2 - calendar.Width / 2;
            calendar.DateSelected += delegate { frm.Tag = calendar.SelectionStart.ToShortDateString(); frm.DialogResult = DialogResult.OK; frm.Close(); };
            frm.Controls.Add(calendar);
            return frm.ShowDialog() != DialogResult.OK ? DateTime.MinValue : calendar.SelectionStart.Date;
        }

        public static string GetInternalCode(string projectType, string pk)
        {
            //внутренний код состоит из кода проекта (code), первичного ключа (pk) и кучки нулей между ними (определяется параметром ZeroWithinInternalCode)
            var code = projectType.Trim().Length > 1 ? projectType.Substring(0, 2) : "??";
            //добавляем нули
            while (pk.Length < Parameters.ZerosWithinInternalCode)
                pk = "0" + pk;
            //return
            return code + pk;
        }

        public static void OpenTheFolders(string ekMain, string projectType, string projectName)
        {
            var s = String.Format(@"{0}\{1}\{2}_{3}\{4}", Directory.GetCurrentDirectory(), Parameters.FolderDocuments, GetInternalCode(projectType, ekMain), projectName, Parameters.FolderMaterials);
            try { Process.Start(s); }
            catch { MessageBox.Show(ErrorFolder + s, MsgWarning); }
        }
        #endregion

        #region //Private methods
        private void RefreshTheMainGrids()
        {
            //===========  Auto  ===============
            //запоминаем текущую строку (или, точнее, первичный ключ (ОБЯЗАТЕЛЬНО первичный ключ из-за возможности сортировки))
            var current = -1;
            if (dataGridViewMainAuto.SelectedRows.Count > 0)
                current = int.Parse(dataGridViewMainAuto.SelectedRows[0].Cells["pk"].Value.ToString());
            //получение данных
            dataGridViewMainAuto.DataSource = TrixOrm.GetInstance().GetDataTable(String.Format(SqlMain, "< 0"));
            dataGridViewMainAuto.Columns["pk"].Visible = false;
            dataGridViewMainAuto.Columns["ek_main"].Visible = false;
            dataGridViewMainAuto.Columns["paydate"].Visible = false;
            dataGridViewMainAuto.Columns["project_type"].Visible = false;
            try                 //try на случай удаления строки
            {
                var x = dataGridViewMainAuto.Rows.Cast<DataGridViewRow>().First(t => int.Parse(t.Cells["pk"].Value.ToString()) == current).Index;
                dataGridViewMainAuto.Rows[x].Selected = true;
                dataGridViewMainAuto.FirstDisplayedScrollingRowIndex = x;
            }
            catch { }
            //===========  Manual  ===============
            current = -1;
            if (dataGridViewMainManual.SelectedRows.Count > 0)
                current = int.Parse(dataGridViewMainManual.SelectedRows[0].Cells["pk"].Value.ToString());
            dataGridViewMainManual.DataSource = TrixOrm.GetInstance().GetDataTable(String.Format(SqlMain, ">= 0"));
            dataGridViewMainManual.Columns["pk"].Visible = false;
            dataGridViewMainManual.Columns["ek_main"].Visible = false;
            dataGridViewMainManual.Columns["paydate"].Visible = false;
            dataGridViewMainManual.Columns["project_type"].Visible = false;
            try
            {
                var x = dataGridViewMainManual.Rows.Cast<DataGridViewRow>().First(t => int.Parse(t.Cells["pk"].Value.ToString()) == current).Index;
                dataGridViewMainManual.Rows[x].Selected = true;
                dataGridViewMainManual.FirstDisplayedScrollingRowIndex = x;
            }
            catch { }
        }

        private void RefreshRightGrid(int ekMain)
        {
            //подгрузка данных
            const string deadline = @"CASE SIGN(DATEDIFF(dd, DATEADD(yyyy, DATEDIFF(yyyy, start_date, GETDATE()), start_date), GETDATE())) WHEN '-1' THEN DATEADD(yyyy, DATEDIFF(yyyy, start_date, GETDATE()), start_date) ELSE DATEADD(yyyy, DATEDIFF(yyyy, start_date, GETDATE()) + 1, start_date) END";
            dataGridViewRight.DataSource = TrixOrm.GetInstance().GetDataTable(String.Format(@"SELECT calc_t_events.pk, text AS '{1}', DATEADD(dd, -value, {2}) AS '{3}', DATEDIFF(dd, GETDATE(), {2})-value-1 AS '{4}', is_done AS '{5}' FROM calc_t_main JOIN calc_t_events ON calc_t_main.pk = ek_calc_main JOIN calc_d_events ON calc_d_events.pk = ek_calc_events WHERE year_number < 0 AND calc_t_main.pk = {0}", ekMain, HeaderManualEvent, deadline, HeaderManualDeadline, HeaderManualDays, HeaderManualDone));
            dataGridViewRight.Columns["pk"].Visible = false;
            dataGridViewRight.Columns[HeaderManualDone].Visible = false;
            if (dataGridViewRight.Rows.Count > 0)
            {
                //прорисовка картинок
                dataGridViewRight.Columns["Column1"].Visible = dataGridViewRight.Rows.Cast<DataGridViewRow>().Min(t => int.Parse(t.Cells[HeaderManualDays].Value.ToString())) > 30 ? false : true;
                dataGridViewRight.Columns["Column1"].DisplayIndex = dataGridViewRight.Columns.Count - 1;        //ставим столбик в конец
                foreach (DataGridViewRow r in dataGridViewRight.Rows)
                    r.Cells["Column1"].Value = (bool)r.Cells[HeaderManualDone].Value ? Resources.yes : Resources.no;
                // указываем ширину столбцов
                for (var i = 0; i < Math.Min(Parameters.ColumnsGridRightWidth.Count, dataGridViewRight.Columns.Count); i++)
                    dataGridViewRight.Columns[i].Width = Parameters.ColumnsGridRightWidth[i];
            }
            //убираем сортировку (в отличие от гл. модуля, это надо делать всякий раз при обновлении грида)
            foreach (DataGridViewColumn t in dataGridViewRight.Columns)
                t.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void RefreshRightBox(string ekMain)
        {
            textBoxCalendar.Text = DateTime.Parse(dataGridViewMainManual.SelectedRows[0].Cells["paydate"].Value.ToString()).ToShortDateString();
            var s = LogicalInference.GetRule(ekMain);
            labelStatus2.Text = s != null ? s.Caption : MsgNoRule;
            panelColour.BackColor = s != null ? s.Colour : Color.Transparent;
            buttonReason.Visible = s != null;
            buttonReason.Tag = s;
        }

        /// <summary>
        /// Ругается, если существуют истёкшие сроки
        /// </summary>
        private void Curse()
        {
            var s = "";
            foreach (DataGridViewRow row in dataGridViewMainAuto.Rows)
            {
                RefreshRightGrid(int.Parse(row.Cells["pk"].Value.ToString()));
                if (dataGridViewRight.Rows.Cast<DataGridViewRow>().Any(t => int.Parse(t.Cells[HeaderManualDays].Value.ToString()) < 2 && !(bool.Parse(t.Cells[HeaderManualDone].Value.ToString()))))
                    s += ", " + row.Cells[HeaderCalcMainProjectName].Value;
            }
            if (!String.IsNullOrWhiteSpace(s))
                MessageBox.Show(MsgCurse + s.Substring(2), MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (dataGridViewMainAuto.Rows.Count > 0) dataGridViewMainAuto.Rows[0].Selected = true;
        }

        private void GenerateDocumentation(string templateName)
        {
            //get the path
            if (dataGridViewMainAuto.SelectedRows.Count == 0) return;
            var destinationPath = String.Format(@"{0}\{1}\{2}_{3}", Directory.GetCurrentDirectory(), Parameters.FolderDocuments, GetInternalCode(dataGridViewMainAuto.SelectedRows[0].Cells["project_type"].Value.ToString(), dataGridViewMainAuto.SelectedRows[0].Cells["ek_main"].Value.ToString()), dataGridViewMainAuto.SelectedRows[0].Cells[HeaderCalcMainProjectName].Value.ToString().Trim());
            //check the file size
            var q = new FileInfo(templateName);
            if (q.Length >= 255 * 1024 * 1024) { MessageBox.Show(ErrorSize, MsgError); return; }
            //show the form
            var frm = new Form { TopMost = true, StartPosition = FormStartPosition.CenterScreen, FormBorderStyle = FormBorderStyle.None, Opacity = 0.8, BackgroundImage = Resources.Wait, Icon = Icon, Size = Resources.Wait.Size };
            frm.Show();
            //start application
            Application wordapp;
            try { wordapp = new Application { Visible = false }; }
            catch { MessageBox.Show(ErrorNoMsWord); frm.Close(); return; }
            //open the document
            Object filename = templateName;
            Object confirmConversions = true;
            Object readOnly = false;
            Object addToRecentFiles = true;
            var passwordDocument = Type.Missing;
            var passwordTemplate = Type.Missing;
            Object revert = false;
            var writePasswordDocument = Type.Missing;
            var writePasswordTemplate = Type.Missing;
            var format = Type.Missing;
            var encoding = Type.Missing;
            var oVisible = Type.Missing;
            var openAndRepair = Type.Missing;
            var documentDirection = Type.Missing;
            Object noEncodingDialog = false;
            var xmlTransform = Type.Missing;
            try
            {
                var worddocument = wordapp.Documents.Open(ref filename, ref confirmConversions, ref readOnly, ref addToRecentFiles, ref passwordDocument, ref passwordTemplate, ref revert, ref writePasswordDocument, ref writePasswordTemplate, ref format, ref encoding, ref oVisible, ref openAndRepair, ref documentDirection, ref noEncodingDialog, ref xmlTransform);
                //parse all the paragraphs
                var quitFromForeach = false;    //если в абзаце несколько подстановочных полей (напр. "^0^...^1^"), то сработает только первое! В foreach добавлять абзац снова НЕЛЬЗЯ (вылезет Exception Collection was modified), поэтому будем циклить сам foreach (с помощью while true)
                while (!quitFromForeach)
                {
                    quitFromForeach = true;                                     //выход из цикла
                    var prgrs = new List<Paragraph>();                          //список из обычных абзацев и абзацев верхнего колонтитула
                    prgrs.AddRange(worddocument.Paragraphs.Cast<Paragraph>());
                    prgrs.AddRange(worddocument.Sections[1].Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range.Paragraphs.Cast<Paragraph>());
                    foreach (var t in prgrs)
                    {
                        //find all "^"-substrings in the document
                        var start = t.Range.Text.IndexOf('^') + 1;
                        if (start == 0) continue;
                        var end = t.Range.Text.Substring(start).IndexOf('^') + start;
                        var num = t.Range.Text.Substring(start, end - start);
                        var isCenter = t.Format.Alignment == WdParagraphAlignment.wdAlignParagraphCenter;           //БЫДЛОКОД! При смене ".Range.Text" Ворд изменяет ".Format" на дефолт => надо запомнить выравнивание (НЕ ЗАПОМИНАТЬ ФОРМАТ ЦЕЛИКОМ !!! Ворд тупит ваще)
                        t.Range.Text = t.Range.Text.Replace("^" + num + "^", "Ура");                            //change the 'Range.Text' property
                        if (isCenter) t.Previous().Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;  //БЫДЛОКОД! Меняем выравнивание обратно (ввиду тупости Ворда ОБЯЗАТЕЛЬНО использовать ".Previous()" !!!)
                        quitFromForeach = false;                                                                    //в строке возможны ещё подстановки - выход из цикла следует отменить
                    }
                }
                //save the document).
                if (!String.IsNullOrWhiteSpace(destinationPath))
                {
                    if (Directory.Exists(destinationPath) && !Directory.Exists(destinationPath + @"\" + "Calculator"))
                        Directory.CreateDirectory(destinationPath + @"\" + "Calculator");
                    Object fileName = destinationPath + @"\" + "Calculator" + @"\" + @"Test";
                    Object fileFormat = Parameters.UseWord2003 ? WdSaveFormat.wdFormatDocument : WdSaveFormat.wdFormatDocumentDefault;
                    Object lockComments = false;
                    Object password = "";
                    Object addToRecentFiless = false;
                    Object writePassword = "";
                    Object readOnlyRecommended = false;
                    Object embedTrueTypeFonts = false;
                    Object saveNativePictureFormat = false;
                    Object saveFormsData = false;
                    var saveAsAoceLetter = Type.Missing;
                    var encodings = Type.Missing;
                    var insertLineBreaks = Type.Missing;
                    var allowSubstitutions = Type.Missing;
                    var lineEnding = Type.Missing;
                    var addBiDiMarks = Type.Missing;
                    worddocument.SaveAs(ref fileName, ref fileFormat, ref lockComments, ref password, ref addToRecentFiless, ref writePassword, ref readOnlyRecommended, ref embedTrueTypeFonts, ref saveNativePictureFormat, ref saveFormsData, ref saveAsAoceLetter, ref encodings, ref insertLineBreaks, ref allowSubstitutions, ref lineEnding, ref addBiDiMarks);
                }
            }
            catch { MessageBox.Show(ErrorGenerating, MsgError); }
            //close all
            wordapp.Quit(true);
            frm.Close();
        }
        #endregion

        #region //Handlers
        private void FormMainLoad(object sender, EventArgs e)
        {
            //применяем настройки
            FolderDocs = Parameters.FolderDocuments;
            CommonMargin = Parameters.CommonMargin;
            Size = Parameters.FormSize;
            Location = Parameters.FormPosition;
            splitContainerMain.SplitterDistance = Parameters.SplitVertical;
            splitContainerLeft.SplitterDistance = Parameters.SplitHorisontal;
            //обновляем гриды
            RefreshTheMainGrids();
            //настройки ширины столбцов, очевидно, следует применять ПОСЛЕ загрузки данных в грид
            //ширину столбцов в правом гриде здесь указывать НЕЛЬЗЯ! Сделать следует это в процедуре RefreshRightGrid()
            for (var i = 0; i < Math.Min(Parameters.ColumnsGridAutoWidth.Count, dataGridViewMainAuto.Columns.Count); i++)
                dataGridViewMainAuto.Columns[i].Width = Parameters.ColumnsGridAutoWidth[i];
            for (var i = 0; i < Math.Min(Parameters.ColumnsGridManualWidth.Count, dataGridViewMainManual.Columns.Count); i++)
                dataGridViewMainManual.Columns[i].Width = Parameters.ColumnsGridManualWidth[i];
            //ругаемся
            Curse();
        }

        private void TextBoxCalendarRequest(object sender, EventArgs e)
        {
            //checks
            if (dataGridViewMainManual.SelectedRows.Count == 0) { textBoxCalendar.Text = @""; return; }
            //запоминаем индекс текущей строки в dataGridView
            var currentpk = (int)dataGridViewMainManual.SelectedRows[0].Cells["pk"].Value;
            //получение даты
            var date = DateTime.Now;
            try { date = DateTime.Parse(textBoxCalendar.Text); }
            catch { }
            //обновляем текстбокс
            textBoxCalendar.Text = GetDateFromCalendar(date).ToShortDateString();
            //обновляем запись в БД
            TrixOrm.GetInstance().Execute(String.Format("UPDATE calc_t_main SET paydate = '{0}' WHERE pk = {1}", textBoxCalendar.Text.Replace('.', '/'), dataGridViewMainManual.SelectedRows[0].Cells["pk"].Value));
            //refresh everything
            RefreshTheMainGrids();
            dataGridViewMainManual.Rows.Cast<DataGridViewRow>().First(t => (int)t.Cells["pk"].Value == currentpk).Selected = true;
            RefreshRightBox(currentpk.ToString());
        }

        private void ButtonReasonClick(object sender, EventArgs e)
        {
            if (buttonReason.Tag == null) return;
            var s = (Trix.InferenceResult)buttonReason.Tag;
            MessageBox.Show(String.Format(MsgReason, s.Number, s.Reason));
        }

        private void FormMainFormClosing(object sender, FormClosingEventArgs e)
        {
            Parameters.ColumnsGridAutoWidth = dataGridViewMainAuto.Columns.Cast<DataGridViewColumn>().Select(t => t.Width).ToList();
            Parameters.ColumnsGridManualWidth = dataGridViewMainManual.Columns.Cast<DataGridViewColumn>().Select(t => t.Width).ToList();
            Parameters.ColumnsGridRightWidth = dataGridViewRight.Columns.Cast<DataGridViewColumn>().Select(t => t.Width).ToList();
            Parameters.SplitVertical = splitContainerMain.SplitterDistance;
            Parameters.SplitHorisontal = splitContainerLeft.SplitterDistance;
            Parameters.FormPosition = Location;
            Parameters.FormSize = Size;
            Tag = Parameters;
            DialogResult = _wasChanged ? DialogResult.OK : DialogResult.Cancel;
        }
        #endregion

        #region //Handlers (Grids)
        private void DataGridViewMainAutoRowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewMainAuto.SelectedRows.Count == 0) return;
            RefreshRightGrid(int.Parse(dataGridViewMainAuto.SelectedRows[0].Cells["pk"].Value.ToString()));
        }

        private void DataGridViewMainAutoCellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            if (dataGridViewMainAuto.SelectedRows.Count == 0) return;
            dataGridViewMainAuto.Rows[e.RowIndex].Selected = true;
            contextMenuStripLeft.Show(MousePosition.X + CommonMargin, MousePosition.Y);
            contextMenuStripLeft.Items[contextMenuStripLeft.Items.Count - 1].Text = MenuGoToManual;
        }

        private void DataGridViewMainAutoEnter(object sender, EventArgs e)
        {
            groupBoxInfo.Visible = false;
            dataGridViewRight.Visible = true;
        }

        private void DataGridViewMainAutoMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (dataGridViewMainAuto.SelectedRows.Count == 0) return;
            OpenTheFolders(dataGridViewMainAuto.SelectedRows[0].Cells["ek_main"].Value.ToString(), dataGridViewMainAuto.SelectedRows[0].Cells["project_type"].Value.ToString(), dataGridViewMainAuto.SelectedRows[0].Cells[HeaderCalcMainProjectName].Value.ToString().Trim());
        }

        private void DataGridViewMainManualRowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewMainManual.SelectedRows.Count == 0) return;
            RefreshRightBox(dataGridViewMainManual.SelectedRows[0].Cells["pk"].Value.ToString());
        }

        private void DataGridViewMainManualCellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            if (dataGridViewMainManual.SelectedRows.Count == 0) return;
            dataGridViewMainManual.Rows[e.RowIndex].Selected = true;
            contextMenuStripLeft.Show(MousePosition.X + CommonMargin, MousePosition.Y);
            contextMenuStripLeft.Items[contextMenuStripLeft.Items.Count - 1].Text = MenuGoToAuto;
        }

        private void DataGridViewMainManualEnter(object sender, EventArgs e)
        {
            groupBoxInfo.Visible = true;
            dataGridViewRight.Visible = false;
        }

        private void DataGridViewMainManualMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (dataGridViewMainManual.SelectedRows.Count == 0) return;
            OpenTheFolders(dataGridViewMainManual.SelectedRows[0].Cells["ek_main"].Value.ToString(), dataGridViewMainManual.SelectedRows[0].Cells["project_type"].Value.ToString(), dataGridViewMainManual.SelectedRows[0].Cells[HeaderCalcMainProjectName].Value.ToString().Trim());
        }

        private void DataGridViewRightCellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dataGridViewMainAuto.SelectedRows.Count == 0) return;
            var pk = int.Parse(dataGridViewRight.SelectedRows[0].Cells["pk"].Value.ToString());
            if (e.Button == MouseButtons.Left && e.ColumnIndex == dataGridViewRight.Columns["Column1"].Index)
            {
                if (((Bitmap)dataGridViewRight.Rows[e.RowIndex].Cells[e.ColumnIndex].Value).GetPixel(10, 10) == Resources.no.GetPixel(10, 10))
                {
                    TrixOrm.GetInstance().Execute(String.Format("UPDATE calc_t_events SET is_done = 'True' WHERE pk = {0}", pk));
                    GenerateDocumentation(Directory.GetCurrentDirectory() + @"\" + Parameters.FolderTemplates + @"\Калькулятор.dotx");
                    RefreshRightGrid(int.Parse(dataGridViewMainAuto.SelectedRows[0].Cells["pk"].Value.ToString()));
                }
                else if (MessageBox.Show(MsgWarnUnchecking, MsgWarning, MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                {
                    TrixOrm.GetInstance().Execute(String.Format("UPDATE calc_t_events SET is_done = 'False' WHERE pk = {0}", pk));
                    RefreshRightGrid(int.Parse(dataGridViewMainAuto.SelectedRows[0].Cells["pk"].Value.ToString()));
                }
            }
        }

        private void DataGridViewRightColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (ActiveControl == null) return;
            Parameters.ColumnsGridRightWidth = dataGridViewRight.Columns.Cast<DataGridViewColumn>().Select(t => t.Width).ToList();
        }
        #endregion

        #region //Handlers (ToolStripMenu)
        private void NewRecordToolStripMenuItemClick(object sender, EventArgs e)
        {
            if ((new FormNewRecordStart()).ShowDialog() != DialogResult.OK) return;
            RefreshTheMainGrids();
            _wasChanged = true; //Изначально было DialogResult = DialogResult.OK;, но это почему-то закрывало форму!!! What a fuck?
        }

        private void RulesToolStripMenuItemClick(object sender, EventArgs e)
        {
            (new FormRules()).ShowDialog();
        }

        private void ChangeTypeToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (contextMenuStripLeft.Items[contextMenuStripLeft.Items.Count - 1].Text == MenuGoToAuto)
            {
                if (dataGridViewMainManual.SelectedRows.Count == 0) return;
                TrixOrm.GetInstance().Execute(String.Format("UPDATE calc_t_main SET year_number = -1 WHERE pk = {0}", dataGridViewMainManual.SelectedRows[0].Cells["pk"].Value));
            }
            else if (contextMenuStripLeft.Items[contextMenuStripLeft.Items.Count - 1].Text == MenuGoToManual)
            {
                if (dataGridViewMainAuto.SelectedRows.Count == 0) return;
                var frm = new TrixInputBox(MsgYearLabel, MsgYearCaption, null, true);
                if (frm.ShowDialog() != DialogResult.OK) return;
                TrixOrm.GetInstance().Execute(String.Format("UPDATE calc_t_main SET year_number = {0} WHERE pk = {1}", frm.Tag, dataGridViewMainAuto.SelectedRows[0].Cells["pk"].Value));
            }
            RefreshTheMainGrids();
        }

        private void ChangeDateToolStripMenuItemClick(object sender, EventArgs e)
        {
            var grid = contextMenuStripLeft.Items[contextMenuStripLeft.Items.Count - 1].Text == MenuGoToAuto ? dataGridViewMainManual : dataGridViewMainAuto;
            if (grid.SelectedRows.Count == 0) return;
            var newDate = GetDateFromCalendar(DateTime.Parse(grid.SelectedRows[0].Cells[HeaderCalcMainDate].Value.ToString()));
            if (newDate == DateTime.MinValue) return;
            TrixOrm.GetInstance().Execute(String.Format("UPDATE calc_t_main SET start_date = '{0}' WHERE pk = {1}", newDate.ToShortDateString().Replace('.', '/'), grid.SelectedRows[0].Cells["pk"].Value));
            if (grid == dataGridViewMainManual)
            {
                var pk = dataGridViewMainManual.SelectedRows[0].Cells["pk"].Value;
                var frm = new TrixInputBox(MsgYearLabel, MsgYearCaption, TrixOrm.GetInstance().GetScalar(String.Format("SELECT year_number FROM calc_t_main WHERE pk = {0}", pk)).ToString(), true);
                if (frm.ShowDialog() != DialogResult.OK) return;
                TrixOrm.GetInstance().Execute(String.Format("UPDATE calc_t_main SET year_number = {0} WHERE pk = {1}", frm.Tag, pk));
            }
            RefreshTheMainGrids();
        }

        private void DeleteToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (MessageBox.Show(MsgDoYouWannaDelete, MsgWarning, MessageBoxButtons.YesNoCancel) != DialogResult.Yes) return;
            // получаем первичный ключ
            var grid = contextMenuStripLeft.Items[contextMenuStripLeft.Items.Count - 1].Text == MenuGoToAuto ? dataGridViewMainManual : dataGridViewMainAuto;
            if (grid.SelectedRows.Count == 0) return;
            var pk = grid.SelectedRows[0].Cells["pk"].Value;
            // удаление (+ каскадка)
            TrixOrm.GetInstance().Execute(String.Format("UPDATE t_main SET is_archive = NULL WHERE pk IN (SELECT ek_main FROM calc_t_main WHERE pk = {0})", pk));
            TrixOrm.GetInstance().Execute(String.Format("DELETE FROM calc_t_events WHERE ek_calc_main = {0}", pk));
            TrixOrm.GetInstance().Execute(String.Format("DELETE FROM calc_t_main WHERE pk = {0}", pk));
            // обновление
            _wasChanged = true;
            RefreshTheMainGrids();
        }
        #endregion
    }
}
