package ru.tomtrix.agentsocks.mathmodel;

import java.util.*;

import javassist.NotFoundException;
import ru.tomtrix.productions.*;
import ru.tomtrix.agentsocks.utils.StringUtils;

import javax.management.openmbean.KeyAlreadyExistsException;

/**
 * dw
 */
public class ProductionAgent extends Agent
{
    /** fs */
    private List<String> _initCode = new ArrayList<>(Arrays.asList("public void initialize() throws Exception {", "    Condition cond;\n", "}"));

    /** fs */
    private List<String> _ruleTexts = new LinkedList<>();

    /**
     * creates an agent with a specific name; it also builds runtime class that will comrise fields and methods of the agent
     * @param name - agent's name (and corresponding runtime class's name)
     * @throws Exception
     */
    public ProductionAgent(String name, String RAClassname) throws Exception
    {
        super(name, RAClassname);
        super.addVariable("public Core _core = new AgentProductionCore(this);");
        super.addVariable("public RuleSet _ruleset = new RuleSet();");
        super.addEvent(0, "initialize");
    }

    public void addVariable(String s, VariableType type) throws Exception
    {
        if (s == null || s.isEmpty()) throw new NullPointerException("Empty code");
        super.addVariable(String.format("Variable _%s = new Variable(\"%s\", VariableType.%s);", s, s, type));
        _initCode.add(2, String.format("    _core.addVariable(_%s);\n", s));
    }

    /**
     * fesfsefo
     * @param variable fsefes
     * @param value gdrh
     */
    public void initializeVariable(String variable, String value)
    {
        _initCode.add(_initCode.size()-1, String.format("    _%s.initialize(\"%s\");\n", variable, value));
    }

    /**
     * nfsenfs
     * @param name grseg
     * @param variables grwege
     * @param signs fwsrge
     * @param values ged
     * @param operations gsge
     * @param resultVariable fggre
     * @param resultValue gwsfwa
     */
    public void addRule(String name, List<String> variables, List<Inequality> signs, List<String> values, List<Operation> operations, String resultVariable, String resultValue)
    {
        // checks
        if (name==null || variables==null || signs==null || values==null || resultVariable==null || resultValue==null || (name.trim()+resultVariable.trim()+resultValue.trim()).isEmpty() || variables.isEmpty() || signs.isEmpty() || values.isEmpty()) throw new NullPointerException("sf");
        if (variables.size()!=signs.size() || variables.size()!=values.size() || variables.size()!=operations.size()+1) throw new IllegalArgumentException("gsef");
        if (ruleExists(name)) throw new KeyAlreadyExistsException("sef");

        // prepare code for a runtime assistant
        _initCode.add(_initCode.size() - 1, String.format("    cond = new Condition(new Operand(_%s, Inequality.%s, \"%s\", _core));", variables.get(0), signs.get(0), values.get(0)));
        for (int i=1; i<variables.size(); i++)
            _initCode.add(_initCode.size()-1, String.format("    cond.addOperand(Operation.%s, new Operand(_%s, Inequality.%s, \"%s\", _core));", operations.get(i-1), variables.get(i), signs.get(i), values.get(i)));
        _initCode.add(_initCode.size()-1, String.format("    _ruleset.addRule(new Rule(\"%s\", cond, _%s, \"%s\", _core));\n", name.trim(), resultVariable, resultValue));

        // save the text into the list
        StringBuilder sb = new StringBuilder(name.trim()).append(": IF (").append(variables.get(0)).append(signs.get(0)==Inequality.EQUALS ? "==\"" : "!=\"").append(values.get(0)).append("\"");
        for (int i=1; i<variables.size(); i++)
            sb.append(" ").append(operations.get(i-1)).append(" ").append(variables.get(i)).append(signs.get(i)==Inequality.EQUALS ? "==\"" : "!=\"").append(values.get(i)).append("\"");
        sb.append(") THEN {").append(resultVariable).append(" = \"").append(resultValue).append("\"}");
        _ruleTexts.add(sb.toString());
    }

    /**
     * grg
     * @param ruleName efwe
     */
    public void removeRule(String ruleName)
    {
        // checks
        if (!ruleExists(ruleName)) return;

        // remove text from the list
        for (String rule : _ruleTexts)
            if (rule.startsWith(ruleName.trim()))
            {
                _ruleTexts.remove(rule);
                break;
            }

        // now we've got to remove source code from the runtime assistant
        // we must delete all the rows from "cond = new Condition ..." till "_ruleset.addRule ..."
        // as long as the count is arbitrary we get the index and come backwards
        int j = -1;
        for (int i=0; i<_initCode.size(); i++)
            if (_initCode.get(i).contains(String.format("_ruleset.addRule(new Rule(\"%s\"", ruleName.trim())))
                j = i;
        boolean f = true;
        while (f && j>=0)
        {
            if (_initCode.get(j).contains("cond = new Condition"))
                f = false;
            _initCode.remove(j--);
        }

        // check whether everything is OK
        if (j < 0) throw new RuntimeException("rfsrs");
    }

    public boolean ruleExists(String ruleName)
    {
        for (String s : _ruleTexts)
            if (s.startsWith(ruleName.trim()))
                return true;
        return false;
    }

    /**
     * fjeoafaon
     * @param goal nfaikebik
     * @throws Exception
     */
    public void addTestConsulting(String goal) throws Exception
    {
        super.addFunction(String.format("public void testConsulting() {\n    System.out.println(_core.startConsulting(_%s, _ruleset));\n}", goal));
    }

    public List<String> get_ruleTexts()
    {
        return _ruleTexts;
    }

    /**
     * grjoeigrjo
     * Переменная м.б. любой! Как обычная, так и продукционная (т.е. начинаться с "_")
     * @param variable - gbd
     * @throws NotFoundException
     */
    @Override
    public void removeVariable(String variable) throws NotFoundException
    {
        super.removeVariable(variable);
        List<String> toDelete = new LinkedList<>();
        for (String s : _initCode)
            if (s.contains(String.format("_core.addVariable(%s", variable.trim())) || s.contains(String.format("%s.initialize(", variable.trim())))
                toDelete.add(s);
        for (String del : toDelete)
            _initCode.remove(del);
    }

    @Override
    public void compileAgents() throws Exception
    {
        super.addFunction(StringUtils.getElements(_initCode, "\n"));
        super.compileAgents();
    }
}
