// ReSharper disable PossibleNullReferenceException
// ReSharper disable MemberCanBeMadeStatic.Local
// ReSharper disable EmptyGeneralCatchClause

// ===== примечание относительно публикации =====
// НЕ ИСПОЛЬЗОВАТЬ ПУБЛИКАЦИЮ для заказчика!
// уже, как минимум, 2 функции (если не больше) не работают по непонятным причинам
// а также не запускается exe-ник (надо только с ярлыка).
// поэтому используем ComponentPack
//===============================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using Calculator;

namespace IPka
{
    public partial class FormMain : Form
    {
        #region //public constants
        public const string Warning = @"Предупреждение";
        public const string HeaderInternalCode = @"Внутренний код";
        public const string HeaderProjectName = @"Название";
        public const string HeaderRequestNumber = @"№ в Роспатент";
        public const string HeaderDateCreated = @"Дата подачи";
        public const string HeaderDateReceived = @"Дата получения";
        public const string HeaderPatentNumber = @"№ свидетельства";
        public const string HeaderIsLegal = @"Юридическое лицо";
        public const string HeaderClient = @"Клиент";
        public const string HeaderColour = @"Отметка";
        public const string FolderDocuments = @"Documents";
        public const string FolderTemplates = @"Templates";
        public const string FolderMaterials = @"Материалы";
        public const string CorrIncoming = @"Входящая корреспонденция";
        public const string CorrOutcoming = @"Исходящая корреспонденция";
        public const int CommonMargin = 20;
        #endregion

        #region //private constants
        private const string FolderRecycle = @"Recycle";
        private const string HeaderProjectShortType = @"Тип";
        private const string HeaderIncoming = @"Дата (вх.)";
        private const string HeaderOutcoming = @"Дата (исх.)";
        private const string HeaderType1 = @"Тип (вх.)";
        private const string HeaderType2 = @"Тип (исх.)";
        private const string HeaderIsSent = @"is_sent";
        private const string ContextMenuToArchive = @"В архив";
        private const string ContextMenuFromArchive = @"Из архива";
        private const string Question = "Вы действительно хотите удалить заявку '{0}' (№ {1})?\n\n(файлы будут перемещены в корзину)";
        private const string QuestionRecycle = @"Очистить каталог с удалёнными файлами?";
        private const string RecycleEmpty = @"Корзина пуста";
        private const string About = "RusPatent Database (2011)\n       Tom-Trix Software";
        private const string ConfigFilename = @"Config.xml";
        private const string SearchFailed = @"Нет данных, удовлетворяющих строке поиска";
        private const string SqlInjection = @"Запрещено вводить символ <'>";
        private const string ErrorRename1 = "Не удалось переименовать каталог '{0}'\n\nДоступ заблокирован другим процессом";
        private const string ErrorRename2 = @"Возникла ошибка. Убедитесь, что данного проекта существует правильный каталог";
        private const string ErrorFolder = "Ошибка. Не удалось найти каталог:\n\n";
        private const string WarningNoFile = @"Файл '{0}' отсутствует. Добавьте шаблон или документ Word в папку Templates или измените корреспонденцию";
        private const string WarningChangeSendStatus = @"Подразумевается, что письмо отправлено. Вы действительно хотите пометить его как 'Неотправленное'?";
        private const string WarningDeleteCorr = "Удалить корреспонденцию '{0}'?\n\n(файлы будут перемещены в корзину)";
        private const string WarningFirstPriority = @"Невозможно добавить корреспонденцию до прихода письма '{0}'";
        private const string WarningPriority = @"Добавить корреспонденцию '{0}'?";
        private const string ErrorRemoveItem = "Действие отменено.\nНевозможно удалить каталог\n{0}\n\nВозможно, один из его файлов заблокирован другим процессом\n\nЗакройте все блокирующие программы и повторите попытку";
        private const string ErrorNotSent = @"Нельзя добавить новую корреспонденцию, т.к. существуют неотправленные письма";
        private const string YouCanAddUsualLetter = @"Однако, доступна корреспонденция '{0}'. Добавить её?";
        private const string ThisIsArchive = "Открыт режим архива.\nДля перехода к стандартному режиму снимите отметку в меню Файл -> Настройки";
        #endregion

        #region //private variables
        private bool _isAsc;                                            //текущий порядок сортировки главного грида
        private readonly List<int> _tCorrPkSearch = new List<int>();    //первичный ключ таблицы t_correspondence, используемый для закраски строк правого грида при поиске
        private String _searchString;                                   //строка поиска (если она не изменяется, то тыкая по кнопке "Поиск" мы тем самым двигаемся по результатам)
        #endregion

        #region //settings
        public static bool SortThirdSymbol { set; get; }                //стоит ли сортировать столбец InternalCode по третьему символу?
        public static int WarnDeadline { set; get; }                    //кол-во дней до deadline, по истечению которых следует подсвечивать красным цветом
        public static bool ShowDeleteAndReport { set; get; }            //показывать кнопки Delete и Report?
        public static string RequestCorrespondence { set; get; }        //Приоритетная справка
        public static string PatentCorrespondence { set; get; }         //Решение о выдаче
        public static string FirstCorrespondence { set; get; }          //Материалы заявки
        public static string SuperCorrespondence { set; get; }          //Произвольное письмо
        public static int CodeLength { set; get; }                      //длина внутреннего кода без первых двух знаков
        public static bool UseWord2003 { set; get; }                    //стоит ли принудительно сохранять документы в формат Word2003
        public static bool ShowC { set; get; }                          //стоит ли показывать кнопку "С" для очистки дат
        public static string PhysicalField { set; get; }                //имя поля для физ. лица для отображения в главном гриде (напр. "1. ФИО")
        public static string LegalField { set; get; }                   //имя поля для юр. лица для отображения в главном гриде (напр. "1. Полное наименование")
        public static List<int> GridMainFieldsWidth { get; set; }       //ширина столбцов главного грида (числа через запятую)
        public static List<int> GridCorrFieldsWidth { get; set; }       //ширина столбцов грида корреспонденции (числа через запятую)
        public static bool ShowArchive { get; set; }                    //показывать ли архивные записи?
        public static Trix.CalcParameters CalcParameters;               //параметры для модуля калькулятора
        #endregion

        #region //static methods
        public static Form CreateInputBox(string label, string header = "", string txtbox = "", bool calendar = false)
        {
            var frm = new Form { StartPosition = FormStartPosition.CenterScreen, MinimizeBox = false, MaximizeBox = false, FormBorderStyle = FormBorderStyle.FixedDialog, Tag = "", Text = header };
            var lbl = new Label { Text = label, Width = frm.Width - CommonMargin, Top = CommonMargin };
            lbl.Left = frm.Width / 2 - lbl.Width / 2;
            var txb = new TextBox { Text = txtbox, Top = lbl.Bottom + CommonMargin };
            txb.Left = frm.Width / 2 - txb.Width / 2;
            var cld = new MonthCalendar { Visible = calendar, Top = txb.Bottom + CommonMargin, MaxSelectionCount = 1, ShowToday = false };
            cld.Left = frm.Width / 2 - cld.Width / 2;
            var btn = new Button { Text = @"OK", Height = 40, Left = 2 * CommonMargin, Top = calendar ? cld.Bottom + 4 * CommonMargin : txb.Bottom + CommonMargin };
            btn.Click += delegate { if (String.IsNullOrWhiteSpace(txb.Text)) { txb.Focus(); return; } frm.Tag = txb.Text.Trim(); frm.DialogResult = DialogResult.OK; frm.Close(); };
            var esc = new Button { Text = @"Отмена", Height = 40, Top = btn.Top };
            esc.Left = frm.Width - 2 * CommonMargin - esc.Width;
            esc.Click += delegate { frm.Close(); };
            frm.Controls.AddRange(new Control[] { lbl, txb, cld, btn, esc });
            frm.CancelButton = esc;
            frm.AcceptButton = btn;
            frm.Height = btn.Bottom + 2 * CommonMargin;
            return frm;
        }

        public static void RemoveTheItem(string pk, string internalCode, string projectName)
        {
            //перемещение каталога в корзину
            var s = String.Format(@"{0}\{1}\{2}_{3}", Directory.GetCurrentDirectory(), FolderDocuments, internalCode, projectName);
            if (Directory.Exists(s))
                try { Directory.Move(s, String.Format(@"{0}\{1}\{2}_{3}_{4}", Directory.GetCurrentDirectory(), FolderRecycle, pk, internalCode, projectName)); }
                catch { MessageBox.Show(String.Format(ErrorRemoveItem, s), Warning); return; }
            //перед удалением получаем код клиента
            var client = TrixOrm.GetInstance().GetScalar(String.Format("SELECT client_code FROM t_main WHERE pk = {0}", pk));
            //основное удаление заявки
            TrixOrm.GetInstance().Execute(String.Format("DELETE FROM t_main WHERE pk = {0}", pk));
            //каскадное удаление
            TrixOrm.GetInstance().Execute(String.Format("DELETE FROM t_requisites WHERE client_code = {0}", client));
            TrixOrm.GetInstance().Execute(String.Format("DELETE FROM t_correspondence WHERE ek_main = {0}", pk));
        }

        public static void RemoveTheCorrItem(string pk, string internalCode, string projectName, string corrShortname, bool deleteEverything = true)
        {
            //основное удаление заявки
            if (deleteEverything)
                TrixOrm.GetInstance().Execute(String.Format("DELETE FROM t_correspondence WHERE pk = {0}", pk));
            //перемещение каталога в корзину
            var s = String.Format(@"{0}\{1}\{2}_{3}\{4}\{2}_{5}_{6}", Directory.GetCurrentDirectory(), FolderDocuments, internalCode.Trim(), projectName.Trim(), CorrOutcoming.Trim(), corrShortname.Trim(), pk.Trim());
            if (Directory.Exists(s))
                try { Directory.Move(s, String.Format(@"{0}\{1}\{2}_{3}_{4}", Directory.GetCurrentDirectory(), FolderRecycle, internalCode.Trim(), corrShortname.Trim(), pk.Trim())); }
                catch { MessageBox.Show(String.Format(ErrorRemoveItem, s), Warning); }
        }

        public static string GetInternalCode(string projectType, string pk, bool isCodeOrCodeshortname, int codelength)
        {
            //внутренний код состоит из кода проекта (code) и первичного ключа (pk)
            var code = projectType.Trim().Length > 1 ? projectType.Substring(0, 2) : "??";
            //projectType может быть типом проекта (БД, ИЗ, ТЗ), а может быть уже числовым идентификатором (решает параметр isCodeOrCodeshortname). Если ещё не числовой код, то надо его получить
            if (!isCodeOrCodeshortname)
            {
                var x = TrixOrm.GetInstance().GetScalar(String.Format("SELECT RTRIM(usercode) FROM d_icodes WHERE codeshortname = '{0}'", projectType));
                code = x != null ? x.ToString() : "??";
            }
            //добавляем нули
            while (pk.Length < codelength)
                pk = "0" + pk;
            //return
            return code + pk;
        }

        public static string GetInternalCodeForSql()
        {
            return String.Format(@"usercode + REPLICATE('0', {0} - LEN(LTRIM(STR(t_main.pk)))) + LTRIM(STR(t_main.pk))", CodeLength);
        }

        public static bool GenerateReportStarter(int pk, string template, string pathToSave, string fileToSave)
        {
            //проверки
            if (String.IsNullOrWhiteSpace(template)) return false;
            var fname = String.Format(@"{0}\{1}\{2}", Directory.GetCurrentDirectory(), FolderTemplates, template.Trim());
            if (!File.Exists(fname)) { MessageBox.Show(String.Format(WarningNoFile, fname)); return false; }
            //подготовка вспомогательных списков
            var paramlst = new List<string>();
            var textlst = new List<string>();
            foreach (var t in TrixOrm.GetInstance().GetListOfCortages("SELECT RTRIM(parameter), RTRIM(text) FROM t_query"))
            {
                paramlst.Add(t[0].ToString());
                textlst.Add(t[1].ToString());
            }
            //генерация отчёта
            return FormReport.GenerateTheReport(pk, paramlst, textlst, fname, pathToSave, fileToSave);
        }

        public static bool RenameDirectories(string oldInternalcode, string newInternalcode, string oldProjectname, string newProjectname)
        {
            //переименовываем каталог и все его файлы/подкаталоги
            var olds = String.Format(@"{0}\{1}\{2}_{3}", Directory.GetCurrentDirectory(), FolderDocuments, oldInternalcode, oldProjectname);
            var news = String.Format(@"{0}\{1}\{2}_{3}", Directory.GetCurrentDirectory(), FolderDocuments, newInternalcode, newProjectname);
            //main directory
            try
            {
                if (!Directory.Exists(news))
                    if (Directory.Exists(olds))
                        Directory.Move(olds, news);
            }
            catch { MessageBox.Show(String.Format(ErrorRename1, olds)); return false; }
            try
            {
                //subdirectory
                foreach (var p in Directory.GetDirectories(news, String.Format("{0}_*", oldInternalcode), SearchOption.AllDirectories))
                {
                    var newdir = Path.GetFileName(p).Replace(oldInternalcode.Trim(), newInternalcode.Trim());
                    if (newdir == Path.GetFileName(p)) continue;
                    Directory.Move(Path.GetDirectoryName(p) + @"\" + Path.GetFileName(p), Path.GetDirectoryName(p) + @"\" + newdir);
                }
                //files in subdirectory
                foreach (var p in Directory.GetFiles(news, String.Format("{0}_*", oldInternalcode.Trim()), SearchOption.AllDirectories))
                {
                    var newfile = Path.GetFileName(p).Replace(oldInternalcode.Trim(), newInternalcode.Trim());
                    if (newfile == Path.GetFileName(p)) continue;
                    File.Move(Path.GetDirectoryName(p) + @"\" + Path.GetFileName(p), Path.GetDirectoryName(p) + @"\" + newfile);
                }
            }
            catch { MessageBox.Show(ErrorRename2); return false; }
            return true;
        }
        #endregion

        #region //private methods
        private void RefreshTheMainGrid(bool needToSort = true, bool bottomSelected = false)
        {
            //запоминаем текущую строку (или, точнее, первичный ключ (ОБЯЗАТЕЛЬНО первичный ключ из-за возможности сортировки))
            var current = -1;
            if (Grid_main.SelectedRows.Count > 0)
                current = int.Parse(Grid_main.SelectedRows[0].Cells["pk"].Value.ToString());
            //SQL
            var s = String.Format("SELECT t_main.pk, LTRIM(STR(colour)) AS {13}, colour, RTRIM(codeshortname) as '{0}', {1} as '{2}', RTRIM(projectname) as '{3}', value as '{9}', RTRIM(request_number) as '{4}', date_created as '{5}', date_received as '{6}', RTRIM(patent_number) as '{7}', t_main.is_legal as '{8}', t_main.client_code FROM t_main INNER JOIN d_icodes ON project_type = usercode LEFT JOIN t_requisites ON t_requisites.client_code = t_main.client_code LEFT JOIN d_requisites ON t_requisites.ek_requis = d_requisites.pk WHERE (is_archive {12}) AND (d_requisites.is_legal IS NULL OR (t_main.is_legal = d_requisites.is_legal AND (requisname = '{10}' OR requisname = '{11}')))", HeaderProjectShortType, GetInternalCodeForSql(), HeaderInternalCode, HeaderProjectName, HeaderRequestNumber, HeaderDateCreated, HeaderDateReceived, HeaderPatentNumber, HeaderIsLegal, HeaderClient, LegalField, PhysicalField, ShowArchive ? "= 'True'" : "IS NULL", HeaderColour);
            //сортировка
            if (needToSort)
            {
                s += " ORDER BY " + (SortThirdSymbol ? "t_main.pk " : "usercode ");
                s += _isAsc ? @"DESC" : @"ASC";
            }
            //получение данных
            Grid_main.DataSource = TrixOrm.GetInstance().GetDataTable(s);
            //скрываем ненужные для просмотра столбцы
            Grid_main.Columns["pk"].Visible = false;
            Grid_main.Columns["client_code"].Visible = false;
            Grid_main.Columns["colour"].Visible = false;
            Grid_main.Columns[HeaderPatentNumber].Visible = false;
            Grid_main.Columns[HeaderIsLegal].Visible = false;
            Grid_main.Columns[HeaderDateReceived].Visible = false;
            //подбираем ширину столбцов
            for (var i = 0; i < Math.Min(GridMainFieldsWidth.Count, Grid_main.Columns.Count); i++)
                Grid_main.Columns[i].Width = GridMainFieldsWidth[i];
            //раскраска (теперь не вычисляется, а поднимается из БД)
            foreach (DataGridViewRow t in Grid_main.Rows)
            {
                t.Cells[HeaderColour].Style.BackColor = t.Cells[HeaderColour].Value.ToString() == @"0" ? SystemColors.Window : int.Parse(t.Cells[HeaderColour].Value.ToString()) % 2 == 1 ? Color.Yellow : Color.Red;
                t.Cells[HeaderColour].Value = @"";      // !!! Не использовать SUBSTRING(STR(colour), 1, 0)!!!
            }
            //вновь выделяем текущую строку (блок try нужен для случая удаления))
            try
            {
                var x = Grid_main.Rows.Cast<DataGridViewRow>().First(t => int.Parse(t.Cells["pk"].Value.ToString()) == current).Index;
                Grid_main.Rows[x].Selected = true;
                Grid_main.FirstDisplayedScrollingRowIndex = x;
            }
            catch { }
            //отдельный случай - создание нового проекта (насильно выделяем последнюю  строку)
            if (bottomSelected)
            {
                Grid_main.Rows[Grid_main.Rows.Count-1].Selected = true;
                Grid_main.FirstDisplayedScrollingRowIndex = Grid_main.Rows.Count-1;
            }
        }

        private void RefreshRightGrids(int ekMain, bool shouldUpdateColoursInDb = false)
        {
            //отображение данных t_correspondence
            Grid_correspondence.DataSource = TrixOrm.GetInstance().GetDataTable(String.Format("SELECT t_correspondence.pk, d_correspondence.pk as 'dpk', is_incoming, corrshortname, months_deadline, ik_deactivator, value, comment, CASE is_incoming WHEN 'True' THEN value END as '{0}', CASE is_incoming WHEN 'True' THEN RTRIM(corrname) END as '{1}', ' ' as '.', CASE is_incoming WHEN 'False' THEN value END as '{2}', CASE is_incoming WHEN 'False' THEN RTRIM(corrname) END as '{3}', CASE is_incoming WHEN 'False' THEN is_sent END AS '{4}' FROM t_correspondence, d_correspondence WHERE t_correspondence.ek_corr = d_correspondence.pk AND ek_main = {5} ORDER BY value ASC, is_incoming DESC", HeaderIncoming, HeaderType1, HeaderOutcoming, HeaderType2, HeaderIsSent, ekMain));
            Grid_correspondence.Columns["pk"].Visible = false;
            Grid_correspondence.Columns["dpk"].Visible = false;
            Grid_correspondence.Columns["is_incoming"].Visible = false;
            Grid_correspondence.Columns["corrshortname"].Visible = false;
            Grid_correspondence.Columns["months_deadline"].Visible = false;
            Grid_correspondence.Columns["ik_deactivator"].Visible = false;
            Grid_correspondence.Columns["value"].Visible = false;
            Grid_correspondence.Columns["comment"].Visible = false;
            Grid_correspondence.Columns[HeaderIsSent].Visible = false;
            Grid_correspondence.Columns[HeaderOutcoming].Visible = false;
            Grid_correspondence.Columns["Column1"].DisplayIndex = Grid_correspondence.Columns.Count - 1;        //ставим столбик в конец            
            //подбираем ширину столбцов
            for (var i = 0; i < Math.Min(GridCorrFieldsWidth.Count, Grid_correspondence.Columns.Count); i++)
                Grid_correspondence.Columns[i].Width = GridCorrFieldsWidth[i];
            //маркеры
            CalculateColoursByBruteforce(ekMain, shouldUpdateColoursInDb);
            //прорисовка картинок с галочками
            foreach (DataGridViewRow t in Grid_correspondence.Rows)
                t.Cells["Column1"].Value = String.IsNullOrWhiteSpace(t.Cells[HeaderIsSent].Value.ToString()) ? Resources._null : t.Cells[HeaderIsSent].Value.ToString() == "True" ? Resources.yes : Resources.no;
            //подсказки
            foreach (DataGridViewRow t in Grid_correspondence.Rows)
                foreach (var p in t.Cells.Cast<DataGridViewCell>())
                    p.ToolTipText = DateTime.Parse(t.Cells["value"].Value.ToString()).ToShortDateString() + "\n" + t.Cells["comment"].Value;
            //закраска ячеек при поиске
            try
            {
                if (Grid_main.SelectedRows[0].Cells[HeaderInternalCode].Style.BackColor == Color.Thistle || Grid_main.SelectedRows[0].Cells[HeaderInternalCode].Style.BackColor == Color.Turquoise)
                    foreach (var t in Grid_correspondence.Rows.Cast<DataGridViewRow>().Where(g => _tCorrPkSearch.Contains(int.Parse(g.Cells["pk"].Value.ToString()))))
                        foreach (var p in t.Cells.Cast<DataGridViewCell>())
                            p.Style.BackColor = Color.Turquoise;
            }
            catch { }
        }

        private void CalculateColoursByBruteforce(int ekMain, bool shouldUpdate = false)
        {
            //"Маркеры" (deadlines) [примеч.: маркер подсвечивается, когда сроки поджимают и не существует письма-деактиватора]
            var mainYellow = false;
            var mainRed = false;
            foreach (var t in Grid_correspondence.Rows.Cast<DataGridViewRow>().Where(p => !String.IsNullOrWhiteSpace(p.Cells["months_deadline"].Value.ToString())))
                if ((bool)t.Cells["is_incoming"].Value)
                {
                    var t1 = t;
                    var yellow = Grid_correspondence.Rows.Cast<DataGridViewRow>().All(z => z.Cells["dpk"].Value.ToString() != t1.Cells["ik_deactivator"].Value.ToString() || DateTime.Parse(z.Cells["value"].Value.ToString()) < DateTime.Parse(t1.Cells["value"].Value.ToString()) || !(bool)z.Cells["is_sent"].Value); //не использовать (int)z.Cells["dpk"].Value (лучше привести к строке)
                    var red = DateTime.Now.AddDays(WarnDeadline) >= ((DateTime)t.Cells[HeaderIncoming].Value).AddMonths((int)t.Cells["months_deadline"].Value);
                    if (yellow && red)
                    {
                        mainRed = true;
                        t.Cells[HeaderType1].Style.BackColor = Color.Red;
                    }
                    else if (yellow)
                    {
                        mainYellow = true;
                        t.Cells[HeaderType1].Style.BackColor = Color.Yellow;
                    }
                }
            //обновление цвета в таблице
            if (!shouldUpdate) return;
            if (Grid_main.SelectedRows.Count == 0) return;
            if (mainRed)
                TrixOrm.GetInstance().Execute(String.Format("UPDATE t_main SET colour = 2 WHERE pk = {0}", ekMain));
            else if (mainYellow)
                TrixOrm.GetInstance().Execute(String.Format("UPDATE t_main SET colour = 1 WHERE colour < 2 AND pk = {0}", ekMain));  //без условия WHERE colour < 2 мы можем нечаянно перекрыть красный цвет
            else
                TrixOrm.GetInstance().Execute(String.Format("UPDATE t_main SET colour = 0 WHERE colour <= 2 AND pk = {0}", ekMain));  //без условия WHERE colour <= 2 мы можем нечаянно потерять юзерское выделение
        }
        #endregion

        #region //constructor
        public FormMain()
        {
            TrixOrm.GetInstance().ConnectionString = @"Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|\RusPatentDB.mdf;Integrated Security=True;Connect Timeout=60;User Instance=True";
            InitializeComponent();
        }
        #endregion

        #region //common handlers (Form)
        private void FormMainLoad(object sender, EventArgs e)
        {
            var frm = new Form { TopMost = true, StartPosition = FormStartPosition.CenterScreen, FormBorderStyle = FormBorderStyle.None, Opacity = 0.8, BackgroundImage = Resources.IPka, Icon = Icon, Size = Resources.IPka.Size };
            Visible = false;
            frm.Show();
            //первоначальная загрузка (на случай, если не удасться загрузить из XML)
            SortThirdSymbol = true;
            WarnDeadline = 14;
            ShowDeleteAndReport = false;
            RequestCorrespondence = "...";
            PatentCorrespondence = "...";
            PhysicalField = "1. ФИО";
            LegalField = "1. Полное наименование";
            CodeLength = 4;
            UseWord2003 = false;

            //первоначальная загрузка параметров калькулятора
            CalcParameters = new Trix.CalcParameters
            {
                ColumnsGridAutoWidth = new List<int> { 20 },
                ColumnsGridManualWidth = new List<int> { 20 },
                ColumnsGridRightWidth = new List<int> { 20 },
                CommonMargin = CommonMargin,
                ZerosWithinInternalCode = CodeLength,
                FolderDocuments = FolderDocuments,
                FolderTemplates = FolderTemplates,
                FolderMaterials = FolderMaterials,
                FolderIncoming = CorrIncoming,
                FolderOutcoming = CorrOutcoming,
                UseWord2003 = false,
                FormPosition = new Point(100, 100),
                FormSize = new Size(800, 500),
                SplitHorisontal = 200,
                SplitVertical = 250
            };

            //загрузка параметров из XML-файла (для КАЖДОГО параметра следует создать свой блок try-catch)
            try
            {
                var load = XDocument.Load(ConfigFilename);
                // Параметры для главного модуля
                try
                {
                    var i = int.Parse(load.Element("Trix").Element("Main").Element("Top").Value);
                    Top = i > 0 ? i : 0;
                }
                catch { }
                try
                {
                    var i = int.Parse(load.Element("Trix").Element("Main").Element("Left").Value);
                    Left = i > 0 ? i : 0;
                }
                catch { }
                try
                {
                    var i = int.Parse(load.Element("Trix").Element("Main").Element("Width").Value);
                    Width = i < Screen.PrimaryScreen.WorkingArea.Width ? i : 0;
                }
                catch { }
                try
                {
                    var i = int.Parse(load.Element("Trix").Element("Main").Element("Height").Value);
                    Height = i < Screen.PrimaryScreen.WorkingArea.Height ? i : 0;
                }
                catch { }
                try
                {
                    SortThirdSymbol = bool.Parse(load.Element("Trix").Element("Main").Element("SortSymbols").Value);
                }
                catch { }
                try
                {
                    WarnDeadline = int.Parse(load.Element("Trix").Element("Main").Element("WarnDeadline").Value);
                }
                catch { }
                try
                {
                    CodeLength = int.Parse(load.Element("Trix").Element("Main").Element("CodeLength").Value);
                }
                catch { }
                try
                {
                    RequestCorrespondence = load.Element("Trix").Element("Main").Element("RequestCorrespondence").Value;
                }
                catch { }
                try
                {
                    PatentCorrespondence = load.Element("Trix").Element("Main").Element("PatentCorrespondence").Value;
                }
                catch { }
                try
                {
                    FirstCorrespondence = load.Element("Trix").Element("Main").Element("FirstCorrespondence").Value;
                }
                catch { }
                try
                {
                    SuperCorrespondence = load.Element("Trix").Element("Main").Element("SuperCorrespondence").Value;
                }
                catch { }
                try
                {
                    ShowDeleteAndReport = buttonRemove.Visible = buttonReport.Visible = bool.Parse(load.Element("Trix").Element("Main").Element("ShowButtons").Value);
                }
                catch { }
                try
                {
                    UseWord2003 = bool.Parse(load.Element("Trix").Element("Main").Element("UseWord2003").Value);
                }
                catch { }
                try
                {
                    ShowC = bool.Parse(load.Element("Trix").Element("Main").Element("ShowC").Value);
                }
                catch { }
                try
                {
                    ShowArchive = bool.Parse(load.Element("Trix").Element("Main").Element("ShowArchive").Value);
                }
                catch { }
                try
                {
                    WindowState = (FormWindowState)Enum.Parse(typeof(FormWindowState), load.Element("Trix").Element("Main").Element("WindowState").Value);
                }
                catch { }
                try
                {
                    splitContainerMain.SplitterDistance = int.Parse(load.Element("Trix").Element("Main").Element("SplitMain").Value);
                }
                catch { }
                try
                {
                    PhysicalField = load.Element("Trix").Element("Main").Element("PhysicalField").Value;
                }
                catch { }
                try
                {
                    LegalField = load.Element("Trix").Element("Main").Element("LegalField").Value;
                }
                catch { }
                try
                {
                    GridMainFieldsWidth = load.Element("Trix").Element("Main").Element("GridMainWidth").Value.Split(',').Select(t => int.Parse(t)).ToList();
                }
                catch { }
                try
                {
                    GridCorrFieldsWidth = load.Element("Trix").Element("Main").Element("GridCorrespondenceWidth").Value.Split(',').Select(t => int.Parse(t)).ToList();
                }
                catch { }
                // Параметры для модуля калькулятора
                try
                {
                    CalcParameters.ColumnsGridAutoWidth = load.Element("Trix").Element("Calc").Element("CalcGridAutoWidth").Value.Split(',').Select(t => int.Parse(t)).ToList();
                }
                catch { }
                try
                {
                    CalcParameters.ColumnsGridManualWidth = load.Element("Trix").Element("Calc").Element("CalcGridManualWidth").Value.Split(',').Select(t => int.Parse(t)).ToList();
                }
                catch { }
                try
                {
                    CalcParameters.ColumnsGridRightWidth = load.Element("Trix").Element("Calc").Element("CalcGridRightWidth").Value.Split(',').Select(t => int.Parse(t)).ToList();
                }
                catch { }
                try
                {
                    var i = int.Parse(load.Element("Trix").Element("Calc").Element("CalcLeft").Value);
                    var j = int.Parse(load.Element("Trix").Element("Calc").Element("CalcTop").Value);
                    CalcParameters.FormPosition = new Point(i > 0 ? i : 0, j > 0 ? j : 0);
                }
                catch { }
                try
                {
                    var i = int.Parse(load.Element("Trix").Element("Calc").Element("CalcWidth").Value);
                    var j = int.Parse(load.Element("Trix").Element("Calc").Element("CalcHeight").Value);
                    CalcParameters.FormSize = new Size(i, j);
                }
                catch { }
                try
                {
                    CalcParameters.SplitHorisontal = int.Parse(load.Element("Trix").Element("Calc").Element("CalcSplitLeft").Value);
                }
                catch { }
                try
                {
                    CalcParameters.SplitVertical = int.Parse(load.Element("Trix").Element("Calc").Element("CalcSplitMain").Value);
                }
                catch { }
                try
                {
                    CalcParameters.UseWord2003 = UseWord2003;
                }
                catch { }
            }
            catch { }

            //обновление главного грида
            RefreshTheMainGrid();
            Grid_main.Columns[HeaderInternalCode].SortMode = DataGridViewColumnSortMode.Programmatic;

            //адское вычисление цвета маркеров (следует сделать это между 2-мя обновлениями главного грида (первое - для получения данных, второе - для прорисовки))
            foreach (DataGridViewRow t in Grid_main.Rows)
            {
                t.Selected = true;
                CalculateColoursByBruteforce(int.Parse(t.Cells["pk"].Value.ToString()), true);
            }
            RefreshTheMainGrid();

            //выбор последней строки
            if (Grid_main.Rows.Count > 0)
            {
                Grid_main.Rows[Grid_main.Rows.Count - 1].Selected = true;
                Grid_main.FirstDisplayedScrollingRowIndex = Grid_main.Rows.Count - 1;
            }

            //проверка на существование каталогов
            var s = Directory.GetCurrentDirectory() + @"\";
            foreach (var t in new[] { s + FolderDocuments, s + FolderTemplates, s + FolderRecycle }.Where(p => !Directory.Exists(p)))
                Directory.CreateDirectory(t);

            //убираем сортировку правого грида
            foreach (DataGridViewColumn t in Grid_correspondence.Columns)
                t.SortMode = DataGridViewColumnSortMode.NotSortable;

            //разбираемся с тем, архив это или нет (use this code after downloading the settings)
            if (ShowArchive) MessageBox.Show(ThisIsArchive, Warning);
            ((ToolStripMenuItem)contextMenuMain.Items["move"]).DropDownItems[1].Text = ShowArchive ? ContextMenuFromArchive : ContextMenuToArchive;

            //вызов сообщений калькулятора (это не быдлокод! Форму нужно не просто создать, а вызвать)
            var calc = new Calculator.FormMain(CalcParameters);
            calc.Show();
            calc.Close();

            //ура! Конец этой страшной загрузки
            frm.Close();
            Visible = true;
        }

        private void FormMainFormClosing(object sender, FormClosingEventArgs e)
        {
            //сохранение параметров в XML
            var save = new XDocument(new XElement("Trix",
                                        new XElement("Main",
                                            new XElement("Top", Top),
                                            new XElement("Left", Left),
                                            new XElement("Width", Width),
                                            new XElement("Height", Height),
                                            new XElement("SortSymbols", SortThirdSymbol),
                                            new XElement("WarnDeadline", WarnDeadline),
                                            new XElement("CodeLength", CodeLength),
                                            new XElement("RequestCorrespondence", RequestCorrespondence),
                                            new XElement("PatentCorrespondence", PatentCorrespondence),
                                            new XElement("FirstCorrespondence", FirstCorrespondence),
                                            new XElement("SuperCorrespondence", SuperCorrespondence),
                                            new XElement("ShowButtons", ShowDeleteAndReport),
                                            new XElement("UseWord2003", UseWord2003),
                                            new XElement("ShowC", UseWord2003),
                                            new XElement("ShowArchive", ShowArchive),
                                            new XElement("WindowState", WindowState),
                                            new XElement("PhysicalField", PhysicalField),
                                            new XElement("LegalField", LegalField),
                                            new XElement("SplitMain", splitContainerMain.SplitterDistance),
                                            new XElement("GridMainWidth", Grid_main.Columns.Count > 0 ? Grid_main.Columns.Cast<DataGridViewColumn>().Select(t => t.Width).Aggregate("", (cur, t) => cur + (", " + t)).Substring(2) : CommonMargin.ToString()),
                                            new XElement("GridCorrespondenceWidth", GridCorrFieldsWidth.Count > 0 ? GridCorrFieldsWidth.Aggregate("", (cur, t) => cur + ", " + t).Substring(2) : CommonMargin.ToString())),
                                        new XElement("Calc",
                                            new XElement("CalcGridAutoWidth", CalcParameters.ColumnsGridAutoWidth.Count > 0 ? CalcParameters.ColumnsGridAutoWidth.Aggregate("", (cur, t) => cur + (", " + t)).Substring(2) : CommonMargin.ToString()),
                                            new XElement("CalcGridManualWidth", CalcParameters.ColumnsGridManualWidth.Count > 0 ? CalcParameters.ColumnsGridManualWidth.Aggregate("", (cur, t) => cur + (", " + t)).Substring(2) : CommonMargin.ToString()),
                                            new XElement("CalcGridRightWidth", CalcParameters.ColumnsGridRightWidth.Count > 0 ? CalcParameters.ColumnsGridRightWidth.Aggregate("", (cur, t) => cur + (", " + t)).Substring(2) : CommonMargin.ToString()),
                                            new XElement("CalcLeft", CalcParameters.FormPosition.X),
                                            new XElement("CalcTop", CalcParameters.FormPosition.Y),
                                            new XElement("CalcWidth", CalcParameters.FormSize.Width),
                                            new XElement("CalcHeight", CalcParameters.FormSize.Height),
                                            new XElement("CalcSplitMain", CalcParameters.SplitVertical),
                                            new XElement("CalcSplitLeft", CalcParameters.SplitHorisontal))));
            save.Save(ConfigFilename);
        }
        #endregion

        #region //common handlers (Grids)
        private void GridMainSelectionChanged(object sender, EventArgs e)
        {
            //видимость кнопок добавления корреспонденции
            buttonIncoming.Enabled = buttonOutcoming.Enabled = Grid_main.SelectedRows.Count > 0;
            //обновляем гриды справа
            if (Grid_main.SelectedRows.Count == 0)
                RefreshRightGrids(-1);
            else
                RefreshRightGrids((int)Grid_main.SelectedRows[0].Cells["pk"].Value);
        }

        private void GridMainCellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            //обязательно надо выделить, т.к. в событии MouseClick выделение полностью снимается (try тоже обязателен, т.к. юзер может ткнуть на заголовок)
            try { Grid_main.Rows[e.RowIndex].Selected = true; }
            catch { }
            //проверки
            if (e.Button != MouseButtons.Right) return;
            if (e.RowIndex < 0) return;
            //вызов контекстного меню
            contextMenuMain.Show(MousePosition.X + CommonMargin, MousePosition.Y);
        }

        private void GridMainMouseClick(object sender, MouseEventArgs e)
        {
            //очистка выделения
            foreach (DataGridViewRow t in Grid_main.Rows)
                t.Selected = false;
            //очистка правого грида
            RefreshRightGrids(int.MinValue);
        }

        private void GridMainMouseDoubleClick(object sender, MouseEventArgs e)
        {
            //двойной щелчок открывает папку проекта
            ToolStripMenuItemMainExploreClick();
        }

        private void GridMainColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //если это не столбец internal_code, то он отсортируется по-обычному
            if (Grid_main.Columns[HeaderInternalCode].Index != e.ColumnIndex) return;
            //иначе сортируем врукопашку (ОБЯЗАТЕЛЬНО через SQL (т.к. у нас Data-bound Grid))
            RefreshTheMainGrid();
            //ставим глиф
            Grid_main.Columns[HeaderInternalCode].HeaderCell.SortGlyphDirection = _isAsc ? SortOrder.Descending : SortOrder.Ascending;
            //меняем направление
            _isAsc = !_isAsc;
        }

        private void GridCorrespondenceCellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //открытие папки
            ToolStripMenuItemCorrExploreClick(this, null);
        }

        private void GridCorrespondenceCellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            //проверки
            if (e.RowIndex < 0) return;
            if (e.Button == MouseButtons.Right)
            {
                //вызов контекстного меню
                Grid_correspondence.Rows[e.RowIndex].Selected = true;
                contextMenuCorrespondence.Show(MousePosition.X + CommonMargin, MousePosition.Y);
            }
            //"отправление" корреспонденции
            else if (e.Button == MouseButtons.Left && e.ColumnIndex == Grid_correspondence.Columns["Column1"].Index && Grid_main.SelectedRows.Count > 0)
            {
                var pk = int.Parse(Grid_main.SelectedRows[0].Cells["pk"].Value.ToString());
                if (((Bitmap)Grid_correspondence.Rows[e.RowIndex].Cells[e.ColumnIndex].Value).GetPixel(10, 10) == Resources.no.GetPixel(10, 10))
                {
                    var frm = new FormCalendar(DateTime.Parse(Grid_correspondence.Rows[e.RowIndex].Cells["value"].Value.ToString()), null);
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        TrixOrm.GetInstance().Execute(String.Format("UPDATE t_correspondence SET is_sent = 'True', value = '{0}' WHERE pk = {1}", DateTime.Parse(frm.Tag.ToString()).ToShortDateString().Replace('/', '.'), Grid_correspondence.Rows[e.RowIndex].Cells["pk"].Value));
                        RefreshRightGrids(pk, true);
                        RefreshTheMainGrid();
                    }
                }
                else if (((Bitmap)Grid_correspondence.Rows[e.RowIndex].Cells[e.ColumnIndex].Value).GetPixel(10, 10) == Resources.yes.GetPixel(10, 10))
                {
                    if (MessageBox.Show(WarningChangeSendStatus, Warning, MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                    {
                        TrixOrm.GetInstance().Execute(String.Format("UPDATE t_correspondence SET is_sent = 'False' WHERE pk = {0}", Grid_correspondence.Rows[e.RowIndex].Cells["pk"].Value));
                        RefreshRightGrids(pk, true);
                        RefreshTheMainGrid();
                    }
                }
            }
        }

        private void GridCorrespondenceColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (ActiveControl == null) return;
            GridCorrFieldsWidth = Grid_correspondence.Columns.Cast<DataGridViewColumn>().Select(t => t.Width).ToList();
        }
        #endregion

        #region //usual buttons click
        private void ButtonAddClick(object sender, EventArgs e)
        {
            DialogResult dres;
            FormNewProject frm;
            //Примечание: форму следует перезагрузить, если нарушена целостность БД. Например, если назвать проект существующим именем, то SQL-Server выдаёт ошибку,
            //но всё же добавляет, сука, единичку к идентифицируемому столбцу! Форма об этом не знает (она получает номер при загрузке). Поэтому форму будем запускать
            //много раз в цикле. Прошу прощения за быдлокод.
            do
            {
                frm = new FormNewProject();
                dres = frm.ShowDialog();
            } while (dres == DialogResult.Retry);
            //если была нажата "ОК", но перезагружаем грид
            if (dres != DialogResult.Cancel)
                RefreshTheMainGrid(true, true);
        }

        private void ButtonRemoveClick(object sender, EventArgs e)
        {
            //проверки
            if (Grid_main.SelectedRows.Count == 0) return;
            var t = Grid_main.SelectedRows[0];
            if (MessageBox.Show(String.Format(Question, t.Cells[HeaderProjectName].Value.ToString().Trim(), t.Cells[HeaderInternalCode].Value.ToString().Trim()), Warning, MessageBoxButtons.YesNoCancel) != DialogResult.Yes) return;
            //удаление
            RemoveTheItem(t.Cells["pk"].Value.ToString().Trim(), t.Cells[HeaderInternalCode].Value.ToString().Trim(), t.Cells[HeaderProjectName].Value.ToString().Trim());
            //обновление
            RefreshTheMainGrid();
        }

        private void ButtonReportClick(object sender, EventArgs e)
        {
            if (Grid_main.SelectedRows.Count == 0) return;
            var frm = new FormReport(int.Parse(Grid_main.SelectedRows[0].Cells["pk"].Value.ToString()));
            frm.ShowDialog();
        }

        private void ButtonSearchClick(object sender, EventArgs e)
        {
            //организация функции "Перейти далее")
            if (!String.IsNullOrWhiteSpace(_searchString) && _searchString == textBoxSearch.Text.Trim())
                try
                {
                    var currentPk = Grid_main.SelectedRows[0].Cells["pk"].Value.ToString();
                    var currentRow = Grid_main.Rows.Cast<DataGridViewRow>().First(t => t.Cells["pk"].Value.ToString() == currentPk).Index;
                    var nextRow = Grid_main.Rows.Cast<DataGridViewRow>().FirstOrDefault(t => (new[] { Color.Thistle, Color.Turquoise }).Contains(t.Cells["pk"].Style.BackColor) && t.Index > currentRow) ??
                                  Grid_main.Rows.Cast<DataGridViewRow>().FirstOrDefault(t => (new[] { Color.Thistle, Color.Turquoise }).Contains(t.Cells["pk"].Style.BackColor));
                    nextRow.Selected = true;
                    Grid_main.FirstDisplayedScrollingRowIndex = nextRow.Index;
                    return;
                }
                catch { }
            //меняем стиль ячеек на обычный
            foreach (DataGridViewRow t in Grid_main.Rows)
                foreach (var p in t.Cells.Cast<DataGridViewCell>().Where(w => Grid_main.Columns[w.ColumnIndex].HeaderText != HeaderColour))
                    p.Style.BackColor = SystemColors.Window;
            //проверки
            if (String.IsNullOrWhiteSpace(textBoxSearch.Text)) return;
            if (textBoxSearch.Text.Contains("'")) { MessageBox.Show(SqlInjection, Warning); return; }
            //поиск данных
            var rows = TrixOrm.GetInstance().GetDataTable(String.Format("SELECT t_main.pk, t_main.pk           FROM t_main INNER JOIN      d_icodes    ON project_type = usercode                                                          WHERE CHARINDEX('{0}', projectname) > 0 OR CHARINDEX('{0}', request_number) > 0 OR CHARINDEX('{0}', patent_number) > 0 OR CHARINDEX('{0}', date_created) > 0 OR CHARINDEX('{0}', date_received) > 0 OR CHARINDEX('{0}', {1}) > 0", textBoxSearch.Text.Trim(), GetInternalCodeForSql())).Rows.Cast<DataRow>().ToList();
            rows.AddRange(TrixOrm.GetInstance().GetDataTable(String.Format("  SELECT t_main.pk, t_main.pk           FROM t_main INNER JOIN   t_requisites   ON t_requisites.client_code = t_main.client_code                                    WHERE CHARINDEX('{0}', value) > 0", textBoxSearch.Text.Trim())).Rows.Cast<DataRow>());
            rows.AddRange(TrixOrm.GetInstance().GetDataTable(String.Format("  SELECT t_main.pk, t_correspondence.pk FROM t_main INNER JOIN t_correspondence ON ek_main = t_main.pk INNER JOIN d_correspondence ON ek_corr = d_correspondence.pk WHERE CHARINDEX('{0}', value) > 0 OR CHARINDEX('{0}', corrname) > 0 OR CHARINDEX('{0}', corrshortname) > 0 OR CHARINDEX('{0}', template) > 0 OR CHARINDEX('{0}', comment) > 0", textBoxSearch.Text.Trim())).Rows.Cast<DataRow>());
            //очищаем список первичных ключей таблицы t_correspondence (чтоб правый грид тоже раскрашивался)
            _tCorrPkSearch.Clear();
            //меняем стиль нужных ячеек и добавляем первичные ключи t_correspondence в список
            foreach (DataRow t in rows)
            {
                var t1 = t;
                _tCorrPkSearch.Add(int.Parse(t1.ItemArray[1].ToString()));
                foreach (var q in Grid_main.Rows.Cast<DataGridViewRow>().Where(p => p.Cells["pk"].Value.ToString() == t1.ItemArray[0].ToString()))
                    foreach (var p in q.Cells.Cast<DataGridViewCell>().Where(w => Grid_main.Columns[w.ColumnIndex].HeaderText != HeaderColour))
                        p.Style.BackColor = p.Value.ToString().Trim().ToUpper().Contains(textBoxSearch.Text.Trim().ToUpper()) ? Color.Thistle : Color.Turquoise;
            }
            //показ сообщения о неуспешном поиске
            if (rows.Count == 0) MessageBox.Show(SearchFailed);
            textBoxSearch.SelectAll();
            textBoxSearch.Focus();
            //запоминаем строчку (для организации функции "Перейти далее")
            _searchString = textBoxSearch.Text.Trim();
        }

        private void ButtonIncomingClick(object sender, EventArgs e)
        {
            if (Grid_main.SelectedRows.Count == 0) return;
            var pk = int.Parse(Grid_main.SelectedRows[0].Cells["pk"].Value.ToString());
            var internalcode = Grid_main.SelectedRows[0].Cells[HeaderInternalCode].Value.ToString().Trim();
            var projectname = Grid_main.SelectedRows[0].Cells[HeaderProjectName].Value.ToString().Trim();
            var minDate = Grid_correspondence.Rows.Count == 0 ? DateTime.Now : Grid_correspondence.Rows.Cast<DataGridViewRow>().Max(t => DateTime.Parse(!String.IsNullOrWhiteSpace(t.Cells[HeaderIncoming].Value.ToString()) ? t.Cells[HeaderIncoming].Value.ToString() : @"1.1.1990"));        //раньше было t.Cells["value"], но теперь по требованию заказчика было изменено на t.Cells[HeaderIncoming]
            //ругаемся на то, что существует неотправленная корреспонденция
            if (Grid_correspondence.Rows.Cast<DataGridViewRow>().Any(p => p.Cells[HeaderIsSent].Value.ToString().ToUpper().Trim() == "FALSE"))
            {
                MessageBox.Show(String.Format(ErrorNotSent));
                return;
            }
            //добавляем приоритетную справку, если её ещё не было
            if (Grid_correspondence.Rows.Cast<DataGridViewRow>().All(p => p.Cells[HeaderType1].Value.ToString().Trim() != RequestCorrespondence))
            {
                if (MessageBox.Show(String.Format(WarningPriority, RequestCorrespondence), "", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                {
                    var frn = new FormCalendar(null, minDate);
                    if (frn.ShowDialog() == DialogResult.OK)
                    {
                        FormReportStart.AddNewCorrespondence(RequestCorrespondence, (DateTime)frn.Tag, true, null, pk, null, null, internalcode, projectname, null);
                        RefreshTheMainGrid();
                    }
                }
                return;
            }
            var frm = new FormReportStart(pk, internalcode, projectname, true, minDate);
            if (frm.ShowDialog() != DialogResult.OK) return;
            RefreshRightGrids(pk, true);
            RefreshTheMainGrid();
        }

        private void ButtonOutcomingClick(object sender, EventArgs e)
        {
            if (Grid_main.SelectedRows.Count == 0) return;
            var pk = int.Parse(Grid_main.SelectedRows[0].Cells["pk"].Value.ToString());
            var internalcode = Grid_main.SelectedRows[0].Cells[HeaderInternalCode].Value.ToString().Trim();
            var projectname = Grid_main.SelectedRows[0].Cells[HeaderProjectName].Value.ToString().Trim();
            var a = Grid_correspondence.Rows.Cast<DataGridViewRow>().Any(p => p.Cells[HeaderIsSent].Value.ToString().ToUpper().Trim() == "FALSE");
            var b = Grid_correspondence.Rows.Cast<DataGridViewRow>().All(p => p.Cells[HeaderType1].Value.ToString().Trim() != RequestCorrespondence);
            if (a)
                MessageBox.Show(ErrorNotSent);
            else if (b)
                MessageBox.Show(String.Format(WarningFirstPriority, RequestCorrespondence));
            if (a || b)
            {
                if (!String.IsNullOrWhiteSpace(SuperCorrespondence) && MessageBox.Show(String.Format(YouCanAddUsualLetter, SuperCorrespondence), Warning, MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                {
                    FormReportStart.AddNewCorrespondence(SuperCorrespondence, DateTime.Now, false, null, pk, null, null, internalcode, projectname, null);
                    RefreshTheMainGrid();
                }
                return;
            }
            var frm = new FormReportStart(pk, internalcode, projectname, false);
            if (frm.ShowDialog() != DialogResult.OK) return;
            RefreshRightGrids(pk, true);
            RefreshTheMainGrid();
        }
        #endregion

        #region //menu buttons click
        private void КалькуляторToolStripMenuItemClick(object sender, EventArgs e)
        {
            var calculator = new Calculator.FormMain(CalcParameters);
            if (calculator.ShowDialog() == DialogResult.OK)
                RefreshTheMainGrid();
            CalcParameters = (Trix.CalcParameters)calculator.Tag;
        }

        private void НастройкиToolStripMenuItemClick(object sender, EventArgs e)
        {
            var t = CodeLength;
            var frm = new FormSettings();
            if (frm.ShowDialog() != DialogResult.OK) return;
            if (t != CodeLength)
            {
                foreach (var p in TrixOrm.GetInstance().GetListOfCortages("SELECT pk, project_type, projectname FROM t_main"))
                {
                    var pk = p[0].ToString().Trim();
                    var projecttype = p[1].ToString().Trim();
                    var oldinternal = GetInternalCode(projecttype, pk, true, t);
                    var newinternal = GetInternalCode(projecttype, pk, true, CodeLength);
                    var projectname = p[2].ToString().Trim();
                    RenameDirectories(oldinternal, newinternal, projectname, projectname);
                }
            }
            //обновление параметров калькулятора
            CalcParameters.ZerosWithinInternalCode = CodeLength;
            CalcParameters.UseWord2003 = UseWord2003;
            //обновление
            RefreshTheMainGrid();
            buttonRemove.Visible = buttonReport.Visible = ShowDeleteAndReport;
            ((ToolStripMenuItem)contextMenuMain.Items["move"]).DropDownItems[1].Text = ShowArchive ? ContextMenuFromArchive : ContextMenuToArchive;
        }

        private void ВыходToolStripMenuItemClick(object sender, EventArgs e)
        {
            Close();
        }

        private void РеквизитыToolStripMenuItemClick(object sender, EventArgs e)
        {
            var frm = new FormRequisites();
            frm.ShowDialog();
            if (Grid_main.SelectedRows.Count > 0)
                RefreshRightGrids(int.Parse(Grid_main.SelectedRows[0].Cells["pk"].Value.ToString()));
        }

        private void КорреспонденцияToolStripMenuItemClick(object sender, EventArgs e)
        {
            var frm = new FormCorrStart();
            frm.ShowDialog();
            if (Grid_main.SelectedRows.Count > 0)
                RefreshRightGrids(int.Parse(Grid_main.SelectedRows[0].Cells["pk"].Value.ToString()));
        }

        private void ТипыПроектовToolStripMenuItemClick(object sender, EventArgs e)
        {
            var frm = new FormProjects();
            frm.ShowDialog();
            RefreshTheMainGrid();
        }

        private void ЗапросыToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (Grid_main.SelectedRows.Count == 0) return;
            var frm = new FormReport(int.Parse(Grid_main.SelectedRows[0].Cells["pk"].Value.ToString()));
            frm.ShowDialog();
            RefreshRightGrids(int.Parse(Grid_main.SelectedRows[0].Cells["pk"].Value.ToString()));
        }

        private void ОткрытьКорзинуToolStripMenuItemClick(object sender, EventArgs e)
        {
            var s = String.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), FolderRecycle);
            try { Process.Start(s); }
            catch { MessageBox.Show(ErrorFolder + s, Warning); }
        }

        private void ОчиститьКорзинуToolStripMenuItemClick(object sender, EventArgs e)
        {
            var recycle = String.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), FolderRecycle);
            try
            {
                if (Directory.EnumerateDirectories(recycle).Count() + Directory.EnumerateFiles(recycle).Count() == 0) { MessageBox.Show(RecycleEmpty); return; }
                if (MessageBox.Show(QuestionRecycle, Warning, MessageBoxButtons.YesNoCancel) != DialogResult.Yes) return;
                Directory.Delete(recycle, true);
                Directory.CreateDirectory(recycle);
            }
            catch { }
        }

        private void СправкаToolStripMenuItem1Click(object sender, EventArgs e)
        {
            //справка отсутствует
        }

        private void ОПрограммеToolStripMenuItemClick(object sender, EventArgs e)
        {
            MessageBox.Show(About);
        }
        #endregion

        #region //context menu buttons click (Grid_main)
        private void ToolStripMenuItemMainExploreClick()
        {
            if (Grid_main.SelectedRows.Count == 0) return;
            var s = String.Format(@"{0}\{1}\{2}_{3}\{4}", Directory.GetCurrentDirectory(), FolderDocuments, Grid_main.SelectedRows[0].Cells[HeaderInternalCode].Value.ToString().Trim(), Grid_main.SelectedRows[0].Cells[HeaderProjectName].Value.ToString().Trim(), FolderMaterials);
            try { Process.Start(s); }
            catch { MessageBox.Show(ErrorFolder + s, Warning); }
        }



        private void ToolStripMenuItemMaterialsClick(object sender, EventArgs e)
        {
            if (Grid_main.SelectedRows.Count == 0) return;
            var s = String.Format(@"{0}\{1}\{2}_{3}\{4}", Directory.GetCurrentDirectory(), FolderDocuments, Grid_main.SelectedRows[0].Cells[HeaderInternalCode].Value.ToString().Trim(), Grid_main.SelectedRows[0].Cells[HeaderProjectName].Value.ToString().Trim(), FolderMaterials);
            try { Process.Start(s); }
            catch { MessageBox.Show(ErrorFolder + s, Warning); }
        }



        private void ToolStripMenuItemMainCopynumberClick(object sender, EventArgs e)
        {
            if (Grid_main.SelectedRows.Count == 0) return;
            var s = Grid_main.SelectedRows[0].Cells[HeaderRequestNumber].Value.ToString().Trim();
            if (String.IsNullOrWhiteSpace(s)) Clipboard.Clear();
            else Clipboard.SetText(s);
        }



        private void ToolStripMenuItemMainTrashClick(object sender, EventArgs e)
        {
            ButtonRemoveClick(this, null);
        }



        private void ToolStripMenuItemMainModifyClick(object sender, EventArgs e)
        {
            //получаем объект выделенной строки
            if (Grid_main.SelectedRows.Count == 0) return;
            var t = Grid_main.SelectedRows[0];
            var frm = new FormDb(new ParametersDb
            {
                ProjectTypeText = t.Cells[HeaderProjectShortType].Value.ToString().Trim(),
                ProjectName = t.Cells[HeaderProjectName].Value.ToString().Trim(),
                RequestNumber = t.Cells[HeaderRequestNumber].Value.ToString().Trim(),
                PatentNumber = t.Cells[HeaderPatentNumber].Value.ToString().Trim(),
                IsLegal = bool.Parse(t.Cells[HeaderIsLegal].Value.ToString()),
                DateCreated = t.Cells[HeaderDateCreated].Value.ToString(),
                DateReceived = t.Cells[HeaderDateReceived].Value.ToString(),
                Pk = int.Parse(t.Cells["pk"].Value.ToString()),
                ClientCode = t.Cells["client_code"].Value.ToString(),
                ColourType = int.Parse(t.Cells["colour"].Value.ToString())
            });
            var dres = frm.ShowDialog();
            //если была нажата "ОК", то перезагружаем грид
            if (dres != DialogResult.OK) return;
            CalculateColoursByBruteforce(int.Parse(t.Cells["pk"].Value.ToString()), true);
            RefreshTheMainGrid();
        }



        private void ToolStripMenuItemArchiveClick(object sender, EventArgs e)
        {
            if (Grid_main.SelectedRows.Count == 0) return;
            TrixOrm.GetInstance().Execute(String.Format("UPDATE t_main SET is_archive = {0} WHERE pk = {1}", ShowArchive ? @"NULL" : @"'True'", Grid_main.SelectedRows[0].Cells["pk"].Value));
            RefreshTheMainGrid();
        }


        private void ToolStripMenuItemToCalculatorClick(object sender, EventArgs e)
        {
            if (Grid_main.SelectedRows.Count == 0) return;
            var calculator = new Calculator.FormMain(CalcParameters, Grid_main.SelectedRows[0].Cells[HeaderInternalCode].Value.ToString());
            if (calculator.ShowDialog() == DialogResult.OK)
                RefreshTheMainGrid();
            CalcParameters = (Trix.CalcParameters)calculator.Tag;
        }



        //код отменён по требованию заказчика
        /*
                private void ToolStripMenuItemMainReportClick(object sender, EventArgs e)
                {
                    ButtonReportClick(this, null);
                }

                private void ToolStripMenuItemMainPatentClick(object sender, EventArgs e)
                {
                    if (Grid_main.SelectedRows.Count == 0) return;
                    var s = Grid_main.SelectedRows[0].Cells[HeaderPatentNumber].Value.ToString().Trim();
                    MessageBox.Show(String.IsNullOrWhiteSpace(s) ? PatentNo : String.Format(PatentYes, s));
                }

                private void ToolStripMenuItemRequestClick(object sender, EventArgs e)
                {
                    if (Grid_main.SelectedRows.Count == 0) return;
                    var s = String.Format(@"{0}\{1}\{2}_{3}\{4}", Directory.GetCurrentDirectory(), FolderDocuments, Grid_main.SelectedRows[0].Cells[HeaderInternalCode].Value.ToString().Trim(), Grid_main.SelectedRows[0].Cells[HeaderProjectName].Value.ToString().Trim(), FolderRequest);
                    try { Process.Start(s); }
                    catch { MessageBox.Show(ErrorFolder + s, Warning); }
                }

                private void ToolStripMenuItemMainAddClick(object sender, EventArgs e)
                {
                    ButtonAddEditClick(this, null);
                }
        */
        #endregion

        #region //context menu buttons click (Grid_correspondence)
        private void ToolStripMenuItemCorrExploreClick(object sender, EventArgs e)
        {
            if (Grid_main.SelectedRows.Count == 0) return;
            var incoming = (bool)Grid_correspondence.SelectedRows[0].Cells["is_incoming"].Value;
            var s = String.Format(incoming ? @"{0}\{1}\{2}_{3}\{4}" : @"{0}\{1}\{2}_{3}\{4}\{2}_{5}_{6}", Directory.GetCurrentDirectory(), FolderDocuments, Grid_main.SelectedRows[0].Cells[HeaderInternalCode].Value.ToString().Trim(), Grid_main.SelectedRows[0].Cells[HeaderProjectName].Value.ToString().Trim(), incoming ? CorrIncoming : CorrOutcoming, Grid_correspondence.SelectedRows[0].Cells["corrshortname"].Value.ToString().Trim(), Grid_correspondence.SelectedRows[0].Cells["pk"].Value.ToString().Trim());
            try { Process.Start(s); }
            catch { MessageBox.Show(ErrorFolder + s, Warning); }
        }

        private void ToolStripMenuItemCommentClick(object sender, EventArgs e)
        {
            if (Grid_correspondence.SelectedRows.Count == 0) return;
            MessageBox.Show(Grid_correspondence.SelectedRows[0].Cells["comment"].Value.ToString());
        }

        private void ToolStripMenuItemDateClick(object sender, EventArgs e)
        {
            if (Grid_correspondence.SelectedRows.Count == 0) return;
            MessageBox.Show(DateTime.Parse(Grid_correspondence.SelectedRows[0].Cells["value"].Value.ToString()).ToShortDateString());
        }

        private void ToolStripMenuItemAlterClick(object sender, EventArgs e)
        {
            if (Grid_main.SelectedRows.Count == 0) return;
            if (Grid_correspondence.SelectedRows.Count == 0) return;
            var pk = int.Parse(Grid_main.SelectedRows[0].Cells["pk"].Value.ToString());
            var incoming = (bool)Grid_correspondence.SelectedRows[0].Cells["is_incoming"].Value;
            var frm = new FormReportStart(pk, Grid_main.SelectedRows[0].Cells[HeaderInternalCode].Value.ToString().Trim(), Grid_main.SelectedRows[0].Cells[HeaderProjectName].Value.ToString().Trim(), incoming, null, Grid_correspondence.SelectedRows[0].Cells[incoming ? HeaderType1 : HeaderType2].Value.ToString(), Grid_correspondence.SelectedRows[0].Cells["corrshortname"].Value.ToString().Trim(), int.Parse(Grid_correspondence.SelectedRows[0].Cells["pk"].Value.ToString()), DateTime.Parse(Grid_correspondence.SelectedRows[0].Cells["value"].Value.ToString()), Grid_correspondence.SelectedRows[0].Cells["comment"].Value.ToString());
            frm.ShowDialog();
            RefreshTheMainGrid();           //обязательно Main_grid, а не Grid_correspondence!
        }

        private void ToolStripMenuItemRemoveClick(object sender, EventArgs e)
        {
            if (Grid_main.SelectedRows.Count == 0) return;
            if (Grid_correspondence.SelectedRows.Count == 0) return;
            if (MessageBox.Show(String.Format(WarningDeleteCorr, Grid_correspondence.SelectedRows[0].Cells[HeaderType1].Value.ToString() + Grid_correspondence.SelectedRows[0].Cells[HeaderType2].Value), Warning, MessageBoxButtons.YesNoCancel) != DialogResult.Yes) return;
            RemoveTheCorrItem(Grid_correspondence.SelectedRows[0].Cells["pk"].Value.ToString(), Grid_main.SelectedRows[0].Cells[HeaderInternalCode].Value.ToString(), Grid_main.SelectedRows[0].Cells[HeaderProjectName].Value.ToString(), Grid_correspondence.SelectedRows[0].Cells["corrshortname"].Value.ToString());
            RefreshRightGrids(int.Parse(Grid_main.SelectedRows[0].Cells["pk"].Value.ToString()));
        }
        #endregion
    }
}
