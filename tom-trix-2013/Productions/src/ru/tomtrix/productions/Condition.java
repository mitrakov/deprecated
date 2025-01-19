package ru.tomtrix.productions;

import ru.tomtrix.productions.core.Core;

import java.util.*;

/**
 * Класс реализует понятие "Посылка продукции" (напр. <b>Weather="Rainy" AND Time="Morning"</b>)<br>
 * Состоит из N операндов и (N-1) операций
 */
public class Condition implements ICommand
{
    /** Список операндов */
    private final List<Operand> _operands = Collections.synchronizedList(new ArrayList<Operand>());
    /** Список операций */
    private final List<Operation> _operations = Collections.synchronizedList(new ArrayList<Operation>());

    /**
     * Создаёт новую посылку правила с 1-м операндом
     * @param firstOperand операнд
     */
    public Condition(Operand firstOperand)
    {
        if (firstOperand == null) throw new NullPointerException(Core.NullParameters);
        _operands.add(firstOperand);
    }

    /**
     * Добавляет новый операнд к посылке
     * @param operation связка И/ИЛИ
     * @param operand операнд
     * @throws NullPointerException
     */
    public void addOperand(Operation operation, Operand operand) throws NullPointerException
    {
        if (operation == null || operand == null) throw new NullPointerException(Core.NullParameters);
        _operations.add(operation);
        _operands.add(operand);
    }

    /**
     * Вычисляет булеву функцию AND или OR
     * @param a первый операнд
     * @param operation операция (AND или OR)
     * @param b второй операнд
     * @return результат вычисления булевой функции
     * @throws EnumConstantNotPresentException
     */
    private boolean calculate(boolean a, Operation operation, boolean b) throws EnumConstantNotPresentException
    {
        switch (operation)
        {
            case AND: return a && b;
            case OR: return a || b;
            default:throw new NoSuchElementException(Core.NoEnumConstant);
        }
    }

    @Override
    public boolean execute()
    {
        boolean result = _operands.get(0).execute();
        for (int i=1; i<_operands.size(); i++)
        {
            Operation operation = _operations.get(i-1);
            boolean nextOperand = _operands.get(i).execute();
            result = calculate(result, operation, nextOperand);
        }
        return result;
    }

    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder(_operands.get(0).toString());
        for (int i=1; i<_operands.size(); i++)
            sb.append(String.format(" %s %s", _operations.get(i-1), _operands.get(i)));
        return sb.toString();
    }
}
