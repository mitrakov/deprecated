package ru.tomtrix.agentsocks.mathmodel;

import java.util.*;
import java.util.Map.Entry;
import org.apache.log4j.Logger;
import javassist.NotFoundException;
import ru.tomtrix.agentsocks.messaging.*;
import ru.tomtrix.javassistwraper.ClassStore;
import javax.activation.UnsupportedDataTypeException;
import ru.tomtrix.agentsocks.infrastructure.IAgentProcessible;

/** fseefsfo
 * @author tom-trix */
public abstract class Agent implements IAgentProcessible
{
	/** agent's name */
	private String				_name;
	/** fse */
	private final List<String>	_state				= new ArrayList<>();
	/** gsrrg */
	private final List<String>	_transformFunctions	= new ArrayList<>();
	/** list of events */
	private final EventList		_eventList			= new EventList();
	/** name of runtime class that corresponds to an agent */
	protected final String		_RAClassname;
	/** runtime assistant */
	private Object				_runtimeAssistant;

	/** creates an agent with a specific name; it also builds runtime class that will comrise fields and methods of the agent
	 * @param name - agent's name (and corresponding runtime class's name)
	 * @throws Exception */
	Agent(String name, String RAClassname) throws Exception
	{
		if (name == null || name.isEmpty()) throw new NullPointerException("Agent must have a name");
		_name = name;
		_RAClassname = RAClassname;
        ClassStore.getInstance().addClass(_RAClassname, null, null);
		Logger.getLogger(getClass()).info(String.format("Agent \"%s\" created", name));
	}

	/** adds a variable to the agent by adding a new field to the corresponding runtime class
	 * @param code - Java code (e.g. "int x = 5;")
	 * @throws Exception */
	public void addVariable(String code) throws Exception
	{
		if (code == null || code.isEmpty()) throw new NullPointerException("Empty code");
		ClassStore.getInstance().addField(_RAClassname, code);
		_state.add(code);						// д.б. после обращения к ClassStore
	}

	/** adds a function to the agent by adding a new method to the corresponding runtime class
	 * @param code - Java code (e.g. "public String sayHello() {return \"Hello\"};")
	 * @throws Exception */
	public void addFunction(String code) throws Exception
	{
		if (code == null || code.isEmpty()) throw new NullPointerException("Empty code");
		ClassStore.getInstance().addMethod(_RAClassname, code);
		_transformFunctions.add(code);			// д.б. после обращения к ClassStore
	}

	/** jsiojfoejs
	 * @param variable - gbd
	 * @throws NotFoundException */
	public void removeVariable(String variable) throws NotFoundException
	{
		ClassStore.getInstance().removeField(_RAClassname, variable);
		for (String s : _state)
			if (Arrays.asList(s.split("[;= ]")).contains(variable)) //TODO
			{
				_state.remove(s);
				return;
			}
		throw new RuntimeException(String.format("ATTENTION! Variable \"%s\" hasn't been removed properly. The model is corrupted. Repair json and restart the application", variable));
	}

	/** ofjaswefhui
	 * @param fid - fs
	 * @throws NotFoundException */
	public void removeFunction(String fid) throws NotFoundException
	{
		ClassStore.getInstance().removeMethods(_RAClassname, fid);
		for (String s : _transformFunctions)
			if (Arrays.asList(s.split("[( ]")).contains(fid))        //TODO
			{
				_transformFunctions.remove(s);
				_eventList.removeEventByFid(fid);
				return;
			}
		throw new RuntimeException(String.format("ATTENTION! Function with fid = \"%s\" hasn't been removed properly. The model is corrupted. Repair json and restart the application", fid));
	}

	/** fseef
	 * @param data - fs
	 * @throws Exception */
	public void notifyAgent(IMessage data, String sender) throws Exception
	{
		if (data instanceof StateChanged)
		{
			StateChanged msg = (StateChanged) data;
			Logger.getLogger(getClass()).info(String.format("%s -> %s: %s = %s", sender, _name, msg.get_variable(), msg.get_value()));
			_runtimeAssistant.getClass().getDeclaredField(msg.get_variable()).set(_runtimeAssistant, msg.get_value());
		}
		else throw new UnsupportedDataTypeException("Message type is unknown");
	}

	/** @return fsaef */
	public Collection<String> getState()
	{
		return new LinkedList<>(_state);
	}

	/** @return fnasiojo */
	public Collection<String> getTransformFunctions()
	{
		return new LinkedList<>(_transformFunctions);
	}

    /** @return fnasiojo */
    public Collection<String> getVariables()
    {
        return ClassStore.getInstance().getFields(_RAClassname);
    }

    /** @return fnasiojo */
    public Collection<String> getEvents()
    {
        Collection<String> result = new PriorityQueue<>();
        for (Entry<Double, String> e : _eventList.getEventsInfo().entrySet())
            result.add(String.format("%.2f: %s", e.getKey(), e.getValue()));
        return result;
    }

    /** @return fnasiojo */
    public Collection<String> getFids()
    {
        return ClassStore.getInstance().getMethods(_RAClassname);
    }

	@Override
	public void loadCode() throws Exception
	{
		try { ClassStore.getInstance().addClass(_RAClassname, null, null); }
        catch(Exception ignored) {}
        for (String s : _state)
			ClassStore.getInstance().addField(_RAClassname, s);
		for (String s : _transformFunctions)
			ClassStore.getInstance().addMethod(_RAClassname, s);
	}

	@Override
	public void compileAgents() throws Exception
	{
		_runtimeAssistant = ClassStore.getInstance().compile(_RAClassname);
		_eventList.setRuntimeAssistant(_runtimeAssistant);
		Logger.getLogger(getClass()).info(String.format("Agent \"%s\" compiled", _name));
	}

	@Override
	public String toString()
	{
		StringBuilder sbuf = new StringBuilder(String.format("      Agent \"%s\" (runtime assistant = \"%s\")\n", _name, _RAClassname));
		if (_state.size() == 0)
			sbuf.append("      <state is empty>\n");
		else sbuf.append("      =======    State    =======\n");
		for (String s : _state)
			sbuf.append("      ").append(s).append("\n");
		if (_transformFunctions.size() == 0)
			sbuf.append("      <function put is empty>\n");
		else sbuf.append("      === Transform functions ===\n");
		for (String s : _transformFunctions)
			sbuf.append("      ").append(s).append("\n");
		if (_eventList.getNextEventTime() == null)
			sbuf.append("      <event list is empty>\n");
		else sbuf.append("      ======   Event list  ======\n");
		for (Entry<Double, String> e : _eventList.getEventsInfo().entrySet())
			sbuf.append("      ").append(e).append("\n");
		return sbuf.toString();
	}

	/** @return the _name */
	public String get_name()
	{
		return _name;
	}

	/** @param name the _name to put */
	public void set_name(String name)
	{
		if (name == null || name.isEmpty()) throw new NullPointerException("Agent must have a name");
		_name = name;
	}

	/** @throws Exception
	 * @see ru.tomtrix.agentsocks.mathmodel.EventList#executeNextEvent() */
	public void executeNextEvent() throws Exception
	{
		_eventList.executeNextEvent();
	}

	/** @return fes
	 * @see ru.tomtrix.agentsocks.mathmodel.EventList#getNextEventTime() */
	public Double getNextEventTime()
	{
		return _eventList.getNextEventTime();
	}

	/** @param timestamp - fes
	 * @param fid - fgs
	 * @param pars - fgrs
	 * @throws Exception
	 * @see ru.tomtrix.agentsocks.mathmodel.EventList#addEvent(double, String, Object...) */
	public void addEvent(double timestamp, String fid, Object... pars) throws Exception
	{
		_eventList.addEvent(timestamp, fid, pars);
	}

	/** @param timestamp - fsr
	 * @see ru.tomtrix.agentsocks.mathmodel.EventList#removeEvent(double) */
	public void removeEvent(double timestamp)
	{
		_eventList.removeEvent(timestamp);
	}
}
