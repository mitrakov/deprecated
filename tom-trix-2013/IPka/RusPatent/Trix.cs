using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Calculator;

namespace IPka
{
    #region //public structures
    public struct ComboTextBehavoiur
    {
        public bool IsAlwaysEnabled;
        public bool IsTemporaryEnabled;
        public bool AllowToBreed;
        public bool BreedByTextboxOnly;
        public bool AllowContextInserting;
    }

    public struct RenameLog
    {
        public string Oldname;
        public string Newname;
    }
    #endregion

    public class ComboText : Panel
    {
        #region //controls
        protected readonly Label Laber = new Label { Width = 25 };
        protected readonly Button Deleter = new Button { Width = 18, Height = 23, Text = @"X", TextAlign = ContentAlignment.MiddleCenter };
        protected readonly ComboBox Combor = new ComboBox { Width = 180 };
        protected readonly TextBox Texter = new TextBox { Width = 200 };
        #endregion

        #region //events & delegates
        public event TrixEventHandler SomethingChanged;
        public event Trix2EventHandler DeletePressed;
        public event Trix2EventHandler ValidContext;
        public delegate void TrixEventHandler(object sender, EventArgs e);
        public delegate void Trix2EventHandler(object sender, string e);
        #endregion

        #region //constructor
        public ComboText(ComboTextBehavoiur behaviuor)
        {
            //deleter
            Deleter.Left = Laber.Right;
            Deleter.Enabled = behaviuor.IsTemporaryEnabled;                 // нельзя писать behaviuor.IsAlwaysEnabled || behaviuor.IsTemporaryEnabled;
            if (behaviuor.IsTemporaryEnabled)
                Deleter.BackColor = Color.LightCoral;
            Deleter.Click += DeleterClick;                                  //срабатывает событие (цель: удалить кортеж из таблицы)
            //combor
            Combor.Left = Deleter.Right + FormMain.CommonMargin;
            Combor.DropDownStyle = behaviuor.AllowContextInserting ? ComboBoxStyle.DropDown : ComboBoxStyle.DropDownList;
            Combor.Validated += ComborValidated;                            //срабатывает событие (цель: контекстное пополнение)
            Combor.Enabled = !Deleter.Enabled;
            if (!behaviuor.IsAlwaysEnabled)
                Combor.TextChanged += ComborTextChangedTexter;              //активируется текстбокс
            if (behaviuor.AllowToBreed && !behaviuor.BreedByTextboxOnly)
                Combor.SelectedValueChanged += TexterTextChanged;           //срабатывает событие (цель: добавить новый ComboText ниже)
            //texter
            Texter.Enabled = behaviuor.IsAlwaysEnabled || behaviuor.IsTemporaryEnabled;
            Texter.Left = Combor.Right + FormMain.CommonMargin;
            if (behaviuor.AllowToBreed)
                Texter.TextChanged += TexterTextChanged;                    //срабатывает событие (цель: добавить новый ComboText ниже)
            //this
            Controls.Add(Laber);
            Controls.Add(Deleter);
            Controls.Add(Texter);
            Controls.Add(Combor);
            Height = Controls.Cast<Control>().Max(t => t.Height);
            Width = Texter.Right + FormMain.CommonMargin;
        }
        #endregion

        #region //public methods
        public void AddData(IEnumerable s)
        {
            foreach (var t in s)
                Combor.Items.Add(t);
            if (Combor.DropDownStyle == ComboBoxStyle.DropDownList)
                Combor.SelectedIndex = 0;
        }

        public bool IsActivated()
        {
            return Deleter.Enabled;
        }

        public string GetTextFromComboboxTrim()
        {
            return Combor.Text.Trim();
        }

        public string GetTextFromTextboxTrim()
        {
            return Texter.Text.Trim();
        }

        public string GetTextFromLabelTrim()
        {
            return Laber.Text.Trim();
        }

        public void SetValueToCombobox(string s)
        {
            Combor.Text = s.Trim();
        }

        public void SetValueToTextbox(string s)
        {
            Texter.Text = s.Trim();
        }

        public void SetValueToLabel(string s)
        {
            Laber.Text = s.Trim();
        }
        #endregion

        #region //handlers
        private void DeleterClick(object sender, EventArgs e)
        {
            if (DeletePressed != null)
                DeletePressed(this, Laber.Text);
        }

        private void TexterTextChanged(object sender, EventArgs e)
        {
            if (SomethingChanged == null) return;
            SomethingChanged(this, e);
            Texter.TextChanged -= TexterTextChanged;
            Combor.SelectedValueChanged -= TexterTextChanged;
            Deleter.Enabled = true;
            Deleter.BackColor = Color.LightCoral;
        }

        private void ComborValidated(object sender, EventArgs e)
        {
            if (ValidContext != null)
                ValidContext(this, Combor.Text.Trim());
        }

        protected void ComborTextChangedTexter(object sender, EventArgs e)
        {
            Texter.Enabled = !String.IsNullOrWhiteSpace(Combor.Text);
        }
        #endregion
    }

    public class ComboTextCalendar : ComboText
    {
        #region //controls
        private readonly Button _butter = new Button { Width = 23 };
        private readonly MonthCalendar _calender = new MonthCalendar();
        #endregion

        #region //constructor
        public ComboTextCalendar(ComboTextBehavoiur behaviour)
            : base(behaviour)
        {
            _butter.Left = Texter.Right + 20;
            _butter.Click += ButterClick;
            _butter.Enabled = false;
            Combor.TextChanged -= ComborTextChangedTexter;
            Combor.TextChanged += ComborTextChangedButter;
            Texter.Enabled = false;
            Controls.Add(_butter);
            Width = _butter.Right + 20;
        }
        #endregion

        #region //handlers
        private void ButterClick(object sender, EventArgs e)
        {
            _calender.Left = Combor.Left;
            _calender.Top = Combor.Bottom;
            _calender.DateSelected += CalenderDateSelected;
            _calender.MouseUp += HideTheCalendar;
            _calender.MaxSelectionCount = 1;
            _calender.ShowToday = false;
            Controls.Add(_calender);
            Height = _calender.Height + 30;
            _calender.Focus();
        }

        private void CalenderDateSelected(object sender, DateRangeEventArgs e)
        {
            HideTheCalendar(sender, new MouseEventArgs(MouseButtons.Right, 1, 0, 0, 0));
            Texter.Text = _calender.SelectionStart.ToShortDateString();
        }

        private void HideTheCalendar(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            Controls.Remove(_calender);
            Height = Controls.Cast<Control>().Max(t => t.Height);
        }

        private void ComborTextChangedButter(object sender, EventArgs e)
        {
            _butter.Enabled = !String.IsNullOrWhiteSpace(Combor.Text);
        }
        #endregion
    }

    /// <summary>
    /// Панель реквизитов. Содержит список из RequisPanelItem'ов и одну RequisPanelItemNew
    /// </summary>
    public sealed class RequisPanel : Panel
    {
        #region //events & delegates
        public event TrixEventHandler NewItemInput;
        public delegate void TrixEventHandler(object sender, string e);
        #endregion

        #region //public methods
        public void ResetDictionary(Dictionary<string, string> keysValues)
        {
            Controls.Clear();
            foreach (var t in keysValues)
                Controls.Add(new RequisPanelItem(t.Key, t.Value) { Top = Controls.Count > 0 ? Controls.Cast<Control>().Max(p => p.Bottom) + 20 : 20 });
            var a = new RequisPanelItemNew { Top = Controls.Count > 0 ? Controls.Cast<Control>().Max(p => p.Bottom) + 20 : 20 };
            a.MeValidated += (sender, e) => NewItemInput(sender, e);
            Controls.Add(a);
        }

        public Dictionary<string, string> GetDictionary()
        {
            return Controls.Cast<Control>().Where(p => p.GetType() == typeof(RequisPanelItem)).ToDictionary(t => ((RequisPanelItem)t).GetKey(), t => ((RequisPanelItem)t).GetValue());
        }
        #endregion
    }

    /// <summary>
    /// Текстбокс реквизита. Содержит 2 поля: disabled и enabled
    /// </summary>
    public class RequisPanelItem : Panel
    {
        #region //const
        private const int WidthOfRequiz = 350;
        #endregion

        #region //controls
        protected readonly TextBox Tboxkey = new TextBox { Enabled = false, Left = 20, Width = 150 };
        public TextBox Tboxvalue = new TextBox { Width = WidthOfRequiz };
        #endregion

        #region //constructor
        public RequisPanelItem(string key, string value)
        {
            Tboxkey.Text = key.Trim();
            Tboxvalue.Text = value.Trim();
            Tboxvalue.Left = Tboxkey.Right + 20;
            Controls.AddRange(new[] { Tboxkey, Tboxvalue });
            Width = Controls.Cast<Control>().Max(t => t.Right) + 20;
            Height = Controls.Cast<Control>().Max(t => t.Bottom) + 20;
        }
        #endregion

        #region //public methods
        public string GetKey()
        {
            return Tboxkey.Text.Trim();
        }

        public string GetValue()
        {
            return Tboxvalue.Text.Trim();
        }
        #endregion
    }

    public class RequisPanelItemNew : RequisPanelItem
    {
        #region //constants
        private const string AddNew = @"Введите новый";
        #endregion

        #region //events & delegates
        public event TrixEventHandler MeValidated;
        public delegate void TrixEventHandler(object sender, string e);
        #endregion

        #region //private variables
        private readonly Font _font;
        #endregion

        #region //constructor
        public RequisPanelItemNew()
            : base(AddNew, "")
        {
            _font = Tboxkey.Font;
            Tboxkey.Enabled = true;
            Tboxkey.Font = new Font(@"Arial", 7, FontStyle.Italic);
            Tboxkey.Click += delegate { Tboxkey.Text = ""; Tboxkey.Font = _font; };
            Tboxkey.TextChanged += delegate { Tboxkey.Font = _font; };
            Tboxkey.Validated += delegate { if (!String.IsNullOrWhiteSpace(Tboxkey.Text) && Tboxkey.Text.Trim() != AddNew) MeValidated(this, Tboxkey.Text.Trim()); };
        }

        #endregion
    }

    /// <summary>
    /// Панель типов проекта. Содержит комбобокс с типами проектов и disabled-текстбокс с номером заявки
    /// </summary>
    public class ProjectTypePanel : Panel
    {
        #region //controls
        private readonly Label _label1 = new Label { Text = @"Тип проекта", Top = 20 };
        private readonly Label _label2 = new Label { Text = @"Внутренний код заявки", Top = 50 };
        private readonly ComboBox _comboBoxProjType = new ComboBox { Left = 150, Top = 20, Width = 100 };
        private readonly TextBox _textBoxInternalCode = new TextBox { Enabled = false, Left = 150, Top = 50 };
        #endregion

        #region //constants
        private const string InputLabel1 = @"Введите код типа проекта";
        private const string InputLabel2 = @"Введите сокращение типа проекта";
        private const string InputLabelHeader = @"Новый код проекта";
        private const string Error = "Операция отменена. Возможные причины ошибки:\n\n 1) Введённый тип проекта и/или его код уже существуют\n 2) Слишком длинное строковое имя (для кода допустимо не более 2 знаков)";
        #endregion

        #region //private variables
        private readonly int? _ekMain;
        #endregion

        #region //constructor
        public ProjectTypePanel(int? ekMain)
        {
            _ekMain = ekMain;
            _comboBoxProjType.TextChanged += ComboBoxProjTypeTextChanged;
            _comboBoxProjType.Validated += ComboBoxProjTypeValidated;
            Controls.Add(_label1);
            Controls.Add(_label2);
            Controls.Add(_comboBoxProjType);
            Controls.Add(_textBoxInternalCode);
            Width = Controls.Cast<Control>().Max(t => t.Right) + 20;
            Height = Controls.Cast<Control>().Max(t => t.Bottom) + 20;
        }
        #endregion

        #region //private methods
        private void RefreshComboboxProjType(string usercode)
        {
            //очистка списка типов проекта
            _comboBoxProjType.Items.Clear();
            //набор данных в список типов проекта
            foreach (var t in TrixOrm.GetInstance().GetArray(@"SELECT RTRIM(codeshortname) FROM d_icodes"))
                _comboBoxProjType.Items.Add(t);
            //выбор элемента в списке типов проекта
            var ucode = TrixOrm.GetInstance().GetScalar(String.Format("SELECT RTRIM(codeshortname) FROM d_icodes WHERE usercode = '{0}'", usercode));
            if (_comboBoxProjType.Items.Count > 0)
                _comboBoxProjType.SelectedIndex = ucode == null ? 0 : _comboBoxProjType.Items.IndexOf(ucode);
        }
        #endregion

        #region //handlers
        private void ComboBoxProjTypeValidated(object sender, EventArgs e)
        {
            //убираем пробелы
            _comboBoxProjType.Text = _comboBoxProjType.Text.Trim();
            //проверки
            if (String.IsNullOrWhiteSpace(_comboBoxProjType.Text) || !String.IsNullOrWhiteSpace(_comboBoxProjType.SelectedText)) return;
            if (_comboBoxProjType.Items.Cast<string>().Any(z => z.ToUpper().Contains(_comboBoxProjType.Text.ToUpper()))) return;
            var t = FormMain.CreateInputBox(InputLabel1, InputLabelHeader);
            t.ShowDialog();
            if (String.IsNullOrWhiteSpace(t.Tag.ToString())) return;
            var p = FormMain.CreateInputBox(InputLabel2, InputLabelHeader);
            p.ShowDialog();
            if (String.IsNullOrWhiteSpace(p.Tag.ToString())) return;
            //контекстное пополнение словарей
            if (TrixOrm.GetInstance().Execute(String.Format("INSERT INTO d_icodes (codename, usercode, codeshortname) VALUES ('{0}', '{1}', '{2}')", _comboBoxProjType.Text.Trim(), t.Tag.ToString().Trim(), p.Tag.ToString().Trim())) < 0)
                MessageBox.Show(Error);
            //обновление комбобокса
            RefreshComboboxProjType(t.Tag.ToString().Trim());
            //пересчитываем значение в текстбоксе
            ComboBoxProjTypeTextChanged(this, null);
        }

        private void ComboBoxProjTypeTextChanged(object sender, EventArgs e)
        {
            //вставка в текстбокс
            _textBoxInternalCode.Text = FormMain.GetInternalCode(_comboBoxProjType.Text, (_ekMain ?? int.Parse(TrixOrm.GetInstance().GetScalar("SELECT IDENT_CURRENT('t_main')").ToString()) + 1).ToString(), false, FormMain.CodeLength);
        }
        #endregion

        #region //public methods
        public void SetToComboBox(string s)
        {
            _comboBoxProjType.Text = s.Trim();
        }

        public void RefreshCombobox()
        {
            RefreshComboboxProjType(_textBoxInternalCode.Text.Substring(0, _textBoxInternalCode.Text.Length >= 2 ? 2 : 0));
        }

        public string GetInternalCodeTrim()
        {
            return _textBoxInternalCode.Text.Trim();
        }
        #endregion
    }

    public class ParametersDb
    {
        #region //properties
        public int Pk { get; set; }
        public string ProjectTypeText { get; set; }
        public string ProjectName { get; set; }
        public bool IsLegal { get; set; }
        public string ClientCode { get; set; }
        public string RequestNumber { get; set; }
        public string DateCreated { get; set; }
        public string PatentNumber { get; set; }
        public string DateReceived { get; set; }
        public int ColourType { get; set; }
        #endregion
    }
}
