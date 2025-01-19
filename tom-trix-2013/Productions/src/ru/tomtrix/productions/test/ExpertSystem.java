package ru.tomtrix.productions.test;

import ru.tomtrix.productions.*;
import ru.tomtrix.productions.core.*;
import static ru.tomtrix.productions.Operation.*;
import static ru.tomtrix.productions.Inequality.*;
import static ru.tomtrix.productions.VariableType.*;

/**
 * Пример экспертной системы "К какой паре идти на занятия" (всего 13 правил)
 */
public class ExpertSystem
{
    public static void main(String[] args) throws Exception
    {
        new ExpertSystem().consult();
    }

    public void consult() throws Exception
    {
        Variable goTo = new Variable("Пойду к N-й паре", INFERRIBLE);
        Variable needTo = new Variable("Нужно к N-й паре", ASKABLE);
        Variable mood = new Variable("Настроение", INFERRIBLY_ASKABLE);
        Variable vivacity = new Variable("Выспался", INFERRIBLY_ASKABLE);
        Variable fallAsleep = new Variable("Лёг спать", ASKABLE);
        Variable sleepDebt = new Variable("Недосыпание", ASKABLE);
        Variable weather = new Variable("Погода", ASKABLE);
        Variable beer = new Variable("Бахнул пивка", ASKABLE);
        Variable haveSex = new Variable("Интим", ASKABLE);
        Variable zenit = new Variable("Зенит", ASKABLE);

        Core core = new ConsoleCore(goTo, needTo, mood, vivacity, fallAsleep, sleepDebt,weather, beer, haveSex, zenit);
        RuleSet ruleset = new RuleSet();
        Condition cond;

        cond = new Condition(new Operand(needTo, EQUALS, "1", core));
        cond.addOperand(AND, new Operand(mood, EQUALS, "Хорошее", core));
        cond.addOperand(AND, new Operand(vivacity, EQUALS, "Да", core));
        ruleset.addRule(new Rule("R1", cond, goTo, "1", core));

        cond = new Condition(new Operand(needTo, EQUALS, "2", core));
        cond.addOperand(AND, new Operand(mood, EQUALS, "Хорошее", core));
        ruleset.addRule(new Rule("R2", cond, goTo, "2", core));

        cond = new Condition(new Operand(needTo, EQUALS, "3", core));
        cond.addOperand(AND, new Operand(mood, NOT_EQUALS, "Плохое", core));
        ruleset.addRule(new Rule("R3", cond, goTo, "2", core));

        cond = new Condition(new Operand(mood, EQUALS, "Плохое", core));
        ruleset.addRule(new Rule("R4", cond, goTo, "Не пойду", core));

        cond = new Condition(new Operand(fallAsleep, EQUALS, "Рано", core));
        cond.addOperand(OR, new Operand(fallAsleep, EQUALS, "Средне", core));
        ruleset.addRule(new Rule("R5", cond, vivacity, "Да", core));

        cond = new Condition(new Operand(fallAsleep, EQUALS, "Поздно", core));
        cond.addOperand(OR, new Operand(sleepDebt, EQUALS, "Да", core));
        ruleset.addRule(new Rule("R6", cond, vivacity, "Нет", core));

        cond = new Condition(new Operand(weather, EQUALS, "Солнечно", core));
        cond.addOperand(AND, new Operand(sleepDebt, EQUALS, "Нет", core));
        ruleset.addRule(new Rule("R7", cond, mood, "Хорошее", core));

        cond = new Condition(new Operand(beer, EQUALS, "Да", core));
        cond.addOperand(OR, new Operand(haveSex, EQUALS, "Да", core));
        ruleset.addRule(new Rule("R8", cond, mood, "Хорошее", core));

        cond = new Condition(new Operand(weather, EQUALS, "Дождь", core));
        cond.addOperand(AND, new Operand(beer, EQUALS, "Нет", core));
        ruleset.addRule(new Rule("R9", cond, mood, "Среднее", core));

        cond = new Condition(new Operand(weather, EQUALS, "Дождь", core));
        cond.addOperand(AND, new Operand(haveSex, EQUALS, "Нет", core));
        ruleset.addRule(new Rule("R10", cond, mood, "Плохое", core));

        cond = new Condition(new Operand(zenit, EQUALS, "Выиграл", core));
        ruleset.addRule(new Rule("R11", cond, mood, "Хорошее", core));

        cond = new Condition(new Operand(zenit, EQUALS, "Вничью", core));
        cond.addOperand(AND, new Operand(beer, EQUALS, "Да", core));
        ruleset.addRule(new Rule("R12", cond, mood, "Среднее", core));

        cond = new Condition(new Operand(zenit, NOT_EQUALS, "Выиграл", core));
        ruleset.addRule(new Rule("R13", cond, mood, "Плохое", core));

        haveSex.initialize("Да");
        System.out.println(core.startConsulting(goTo, ruleset));
    }
}
