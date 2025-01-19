package ru.tomtrix.mpiagent;

/** Proposes classes to implement an MPIAgent listener functionality
 * @author tom-trix */
public interface MPIAgentListener
{
	/** Invokes as soon as a new message received
	 * @param data */
	public void messageReceived(byte data[], int sender, int tag);
}
