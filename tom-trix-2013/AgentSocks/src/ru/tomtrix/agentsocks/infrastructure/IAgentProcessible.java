package ru.tomtrix.agentsocks.infrastructure;

/** fs
 * @author tom-trix */
public interface IAgentProcessible
{
	/** sghdr
	 * @throws Exception */
	public void loadCode() throws Exception;

	/** sge
	 * @throws Exception */
	public void compileAgents() throws Exception;
}
