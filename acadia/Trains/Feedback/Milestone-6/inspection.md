Pair: acadia

Commit: [`a34fd9`](https://github.ccs.neu.edu/CS4500-F21/acadia/tree/a34fd9d05970aaf4b32f2990f38039e1eedca445) 

Score: 187/215

Grader: Manan

30/30: accurate self-eval

player.PP and referee.PP

147/170

player.PP

50/50 points: All required methods present with meaningful purpose statement.

 
referee.PP

97/120

30/30 points for purpose statements and signatures of the major functions (start--up, play-turns, shut-down)

40/60 points scoring functions 
- 12/20  Scoring Segments 
   - 3/5: The purpose statement needs to be in english. Don't use symbols like ':' as much as possible.
   - 5/5: A seperate method
   - 4/10: Would be a 0 for testing, but you were honest about it in your selfeval.
- 14/20   Scoring longest path 
   - 5/5: The purpose statement
   - 5/5: A seperate method
   - 4/10: Would be a 0 for testing, but you were honest about it in your selfeval.
- 14/20   Scoring destination
   - 5/5: The purpose statement
   - 5/5: A seperate method
   - 4/10: Would be a 0 for testing, but you were honest about it in your selfeval.

10/10 points for the ranking function (existence, sig/purp)

17/20 points for dealing with cheating/failing players:
 - 7/10 pts: a single-point of control functionality for calling a player's functions/methods.
   - I see you have made different methods for checking timeout. While this is better then having it in the same method which has the Business logic, an ideal     solution would be to have a single method which takes care of such errors.
 - 10/10 pts: a check that the desired action is legal. Note The function may just indicate things went wrong (see #false) or directly eliminate the player. Both choices are fine.



10/15 - manager-player-interface.md

 - 0/5 points for a purpose statement for the unit itself.
 - 10/10 points for identifying that the manager should tell players:
the result of the tournament (you won/lost the big prize) at the very end of the sequence
 - 0/5 BONUS points for suggesting a "the tournament has started" method
