// ReSharper disable PossibleNullReferenceException
using System;
using System.IO;
using System.Windows.Forms;
using Calculator;

namespace IPka
{
    public partial class FormCorrStart : Form
    {
        #region //constants
        private const string RenameError = "Возникла ошибка при попытке переименования файла:\n{0}\n\nВозможно, файл занят другим процессом. Рекомендуется переименовать файл вручную на '{1}'";
        private const string Warning = "Внимание! Данный тип корреспонденции используется (всего объектов: {0})\nУдаление приведёт к потере этих данных. Продолжить?";
        #endregion

        #region //constructor
        public FormCorrStart()
        {
            InitializeComponent();
        }
        #endregion

        #region //private methods
        private void RefreshTheList()
        {
            listBoxCorrespondence.Items.Clear();
            foreach (var t in TrixOrm.GetInstance().GetArray("SELECT corrname FROM d_correspondence"))
                listBoxCorrespondence.Items.Add(t);
        }
        #endregion

        #region //handlers
        private void FormCorrStartLoad(object sender, EventArgs e)
        {
            RefreshTheList();
        }

        private void ListBoxCorrespondenceMouseDoubleClick(object sender, MouseEventArgs e)
        {
            ButtonChooseClick(this, null);
        }
        #endregion

        #region //handlers (Buttons click)
        private void ButtonChooseClick(object sender, EventArgs e)
        {
            //проверки
            if (listBoxCorrespondence.SelectedItem == null) return;
            //запоминаем текущее значение
            var callpoint = listBoxCorrespondence.SelectedItem;
            //открываем форму FormCorrespondence в режиме изменений
            var frm = new FormCorrespondence(listBoxCorrespondence.SelectedItem.ToString());
            if (frm.ShowDialog() != DialogResult.OK) return;
            //переименование корреспонденции в папке Documents
            var t = (string[])frm.Tag;                              //подразумевается, что в Tag лежит массив строк (старое и новое сокращение)
            if (t[0] != t[1])
                foreach (var p in Directory.GetFiles(String.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), FormMain.FolderDocuments), String.Format("*_{0}_*", t[0]), SearchOption.AllDirectories))
                {
                    var oldpath = Path.GetDirectoryName(p);
                    var newpath = oldpath.Replace(t[0], t[1]);      //немножко неправильно: вдруг каталог содержит такие символы? Ну и фиг с ним :)
                    var newfile = Path.GetFileName(p).Replace(t[0], t[1]);
                    try
                    {
                        Directory.Move(oldpath, newpath);
                        File.Move(newpath + @"\" + Path.GetFileName(p), newpath + @"\" + newfile);
                    }
                    catch { MessageBox.Show(String.Format(RenameError, p, newpath + @"\" + newfile)); }
                }
            //обновление и возврат значения
            RefreshTheList();
            listBoxCorrespondence.SelectedItem = callpoint;
        }

        private void ButtonRemoveClick(object sender, EventArgs e)
        {
            //проверки
            if (listBoxCorrespondence.SelectedItem == null) return;
            //удаление (ВНИМАНИЕ! Реальные файлы удалять не будем (это немного мерзко - заказчик охренеет ваще))
            //получаем кол-во экземпляров
            var a = int.Parse(TrixOrm.GetInstance().GetScalar(String.Format("SELECT COUNT(*) FROM t_correspondence INNER JOIN d_correspondence ON ek_corr = d_correspondence.pk WHERE corrname = '{0}'", listBoxCorrespondence.SelectedItem)).ToString());
            //каскадное удаление)
            if (a > 0)
            {
                if (MessageBox.Show(String.Format(Warning, a), FormMain.Warning, MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                    TrixOrm.GetInstance().Execute(String.Format("DELETE FROM t_correspondence WHERE ek_corr IN (SELECT pk FROM d_correspondence WHERE corrname = '{0}')", listBoxCorrespondence.SelectedItem));
                else return;
            }
            //обычное удаление
            TrixOrm.GetInstance().Execute(String.Format("DELETE FROM d_correspondence WHERE corrname = '{0}'", listBoxCorrespondence.SelectedItem));
            //обновление
            RefreshTheList();
        }

        private void ButtonQuitClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
        #endregion
    }
}
