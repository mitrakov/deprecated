package ru.tomtrix.productions;

import ru.tomtrix.productions.core.Core;

/**
 * Класс реализует понятие "Операнд", используемого в посылке правила
 */
public class Operand implements ICommand
{
    /** Переменная */
    private final Variable _variable;
    /** Знак операции сравнения (==, !=, ...) */
    private final Inequality _equals;
    /** Значение */
    private final String _value;
    /** Ссылка на ядро */
    private final Core _coreRef;

    /**
     * Создаёт новый операнд
     * @param variable переменная
     * @param equals знак операции сравнения
     * @param value значение
     * @param core ссылка на ядро
     */
    public Operand(Variable variable, Inequality equals, String value, Core core)
    {
        if (variable == null || value == null || value.trim().isEmpty() || core == null) throw new NullPointerException(Core.NullParameters);
        _variable = variable;
        _equals = equals;
        _value = value.trim();
        _coreRef = core;
    }

    @Override
    public boolean execute()
    {
        boolean f = _value.equals(_coreRef.getValue(_variable));
        return _equals==Inequality.EQUALS ? f : !f;
    }

    @Override
    public String toString()
    {
        return String.format("%s%s\"%s\"", _variable, _equals==Inequality.EQUALS ? "==" : "!=", _value);
    }
}
