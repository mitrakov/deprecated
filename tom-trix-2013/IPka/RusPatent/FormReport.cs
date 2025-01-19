// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable EmptyGeneralCatchClause
#pragma warning disable 168

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Calculator;
using Microsoft.Office.Interop.Word;
using Application = Microsoft.Office.Interop.Word.Application;

namespace IPka
{
    public partial class FormReport : Form
    {
        #region //constants
        private const string Legal = @"Юридическое лицо";
        private const string NonLegal = @"Физическое лицо";
        private const string HeaderProjType = @"Тип проекта";
        private const string HeaderToday = @"Текущая дата";
        private const string DefaultString = @" {ERROR} ";
        private const string WordError = @"Произошла ошибка при работе с Word";
        private const string SizeError = @"Ошибка. Не используйте файлы размером более 255 Мб";
        private const string DbError = "Произошла ошибка при сохранении поля '{0}' в БД\nВозможно, длина текста превышает допустимую";
        private const string NoMsWord = @"Не удалось запустить MS Office Word. Проверьте, что приложение установлено должным образом";
        #endregion

        #region //variables
        private int _number;                //номер подстановочного блока (надпись слева в ComboText)
        private readonly int _ekMain;       //первичный ключ таблицы t_main
        #endregion

        #region //constructor
        public FormReport(int ekMain)
        {
            InitializeComponent();
            _ekMain = ekMain;
            openFileDialog1.InitialDirectory = String.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), FormMain.FolderTemplates);
        }
        #endregion

        #region //static methods
        private static string GetDataByParameter(string parameter, int ekMain, string client)
        {
            try
            {
                switch (parameter)
                {
                    case FormMain.HeaderInternalCode:
                        return TrixOrm.GetInstance().GetScalar(String.Format("SELECT RTRIM({1}) FROM t_main INNER JOIN d_icodes ON project_type = usercode WHERE t_main.pk = {0}", ekMain, FormMain.GetInternalCodeForSql())).ToString();
                    case FormMain.HeaderProjectName:
                        return TrixOrm.GetInstance().GetScalar(String.Format("SELECT RTRIM(projectname) FROM t_main WHERE pk = {0}", ekMain)).ToString();
                    case FormMain.HeaderRequestNumber:
                        return TrixOrm.GetInstance().GetScalar(String.Format("SELECT RTRIM(request_number) FROM t_main WHERE pk = {0}", ekMain)).ToString();
                    case FormMain.HeaderPatentNumber:
                        return TrixOrm.GetInstance().GetScalar(String.Format("SELECT RTRIM(patent_number) FROM t_main WHERE pk = {0}", ekMain)).ToString();
                    case FormMain.HeaderIsLegal:
                        return (bool)TrixOrm.GetInstance().GetScalar(String.Format("SELECT is_legal FROM t_main WHERE pk = {0}", ekMain)) ? Legal : NonLegal;
                    case FormMain.HeaderDateCreated:
                        var w = TrixOrm.GetInstance().GetScalar(String.Format("SELECT date_created FROM t_main WHERE pk = {0}", ekMain));
                        return w != null ? ((DateTime)w).ToShortDateString() : @"";
                    case HeaderProjType:
                        return TrixOrm.GetInstance().GetScalar(String.Format("SELECT RTRIM(codename) FROM t_main INNER JOIN d_icodes ON project_type = usercode WHERE t_main.pk = {0}", ekMain)).ToString();
                    case HeaderToday:
                        return DateTime.Now.ToShortDateString();
                    default:
                        //данные выбираются из динамического поля Requisites
                        object query;
                        var x = int.Parse(TrixOrm.GetInstance().GetScalar(String.Format("SELECT COUNT(*) FROM d_requisites WHERE requisname = '{0}'", parameter)).ToString());
                        if (x > 0)
                        {
                            query = TrixOrm.GetInstance().GetScalar(String.Format("SELECT RTRIM(value) FROM t_requisites WHERE client_code = {0} AND ek_requis IN (SELECT pk FROM d_requisites WHERE requisname = '{1}')", client, parameter));
                            return query == null ? DefaultString : query.ToString();
                        }
                        //данные выбираются из динамического поля Correspondence
                        var y = int.Parse(TrixOrm.GetInstance().GetScalar(String.Format("SELECT COUNT(*) FROM d_correspondence WHERE corrname = '{0}'", parameter)).ToString());
                        if (y > 0)
                        {
                            query = TrixOrm.GetInstance().GetScalar(String.Format("SELECT RTRIM(value) FROM t_correspondence WHERE ek_main = {0} AND ek_corr IN (SELECT pk FROM d_correspondence WHERE corrname = '{1}')", ekMain, parameter));
                            return query == null ? DefaultString : DateTime.Parse(query.ToString()).ToShortDateString();
                        }
                        break;
                }
            }
            catch { }
            return null;
        }

        public static bool GenerateTheReport(int ekMain, IEnumerable<string> parameterlist, IEnumerable<string> textlist, string templateName, string destinationPath, string destinationFilename)
        {
            var success = true;             //result
            var quitFromForeach = false;    //если в абзаце несколько подстановочных полей (напр. "^0^...^1^"), то сработает только первое! В foreach добавлять абзац снова НЕЛЬЗЯ (вылезет Exception Collection was modified), поэтому будем циклить сам foreach (с помощью while true)
            //checks
            if (parameterlist.Count() != textlist.Count()) return false;
            //check the file size
            var q = new FileInfo(templateName);
            if (q.Length >= 255 * 1024 * 1024) { MessageBox.Show(SizeError, FormMain.Warning); return false; }
            //show the form
            var frm = new Form { TopMost = true, StartPosition = FormStartPosition.CenterScreen, FormBorderStyle = FormBorderStyle.None, Opacity = 0.8, BackgroundImage = Resources.Wait, Size = Resources.Wait.Size };
            frm.Show();
            //start application
            Application wordapp;
            try { wordapp = new Application { Visible = String.IsNullOrWhiteSpace(destinationPath) }; }
            catch { MessageBox.Show(NoMsWord); frm.Close(); return false; }
            //get the client code
            var client = TrixOrm.GetInstance().GetScalar(String.Format("SELECT client_code FROM t_main WHERE pk = {0}", ekMain)).ToString();
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
                        //obtain the data
                        //мы имеем 2 поля в таблице: parameter и text
                        // 1 случай !!! Имеем обычный текст для подстановки
                        var newstring = textlist.ElementAtOrDefault(int.Parse(num)).Trim();
                        // 2 случай !!! Разбор строки на предмет тернарных операторов (ну я ваще извращенец, ага? :))
                        if (!String.IsNullOrWhiteSpace(newstring))
                        {
                            if (newstring.Contains("?") && newstring.Contains(":"))
                            {
                                //разделение строки
                                var str = newstring;
                                var leftOperand = str.Substring(0, str.IndexOf("==")).Trim();
                                str = str.Substring(str.IndexOf("==") + 2).Trim();
                                var rightOperand = str.Substring(0, str.IndexOf("?")).Trim();
                                str = str.Substring(str.IndexOf("?") + 1).Trim();
                                var trueExpr = str.Substring(0, str.IndexOf(":")).Trim();
                                str = str.Substring(str.IndexOf(":") + 1).Trim();
                                var falseExpr = str.Trim();
                                //проверка
                                newstring = GetDataByParameter(leftOperand, ekMain, client).ToUpper().Trim() == rightOperand.ToUpper().Trim() ? trueExpr : falseExpr;
                            }
                        }
                        // 3 случай!!! Текст пуст => подставляем параметр
                        else newstring = GetDataByParameter(parameterlist.ElementAt(int.Parse(num)), ekMain, client);
                        //change the 'Range.Text' property
                        var isCenter = t.Format.Alignment == WdParagraphAlignment.wdAlignParagraphCenter;           //БЫДЛОКОД! При смене ".Range.Text" Ворд изменяет ".Format" на дефолт => надо запомнить выравнивание (НЕ ЗАПОМИНАТЬ ФОРМАТ ЦЕЛИКОМ !!! Ворд сука тупит ваще)
                        t.Range.Text = t.Range.Text.Replace("^" + num + "^", newstring);
                        if (isCenter) t.Previous().Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;  //БЫДЛОКОД! Меняем выравнивание обратно (ввиду тупости Ворда ОБЯЗАТЕЛЬНО использовать ".Previous()" !!!)
                        //в строке возможны ещё подстановки - выход из цикла следует отменить
                        quitFromForeach = false;
                    }
                }
                //save the document
                if (!String.IsNullOrWhiteSpace(destinationPath))
                {
                    //подразумевается, что папка уже существует
                    ////if (!Directory.Exists(destinationPath)) Directory.CreateDirectory(destinationPath);
                    Object fileName = destinationPath + @"\" + destinationFilename;
                    Object fileFormat = FormMain.UseWord2003 ? WdSaveFormat.wdFormatDocument : WdSaveFormat.wdFormatDocumentDefault;
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
                if (!String.IsNullOrWhiteSpace(destinationPath)) wordapp.Quit(true);
            }
            catch { MessageBox.Show(WordError, FormMain.Warning); success = false; }
            frm.Close();
            return success;
        }
        #endregion

        #region //private methods
        private void CreateComboText(int num, string combo = "", string txt = "")
        {
            //формирование источника данных
            var lst = new List<string> { FormMain.HeaderInternalCode, FormMain.HeaderProjectName, FormMain.HeaderRequestNumber, FormMain.HeaderPatentNumber, FormMain.HeaderIsLegal, FormMain.HeaderDateCreated, HeaderProjType, HeaderToday };
            lst.AddRange(TrixOrm.GetInstance().GetArray("SELECT RTRIM(requisname) FROM d_requisites UNION (SELECT RTRIM(corrname) FROM d_correspondence)").Cast<String>());
            //создание ComboText-элемента
            var top = panelMain.Controls.Count == 0 ? FormMain.CommonMargin : panelMain.Controls.Cast<Control>().Max(d => d.Bottom) + FormMain.CommonMargin;
            var t = new ComboText(new ComboTextBehavoiur { AllowToBreed = String.IsNullOrWhiteSpace(combo + txt), IsAlwaysEnabled = true, IsTemporaryEnabled = !String.IsNullOrWhiteSpace(combo + txt) }) { Top = top, Left = FormMain.CommonMargin };
            t.AddData(lst);
            t.SomethingChanged += SomethingChanged; // !!! Не добавлять до внесения данных (во избежание зацикливания)
            t.DeletePressed += DeletePressed;
            t.SetValueToLabel(num.ToString());
            t.SetValueToCombobox(combo);
            t.SetValueToTextbox(txt);
            panelMain.Controls.Add(t);
        }
        #endregion

        #region //my handlers
        private void SomethingChanged(object sender, EventArgs e)
        {
            CreateComboText(_number++);
        }

        private void DeletePressed(object sender, string e)
        {
            //удаление ComboText-объекта и подъём нижних объектов наверх
            panelMain.Controls.Remove((ComboText)sender);
            foreach (var t in panelMain.Controls.Cast<ComboText>().Where(p => p.Top > ((ComboText)sender).Top))
                t.Top -= ((ComboText)sender).Height + FormMain.CommonMargin;
            //пересчёт всех номеров
            foreach (ComboText t in panelMain.Controls)
            {
                var a = int.Parse(t.GetTextFromLabelTrim());
                if (a > int.Parse(e))
                    t.SetValueToLabel(a - 1 + "");
            }
            _number--;
        }
        #endregion

        #region //common handlers
        private void FormReportLoad(object sender, EventArgs e)
        {
            //очистка запроса
            panelMain.Controls.Clear();
            _number = 0;
            //загрузка новых данных
            foreach (var t in TrixOrm.GetInstance().GetListOfCortages(@"SELECT parameter, text FROM t_query"))
                CreateComboText(_number++, t[0].ToString(), t[1].ToString());
            //создание одного элемента
            CreateComboText(_number++);
        }

        private void ButtonBuildClick(object sender, EventArgs e)
        {
            //checks
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            if (String.IsNullOrWhiteSpace(openFileDialog1.FileName)) return;
            //come on! (без сохранения)
            GenerateTheReport(_ekMain, panelMain.Controls.Cast<ComboText>().Where(p => p.IsActivated()).Select(t => t.GetTextFromComboboxTrim()), panelMain.Controls.Cast<ComboText>().Where(p => p.IsActivated()).Select(t => t.GetTextFromTextboxTrim()), openFileDialog1.FileName, null, null);
        }

        private void ButtonOkClick(object sender, EventArgs e)
        {
            var close = true;
            //сохранение параметров запроса
            TrixOrm.GetInstance().Execute(@"DELETE FROM t_query");
            foreach (var t in panelMain.Controls.Cast<ComboText>().Where(p => p.IsActivated()))
                if (TrixOrm.GetInstance().Execute(String.Format("INSERT INTO t_query (parameter, text) VALUES ('{0}', '{1}')", t.GetTextFromComboboxTrim(), t.GetTextFromTextboxTrim())) < 0) { MessageBox.Show(String.Format(DbError, t.GetTextFromLabelTrim())); close = false; }
            if (!close) return;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonQuitClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
        #endregion
    }
}
