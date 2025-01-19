package ru.tomtrix.productions;

import ru.tomtrix.productions.core.Core;

/**
 * Класс реализует понятие "Переменная"
 */
public class Variable implements Comparable<Variable> {
    /** Имя переменной */
    private final String _name;
    /** Тип переменной (запрашиваемая, выводимая, ...) */
    private final VariableType _type;
    /** Значение переменной */
    private String _value;
    /** Показывает, инициализирована ли переменная перед логическим выводом */
    private boolean _isInitialized = false;

    /**
     * Создаёт новую неозначенную переменную
     * @param name имя переменной
     * @param type тип переменной (запрашиваемая, выводимая, ...)
     */
    public Variable(String name, VariableType type)
    {
        if (name == null || name.trim().isEmpty() || type == null) throw new NullPointerException(Core.NullParameters);
        _name = name.trim();
        _type = type;
    }

    /**
     * Сбрасывает переменную в неозначенное состояние
     */
    public void reset()
    {
        if (!_isInitialized) _value = null;
    }

    /**
     * @return значение переменной
     */
    public String get_value()
    {
        return _value;
    }

    /**
     * @return тип переменной (запрашиваемая, выводимая, ...)
     */
    public VariableType get_type()
    {
        return _type;
    }

    /**
     * Устанавливает значение переменной в новое состояние
     * @param value новое значение переменной
     */
    public void set_value(String value)
    {
        _value = value;
    }

    /**
     * Устанавливает значение переменной в новое состояние
     * @param value новое значение переменной
     */
    public void initialize(String value)
    {
        _isInitialized = true;
        _value = value;
    }

    /**
     * @return <b>true</b>, если переменная означена
     */
    public boolean is_designated()
    {
        return _value != null && !_value.trim().isEmpty();
    }

    @Override
    public int compareTo(Variable o)
    {
        return _name.compareTo(o._name);
    }

    @Override
    public String toString()
    {
        return  _name;
    }

    @Override
    public boolean equals(Object x)
    {
        return x instanceof Variable && _name.equals(((Variable) x)._name);
    }
}
