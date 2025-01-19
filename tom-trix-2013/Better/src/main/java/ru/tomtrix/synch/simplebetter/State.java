package ru.tomtrix.synch.simplebetter;

import java.util.*;
import java.util.concurrent.ConcurrentHashMap;
import scala.Serializable;
import ru.tomtrix.synch.ApacheLogger;

/**
 * State
 */
public class State implements Serializable {

    public long fingerprint = 0;
    public Map<String, String> remoteAgents;
    public Map<String, Agent> agents;

    public State(Map<String, Agent> agents, Map<String, String> remoteAgents) {
        this.agents = new ConcurrentHashMap<>(agents);
        this.remoteAgents = new ConcurrentHashMap<>(remoteAgents);
        ApacheLogger.logger().debug("State created! " + this);
    }

    @Override
    public String toString() {
        int total = 0;
        for (Agent agent : agents.values())
            total += agent.events.size();
        return String.format("State #%d; Total events: %d", fingerprint, total);
    }
}
