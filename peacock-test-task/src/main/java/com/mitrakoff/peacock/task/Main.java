package com.mitrakoff.peacock.task;

import java.io.*;
import java.nio.file.*;
import java.util.*;
import java.util.zip.GZIPInputStream;

/**
 * https://github.com/PeacockTeam/new-job/blob/master/lng-java.md
 * Build: `mvn package`
 * Run: `java -jar -Xmx1G mitrakov.jar lng-4.txt.gz`
 * Minimum Java: 1.8
 * Note that input file is supposed to be GZipped
 * Unit tests can be found in "test" folder
 * File "lng-4.txt.gz" takes ≈ 5983 msec. with -Xmx1G, and ≈ 5280 msec. w/o memory restrictions (Mac M1 2020)
 * Total group count for "lng-4.txt.gz": 998085
 */
public class Main {
    public static void main(String[] args) {
        // checking args
        final long start = System.currentTimeMillis();
        if (args.length < 1) {
            System.err.println("Usage: java -jar mitrakov.jar filename");
            System.exit(1);
        }

        // reading file
        final BusinessLogic program = new BusinessLogic();
        try (final BufferedReader reader = new BufferedReader(new InputStreamReader(new GZIPInputStream(Files.newInputStream(Paths.get(args[0])))))) {
            System.out.printf("Working on file %s...\n", args[0]);
            String line;
            while ((line = reader.readLine()) != null) {
                final String[] row = Arrays.stream(line.split(";")).map(s -> s.replaceAll("^\"|\"$", "")).toArray(String[]::new);
                program.addRow(new Row(row));
            }
        } catch (IOException e) {
            System.err.printf("Error reading file: %s\n", e.getMessage());
            System.exit(1);
        }

        // printing results
        final List<Set<Row>> result = program.getGroupedData();
        System.out.printf("Total group count = %d\n", result.size());
        System.out.printf("Group 1 contains %d item(s)\n", result.size() > 0 ? result.get(0).size() : 0);
        System.out.printf("Group 2 contains %d item(s)\n", result.size() > 1 ? result.get(1).size() : 0);
        System.out.printf("Group 3 contains %d item(s)\n", result.size() > 2 ? result.get(2).size() : 0);

        // writing to file
        try (final BufferedWriter writer = new BufferedWriter(new FileWriter(String.format("%s.out", args[0])))) {
            writer.write(String.format("%d\n", result.size()));
            for (int i = 0; i < result.size(); i++) {
                writer.write(String.format("Группа %d\n", i + 1));
                final Set<Row> set = result.get(i);
                for (Row row : set) {
                    writer.write(row.toString());
                    writer.newLine();
                }
                writer.newLine();
            }
        } catch (IOException e) {
            System.err.printf("Error writing file: %s\n", e.getMessage());
            System.exit(1);
        }

        // checking elapsed time
        final long end = System.currentTimeMillis();
        System.out.printf("Elapsed time = %d milliseconds\n", end - start);
    }
}
