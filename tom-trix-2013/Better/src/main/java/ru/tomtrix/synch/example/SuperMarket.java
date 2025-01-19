package ru.tomtrix.synch.example;

import java.util.*;
import ru.tomtrix.synch.simplebetter.*;
import ru.tomtrix.synch.structures.TimeEvent;

/**
 * Environment
 */
@SuppressWarnings("unused")
public class SuperMarket extends Environment {
    public SuperMarket(String name) {
        super(name);
    }

    @Override
    public Collection<TimeEvent> init() {
        _variables.put("Door", "closed");
        _variables.put("TotalCash", 0);
        _variables.put("TotalCashless", 0);
        _variables.put("TotalPurchasers", 0);
        _variables.put("Purchasers", 0);
        _variables.put("Thefts", 0);
        _variables.put("GuardAvailable", true);
        _variables.put("Cashier1isAvailable", true);
        _variables.put("Cashier2isAvailable", true);
        return Collections.emptyList();
    }
}
