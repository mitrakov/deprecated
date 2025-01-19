package ru.tomtrix.agentsocks.messaging;

import java.io.Serializable;

/** cdsfsefl
 * @author tom-trix */
public class Envelope implements Serializable
{
	/** fsefsefes */
	private static final long	serialVersionUID	= 1059235822217521090L;
	/** fsefsefs */
	private final IMessage		_data;
	/** fskpofjsop */
	private final String		_process;
	/** fapfj */
	private final String		_agent;
	/** fajpefuis */
	private final String		_sender;

	/** fafesfe
	 * @param data - grdgs
	 * @param process - gsae
	 * @param agent - fase
	 * @param sender - fse*/
	Envelope(IMessage data, String process, String agent, String sender)
	{
		if (data == null || process == null || process.trim().isEmpty() || agent == null || agent.trim().isEmpty()) throw new IllegalArgumentException("None of the parameters (except sender) may be null!");
		_process = process;
		_agent = agent;
		_data = data;
		_sender = sender;
	}

	/** @return the _process */
	String get_process()
	{
		return _process;
	}

	/** @return the _agent */
	String get_agent()
	{
		return _agent;
	}

	/** @return the _data */
	IMessage get_data()
	{
		return _data;
	}

	/** @return the _sender */
	String get_sender()
	{
		return _sender;
	}
}
