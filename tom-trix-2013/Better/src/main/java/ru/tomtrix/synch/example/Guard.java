package ru.tomtrix.synch.example;

import java.util.*;
import ru.tomtrix.synch.structures.*;
import ru.tomtrix.synch.simplebetter.*;

@SuppressWarnings("unused")
public class Guard extends Agent {

    public Guard(String name) {
        super(name);
    }

    @Override
    public Collection<TimeEvent> init() {
        return Arrays.asList(new TimeEvent(5, new AgentEvent(name, "SuperMarket", "setVariable").withData("Door#open")),
                             new TimeEvent(15 + rand(15), new AgentEvent(name, "Cashier2", "requestToSmoke")),
                             new TimeEvent(65 + rand(9), new AgentEvent(name, name, "goWC")));
    }

    public Collection<TimeEvent> responseToSmoke(TimeEvent event) {
        List<TimeEvent> events = new ArrayList<>(Arrays.asList(new TimeEvent(event.t() + 15 + rand(15), new AgentEvent(name, "Cashier2", "requestToSmoke"))));
        if (event.event().userdata().equals("true") || rand())
            events.addAll(Arrays.asList(
                new TimeEvent(event.t() + rand(1), new AgentEvent(name, "SuperMarket", "setVariable").withData("GuardAvailable#false")),
                new TimeEvent(event.t() + 2 + rand(7), new AgentEvent(name, name, "goBack"))));
        return events;
    }

    public Collection<TimeEvent> goWC(TimeEvent event) {
        return Arrays.asList(new TimeEvent(event.t() + rand(1), new AgentEvent(name, "SuperMarket", "setVariable").withData("GuardAvailable#false")),
                             new TimeEvent(event.t() + 1 + rand(3), new AgentEvent(name, name, "goBack")));
    }

    public Collection<TimeEvent> goBack(TimeEvent event) {
        return Arrays.asList(new TimeEvent(event.t(), new AgentEvent(name, "SuperMarket", "setVariable").withData("GuardAvailable#true")));
    }

    public Collection<TimeEvent> suspect(TimeEvent event) {
        return Collections.emptyList();
    }
}
