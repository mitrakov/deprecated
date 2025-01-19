// ReSharper disable EmptyGeneralCatchClause
using System;
using System.Drawing;
using System.Windows.Forms;
using Calculator;

namespace IPka
{
    public partial class FormRequisites : Form
    {
        #region //constants
        private const string Error = @"Переименование не выполнено. Возможно, данное значение уже существует";
        private const string Warning = @"Внимание! Данное поле используется (всего объектов: {0}). При удалении соответствующие данные будут утеряны. Продолжить?";
        #endregion

        #region //private variables
        private int _legalIndex;                                    //текущий индекс в listBoxLegal
        private int _physIndex;                                     //текущий индекс в listBoxPhysical
        private bool _isLegalSelected;                              //показывает, что было выбрано последним
        private readonly Cursor _cursor = new Cursor("Drag.cur");   //загружаемый для Drag'n'Drop курсор
        #endregion

        #region //constructor
        public FormRequisites()
        {
            InitializeComponent();
        }
        #endregion

        #region //private methods
        private void RefreshListboxes()
        {
            listBoxLegal.Items.Clear();
            listBoxPhysical.Items.Clear();
            foreach (var t in TrixOrm.GetInstance().GetArray("SELECT requisname FROM d_requisites WHERE is_legal = 'True'"))
                listBoxLegal.Items.Add(t);
            foreach (var t in TrixOrm.GetInstance().GetArray("SELECT requisname FROM d_requisites WHERE is_legal = 'False'"))
                listBoxPhysical.Items.Add(t);
        }
        #endregion

        #region //handlers
        private void FormDictionariesLoad(object sender, EventArgs e)
        {
            RefreshListboxes();
        }

        private void ListBoxLegalSelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxLegal.SelectedItem == null) return;
            textBoxRename.Text = listBoxLegal.SelectedItem.ToString();
            _isLegalSelected = true;
        }

        private void ListBoxPhysicalSelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxPhysical.SelectedItem == null) return;
            textBoxRename.Text = listBoxPhysical.SelectedItem.ToString();
            _isLegalSelected = false;
        }
        #endregion

        #region //handlers (buttons)
        private void ButtonRenameClick(object sender, EventArgs e)
        {
            //проверки
            if (String.IsNullOrWhiteSpace(textBoxRename.Text)) return;
            if (_isLegalSelected && listBoxLegal.SelectedItem == null) return;
            if (!_isLegalSelected && listBoxPhysical.SelectedItem == null) return;
            //переименование
            if (TrixOrm.GetInstance().Execute(String.Format("UPDATE d_requisites SET requisname = '{0}' WHERE requisname = '{1}' AND is_legal = '{2}'", textBoxRename.Text.Trim(), _isLegalSelected ? listBoxLegal.SelectedItem : listBoxPhysical.SelectedItem, _isLegalSelected)) < 0)
                MessageBox.Show(Error);
            //обновление
            RefreshListboxes();
        }

        private void ButtonRemoveClick(object sender, EventArgs e)
        {
            //проверки
            if (String.IsNullOrWhiteSpace(textBoxRename.Text)) return;
            if (_isLegalSelected && listBoxLegal.SelectedItem == null) return;
            if (!_isLegalSelected && listBoxPhysical.SelectedItem == null) return;
            //подготовка
            var requisname = _isLegalSelected ? listBoxLegal.SelectedItem : listBoxPhysical.SelectedItem;
            //получаем количество кортежей, использующий данный реквизит
            var a = int.Parse(TrixOrm.GetInstance().GetScalar(String.Format("SELECT COUNT(*) FROM t_requisites WHERE ek_requis IN (SELECT pk FROM d_requisites WHERE requisname = '{0}' AND is_legal = '{1}')", requisname, _isLegalSelected)).ToString());
            //каскадное удаление
            if (a > 0)
                if (MessageBox.Show(String.Format(Warning, a), FormMain.Warning, MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                    TrixOrm.GetInstance().Execute(String.Format("DELETE FROM t_requisites WHERE ek_requis IN (SELECT pk FROM d_requisites WHERE requisname = '{0}' AND is_legal = '{1}')", requisname, _isLegalSelected));
                else return;
            //обычное удаление
            TrixOrm.GetInstance().Execute(String.Format("DELETE FROM d_requisites WHERE requisname = '{0}' AND is_legal = '{1}'", requisname, _isLegalSelected));
            //обновление
            RefreshListboxes();
        }

        private void ButtonQuitClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
        #endregion

        #region //drag'n'drop (Legal)
        private void ListBoxLegalMouseDown(object sender, MouseEventArgs e)
        {
            //drag'n'drop (запомнили индекс элемента)
            if (e.Button != MouseButtons.Left) return;
            try { _legalIndex = listBoxLegal.SelectedIndex; }
            catch { }
        }

        private void ListBoxLegalMouseMove(object sender, MouseEventArgs e)
        {
            //drag'n'drop (запретили изменять индекс элемента)
            if (e.Button != MouseButtons.Left) return;
            try
            {
                listBoxLegal.SelectedIndex = _legalIndex;
                Cursor = RectangleToScreen(groupBoxPhysical.Bounds).IntersectsWith(Rectangle.FromLTRB(MousePosition.X, MousePosition.Y, MousePosition.X + 1, MousePosition.Y + 1)) ? _cursor : Cursors.No;
            }
            catch { }
        }

        private void ListBoxLegalMouseUp(object sender, MouseEventArgs e)
        {
            //drag'n'drop (вставили элемент на новое место и удалили из старого)
            if (e.Button != MouseButtons.Left) return;
            Cursor = Cursors.Default;
            try
            {
                if (RectangleToScreen(groupBoxPhysical.Bounds).IntersectsWith(Rectangle.FromLTRB(MousePosition.X, MousePosition.Y, MousePosition.X + 1, MousePosition.Y + 1)))
                {
                    TrixOrm.GetInstance().Execute(String.Format("UPDATE d_requisites SET is_legal = 'False' WHERE requisname = '{0}' AND is_legal = 'True'", listBoxLegal.SelectedItem));
                    RefreshListboxes();
                }
            }
            catch { }
        }
        #endregion

        #region //drag'n'drop (Physical)
        private void ListBoxPhysicalMouseDown(object sender, MouseEventArgs e)
        {
            //drag'n'drop (запомнили индекс элемента)
            if (e.Button != MouseButtons.Left) return;
            try { _physIndex = listBoxPhysical.SelectedIndex; }
            catch { }
        }

        private void ListBoxPhysicalMouseMove(object sender, MouseEventArgs e)
        {
            //drag'n'drop (запретили изменять индекс элемента)
            if (e.Button != MouseButtons.Left) return;
            try
            {
                listBoxPhysical.SelectedIndex = _physIndex;
                Cursor = RectangleToScreen(groupBoxLegal.Bounds).IntersectsWith(Rectangle.FromLTRB(MousePosition.X, MousePosition.Y, MousePosition.X + 1, MousePosition.Y + 1)) ? _cursor : Cursors.No;
            }
            catch { }
        }

        private void ListBoxPhysicalMouseUp(object sender, MouseEventArgs e)
        {
            //drag'n'drop (вставили элемент на новое место и удалили из старого)
            if (e.Button != MouseButtons.Left) return;
            Cursor = Cursors.Default;
            try
            {
                if (RectangleToScreen(groupBoxLegal.Bounds).IntersectsWith(Rectangle.FromLTRB(MousePosition.X, MousePosition.Y, MousePosition.X + 1, MousePosition.Y + 1)))
                {
                    TrixOrm.GetInstance().Execute(String.Format("UPDATE d_requisites SET is_legal = 'True' WHERE requisname = '{0}' AND is_legal = 'False'", listBoxPhysical.SelectedItem));
                    RefreshListboxes();
                }
            }
            catch { }
        }
        #endregion
    }
}
