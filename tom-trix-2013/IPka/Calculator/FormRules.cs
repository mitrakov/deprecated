// ReSharper disable EmptyGeneralCatchClause
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Calculator
{
    public partial class FormRules : Form
    {
        #region // Consts & variables
        private const string RuleRenameDiscription = @"Введите новое описание правила";
        private const string RuleNameDiscription = @"Введите описание нового правила";

        private readonly List<TrixConjunctPanel> _list = new List<TrixConjunctPanel>();                 //текущий список конъюнктов для правил
        private readonly List<TrixConjunctPanel> _removeList = new List<TrixConjunctPanel>();           //список подготовленных для удаления конъюнктов для правил
        private readonly List<TrixConjunctPanel> _addList = new List<TrixConjunctPanel>();              //список новых конъюнктов для правил
        private readonly List<TrixConjunctPanel> _alterList = new List<TrixConjunctPanel>();            //список изменившихся конъюнктов для правил

        private readonly Dictionary<string, string> _captionsList = new Dictionary<string, string>();   //список кэпшенов для правил
        private readonly Dictionary<string, Color> _coloursList = new Dictionary<string, Color>();      //список цветов для правил
        private readonly List<string> _newRulesList = new List<string>();                               //список новых правил
        private readonly List<String> _databaseQueryPool = new List<string>();                          //список SQL-команд для отложенного изменения таблицы calc_t_rules (содержит набор команд типа "Измени цвет такого-то правила на такой-то")

        private readonly Cursor _cursor = new Cursor("Drag.cur");                                       //загружаемый для Drag'n'Drop курсор
        private int _dragDropIndex = -1;                                                                //индекс строки листбокса для реализации Drag'n'Drop
        #endregion

        #region // Constructor
        public FormRules()
        {
            InitializeComponent();
        }
        #endregion

        #region // Private methods
        private void ReLoadConjuncts()
        {
            //checks
            if (listBoxRules.SelectedItems.Count == 0) return;
            //очистка старых компонентов
            panelCenter.Controls.Clear();
            //добавление новых
            buttonAddNew.Visible = true;
            var top = 0;
            foreach (var h in _list.Where(t => t.PkRule == listBoxRulesMapper.SelectedItem.ToString()))
            {
                h.Top = top += 50;
                panelCenter.Controls.Add(h);
                buttonAddNew.Visible = false;
            }
        }
        #endregion

        #region // Handlers
        private void FormRulesLoad(object sender, EventArgs e)
        {
            //заполнение листбокса и его маппера
            listBoxRules.Items.AddRange(TrixOrm.GetInstance().GetArray("  SELECT reason FROM calc_t_rules ORDER BY order_number").ToArray());
            listBoxRulesMapper.Items.AddRange(TrixOrm.GetInstance().GetArray("SELECT pk FROM calc_t_rules ORDER BY order_number").ToArray());
            //заполнение ПОЛНОГО перечня конъюнктов
            foreach (var w in TrixOrm.GetInstance().GetListOfCortages("SELECT calc_t_rules.pk, calc_t_conjuncts.pk, op1.text, operation, value, op2.text, parameter FROM calc_t_rules INNER JOIN calc_t_rules2conjuncts ON ek_calc_rules = calc_t_rules.pk INNER JOIN calc_t_conjuncts ON ek_calc_conjuncts = calc_t_conjuncts.pk INNER JOIN calc_t_operands AS op1 ON ek_operand1 = op1.pk LEFT JOIN calc_t_operands AS op2 ON ek_operand2 = op2.pk"))
            {
                var a = new TrixConjunctPanel(w[0].ToString(), w[1].ToString())
                              {
                                  comboBoxVariable = { Text = w[2].ToString() },
                                  comboBoxOperation = { Text = w[3].ToString() },
                                  comboBoxValue = { Text = !String.IsNullOrWhiteSpace(w[4].ToString()) ? w[4].ToString() : w[5].ToString() },
                                  textBoxParameter = { Text = w[6].ToString() },
                                  Left = FormMain.Parameters.CommonMargin
                              };
                a.NewPressed += PanelNewPressed;
                a.DeletePressed += PanelDeletePressed;
                a.SomethingChanged += PanelSomethingChanged;
                _list.Add(a);
            }
            //заполнение вспомогательных словарей (например, при выборе правила его цвет берётся НЕ из БД, а из словаря; при смене цвета значение обновляется только в словаре (юзер видит, что всё фурычит), а по кнопке ОК изменённые куски словаря переливаются в БД)
            foreach (var t in TrixOrm.GetInstance().GetListOfCortages("SELECT pk, colour, caption FROM calc_t_rules ORDER BY order_number"))
            {
                _captionsList.Add(t[0].ToString(), t[2].ToString());
                _coloursList.Add(t[0].ToString(), Color.FromArgb(int.Parse(t[1].ToString())));
            }
        }

        private void ListBoxRulesSelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxRules.SelectedItems.Count == 0) return;
            //обновление маппера
            listBoxRulesMapper.SelectedIndex = listBoxRules.SelectedIndex;
            //перерисовка конъюнктов
            ReLoadConjuncts();
            //обновление цвета и заголовка
            textBoxCaption.Text = _captionsList[listBoxRulesMapper.SelectedItem.ToString()];
            buttonColour.BackColor = _coloursList[listBoxRulesMapper.SelectedItem.ToString()];
        }

        private void ListBoxRulesDoubleClick(object sender, EventArgs e)
        {
            if (listBoxRules.SelectedItems.Count == 0) return;
            var f = new TrixInputBox(RuleRenameDiscription, RuleRenameDiscription, listBoxRules.SelectedItem.ToString());
            if (f.ShowDialog() == DialogResult.OK)
            {
                listBoxRules.Items[listBoxRules.SelectedIndex] = f.Tag;
                _databaseQueryPool.Add(String.Format("UPDATE calc_t_rules SET reason = '{0}' WHERE pk = {1}", f.Tag, listBoxRulesMapper.SelectedItem));
            }
        }

        private void TextBoxCaptionValidating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (listBoxRules.SelectedItems.Count == 0) return;
            if (String.IsNullOrWhiteSpace(textBoxCaption.Text))
                textBoxCaption.Text = _captionsList[listBoxRulesMapper.SelectedItem.ToString()];
            else
            {
                _captionsList[listBoxRulesMapper.SelectedItem.ToString()] = textBoxCaption.Text.Trim();
                _databaseQueryPool.Add(String.Format("UPDATE calc_t_rules SET caption = '{0}' WHERE pk = {1}", textBoxCaption.Text.Trim(), listBoxRulesMapper.SelectedItem));
            }
        }
        #endregion

        #region // Handlers from TrixConjunctPanel
        void PanelDeletePressed(TrixConjunctPanel sender, string e)
        {
            if (listBoxRules.SelectedItems.Count == 0) return;
            _list.Remove(sender);
            _addList.Remove(sender);
            _removeList.Add(sender);
            ReLoadConjuncts();
        }

        void PanelNewPressed(TrixConjunctPanel sender, string e)
        {
            if (listBoxRules.SelectedItems.Count == 0) return;
            var a = new TrixConjunctPanel(listBoxRulesMapper.SelectedItem.ToString(), null) { Left = FormMain.Parameters.CommonMargin };
            a.NewPressed += PanelNewPressed;
            a.DeletePressed += PanelDeletePressed;
            _list.Add(a);
            _addList.Add(a);
            ReLoadConjuncts();
        }

        void PanelSomethingChanged(TrixConjunctPanel sender, string e)
        {
            if (listBoxRules.SelectedItems.Count == 0) return;
            _alterList.Add(sender);
        }
        #endregion

        #region // Drag-n-Drop
        private void ListBoxRulesMouseDown(object sender, MouseEventArgs e)
        {
            if (listBoxRules.SelectedItems.Count == 0) return;
            _dragDropIndex = listBoxRules.SelectedIndex;
        }

        private void ListBoxRulesMouseMove(object sender, MouseEventArgs e)
        {
            //drag'n'drop (запретили изменять индекс элемента)
            if (e.Button != MouseButtons.Left) return;
            try
            {
                Cursor = RectangleToScreen(listBoxRules.Bounds).IntersectsWith(Rectangle.FromLTRB(MousePosition.X, MousePosition.Y, MousePosition.X + 1, MousePosition.Y + 1)) ? _cursor : Cursors.No;
            }
            catch { }
        }

        private void ListBoxRulesMouseUp(object sender, MouseEventArgs e)
        {
            // ==== drag&drop ====
            Cursor = Cursors.Default;
            if (listBoxRules.SelectedItems.Count == 0) { _dragDropIndex = -1; return; }
            // init
            var from = _dragDropIndex;
            var to = listBoxRules.SelectedIndex;
            _dragDropIndex = -1;
            if (from == to || from < 0) return;
            //обновление листбокса
            var item = listBoxRules.Items[from];
            listBoxRules.Items.Remove(item);
            listBoxRules.Items.Insert(to, item);
            //обновление листбокса-маппера
            var pk = listBoxRulesMapper.Items[from];
            listBoxRulesMapper.Items.Remove(pk);
            listBoxRulesMapper.Items.Insert(to, pk);
            //обновление в БД
            _databaseQueryPool.Add(String.Format("UPDATE calc_t_rules SET order_number = -1 WHERE order_number = {0}", from));
            _databaseQueryPool.Add(String.Format("UPDATE calc_t_rules SET order_number = order_number{0}1 WHERE {1} <= order_number AND order_number <= {2}", from > to ? "+" : "-", Math.Min(from, to), Math.Max(from, to)));
            _databaseQueryPool.Add(String.Format("UPDATE calc_t_rules SET order_number = {0} WHERE order_number = -1", to));
            // ===================
        }
        #endregion

        #region // Buttons click
        private void ButtonAddNewClick(object sender, EventArgs e)
        {
            PanelNewPressed(null, null);
        }

        private void ButtonNewRuleClick(object sender, EventArgs e)
        {
            //получаем ризон и номер сортировки для нового правила
            var frm = new TrixInputBox(RuleNameDiscription, RuleNameDiscription);
            if (frm.ShowDialog() != DialogResult.OK) return;
            int orderNum = 0;
            try
            {
                orderNum = int.Parse(TrixOrm.GetInstance().GetScalar("SELECT MAX(order_number) FROM calc_t_rules").ToString()) + 1;
            }
            catch { }
            //добавляем новое правило
            TrixOrm.GetInstance().Execute(String.Format("INSERT INTO calc_t_rules (order_number, reason, caption, colour) VALUES ({0}, '{1}', '', {2})", orderNum, frm.Tag, Color.Transparent.ToArgb()));
            listBoxRules.Items.Add(frm.Tag);
            //получаем pk и добавляем во вспомогательные списки
            var pk = TrixOrm.GetInstance().GetScalar("SELECT IDENT_CURRENT('calc_t_rules')").ToString();
            listBoxRulesMapper.Items.Add(pk);
            _captionsList.Add(pk, @"");
            _coloursList.Add(pk, Color.Transparent);
            _newRulesList.Add(pk);
        }

        private void ButtonDeleteRuleClick(object sender, EventArgs e)
        {
            if (listBoxRules.SelectedItems.Count == 0) return;
            var pk = listBoxRulesMapper.SelectedItem.ToString();
            listBoxRules.Items.Remove(listBoxRules.SelectedItem);
            listBoxRulesMapper.Items.Remove(listBoxRulesMapper.SelectedItem);
            _captionsList.Remove(pk);
            _coloursList.Remove(pk);
            _databaseQueryPool.Add(String.Format("DELETE FROM calc_t_rules WHERE pk = {0}", pk));
            _databaseQueryPool.Add(String.Format("DELETE FROM calc_t_rules2conjuncts WHERE ek_calc_rules = {0}", pk));      //по уму надо бы ещё удалить из calc_t_conjuncts (при условии, что конъюнкты больше нигде не используются)... ну ладно... бог с ними
        }

        private void ButtonColourClick(object sender, EventArgs e)
        {
            if (listBoxRules.SelectedItems.Count == 0) return;
            var dial = new ColorDialog();
            if (dial.ShowDialog() == DialogResult.OK)
            {
                buttonColour.BackColor = dial.Color;
                _coloursList[listBoxRulesMapper.SelectedItem.ToString()] = dial.Color;
                _databaseQueryPool.Add(String.Format("UPDATE calc_t_rules SET colour = {0} WHERE pk = '{1}'", dial.Color.ToArgb(), listBoxRulesMapper.SelectedItem));
            }
        }

        private void ButtonCancelClick(object sender, EventArgs e)
        {
            //удаляем добавленные правила (это единственное, что мы РЕАЛЬНО добавляли в базу; всё остальное хранилось в пуле, поэтому удалять не надо)
            foreach (var t in _newRulesList)
                TrixOrm.GetInstance().Execute(String.Format("DELETE FROM calc_t_rules WHERE pk = {0}", t));
            Close();
        }

        private void ButtonOkClick(object sender, EventArgs e)
        {
            //удаление элементов из списка _removeList (условие "t.PkConjunct != null" означает, что запись РЕАЛЬНО СУЩЕСТВУЕТ в базе данных, а не была добавлена и удалена)
            foreach (var p in _removeList.Where(t => t.PkConjunct != null))
            {
                TrixOrm.GetInstance().Execute(String.Format("DELETE FROM calc_t_conjuncts WHERE pk = {0}", p.PkConjunct));
                TrixOrm.GetInstance().Execute(String.Format("DELETE FROM calc_t_rules2conjuncts WHERE ek_calc_rules = {0} AND ek_calc_disjuncts = {1}", p.PkRule, p.PkConjunct));
            }
            //добавление новых и обновление изменившихся записей
            var lst = new List<TrixConjunctPanel>(_addList);
            lst.AddRange(_alterList);
            foreach (var a in lst)
            {
                var operand1 = TrixOrm.GetInstance().GetScalar(String.Format("SELECT pk FROM calc_t_operands WHERE text = '{0}'", a.comboBoxVariable.Text));
                var operation = a.comboBoxOperation.Text;
                var operand2 = @"-1";
                var value = a.comboBoxValue.Text.Trim();
                var parameter = a.textBoxParameter.Text.Trim();
                if (a.comboBoxValue.SelectedIndex >= 0)
                {
                    operand2 = TrixOrm.GetInstance().GetScalar(String.Format("SELECT pk FROM calc_t_operands WHERE text = '{0}'", a.comboBoxValue.Text)).ToString();
                    value = @"";
                }
                if (operand1 == null || String.IsNullOrWhiteSpace(operation) || (operand2 == @"-1" && String.IsNullOrWhiteSpace(value))) continue;
                if (_addList.Contains(a))           //добавляем новые
                {
                    TrixOrm.GetInstance().Execute(String.Format("INSERT INTO calc_t_conjuncts (ek_operand1, operation, ek_operand2, value, parameter) VALUES ({0}, '{1}', {2}, '{3}', '{4}')", operand1, operation, operand2, value, parameter));
                    TrixOrm.GetInstance().Execute(String.Format("INSERT INTO calc_t_rules2conjuncts (ek_calc_rules, ek_calc_conjuncts) VALUES ({0}, {1})", a.PkRule, TrixOrm.GetInstance().GetScalar("SELECT IDENT_CURRENT('calc_t_conjuncts')")));
                }
                else if (_alterList.Contains(a))    //обновляем изменившиеся
                    TrixOrm.GetInstance().Execute(String.Format("UPDATE calc_t_conjuncts SET ek_operand1 = {0}, operation = '{1}', ek_operand2 = {2}, value = '{3}', parameter = '{4}' WHERE pk = {5}", operand1, operation, operand2, value, parameter, a.PkConjunct));
            }
            //изменение самих правил
            foreach (var z in _databaseQueryPool)
                TrixOrm.GetInstance().Execute(z);
            //close
            Close();
        }
        #endregion
    }
}
