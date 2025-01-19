// ReSharper disable EmptyGeneralCatchClause
using System;
using System.Drawing;
using System.Linq;

namespace Calculator
{
    public static class LogicalInference
    {
        /// <summary>
        /// Метод возвращает результат вывода правила для конкретного патента (кэпшен (напр, прекратил своё действие), цвет (напр, синий если всё ОК), номер сработавшего правила и т.д.)
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        public static Trix.InferenceResult GetRule(string pk)
        {
            try
            {
                //бежим по списку правил; если какое-то из них срабатывает, вывод прекращается
                foreach (var rule in TrixOrm.GetInstance().GetListOfCortages("SELECT pk, caption, colour, reason, order_number FROM calc_t_rules ORDER BY order_number"))
                {
                    //чтобы правило сработало, необходимо, чтобы ВСЕ его конъюнкты обратились в истину
                    var qqq = TrixOrm.GetInstance().GetListOfCortages(String.Format("SELECT op1.sqltext, operation, op2.sqltext, value, parameter FROM calc_t_rules2conjuncts INNER JOIN calc_t_conjuncts ON ek_calc_conjuncts = calc_t_conjuncts.pk INNER JOIN calc_t_operands AS op1 ON ek_operand1 = op1.pk LEFT OUTER JOIN calc_t_operands AS op2 ON ek_operand2 = op2.pk WHERE ek_calc_rules = {0}", rule[0]));
                    int a;
                    var where = qqq.Aggregate(@"", (current, t) => current + String.Format(@" AND {0} {1} {2}", t[0].ToString().Replace("%d", t[4].ToString()), t[1], !String.IsNullOrWhiteSpace(t[3].ToString()) ? (int.TryParse(t[3].ToString(), out a) ? t[3] : "'" + t[3] + "'") : t[2].ToString().Replace("%d", t[4].ToString())));
                    //запрос к БД. Если число записей > 0, то правило успешно сработало
                    if (int.Parse(TrixOrm.GetInstance().GetScalar(String.Format("SELECT COUNT(*) FROM calc_t_main INNER JOIN t_main ON ek_main = t_main.pk INNER JOIN d_icodes ON project_type = usercode WHERE {0} AND calc_t_main.pk = {1}", where.Substring(5), pk)).ToString()) <= 0) continue;
                    //ура! возвращаем полученный результат
                    return new Trix.InferenceResult{Caption = rule[1].ToString(), Colour = Color.FromArgb(int.Parse(rule[2].ToString())), Number = int.Parse(rule[4].ToString()) + 1, Reason = rule[3].ToString()};
                }
            }
            catch {}
            //возвращаем пустоту в случае неудачи
            return null;
        }
    }
}
