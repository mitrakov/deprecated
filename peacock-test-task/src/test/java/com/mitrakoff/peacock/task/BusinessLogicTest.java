package com.mitrakoff.peacock.task;

import org.junit.jupiter.api.Test;
import java.util.*;
import static org.junit.jupiter.api.Assertions.*;

public class BusinessLogicTest {
    @Test
    public void test() {
        final BusinessLogic p = new BusinessLogic();
        p.addRow(new Row(new String[] {"12","16","11","13","21","32","44"}));
        p.addRow(new Row(new String[] {"17","15","22","13","27"}));
        p.addRow(new Row(new String[] {"11","16","12","14"}));
        p.addRow(new Row(new String[] {"13","21","41"}));
        p.addRow(new Row(new String[] {"18","22","21"}));
        p.addRow(new Row(new String[] {"31","35","38"}));
        p.addRow(new Row(new String[] {"19","21","51"}));
        p.addRow(new Row(new String[] {"14","19","21"}));
        p.addRow(new Row(new String[] {"15","22","13","15","26","32"}));
        p.addRow(new Row(new String[] {"18","16","29","16"}));
        p.addRow(new Row(new String[] {"18","22","21"}));

        final List<Set<Row>> result = p.getGroupedData();

        assertEquals(3, result.size());
        assertEquals(7, result.get(0).size());
        assertEquals(2, result.get(1).size());
        assertEquals(1, result.get(2).size());
        assertEquals(new HashSet<>(Arrays.asList(new Row(new String[] {"13","21","41"}), new Row(new String[] {"19","21","51"}))), result.get(1));
    }

    @Test
    public void testWithEmptyValues() {
        final BusinessLogic p = new BusinessLogic();
        p.addRow(new Row(new String[] {"15","29","39", ""}));
        p.addRow(new Row(new String[] {"12","15","11","27", ""}));
        p.addRow(new Row(new String[] {"31","25","18","28","30"}));
        p.addRow(new Row(new String[] {"41","42","43","44","41","42"}));
        p.addRow(new Row(new String[] {"26","26","26","26","26","26","26"}));
        p.addRow(new Row(new String[] {"12","16","11","38","30","11","28","14"}));
        p.addRow(new Row(new String[] {"10","12","31", ""}));
        p.addRow(new Row(new String[] {"14","42","26","51", ""}));
        p.addRow(new Row(new String[] {"25","13","40","27","11","42","21","19"}));
        p.addRow(new Row(new String[] {"11","27","31","10", ""}));

        final List<Set<Row>> result = p.getGroupedData();

        assertEquals(3, result.size());
        assertEquals(7, result.get(0).size());
        assertEquals(2, result.get(1).size());
        assertEquals(1, result.get(2).size());
        assertEquals(new HashSet<>(Arrays.asList(new Row(new String[] {"10","12","31", ""}), new Row(new String[] {"11","27","31","10", ""}))), result.get(1));
    }
}
