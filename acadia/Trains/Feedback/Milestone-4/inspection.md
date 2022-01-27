Pair: acadia

Commit: 1679ab85f9677557c5accb6896d4d52b22674a01

Score: 105/155

Grader: Eshwari

**10/10: accurate self eval**

**80/85: Code Inspection**

22/25: function for "determine all connections that can still be acquired" (sig, purp st, comprehensibility, >= 1 unit test)
- -3 pts: design issues
  - `AvailableConnections` is passed into the `PlayerGameState`. But, it is unclear how exactly this value is going to ever be updated. The game state has to change as the game progresses, and you do not have setters in this class, so perhaps this indicates that when you create a `Player` object later on, you are going to pass it in, or create within that class, an updated, current `PlayerGameState` either each time *a* player takes a turn or each time that player takes its turn. However, this is not documented anywhere, so I cannot tell if this was your intention. 
  - Also, for fields like `GameMap` and `Rails` you specified in comments that they are immutable but did not mention this for other fields like `AvailableConnections` (I can assume because you only have getters but still, be consistent).

40/40: function for "decide whether it is legal to acquire" (sig, purp st, comprehensibility, >= 1 unit test for true, >= 1 unit test for false)

18/20: function for "produce a player game state" (sig, purp st, comprehensibility, >= 1 unit test)
- -2 pts: design issues: 
  - You don't appear to have any clear indication of the order in which players play, which is actually **required** for `RefereeGameState`. You do have `PlayerGameStates`, but there is no comment about whether the order of the players in this set is the order of players in the game. `CurrentPlayerGameState` is part of `RefereeGameState` which is going to be tied to a `Referee`, so I don't know if you intended to have the referee itself keep track of this information? and then reset `RefereeGameState` accordingly but this should not be the case. 

**0/20: Design Inspection**: 

- 0/5 pts: a "setup" function where the referee can hand out the game pieces (like the map, rails, cards)
- 0/5 pts: a "more" function where the referee can hand out more colored cards to the player.
- 0/10 pts: there is a "play" function allowing the player to take a turn, and this function allows two distinct kinds of results: acquire-occupy and more-cards.
- It's hard to tell if there was a misunderstanding about the meaning of a player API, as you do mention that that these functions are going to be called by the referee on the player. But you also mention that "A Player conducts a turn by interacting with the Referee and passing it an updated PlayerGameState object" which makes it sound like the player is going to also be calling functions on the referee.
- Nevertheless, this functions in this API are going to be called by the referee, so things that you would need in this API are functions such as the above (setup, more, play), along with functions such as asking the player to pick some destinations and to return the remainder, and informing the player whether it won or lost the game (which you do appear to have with `disconnect`).

**15/40: `xvisualize` inspection**:
- 5/10 pts: `xvisualize` runs on server with your test case and was not boring (at a bare minimum, has double connections)
 - -3 pts: no double connections
 - -2 pts: no clear spacing between connections (green and blue get sort of combined)
 ![test](https://media.github.ccs.neu.edu/user/4987/files/2e69adf6-0daf-4543-a185-d4f6f2e10741)

- 10/30 pts: `xvisualize` runs on server with Matthias's test case
 - -10 pts: Colors are incorrect (there's supposed to be a second, blue segment going from san diego to orlando but yours doesn't render this)
 - -10 pts: missing "two connections from San Diego to Orlando, cleanly separated"

This is what yours rendered:
![mf](https://media.github.ccs.neu.edu/user/4987/files/b2e150a7-8fe7-4d5b-a9b6-bc2c12b8f719)





