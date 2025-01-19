package ru.tomtrix.productions;


import java.util.*;
import static java.lang.Math.min;
import static java.lang.Math.max;
import com.sun.istack.internal.FinalArrayList;
import ru.tomtrix.productions.core.Core;

import javax.management.openmbean.KeyAlreadyExistsException;

/**
 * Класс реализует понятие Ruleset (набор правил)
 */
public class RuleSet {
    /** Список правил */
    private final List<Rule> _rules = Collections.synchronizedList(new ArrayList<Rule>());

    /**
     * Добавляет правило в множество
     * @param rule правило
     * @throws Exception
     */
    public void addRule(Rule rule) throws Exception
    {
        if (rule == null) throw new NullPointerException(Core.NullParameters);
        if (_rules.contains(rule)) throw new KeyAlreadyExistsException(Core.RuleExists);
        _rules.add(rule);
    }

    /**
     * Удаляет правило из множества
     * @param s имя правила
     * @throws Exception
     */
    public void deleteRuleByName(String s) throws Exception
    {
        if (s == null || s.trim().isEmpty()) throw new NullPointerException(Core.NullParameters);
        for (Rule r : _rules)
            if (r.get_name().equals(s.trim()))
            {
                _rules.remove(r);
                return; //обязательно!!!
            }
    }

    /**
     * Удаляет правило из множества
     * @param index позиция правила в списке
     * @throws Exception
     */
    public void deleteRule(int index) throws Exception
    {
        if (index <0 || index >= _rules.size()) throw new IndexOutOfBoundsException(Core.WrongIndex);
        _rules.remove(index);
    }

    /**
     * Выполняет перестановку правил
     * @param index позиция переставляемого правила
     * @param newIndex новая позиция
     */
    public void moveRule(int index, int newIndex)
    {
        if (min(index, newIndex) < 0 || max(index, newIndex) >= _rules.size()) throw new IndexOutOfBoundsException(Core.WrongIndex);
        _rules.add(newIndex, _rules.remove(index));
    }

    /**
     * @return список правил
     */
    public List<Rule> get_rules()
    {
        return new FinalArrayList<>(_rules);
    }

    /**
     * @return размер множества правил
     */
    public int get_size()
    {
        return _rules.size();
    }

    @Override
    public String toString()
    {
        StringBuilder sbuf = new StringBuilder();
        for (Rule r : _rules)
            sbuf.append(", ").append(r.get_name());
        return sbuf.substring(2);
    }
}
