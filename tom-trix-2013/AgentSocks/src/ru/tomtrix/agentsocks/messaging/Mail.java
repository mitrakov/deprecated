package ru.tomtrix.agentsocks.messaging;

import mpi.MPI;
import java.io.*;
import ru.tomtrix.mpiagent.*;
import org.apache.log4j.Logger;
import ru.tomtrix.agentsocks.Constants;
import ru.tomtrix.agentsocks.utils.Collections;
import ru.tomtrix.agentsocks.infrastructure.Node;

/** <b>IMPORTANT! This class must be added to the Javassist Wrapper!</b>
 * @author tom-trix */
public class Mail implements MPIAgentListener
{
	/** fshiohgsrio
	 * @param data - fes
	 * @param node - gs
	 * @param process - grs
	 * @param agent - efa
	 * @param sender - fse */
	public static void send(IMessage data, int node, String process, String agent, String sender)
	{
		// checks
		if (data == null) throw new NullPointerException("Data can't be null");
		if (node < 0 || node >= MPI.COMM_WORLD.Size()) throw new ArrayIndexOutOfBoundsException(String.format("There is no such a node (%d)", node));
		if (node == MPI.COMM_WORLD.Rank()) throw new IllegalArgumentException("You can't send a message to yourself");
		if (process == null || process.trim().isEmpty()) throw new NullPointerException("Process that you're attempting to send can't be null");
		if (agent == null || agent.trim().isEmpty()) throw new NullPointerException("Agent that you're attempting to send can't be null");

		// sending
		try
		{
			MPIAgent.getInstance().send(Collections.serialize(new Envelope(data, process, agent, sender)), node);
		}
		catch (IOException e)
		{
			Logger.getLogger(Mail.class).error("Failed to send a message", e);
		}
	}

	/** fsefe */
	private final Node	_nodeRef;

	/** fsef
	 * @param node - grsd */
	public Mail(Node node)
	{
		if (node == null) throw new NullPointerException("Node can't be equal to null");
		_nodeRef = node;
	}

	/** fvsef
	 * @param bufferSize fse */
	public void startListening(int bufferSize)
	{
		if (bufferSize < Constants.MIN_BUFFER_SIZE || bufferSize > Constants.MAX_BUFFER_SIZE) throw new IllegalArgumentException(String.format("Wrong buffer size (%d byte(s))\nThe size must be between %d and %d bytes inclusively", bufferSize, Constants.MIN_BUFFER_SIZE, Constants.MAX_BUFFER_SIZE));
		MPIAgent.getInstance().start(this, bufferSize);
	}

	/** daiofheo */
	public void stopListening()
	{
		MPIAgent.getInstance().stop();
	}

	@Override
	public void messageReceived(byte data[], int sender, int tag)
	{
		try
		{
			Envelope m = (Envelope) Collections.deserialize(data);
			_nodeRef.get_container().getProcessByName(m.get_process()).getAgentByName(m.get_agent()).notifyAgent(m.get_data(), m.get_sender());
		}
		catch (Exception e)
		{
			Logger.getLogger(getClass()).error("Can't extract data from the byte array", e);
		}
	}
}
