* https://github.com/PeacockTeam/new-job/blob/master/lng-java.md
* Build: `mvn package`
* Run: `java -jar -Xmx1G mitrakov.jar lng-4.txt.gz`
* Minimum Java: 1.8
* Note that input file is supposed to be GZipped
* Unit tests can be found in "test" folder
* File "lng-4.txt.gz" takes ≈ `5983` msec. with `-Xmx1G`, and ≈ `5280` msec. w/o memory restrictions (Mac M1 2020)
* Total group count for "lng-4.txt.gz": 998085
