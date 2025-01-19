package ru.tomtrix.agentsocks.messaging;

/** esfioe
 * @author tom-trix */
public class StateChanged implements IMessage
{
	/** fsefe */
	private static final long	serialVersionUID	= -705717442418397054L;
	/** fsefes */
	private final String		_variable;
	/** dahjoi */
	private final Object		_value;

	/** fmjos
	 * @param variable gerg
	 * @param value rw */
	public StateChanged(String variable, Object value)
	{
		_variable = variable;
		_value = value;
	}

	/** @return the _variable */
	public String get_variable()
	{
		return _variable;
	}

	/** @return the _value */
	public Object get_value()
	{
		return _value;
	}
}
