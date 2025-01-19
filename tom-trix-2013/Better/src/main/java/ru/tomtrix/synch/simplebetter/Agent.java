package ru.tomtrix.synch.simplebetter;

import java.util.*;

import ru.tomtrix.synch.structures.TimeEvent;
import scala.Serializable;

/**
 * Agent
 */
public abstract class Agent implements Serializable {

    private static final Random _random = new Random(System.currentTimeMillis());

    public static boolean rand() {
        return _random.nextBoolean();
    }

    public static float rand(int n) {
        return _random.nextInt(n) + _random.nextFloat();
    }

    protected final String name;
    List<TimeEvent> events = Collections.synchronizedList(new ArrayList<TimeEvent>());

    public Agent(String name) {
        this.name = name;
    }

    abstract protected Collection<TimeEvent> init();

    synchronized void addEvents(Collection<TimeEvent> events) {
        this.events.addAll(events);
        Collections.sort(this.events);
    }

    synchronized Float getCurrentTimestamp() {
        return events.isEmpty() ? null : events.get(0).t();
    }

    synchronized TimeEvent popEvent() {
        return events.remove(0);
    }

    synchronized void flush() {
        events.clear();
        addEvents(init());
    }
}
