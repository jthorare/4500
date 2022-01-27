Pair: mount-rainier

Commit: [`a02086`](https://github.ccs.neu.edu/CS4500-F21/mount-rainier/tree/a02086a0c4c2a52dc05f723eea8196b4f0a3a2a3) *(Referenced URLs are not permalinks. Using `a02086` instead.)*

Score: 120/155

Grader: Chukwurah Somtoo


20/20: accurate self eval
 - Point to the functions next time.

`cheat strategy`

2/15 

- 0/10 proper design: (derive (extend) the BuyNow class and override the turn method) you extend the abstract strategy class 
- 2/5: a unit test that makes sure that the requested acquisition is not on the map. You didnt do this. 

`manager`

78/100 

The manager performs five totally distinct tasks:

- 10/10 pts: inform players of the beginning of the tournament, retrieve maps
- 10/20 pts: check that there are maps with enough destinations and pick one of those for the games 
	- 10/10pts: it doesn't matter how the manager picks a "good" map 
	- 0/10 pts:  the **same** predicate is used in both the `manager` and the `referee` (not copied code) - Looks like predicate was copied
- 10/10 pts: allocating players to a bunch of games per round
- 30/30 pts: running the tournament, separate two functions
 	- 15/15: run a round of games
	- 15/15: run all rounds	
- 10/10 pts: inform surviving players at the very end whether they won the tournament

The points are for the existence of separate methods/functions _and_ well-chosen
names and/or purpose statements.

- 8/20 points for tests of the manager as a whole:
  - 4/10 for testing a single game (they know what the ref does, the manager must compute the same result (same inputs))
    - acknowledged in self eval this test is not present => 40%
  - 4/10 for testing the allocation of players to games per round

`remote.md`

20/20 

- 5/5 points for diagrams that mention for the exact same scenarios as in
  - [Logical Interactions](https://www.ccs.neu.edu/home/matthias/4500-f21/local_protocol.html)
  - [Logical Interactions 2](https://www.ccs.neu.edu/home/matthias/4500-f21/manager_protocol.html)

- 5/5 points for JSON format definitions, for each call and return in these diagrams

- 10/10 a "helpful" English explanation (as in helps you navigate their description)

