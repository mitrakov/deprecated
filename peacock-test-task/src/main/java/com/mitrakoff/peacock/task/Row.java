package com.mitrakoff.peacock.task;

import java.util.Arrays;

/**
 * Row is a parsed internal representation of a single line, without quotes ("") and without ";".
 * Row contains N substring components, and may include empty items.
 * Row re-implements equals() and hashCode(), so that `row1 == row2` if they are internally equal.
 * Use .toString() to render the Row back to original text view, with quotes and ";"
 */
public class Row {
    private final String[] data;

    public Row(String[] row) {
        this.data = row;
    }

    public String get(int i) {
        return data[i];
    }

    public int size() {
        return data.length;
    }

    @Override
    public String toString() {
        final int iMax = data.length - 1;
        if (iMax == -1) return "";

        final StringBuilder b = new StringBuilder();
        for (int i = 0; ; i++) {
            b.append("\"").append(data[i]).append("\"");
            if (i == iMax) return b.toString();
            b.append(";");
        }
    }

    @Override
    public boolean equals(Object o) {
        if (o == null || getClass() != o.getClass()) return false;
        return Arrays.equals(data, ((Row) o).data);
    }

    @Override
    public int hashCode() {
        return Arrays.hashCode(data);
    }
}
