package ru.tomtrix.agentsocks.infrastructure;

import ru.tomtrix.agentsocks.mathmodel.Environment;

import java.util.*;
import java.util.Map.Entry;

/** fd
 * @author tom-trix */
public class Model implements IAgentProcessible
{
	/** csdfse */
	private final String				_name;
	/** sfee */
	private final Map<Integer, Node>	_nodes	     = new TreeMap<>();
    /** fjsrojf */
    private final Environment           _environment = new Environment(this);
    /** khgtd */
	private final boolean _isDebug;

	/** fse
	 * @param name - fgr */
	public Model(String name)
	{
		if (name == null || name.trim().isEmpty()) throw new NullPointerException("Model name can't be empty!");
		_name = name;
		_isDebug = name.trim().toLowerCase().equals("debug");
	}

	/** @return gre */
	public int addNode()
	{
		int result = _nodes.size();
		_nodes.put(result, new Node(result, _isDebug));
		return result;
	}

	/** fsefe */
	public void deleteLastNode()
	{
		_nodes.remove(_nodes.size() - 1);
	}

	/** @return sbsei */
	public int getNodesCount()
	{
		return _nodes.size();
	}

	/** @param num - fsefs
	 * @return fw */
	public Node getNodeByNumber(int num)
	{
		if (!_nodes.containsKey(num)) throw new NullPointerException(String.format("There is no such a node (%d)", num));
		return _nodes.get(num);
	}

	@Override
	public void loadCode() throws Exception
	{
		_environment.init();
        for (Node node : _nodes.values())
			node.get_container().loadCode();
	}

	@Override
	public void compileAgents() throws Exception
	{
		for (Node node : _nodes.values())
			node.get_container().compileAgents();
	}

	@Override
	public String toString()
	{
		StringBuilder sbuf = new StringBuilder(String.format("Model \"%s\". Node collection:\n", _name));
		if (_nodes.size() == 0) sbuf.append("<no nodes>\n");
		for (Entry<Integer, Node> node : _nodes.entrySet())
			sbuf.append(node);
		return sbuf.toString();
	}

	/** @return the _name */
	public String get_name()
	{
		return _name;
	}

    /** @return nodes */
    public Collection<Node> get_nodes()
    {
        return _nodes.values();
    }

    /** @return environment */
    public Environment get_environment()
    {
        return _environment;
    }
}
