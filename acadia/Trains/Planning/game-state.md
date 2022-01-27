## Referee-Player Game State

#### Data Context

A Card is represented as a single class that represents a single playing card. It possesses the following property:
+ A GamePieceColor object representing the color of the card.

A RequestType is one of: requesting cards, obtaining a connection. It is represented as an enumeration.
```
Enum RequestType {
	RequestCards=1,
	AcquireConnection=2
}
```

A PlayerAction is represented as a single class that represents the action that a Player takes on their turn: either requesting Cards, or obtaining a Connection.
It possesses the following properties:
+ A Player object representing the player who is requesting to take an action.
+ A Connection object representing the Connection that the player is potentially trying to obtain.
+ A RequestType object representing the type of action the Player is requesting to take.

#### Game State Class

The game state of a _Trains.com_ game is represented as a single class that is used by both the Players and the Referee.
It possesses the following three properties:
+ An immutable, readonly Map object representing the game's map.
+ An immutable, readonly mapping of Players to a HashSet of their owned Connections.
	+ Dictionary\<Player, IEnumerable\<Connection\>\>
+ The immutable, readonly number of remaining colored cards available for players, represented as an int within [0, 200].
+ A mutable PlayerAction, representing a Player's requested action on their turn

The Players and the Referee both need access to all four properties during the game; the players for their ability to play the game unimpeded with all necessary information,
and the Referee to maintain legality of the game state. This shared need for the same game state information makes a unified game state for both Players and Referees useful.
And with only the PlayerAction being mutable, the Referee can ensure its legality via comparison against the last legal GameState.

#### Wish List

+ Context
	+ Players have a GameState that a Player will update with their requested PlayerAction on their turn.
	+ The Referee keeps track of the last legal GameState (current snapshot of the game).
	+ The referee updates all Players' GameStates after a legal turn.

+ Referee Actions
	+ SetUpGame
		+ Map IEnumerable\<Player\> int -> GameState
		+ Sets up the game by handing each player the total number of participants, a map for the game, that is, the map of places and connections, four randomly chosen colored cards,
		and the specified number of rails. Returns the initial GameState.
		+ (define (SetUpGame map players railAmount) ...)
	+ CheckLegality
		+ GameState -> boolean
		+ Returns whether the game state is valid and legal.
			+ If the Player's action is requesting a Connection, the action is valid if the Connection is unowned, the player has enough rails according to the requested Connection's length,
			and the Player has the number of matching colored cards to the Connection.
			+ If the given GameState differs from the Referee's last legal GameState other than its PlayerAction property, then the GameState is illegal.
			+ If a Player updates their GameState to have a PlayerAction corresponding to a different Player, then it is illegal.
		+ (define (CheckLegality gs) ...)
	+ ExecutePlayerAction
		+ GameState -> 
		+ Updates the game's last legal GameState to be the given Player's legal GameState.
		+ (define (ExecutePlayerAction gs) ...)
	+ CheckLastTurn
		+  -> Boolean
		+ Returns whether the Referee's last legal GameState indicates that the last turn should start. Once a Player's number of rails drop to 2, 1, or 0 at the end of a turn,
		each of the remaining players get to take one more turn.
		+ (define (CheckLastTurn) ...)
+ Player Actions
	+ RequestAction
		+ PlayerAction -> 
		+ Updates the Player's GameState property with a new PlayerAction
		+ (define (RequestAction pa) ...)
