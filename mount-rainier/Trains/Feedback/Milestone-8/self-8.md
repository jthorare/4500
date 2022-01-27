## Self-Evaluation Form for Milestone 8

Indicate below each bullet which file/unit takes care of each task.

The `manager` performs five completely distinct tasks, with one
closely related sub-task. Point to each of them:  

1. inform players of the beginning of the game, retrieve maps
https://github.ccs.neu.edu/CS4500-F21/mount-rainier/blob/master/Trains/src/Trains/Admin/Manager.cs

2. pick a map with enough destinations
	- including the predicate that decides "enough destinations"
Manager.ChooseMap()
https://github.ccs.neu.edu/CS4500-F21/mount-rainier/blob/master/Trains/src/Trains/Admin/Manager.cs

3. allocating players to a bunch of games per round
Manager.AssignPlayersToGame()
https://github.ccs.neu.edu/CS4500-F21/mount-rainier/blob/master/Trains/src/Trains/Admin/Manager.cs

4. run the tournament and its two major pieces of functionality:
   - run a  round of games
   - run all rounds, discover termination conditions
Manager.RunTournament()
https://github.ccs.neu.edu/CS4500-F21/mount-rainier/blob/master/Trains/src/Trains/Admin/Manager.cs
Manager.PlayTournamentRound()
https://github.ccs.neu.edu/CS4500-F21/mount-rainier/blob/master/Trains/src/Trains/Admin/Manager.cs

5. inform survining players at the very end whether they won the tournament
Manager.SendWinLoss()
https://github.ccs.neu.edu/CS4500-F21/mount-rainier/blob/master/Trains/src/Trains/Admin/Manager.cs

Next point to unit tests for:

- testing the `manager` on the same inputs as the `referee`, because
  you know the outcome
We do not have this unit test.

- testing the allocation of players to the games of one round
We do not have this unit test.

Finally, the specification of the `cheat` strategy says "like BuyNow",
which suggests (F II) to derive (`extend`) the base class or re-use some
functionality:

- point to the cheat strategy and how it partially reusess existing code
https://github.ccs.neu.edu/CS4500-F21/mount-rainier/blob/master/Trains/src/Trains/Player/Strategy/CheatStrategy.cs
Extends AbstractStrategy, overrides PlayTurn.

- point to a unit test that makes sure the requested acquisition is impossible
We do not have that specific unit test, but we do know that CheatPlayers are removed correctly by our TournamentManager tests.

The ideal feedback for each of these three points is a GitHub
perma-link to the range of lines in a specific file or a collection of
files.

A lesser alternative is to specify paths to files and, if files are
longer than a laptop screen, positions within files are appropriate
responses.

You may wish to add a sentence that explains how you think the
specified code snippets answer the request.

If you did *not* realize these pieces of functionality, say so