Pair: nez-perce

Commit: [`9d4801`](https://github.ccs.neu.edu/CS4500-F21/nez-perce/tree/9d48014c90c50c03078df5d8c3a0cc3f933fe5a0) *(Multiple (or no) hashes found: . Using `9d4801` instead.)*

Score: 80/100

Grader: Alanna

20/20: accurate self eval

60/80:

1. 20/20 pts for `remote-proxy-player` implementation satisfying the player interface

2. 0/20 pts for unit tests of `remote-proxy-player`:

   - Does it come with unit tests for all methods
     (start, setup, pick, play, more, win, end)?

3. 20/20 pts for separating the `server` function (at least) into the following two pieces of functionality:
   - signing up enough players in at most two rounds of waiting, with a different requirement for a min number of players
   - signing up a single player: which requires three steps: connect, check name, create remote-proxy player

4. 20/20 pts for implementing `remote-proxy-manager-referee` to the manager and referee interfaces.
