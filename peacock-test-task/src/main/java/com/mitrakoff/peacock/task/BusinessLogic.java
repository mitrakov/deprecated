package com.mitrakoff.peacock.task;

import java.util.*;
import java.util.stream.Collectors;

/**
 * Main business logic.
 * Algorithm uses a DB approach:
 * 1) there is a "table" which simulates a DB table, and keeps sets of rows for each group
 * 2) there is an "index" for that "table" which simulates N indexes for each column of the table
 */
public final class BusinessLogic {
    private final Map<GroupId, Set<Row>> table = new HashMap<>();
    private final List<Map<String, GroupId>> index = new ArrayList<>();

    /**
     * Adds a new row to DB
     */
    public void addRow(Row row) {
        final GroupId groupId = addToIndex(row);
        addToTable(groupId, row);
    }

    /**
     * @return a list of grouped data, sorted by size of group, in DESC order
     */
    public List<Set<Row>> getGroupedData() {
        return table
            .values()
            .stream()
            .sorted(Comparator.comparingInt((Set<Row> e) -> e.size()).reversed())
            .collect(Collectors.toList());
    }

    private GroupId addToIndex(Row row) {
        // if a new row is too long => create new buckets for index
        ensureIndexSize(row.size());

        // find the collisions of a new row with what exists in index
        final List<GroupId> collisions = new ArrayList<>();
        for (int i = 0; i < row.size(); i++) {
            final String t = row.get(i);
            final Map<String, GroupId> bucket = index.get(i);
            final GroupId grpId = bucket.get(t);
            if (grpId != null)
                collisions.add(grpId);
        }

        // rebalance index depending on collisions count
        final GroupId groupId;
        switch (collisions.size()) {
            case 0: { // no match in index => create a new group
                groupId = new GroupId();
                break;
            }
            case 1: { // single match in index => add row to the existing group
                groupId = collisions.get(0);
                break;
            }
            default: { // 2 or more groups => merge
                groupId = collisions.remove(0);
                for (GroupId group : collisions) {
                    final Set<Row> rows = table.remove(group);
                    if (rows != null)
                        table.merge(groupId, rows, (o, n) -> {o.addAll(n); return o;});
                }
            }
        }

        putToIndex(groupId, row);
        return groupId;
    }

    private void ensureIndexSize(int size) {
        while (index.size() < size)
            index.add(new HashMap<>());
    }

    private void putToIndex(GroupId group, Row row) {
        for (int i = 0; i < row.size(); i++) {
            final String s = row.get(i);
            if (!s.isEmpty()) { // empty values should not go to index
                final Map<String, GroupId> bucket = index.get(i);
                bucket.put(s, group);
            }
        }
    }

    private void addToTable(GroupId group, Row row) {
        if (!table.containsKey(group))
            table.put(group, new HashSet<>());
        table.get(group).add(row);
    }

    private static class GroupId{} // artificial group ID, for internal usage
}
