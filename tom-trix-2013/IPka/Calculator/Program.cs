// ВНИМАНИЕ! Здесь явное копирование констант (new Trix.CalcParameters) из класса IPka.FormMain!!!
// Используется исключительно в целях отладки.
// Так по идее константы по-нормальному передаются внутри кода IPka.FormMain

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Calculator
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            TrixOrm.GetInstance().ConnectionString = @"Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|\RusPatentDB.mdf;Integrated Security=True;Connect Timeout=60;User Instance=True";
            Application.Run(
                new FormMain(new Trix.CalcParameters
                                 {
                                     ColumnsGridAutoWidth = new List<int> {20},
                                     ColumnsGridManualWidth = new List<int> {20},
                                     ColumnsGridRightWidth = new List<int> {20},
                                     CommonMargin = 20,
                                     ZerosWithinInternalCode = 4,
                                     FolderDocuments = @"Documents",
                                     FolderTemplates = @"Templates",
                                     FolderMaterials = @"Материалы",
                                     FolderIncoming = @"Входящая корреспонденция",
                                     FolderOutcoming = @"Исходящая корреспонденция",
                                     UseWord2003 = false,
                                     FormPosition = new Point(100, 100),
                                     FormSize = new Size(800, 500),
                                     SplitHorisontal = 200,
                                     SplitVertical = 250
                                 }));
        }
    }
}
