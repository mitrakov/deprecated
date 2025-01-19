// ReSharper disable EmptyGeneralCatchClause
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Calculator;

namespace IPka
{
    public partial class FormClients : Form
    {
        /// <summary>
        /// Класс для хранения записи в Листбоксе
        /// </summary>
        private class ListBoxRecord
        {
            public int ClientCode { get; set; }
            public string Text { private get; set; }

            public override String ToString()
            {
                return Text;
            }
        }

        private readonly bool _isLegal;         //физическое лицо или юридическое
        private List<object[]> _records;        //список всех записей из БД (чтобы подгрузить только 1 раз)

        //constructor
        public FormClients(bool isLegal)
        {
            InitializeComponent();
            _isLegal = isLegal;
            Tag = null;
            DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Метод загружает листбокс (в 1-й раз вытягивает из БД, потом - из памяти)
        /// </summary>
        private void LoadAll()
        {
            try
            {
                listBoxClients.Items.Clear();
                if (_records == null)
                    _records = TrixOrm.GetInstance().GetListOfCortages(String.Format(@"SELECT client_code, RTRIM(value) FROM t_requisites JOIN d_requisites ON ek_requis = d_requisites.pk WHERE is_legal = '{0}' ORDER BY client_code, ek_requis", _isLegal));
                var clientCode = int.Parse(_records[0][0].ToString());
                var s = _records[0][1].ToString();
                for (var i = 1; i < _records.Count; i++)
                    if (clientCode == int.Parse(_records[i][0].ToString()))
                        s += ", " + _records[i][1];
                    else
                    {
                        if (!String.IsNullOrWhiteSpace(s))
                            listBoxClients.Items.Add(new ListBoxRecord { ClientCode = clientCode, Text = s });
                        clientCode = int.Parse(_records[i][0].ToString());
                        s = _records[i][1].ToString();
                    }
                listBoxClients.Items.Add(new ListBoxRecord { ClientCode = clientCode, Text = s });
            }
            catch { }
        }

        private void FormClientsLoad(object sender, EventArgs e)
        {
            LoadAll();
        }

        private void ListBoxClientsDoubleClick(object sender, EventArgs e)
        {
            ButtonOkClick(sender, e);
        }

        private void ButtonOkClick(object sender, EventArgs e)
        {
            if (listBoxClients.SelectedItem == null) return;
            Tag = ((ListBoxRecord)listBoxClients.SelectedItem).ClientCode;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void TextBoxSearchMouseDown(object sender, MouseEventArgs e)
        {
            textBoxSearch.Text = "";
        }

        private void TextBoxSearchTextChanged(object sender, EventArgs e)
        {
            LoadAll();
            var remList = new List<object>();
            remList.AddRange(listBoxClients.Items.Cast<ListBoxRecord>().Where(t => !t.ToString().ToUpper().Trim().Contains(textBoxSearch.Text.ToUpper().Trim())));
            foreach (var o in remList)
                listBoxClients.Items.Remove(o);
        }
    }
}
