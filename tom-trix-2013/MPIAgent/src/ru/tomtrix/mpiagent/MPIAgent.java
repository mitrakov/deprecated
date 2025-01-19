package ru.tomtrix.mpiagent;

import mpi.MPI;
import mpi.Status;
import java.util.*;
import java.util.concurrent.ConcurrentLinkedDeque;

/** Represents the work of 2 threads: one sends a byte array to a specific node using the message queue; the other waits at a blocking receive method for incoming messages
 * @author tom-trix */
public class MPIAgent
{
	/** Singleton instance */
	private static MPIAgent	_mpi	= new MPIAgent();

	/** Singleton private constructor */
	private MPIAgent()
	{}

	/** @return singleton instance */
	public static MPIAgent getInstance()
	{
		return _mpi;
	}

	/** Subordinate class incapsulating byte array data and a destination node
	 * @author tom-trix */
	private class Message
	{
		/** Data */
		public byte[]	data;
		/** Number of a node (starting with 0) */
		public int		destination;

		/** Creates new message for a node with number <b>destination</b>
		 * @param data
		 * @param destination */
		public Message(byte[] data, int destination)
		{
			this.data = data;
			this.destination = destination;
		}
	}

	/** Message buffer */
	private final Queue<Message>	_queue	= new ConcurrentLinkedDeque<>();
	private boolean					_alive	= true;

	/** Starts the MPIAgent service (creates 2 threads: one sends a byte array to a specific node using the message queue; the other waits at a blocking receive method for incoming messages)
	 * @param listener
	 * @param bufferSize */
	public void start(final MPIAgentListener listener, final int bufferSize)
	{
		_alive = true;
		// listening thread
		new Thread(new Runnable()
		{
			@Override
			public void run()
			{
				while (_alive)
				{
					try
					{
						byte[] buf = new byte[bufferSize];
						Status s = MPI.COMM_WORLD.Recv(buf, 0, bufferSize, MPI.BYTE, MPI.ANY_SOURCE, MPI.ANY_TAG);
						listener.messageReceived(Arrays.copyOf(buf, s.count), s.source, s.tag);
					}
					catch (Exception e)
					{
						e.printStackTrace();
					}
				}
			}
		}).start();
		// sending thread
		new Thread(new Runnable()
		{
			@Override
			public void run()
			{
				while (_alive)
				{
					try
					{
						Thread.sleep(25);
						if (_queue.isEmpty()) continue;
						Message m = _queue.poll();
						MPI.COMM_WORLD.Send(m.data, 0, m.data.length, MPI.BYTE, m.destination, 0);
					}
					catch (Exception e)
					{
						e.printStackTrace();
					}
				}
			}
		}).start();
	}

	/** Stops the MPIAgent service and releases the working threads */
	public void stop()
	{
		_alive = false;
	}

	/** Sends a specific byte array to a specific node
	 * @param data
	 * @param destination */
	public void send(byte[] data, int destination)
	{
		if (data == null || data.length == 0) throw new NullPointerException("Empty messages are not allowed");
		if (destination < 0 || destination >= MPI.COMM_WORLD.Size()) throw new IllegalArgumentException(String.format("Wrong number of a destination node: %d", destination));
		if (destination == MPI.COMM_WORLD.Rank()) throw new IllegalArgumentException(String.format("You're attempting to send a message to yourself (%d)", destination));
		_queue.add(new Message(data, destination));
	}
}
