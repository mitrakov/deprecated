package ru.tomtrix.agentsocks.mathmodel;

import java.util.*;
import java.util.Map.Entry;
import java.util.concurrent.ConcurrentHashMap;

/** fvsdfvsejop
 * @author tom-trix */
public class EventList
{
	/** map containing functions & their timestamps */
	private final Map<Double, Function>	_eventList	= new ConcurrentHashMap<>();
	/** reference to transform functions */
	private Object						_RAref;

	/** adds new function to the event list
	 * @param timestamp - time the function to be invoked
	 * @param fid - function id
	 * @param pars - parameters for functions
	 * @throws Exception - if something goes wrong */
	void addEvent(double timestamp, String fid, Object... pars) throws Exception
	{
		if (timestamp < 0) throw new IllegalArgumentException("Timestamp can't be negative");
		if (fid == null || fid.trim().isEmpty()) throw new IllegalArgumentException("Function id can't be empty");
		_eventList.put(timestamp, new Function(fid, pars));
	}

	/** fseiofeshjoisefoo;h
	 * @param timestamp - gdrg */
	synchronized void removeEvent(double timestamp)
	{
		if (!_eventList.containsKey(timestamp)) throw new NoSuchElementException(String.format("There are no events planned on t=%f", timestamp));
		_eventList.remove(timestamp);
	}

	/** fseiofeshsfe
	 * @param fid - grfsd */
	synchronized void removeEventByFid(String fid)
	{
		List<Double> rmv = new LinkedList<>();
		for (Entry<Double, Function> e : _eventList.entrySet())
			if (e.getValue().get_fid().equals(fid)) rmv.add(e.getKey());
		for (Double d : rmv)
			_eventList.remove(d);
	}

	/** @throws Exception */
	void executeNextEvent() throws Exception
	{
		if (_RAref == null) throw new NullPointerException("Runtime Assistant is not assigned. Perhaps the agent hasn't been compiled");
		Function f;
		// this must be synchronized cause someone could remove the item while the other finds the minimum
		synchronized (this)
		{
            Double d = Collections.min(_eventList.keySet()); 	// TODO сортировка коллекции является bottleneck'ом
			f = _eventList.get(d);
			_eventList.remove(d);
		}
		f.execute(_RAref);
	}

	/** @param runtimeAssistant - gd */
	void setRuntimeAssistant(Object runtimeAssistant)
	{
		if (runtimeAssistant == null) throw new NullPointerException("Runtime Assistant can't be equal to null");
		_RAref = runtimeAssistant;
	}

	/** @return timestamp of the nearest event in the event list (or <b>NULL</b> if there are no events presented) */
	synchronized Double getNextEventTime()
	{
		// this must be synchronized cause someone could remove the item while the other finds the minimum
		if (_eventList.isEmpty()) return null;
		return Collections.min(_eventList.keySet()); 			// TODO сортировка коллекции является bottleneck'ом
	}

	synchronized Map<Double, String> getEventsInfo()
	{
		Map<Double, String> result = new TreeMap<>();
		for (Entry<Double, Function> e : _eventList.entrySet())
			result.put(e.getKey(), e.getValue().get_fid());
		return result;
	}
}
