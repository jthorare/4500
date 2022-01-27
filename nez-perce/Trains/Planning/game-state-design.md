## Prerequisite data structures
### GamePhase
```
Enum GamePhase {
    SignIn,     // The game is waiting for the registered players to start.
    InProgress, // The game is in progress and moves can be made
    Finished,   // The game is over and this is the final state of the game
    Cheated     // The player cheated and is booted from the game.
}
```

### PlayerInventory
```
/// All the cards a player can have
uint RedCards;
uint BlueCards;
uint GreenCards;
uint WhiteCards;
uint RailCards;
```

### PlayerGameState
```
/// Represents the current phase of the game
GamePhase Phase;

/// The number of players in the game
uint NumberOfPlayers
/// The current active player (id in order of when they joined)
uint ActivePlayer

/// The inventory of the player
Inventory PlayerInventory
/// Map each player id to the set of connections that they have claimed
IDictionary<uint, Set<IConnection>> ClaimedConnections
```

### Player Class
```
uint ID;
```
Player is used to validate that the player is who they say they are. You cannot perform actions for a player 
without the same instance of the player's Player object