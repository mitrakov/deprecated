package ru.tomtrix.agentsocks.messaging;

import org.apache.log4j.Logger;
import ru.tomtrix.agentsocks.infrastructure.Container;
import ru.tomtrix.agentsocks.infrastructure.LogicProcess;
import ru.tomtrix.agentsocks.infrastructure.Node;
import ru.tomtrix.agentsocks.mathmodel.Agent;

/** vsohjfs
 * @author tom-trix */
public class LocalMail
{
	/** saeopjfoe */
	private static Node	_nodeRef;

	/** fosehfuise
	 * @param node - fase */
	public LocalMail(Node node)
	{
		if (node == null) throw new NullPointerException("Node can't be equal to null");
		_nodeRef = node;
	}

	/** osfiohnefo
	 * @param data - fes
	 * @param process - fea
	 * @param agent - gnbdrik
	 * @param sender - gold
	 * @throws Exception */
	public static void send(IMessage data, String process, String agent, String sender) throws Exception
	{
		// checks
		if (data == null) throw new NullPointerException("Data can't be null");
		if (process == null || process.trim().isEmpty()) throw new NullPointerException("Process that you're attempting to send can't be null");
		if (agent == null || agent.trim().isEmpty()) throw new NullPointerException("Agent that you're attempting to send can't be null");

		// sending
		try
		{
			Container c = _nodeRef.get_container();
			LogicProcess p = c.getProcessByName(process);
			Agent a = p.getAgentByName(agent);
			a.notifyAgent(data, sender);
		}
		catch (Exception e)
		{
			Logger.getLogger(Mail.class).error("Failed to send a local message", e);
		}
	}
}
