package ru.tomtrix.agentsocks.infrastructure;

import java.util.Collection;
import ru.tomtrix.agentsocks.Constants;

import ru.tomtrix.agentsocks.messaging.LocalMail;
import ru.tomtrix.agentsocks.messaging.Mail;

/** @author tom-trix */
public class Node
{
	/** fse */
	private final Container	_container;
	/** dawfse */
	private final Mail		_mail;
	private final int		_rank;

	/** fs */
	Node(int rank, boolean isDebug)
	{
		_rank = rank;
		_container = new Container(Constants.DEFAULT_THREADS, rank);
		_mail = isDebug ? null : new Mail(this);
		new LocalMail(this);
	}

	/** @return the _container */
	public Container get_container()
	{
		return _container;
	}

	/** fsefs
	 * @throws Exception
	 * @see ru.tomtrix.agentsocks.messaging.Mail#startListening(int) */
	public void run() throws Exception
	{
		if (_mail != null) _mail.startListening(Constants.BUFFER_SIZE);
		_container.run();
	}

	@Override
	public String toString()
	{
		StringBuilder sbuf = new StringBuilder(String.format("Node %d. Container has the following processes:\n", _rank));
		try
		{
			Collection<LogicProcess> processes = _container.get_processes();
			if (processes.size() == 0) sbuf.append("<no processes>\n");
			for (LogicProcess process : processes)
				sbuf.append(process);
		}
		catch (IllegalAccessException e)
		{
			sbuf.append("Information is unavailable cause the container is running...\n");
		}
		return sbuf.toString();
	}

	/** @return the _rank */
	public int get_rank()
	{
		return _rank;
	}
}
