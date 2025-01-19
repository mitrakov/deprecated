package ru.tomtrix.synch.simplebetter;

import java.util.*;
import java.util.concurrent.ConcurrentHashMap;
import ru.tomtrix.synch.structures.*;

/**
 * Environment
 */
@SuppressWarnings("unused")
public abstract class Environment extends Agent {

    protected Map<String, Object> _variables = new ConcurrentHashMap<>();

    public Environment(String name) {
        super(name);
    }

    public Collection<TimeEvent> setVariable(TimeEvent event) {
        String arg = event.event().userdata().toString();
        _variables.put(arg.split("#")[0], arg.split("#")[1]);
        return Collections.emptyList();
    }

    public Collection<TimeEvent> checkVariable(TimeEvent event) {
        String arg = event.event().userdata().toString();
        return Arrays.asList(new TimeEvent(event.t(), new AgentEvent(name, event.event().agens(), "variableIs").withData(String.format("%s#%s", arg, _variables.get(arg)))));
    }

    public Collection<TimeEvent> incVariable(TimeEvent event) {
        String arg = event.event().userdata().toString();
        _variables.put(arg, (int) _variables.get(arg) + 1);
        return Collections.emptyList();
    }

    public Collection<TimeEvent> decVariable(TimeEvent event) {
        String arg = event.event().userdata().toString();
        _variables.put(arg, (int) _variables.get(arg) - 1);
        return Collections.emptyList();
    }
}
