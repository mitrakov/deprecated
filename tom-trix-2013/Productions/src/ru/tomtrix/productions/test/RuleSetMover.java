package ru.tomtrix.productions.test;

import org.junit.Test;
import java.util.*;
import ru.tomtrix.productions.*;
import ru.tomtrix.productions.core.Core;

import static org.junit.Assert.assertTrue;
import static org.junit.Assert.assertEquals;

/**
 * Класс тестирует правильность перемещения правил внутри Ruleset'a
 */
public class RuleSetMover
{
    @Test
    public void moveRules() throws Exception
    {
        Core core = new Core() {
            @Override
            public String askForVariable(Variable variable) {return null;}
        };
        Variable A = new Variable("A", VariableType.INFERRIBLE);
        Condition cond = new Condition(new Operand(A, Inequality.EQUALS, "Tom", core));
        Map<Variable, String> newValues = new TreeMap<>();
        newValues.put(A, "Trix");
        RuleSet rs = new RuleSet();

        rs.addRule(new Rule("R0", cond, newValues, core));
        rs.addRule(new Rule("R1", cond, newValues, core));
        rs.addRule(new Rule("R2", cond, newValues, core));
        rs.addRule(new Rule("R3", cond, newValues, core));
        rs.addRule(new Rule("R4", cond, newValues, core));
        rs.addRule(new Rule("R5", cond, newValues, core));
        rs.addRule(new Rule("R6", cond, newValues, core));
        rs.addRule(new Rule("R7", cond, newValues, core));
        rs.addRule(new Rule("R8", cond, newValues, core));
        rs.addRule(new Rule("R9", cond, newValues, core));

        assertEquals(rs.toString(), "R0, R1, R2, R3, R4, R5, R6, R7, R8, R9");
        rs.moveRule(3, 8);
        assertEquals(rs.toString(), "R0, R1, R2, R4, R5, R6, R7, R8, R3, R9");
        rs.moveRule(4, 1);
        assertEquals(rs.toString(), "R0, R5, R1, R2, R4, R6, R7, R8, R3, R9");
        rs.moveRule(9, 0);
        assertEquals(rs.toString(), "R9, R0, R5, R1, R2, R4, R6, R7, R8, R3");
        rs.moveRule(0, 6);
        assertEquals(rs.toString(), "R0, R5, R1, R2, R4, R6, R9, R7, R8, R3");
        boolean b = false;
        try
        {
            rs.moveRule(0, 10);
        }
        catch (Exception ignore) {b = true;}
        assertTrue(b);
        rs.deleteRule(3);
        assertEquals(rs.toString(), "R0, R5, R1, R4, R6, R9, R7, R8, R3");
        rs.deleteRuleByName("R7 ");
        assertEquals(rs.toString(), "R0, R5, R1, R4, R6, R9, R8, R3");
        b = false;
        try
        {
            rs.addRule(new Rule("R4 ", cond, newValues, core));
        }
        catch (Exception ignore) {b = true;}
        assertTrue(b);
    }
}
