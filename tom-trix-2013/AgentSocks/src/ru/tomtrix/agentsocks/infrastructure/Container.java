package ru.tomtrix.agentsocks.infrastructure;

import java.util.*;
import org.apache.log4j.Logger;
import ru.tomtrix.agentsocks.Constants;
import java.util.concurrent.ConcurrentLinkedDeque;

/** @author tom-trix */
public class Container implements IAgentProcessible
{
	/** fseef */
	private final Queue<LogicProcess>	_processes		= new ConcurrentLinkedDeque<>();
	/** concurrent troubles! */
	private Map<String, LogicProcess>	_processNames	= new TreeMap<>();
	/** wwd */
	private final int					_node;
	/** fae */
	private final int					_threads;
	/** fes */
	private boolean						_alive			= false;

	/** @param threads - gtd
	 * @param node - tgid */
	Container(int threads, int node)
	{
		if (threads < 0 || threads > Constants.MAX_THREADS) throw new IllegalArgumentException(String.format("Container can't contain %d threads", threads));
		_threads = threads;
		_node = node;
	}

	/** fsef */
	void run()
	{
		// reconstruct the map: "processname -> process"
		_processNames.clear();
		for (LogicProcess process : _processes)
			_processNames.put(process.get_name(), process);
		// create threads that will pick a logic processes from the queue and call their "next" method
		_alive = true;
		for (int i = 0; i < _threads; i++)
		{
			final String threadName = String.format("node%d.thread%d", _node, i);
			new Thread(new Runnable()
			{
				@Override
				public void run()
				{
					while (_alive)
					{
						LogicProcess process = null;
						try
						{
							Thread.sleep(40);
							process = _processes.poll();
							if (process == null) continue;
							process.nextStep();
							_processes.add(process);
						}
						catch (InterruptedException ignored) 		// done specially to avoid NullPointerException!
						{}
						catch (Exception e)
						{
							Logger.getLogger(getClass()).error(String.format("Error in a logical process \"%s\" (thread \"%s\"):", process != null ? process.get_name() : null, threadName), e);
						}
					}
				}
			}, threadName).start();
		}
	}

	/** fsef */
	void stop()
	{
		_alive = false;
	}

	/** dae
	 * @param name - bu
	 * @throws IllegalAccessException */
	public void addLogicProcess(String name) throws IllegalAccessException
	{
		if (name == null || name.trim().isEmpty()) throw new NullPointerException("Process must have a correct name");
		if (_alive) throw new IllegalAccessException("It's totally forbidden to add a new process while the container is runnung!");
		_processes.add(new LogicProcess(name));
	}

	/** fasef
	 * @param process - grui
	 * @throws IllegalAccessException */
	public void addLogicProcess(LogicProcess process) throws IllegalAccessException
	{
		if (process == null) throw new NullPointerException("Logic process can't be null");
		if (_alive) throw new IllegalAccessException("It's totally forbidden to add a new process while the container is runnung!");
		_processes.add(process);
	}

	/** vgfsr
	 * @param name - grni
	 * @throws IllegalAccessException */
	public void removeProcess(String name) throws IllegalAccessException
	{
		if (_alive) throw new IllegalAccessException("It's totally forbidden to remove processes while the container is runnung!");
		LogicProcess p = getProcessByName(name);
		if (p == null) throw new NullPointerException("There is no such a process");
		_processes.remove(p);
	}

	/** gugu
	 * @param name - fsrui
	 * @return fes */
	public LogicProcess getProcessByName(String name)
	{
		//checks
		if (name == null || name.trim().isEmpty()) throw new NullPointerException("Name parameter can't be null");
		
		//if container is running get the value only from an accessory map
		if (_alive)
			return _processNames.get(name);
		
		//otherwise it's not dangerous to get the value directly from the main queue 
		for (LogicProcess process : _processes)
			if (process.get_name().equals(name))
				return process;
		
		//there is no such an element
		throw new NoSuchElementException(String.format("Process \"%s\" is not found", name));
	}

	@Override
	public void loadCode() throws Exception
	{
		if (_alive) throw new IllegalAccessException("The operation is prohibited while the container is runnung!");
		for (IAgentProcessible process : _processes)
			process.loadCode();
	}

	@Override
	public void compileAgents() throws Exception
	{
		if (_alive) throw new IllegalAccessException("The operation is prohibited while the container is runnung!");
		for (IAgentProcessible process : _processes)
			process.compileAgents();
	}

    /** feno
     * @return the _processes
     * @throws IllegalAccessException */
    public Collection<LogicProcess> get_processes() throws IllegalAccessException
    {
        if (_alive) throw new IllegalAccessException("It's impossible to obtain the processes while the container is runnung!");
        return new LinkedList<>(_processes);
    }
}
