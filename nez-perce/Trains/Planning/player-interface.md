# Player Interface Design

## Player-Referee Interface
### On Referee class
#### bool SignIn(IPlayer player)
A player may request to sign into the game. It returns whether the player was succesfully registered and signed in to the game. When a player is signed in, it is passed a copy of the game map and a reference to the referee that it can then use to perform actions in the game. The referee will keep track of turn order based on the "age" of the player which is the order in which they players signed in. 

#### IMap RequestMap()
A player may request for a copy of the map. This will be returned as an IMap object. The map does not change throughout the game, but the player can request this object whenever.

#### 

### On Player class

#### void RecieveGameState(PlayerGameState gameState)
A player can recieve an updated game state from the referee in between player turns. They should check the game state immediately to see if it is their turn. If it is their turn they should communicate with the referee their intended action.

#### Tuple<IDestination, IDestination> ChooseDestinations(Set<IDestination> destinations)
A player is delt 5 destination cards, the player returns the 2 destinations from the 5 that they wish to keep.

#### PlayerResponse PlayTurn(PlayerGameState gameState)
A player can recieve the current game state when it is their turn and indicate the move that it would like to perform.

#### void RecieveScore(List<Tuple<string, uint>> podium, uint position)

### PlayerResponse:
PlayerResponse is an Optional<IConnection> where the connection is what the player would like to claim or if it is null, a request for more cards.


## Discussed Assumptions
1) A Player cannot quit. They stick around and will behave by playing when it is their turn.
    * This could potentially break the game in multiple ways. Games cannot continue to run without all players and waiting
    for a player who has quit would stall the game. Additionally, it is against the player's interest to quit as they spent
    money to register for the game.
2) When handing out destinations in the setup of the game, each player in turn is delt 5 cards, before returning 3 cards.
    Which are then placed in the deck before moving on to the next player.


 
## Overview of Player-Manager Interface




