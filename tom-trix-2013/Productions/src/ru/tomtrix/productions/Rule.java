package ru.tomtrix.productions;

import ru.tomtrix.productions.core.Core;

import java.util.*;
import java.util.Map.Entry;

/**
 * Класс реализует понятие "Продукция"
 */
public class Rule implements ICommand
{
    /** Имя продукции */
    private final String _name;
    /** Посылка */
    private final Condition _condition;
    /** Заключение (новых переменных м.б. несколько) */
    private final Map<Variable, String> _newValues;
    /** Ссылка на ядро */
    private final Core _coreRef;

    /**
     * Создаёт новое правило с множественным заключением (<b>newValues</b>)
     * @param name имя правила
     * @param condition посылка
     * @param newValues заключение
     * @param core ссылка на ядро
     */
    public Rule(String name, Condition condition, Map<Variable, String> newValues, Core core)
    {
        if (name == null || condition == null || newValues == null || newValues.size() == 0 || name.trim().isEmpty()) throw new IllegalArgumentException(Core.NullParameters);
        _name = name.trim();
        _condition = condition;
        _newValues = newValues;
        _coreRef = core;
    }

    /**
     * Создаёт новое правило с обычным заключением (<b>variable <- value</b>)
     * @param name имя правила
     * @param condition посылка
     * @param variable заключение (означиваемая переменная)
     * @param value заключение (новое значение)
     * @param core ссылка на ядро
     */
    public Rule(String name, Condition condition, Variable variable, String value, Core core)
    {
        if (name == null || condition == null || variable == null || value == null || (name.trim()+value.trim()).isEmpty()) throw new IllegalArgumentException(Core.NullParameters);
        _name = name.trim();
        _condition = condition;
        Map<Variable, String> newValues = new TreeMap<>();
        newValues.put(variable,  value);
        _newValues = newValues;
        _coreRef = core;
    }

    /**
     * @return имя продукции
     */
    public String get_name()
    {
        return _name;
    }

    /**
     * @return список переменных, которые может означить правило в случае срабатывания
     */
    public Set<Variable> get_newVariables()
    {
        return _newValues.keySet();
    }

    public boolean equals(Object w)
    {
        return (w instanceof Rule) && _name.equals(((Rule) w)._name);
    }

    @Override
    public boolean execute()
    {
        boolean result = _condition.execute();
        if (result)
            for (Entry<Variable, String> e : _newValues.entrySet())
                _coreRef.setVariable(e.getKey(), e.getValue());
        return result;
    }

    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();
        for (Entry<Variable, String> e : _newValues.entrySet())
            sb.append(String.format("; %s = %s", e.getKey(), e.getValue()));
        return String.format("%s: IF (%s) THEN {%s}", _name, _condition, sb.substring(2));
    }
}
