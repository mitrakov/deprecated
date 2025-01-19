package ru.tomtrix.agentsocks.mathmodel;

import java.lang.reflect.Method;

/** fseqrwer
 * @author tom-trix */
public class Function
{
	/** function id */
	private final String	_fid;
	/** parameters for function invoking */
	private final Object[]	_parameters;

	/** creates a new function call object
	 * @param fid - function ID
	 * @param parameters - parameters for invoking */
	Function(String fid, Object... parameters)
	{
		if (fid == null || fid.trim().isEmpty()) throw new NullPointerException("Function id can't be null");
		_fid = fid;
		_parameters = parameters;
	}

	/** fgsef
	 * @param methodKeeper - gfsdr
	 * @return fs
	 * @throws Exception */
	Object execute(Object methodKeeper) throws Exception
	{
		// checks
		if (methodKeeper == null) throw new NullPointerException("Method keeper can't be null");

		// for each parameter obtain its class
		Class<?>[] classes = new Class<?>[_parameters.length];
		for (int i = 0; i < _parameters.length; i++)
			classes[i] = _parameters[i].getClass();

		// trying to find a method via Java Reflection API
		Method m = methodKeeper.getClass().getMethod(_fid, classes);
		if (m == null) throw new NullPointerException(String.format("There are no such a method that corresponds to fid \"%s\" and specified %d parameter(s)", _fid, _parameters.length));

		// execute one
		return m.invoke(methodKeeper, _parameters);
	}

	/**
	 * @return the _fid
	 */
	String get_fid()
	{
		return _fid;
	}
}
