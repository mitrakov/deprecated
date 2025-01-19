package ru.tomtrix.productions.test;

import org.junit.Test;
import ru.tomtrix.productions.*;
import ru.tomtrix.productions.core.Core;

import static org.junit.Assert.assertTrue;
import static org.junit.Assert.assertFalse;

/**
 * Класс тестирует правильность вычисления логического выражения посылки
 */
public class ConditionTest
{
    @Test
    public void testConditions()
    {
        Variable A = new Variable("A", VariableType.INFERRIBLE);
        Variable B = new Variable("B", VariableType.INFERRIBLE);
        Variable C = new Variable("C", VariableType.INFERRIBLE);

        Core core = new Core(A, B, C) {
            @Override
            public String askForVariable(Variable variable) {return null;}
        };

        Operand a = new Operand(A, Inequality.EQUALS, "Tom", core);
        Operand b = new Operand(B, Inequality.EQUALS, "Trix", core);
        Operand c = new Operand(C, Inequality.EQUALS, "Rem", core);

        // A
        Condition cond = new Condition(a);
        core.setVariable(A, "Tom");
        assertTrue(cond.execute());
        core.setVariable(A, "Rom");
        assertFalse(cond.execute());

        // A && B
        cond = new Condition(a);
        cond.addOperand(Operation.AND, b);
        core.setVariable(A, "Tom");
        core.setVariable(B, "Trix");
        assertTrue(cond.execute());
        core.setVariable(A, "Rom");
        core.setVariable(B, "Trix");
        assertFalse(cond.execute());
        core.setVariable(A, "Tom");
        core.setVariable(B, "Srix");
        assertFalse(cond.execute());
        core.setVariable(A, "Rom");
        core.setVariable(B, "Srix");
        assertFalse(cond.execute());

        // A || B
        cond = new Condition(a);
        cond.addOperand(Operation.OR, b);
        core.setVariable(A, "Tom");
        core.setVariable(B, "Trix");
        assertTrue(cond.execute());
        core.setVariable(A, "Rom");
        core.setVariable(B, "Trix");
        assertTrue(cond.execute());
        core.setVariable(A, "Tom");
        core.setVariable(B, "Srix");
        assertTrue(cond.execute());
        core.setVariable(A, "Rom");
        core.setVariable(B, "Srix");
        assertFalse(cond.execute());

        // A && B && C
        cond = new Condition(a);
        cond.addOperand(Operation.AND, b);
        cond.addOperand(Operation.AND, c);
        core.setVariable(A, "Tom");
        core.setVariable(B, "Trix");
        core.setVariable(C, "Rem");
        assertTrue(cond.execute());
        core.setVariable(A, "Rom");
        core.setVariable(B, "Trix");
        core.setVariable(C, "Rem");
        assertFalse(cond.execute());
        core.setVariable(A, "Rom");
        core.setVariable(B, "Srix");
        core.setVariable(C, "Rem");
        assertFalse(cond.execute());
        core.setVariable(A, "Rom");
        core.setVariable(B, "Trix");
        core.setVariable(C, "Gem");
        assertFalse(cond.execute());
        core.setVariable(A, "Rom");
        core.setVariable(B, "Srix");
        core.setVariable(C, "Gem");
        assertFalse(cond.execute());
        core.setVariable(A, "Tom");
        core.setVariable(B, "Srix");
        core.setVariable(C, "Rem");
        assertFalse(cond.execute());
        core.setVariable(A, "Tom");
        core.setVariable(B, "Trix");
        core.setVariable(C, "Gem");
        assertFalse(cond.execute());
        core.setVariable(A, "Tom");
        core.setVariable(B, "Srix");
        core.setVariable(C, "Gem");
        assertFalse(cond.execute());

        // A || B || C
        cond = new Condition(a);
        cond.addOperand(Operation.OR, b);
        cond.addOperand(Operation.OR, c);
        core.setVariable(A, "Tom");
        core.setVariable(B, "Trix");
        core.setVariable(C, "Rem");
        assertTrue(cond.execute());
        core.setVariable(A, "Tom");
        core.setVariable(B, "Srix");
        core.setVariable(C, "Rem");
        assertTrue(cond.execute());
        core.setVariable(A, "Tom");
        core.setVariable(B, "Trix");
        core.setVariable(C, "Gem");
        assertTrue(cond.execute());
        core.setVariable(A, "Tom");
        core.setVariable(B, "Srix");
        core.setVariable(C, "Gem");
        assertTrue(cond.execute());
        core.setVariable(A, "Rom");
        core.setVariable(B, "Trix");
        core.setVariable(C, "Rem");
        assertTrue(cond.execute());
        core.setVariable(A, "Rom");
        core.setVariable(B, "Srix");
        core.setVariable(C, "Rem");
        assertTrue(cond.execute());
        core.setVariable(A, "Rom");
        core.setVariable(B, "Trix");
        core.setVariable(C, "Gem");
        assertTrue(cond.execute());
        core.setVariable(A, "Rom");
        core.setVariable(B, "Srix");
        core.setVariable(C, "Gem");
        assertFalse(cond.execute());

        // A && B || C
        cond = new Condition(a);
        cond.addOperand(Operation.AND, b);
        cond.addOperand(Operation.OR, c);
        core.setVariable(A, "Tom");
        core.setVariable(B, "Trix");
        core.setVariable(C, "Rem");
        assertTrue(cond.execute());
        core.setVariable(A, "Tom");
        core.setVariable(B, "Srix");
        core.setVariable(C, "Rem");
        assertTrue(cond.execute());
        core.setVariable(A, "Tom");
        core.setVariable(B, "Trix");
        core.setVariable(C, "Gem");
        assertTrue(cond.execute());
        core.setVariable(A, "Tom");
        core.setVariable(B, "Srix");
        core.setVariable(C, "Gem");
        assertFalse(cond.execute());
        core.setVariable(A, "Rom");
        core.setVariable(B, "Trix");
        core.setVariable(C, "Rem");
        assertTrue(cond.execute());
        core.setVariable(A, "Rom");
        core.setVariable(B, "Srix");
        core.setVariable(C, "Rem");
        assertTrue(cond.execute());
        core.setVariable(A, "Rom");
        core.setVariable(B, "Trix");
        core.setVariable(C, "Gem");
        assertFalse(cond.execute());
        core.setVariable(A, "Rom");
        core.setVariable(B, "Srix");
        core.setVariable(C, "Gem");
        assertFalse(cond.execute());

        // A || B && C
        cond = new Condition(a);
        cond.addOperand(Operation.OR, b);
        cond.addOperand(Operation.AND, c);
        core.setVariable(A, "Tom");
        core.setVariable(B, "Trix");
        core.setVariable(C, "Rem");
        assertTrue(cond.execute());
        core.setVariable(A, "Tom");
        core.setVariable(B, "Srix");
        core.setVariable(C, "Rem");
        assertTrue(cond.execute());
        core.setVariable(A, "Tom");
        core.setVariable(B, "Trix");
        core.setVariable(C, "Gem");
        assertFalse(cond.execute());
        core.setVariable(A, "Tom");
        core.setVariable(B, "Srix");
        core.setVariable(C, "Gem");
        assertFalse(cond.execute());
        core.setVariable(A, "Rom");
        core.setVariable(B, "Trix");
        core.setVariable(C, "Rem");
        assertTrue(cond.execute());
        core.setVariable(A, "Rom");
        core.setVariable(B, "Srix");
        core.setVariable(C, "Rem");
        assertFalse(cond.execute());
        core.setVariable(A, "Rom");
        core.setVariable(B, "Trix");
        core.setVariable(C, "Gem");
        assertFalse(cond.execute());
        core.setVariable(A, "Rom");
        core.setVariable(B, "Srix");
        core.setVariable(C, "Gem");
        assertFalse(cond.execute());
    }
}
