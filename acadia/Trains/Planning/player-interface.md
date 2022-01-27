## Player API Design

+ A Player conducts a turn by interacting with the Referee and passing it an updated PlayerGameState object. The Referee will only interact with the Player to say it is currently that
	Player's turn, disconnect the Player, or tell the Player it is the last round of the Trains.com game.

#### GOAL 

The player components must communicate with the referee, which manages and arbitrates the game. This communication involves both function/method calls
and orderings of those. (Recall this is called a protocol.) Since outsiders will create AI players to this interface, it must be spelled out precisely 
and in detail. Hint To start your brainstorming�what is a software system? What are the key phases of the game? An API is a documentation of a logical interface 
to a code library and its calling conventions. Design the API for a player component. The document, named player-interface.PPmd should be a module in your
language whose body is a wish list (signatures, interpretations, purpose statements).


#### Wish List
+ void SetUp(Map map, int rails, IDictionary<ColoredCard, int> cards) - Sets up the Player with the basic game pieces needed to play; map, rails, cards.
	+ map - the Map the Trains.com game is being played on
	+ rails - the initial number of rails
	+ cards - mapping of ColoredCard to the number of cards of that color; contains four total cards.

+ ICollection<Destination> ChooseDestinations(ICollection<Destination> destinations) - Returns 2 Destination from the given Collection of Destination.
	+ destinations - The Collection of Destination to choose this Player's Destination from
	+ -> Returns the Two Destination this Player chose

+ ITurn PlayTurn(PlayerGameState pgs) - Allows a Player to play a turn
	+ pgs - A PlayerGameState representing this Player's current status in the game
	+ -> Returns the ITurn representing this Player's Turn behavior

+ void ReceiveMoreCards(ICollection<ColoredCard> moreCards) - Deals the Player more cards after a PlayTurn call returns a DrawCardsTurn

+ void Disconnect(bool won) - Disconnects the Player from the Game
	+ won - bool that is true if the Player won the game or false otherwise