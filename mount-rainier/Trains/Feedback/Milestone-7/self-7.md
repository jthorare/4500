## Self-Evaluation Form for Milestone 7

Please respond to the following items with

1. the item in your `todo` file that addresses the points below.

2. a link to a git commit (or set of commits) and/or git diffs the resolve
   bugs/implement rewrites: 

It is possible that you had "perfect" data definitions/interpretations
(purpose statement, unit tests, etc) and/or responded to feedback in a
timely manner. In that case, explain why you didn't have to add this
to your `todo` list.

These questions are taken from the rubric and represent some of 
critical elements of the project, though by no means all of them.

If there is anything special about any of these aspects below, you may also point to your `reworked.md` and/or `bugs.md` files. 

### Game Map 

- a proper data definition with an _interpretation_ for the game _map_
https://github.ccs.neu.edu/CS4500-F21/mount-rainier/blob/52681a0c863997d1cc1887322ff50274a023fd8f/Trains/src/Trains/Common/Map/TrainsMap.cs
line 14, 15

### Game States 

- a proper data definition and an _interpretation_ for the player game state
https://github.ccs.neu.edu/CS4500-F21/mount-rainier/blob/52681a0c863997d1cc1887322ff50274a023fd8f/Trains/src/Trains/Common/GameState/PlayerGameState.cs
line 7

- a purpose statement for the "legality" functionality on states and connections
https://github.ccs.neu.edu/CS4500-F21/mount-rainier/blob/52681a0c863997d1cc1887322ff50274a023fd8f/Trains/src/Trains/Common/GameState/PlayerGameState.cs
line 48

- at least _two_ unit tests for the "legality" functionality on states and connections 
No tests

### Referee and Scoring a Game

The functionality for computing scores consists of 4 distinct pieces of functionality:

  - awarding players for the connections they connected
  https://github.ccs.neu.edu/CS4500-F21/mount-rainier/blob/52681a0c863997d1cc1887322ff50274a023fd8f/Trains/src/Trains/Admin/RefereeGameState.cs
  line 248

  - awarding players for destinations connected
  https://github.ccs.neu.edu/CS4500-F21/mount-rainier/blob/52681a0c863997d1cc1887322ff50274a023fd8f/Trains/src/Trains/Admin/RefereeGameState.cs
  line 249

  - awarding players for constructing the longest path(s)
  https://github.ccs.neu.edu/CS4500-F21/mount-rainier/blob/52681a0c863997d1cc1887322ff50274a023fd8f/Trains/src/Trains/Admin/RefereeGameState.cs
  line 244

  - ranking the players based on their scores 
  https://github.ccs.neu.edu/CS4500-F21/mount-rainier/blob/52681a0c863997d1cc1887322ff50274a023fd8f/Trains/src/Trains/Admin/RefereeGameState.cs
  line 259

Point to the following for each of the above: 

  - piece of functionality separated out as a method/function:
  Longest path - https://github.ccs.neu.edu/CS4500-F21/mount-rainier/blob/52681a0c863997d1cc1887322ff50274a023fd8f/Trains/src/Trains/Admin/RefereeGameState.cs
				line 200
  GetScore - https://github.ccs.neu.edu/CS4500-F21/mount-rainier/blob/52681a0c863997d1cc1887322ff50274a023fd8f/Trains/src/Trains/Admin/RefereeGameState.cs
				line 246
  - a unit test per functionality
  Longest path - https://github.ccs.neu.edu/CS4500-F21/mount-rainier/blob/52681a0c863997d1cc1887322ff50274a023fd8f/Trains/src/Trains.Tests.Admin/RefereeTest.cs
				line 104
  GetScore - https://github.ccs.neu.edu/CS4500-F21/mount-rainier/blob/52681a0c863997d1cc1887322ff50274a023fd8f/Trains/src/Trains.Tests.Admin/RefereeTest.cs
				line 94
  Rankings - https://github.ccs.neu.edu/CS4500-F21/mount-rainier/blob/52681a0c863997d1cc1887322ff50274a023fd8f/Trains/src/Trains.Tests.Admin/RefereeTest.cs
				line 86


### Bonus

Explain your favorite "debt removal" action via a paragraph with
supporting evidence (i.e. citations to git commit links, todo, `bug.md`
and/or `reworked.md`).
We took inspiration from Matthias' implementation suggestion on abstracting the Strategy's should take in a function/method call in order to abstract away the details of the function away.
Using that, we wanted to find a way to have a single point of control for wrapping Player calls and ensure that they handled kicking for misbehavior or lack of behavior appropriately. We
implemented this via the Action class and using lambdas to take away the Player call's arguments and details out of the wrapping. This means that even if we add another method to the IPlayer,
we can still use this wrapper call to handle the Player appropriately.
https://github.ccs.neu.edu/CS4500-F21/mount-rainier/blob/52681a0c863997d1cc1887322ff50274a023fd8f/Trains/Planning/todo.md
3rd checkbox

https://github.ccs.neu.edu/CS4500-F21/mount-rainier/blob/52681a0c863997d1cc1887322ff50274a023fd8f/Trains/src/Trains/Admin/Referee.cs
line 263
