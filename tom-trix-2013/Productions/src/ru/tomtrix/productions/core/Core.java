package ru.tomtrix.productions.core;

import java.util.*;
import ru.tomtrix.productions.*;
import java.util.concurrent.ConcurrentHashMap;

/**
 * Класс реализует ядро оболочки экспертной системы. Здесь имеется рабочая память, хранятся ссылки на множества правил и т.д.<br>
 * При логическом выводе переменные запрашиваются именно отсюда<br>
 * Класс реализует все методы за исключением <b>askForVariable</b> (запрос для запрашиваемой переменной)
 */
@SuppressWarnings("unused")
public abstract class Core {
    public static String NullParameters = "Parameters can't be NULL";
    public static String NoEnumConstant = "There is no such a constant";
    public static String EmptyRuleset = "Current ruleset is not assigned";
    public static String NoVariable = "There is no such a variable";
    public static String RuleExists = "Rule with such a name already exists";
    public static String WrongIndex = "Index is out of bounds";

    /** Рабочая память (выбор Map был осознанным!) */
    private final Map<Variable, Variable> _variables = new ConcurrentHashMap<>();
    /** Генератор случайных чисел для Random-стратегии выборки правил */
    private final Random _random = new Random(System.currentTimeMillis());
    /** Стратегия выборки правил */
    private RulesSelectionStrategies _ruleSelectionStrategy = RulesSelectionStrategies.FIRST;
    /** Ссылка на текущий RuleSet */
    private RuleSet _curRuleset;

    public void addVariable(Variable var)
    {
        if (var==null || _variables.containsKey(var)) throw new IllegalArgumentException("Wrong argument");
        _variables.put(var, var);
    }

    /**
     * Запрашивает переменную у пользователя
     * @param variable переменная
     * @return значение переменной
     */
    public abstract String askForVariable(Variable variable);

    /**
     * Создаёт ядро оболочки ЭС с заданным списком переменных
     * @param variables набор переменных
     */
    public Core(Variable ... variables)
    {
        for (Variable var : variables)
            _variables.put(var, var);
    }

    /**
     * Присваивает переменной определённое значение
     * @param var переменная
     * @param value значение
     */
    public void setVariable(Variable var, String value)
    {
        if (var == null || value == null || value.trim().isEmpty() || !_variables.containsKey(var)) throw new NullPointerException(String.format("%s [var=%s; value=%s]", NullParameters, var, value));
        _variables.get(var).set_value(value.trim());
    }

    /**
     * Возвращает значение переменной из рабочей памяти. Если переменная не означена, то <b>Core</b> пытается её означить (запрос у пользователя и/или логический вывод)
     * @param var переменная
     * @return <li>значение переменной, если она уже была означена (либо означена в ходе логического вывода)
     * <li><b>NULL</b>, если переменная не означена даже в ходе логического вывода
     */
    public String getValue(Variable var)
    {
        if (var == null || !_variables.containsKey(var)) throw new NullPointerException(NoVariable);
        if (!var.is_designated())
            switch (var.get_type())
            {
                case INFERRIBLE:
                    inference(var);
                    break;
                case ASKABLE:
                    setVariable(var, askForVariable(var));
                    break;
                case INFERRIBLY_ASKABLE:
                    inference(var);
                    if (!var.is_designated())
                        setVariable(var, askForVariable(var));
                    break;
                default: throw new NoSuchElementException(NoEnumConstant);
            }
        String s = var.get_value();
        return s.trim();
    }

    /**
     * Запускает логический вывод, который попытается означить переменную <b>goal</b> на заданном множестве правил
     * @param goal целевая переменная
     * @param ruleset множество правил вывода
     * @return результат вывода
     */
    public String startConsulting(Variable goal, RuleSet ruleset)
    {
        if (goal==null || ruleset==null || ruleset.get_size()==0) throw new NullPointerException(NullParameters);
        _curRuleset = ruleset;
        for (Variable var : _variables.keySet())
            var.reset();
        return inference(goal);
    }

    /**
     * Реализует логический вывод
     * @param subgoal текущая подцель
     * @return результат вывода (или <b>NULL</b>, если вывод на текущем ruleset неудачен)
     */
    private String inference(Variable subgoal)
    {
        if (_curRuleset == null) throw new NullPointerException(EmptyRuleset);
        if (subgoal.is_designated())
            return subgoal.get_value();

        //BACKWARD CHAINING
        //find the fired rules
        List<Rule> firedRules = new LinkedList<>();
        for (Rule r : _curRuleset.get_rules())
            if (r.get_newVariables().contains(subgoal))
                firedRules.add(r);
        //handle fired rules one-by-one
        while (!firedRules.isEmpty())
        {
            Rule chosenRule = chooseTheRule(firedRules);
            chosenRule.execute();
            if (subgoal.is_designated())
                return subgoal.get_value();
            firedRules.remove(chosenRule);
        }
        return null;
    }

    /**
     * Выбирает правило из некоторого списка согласно текущей стратегии выборки правил (напр, первое, последнее, рандом)
     * @param rules список правил
     * @return выбранное правило из списка (или <b>NULL</b> в случае пустого списка)
     */
    private Rule chooseTheRule(List<Rule> rules)
    {
        if (rules == null || rules.isEmpty()) return null;
        switch (_ruleSelectionStrategy)
        {
            case FIRST: return rules.get(0);
            case LAST: return rules.get(rules.size()-1);
            case RANDOM: return rules.get(_random.nextInt(rules.size()));
            default: throw new NoSuchElementException("gr");
        }
    }
}
