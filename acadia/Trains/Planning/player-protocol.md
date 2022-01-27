## Player Protocol Design

A Player operates in 3 distinct phases. They are as follows: Setup, Turn, and Closing.     
Each function is further explained in player-interface.md.     
Updated to reflect the [example UML formatting](https://www.ccs.neu.edu/home/matthias/4500-f21/6.html).

#### SETUP

The Setup phase sets up each Player with their initial Destination, ColoredCard, and rails.

    referee                                                                                     player (p_1) . . . player (p_n)
          |                                                                                     |                 |
          |                                                                                     |                 |
          |     SetUp(Map map, int rails, Dictionary<ColoredCard, int> cards)                   |                 | % the map for this game, the number of rails
          | ----------------------------------------------------------------------------------> |                 | % a set of four random cards
          |                                                                                     |                 |
          |     PickDestinations(ICollection<Destination> destinations)                         |                 | % given these 5 destinations,
          | ----------------------------------------------------------------------------------> |                 | % where does the player want to go
          |     ICollection<Destination> destinations                                           |                 | % return 3 not chosen destinations
          | <================================================================================== |                 |
          |                                                                                     |                 |
          .                                                                                     .                 .
          .                                                                                     .                 . % repeat down age
          .                                                                                     .                 .
          |                                                                                     |                 |
          |     SetUp(Map map, int rails, IDictionary<ColoredCard, int> cards)                  |                 |
          | ----------------------------------------------------------------------------------> |                 |
          |                                                                                     |                 |
          |     PickDestinations(ICollection<Destination> destinations)                         |                 |
          | ----------------------------------------------------------------------------------> |                 |
          |     ICollection<Destination> destinations                                           |                 |
          | <================================================================================== |                 |
          |                                                                                     |                 |

#### TURNS

The Turn phase consists of the Referee communicating with the Players and updating their information to carry out the game and complete turns.

    referee                                                                      player (p_1) . . . player (p_n)
          |                                                                       |                 |
          |   PlayTurn(PlayerGameState state)                                     |                 | % player receives:
          | --------------------------------------------------------------------> |                 | % - current state            

    action 1:
          |     DrawCardsTurn                                                     |                 |
          | <===================================================================  |                 | % request cards
          |     GiveMoreCards(IDictionary<ColoredCard, int> moreCards)            |                 |
          | --------------------------------------------------------------------> |                 | % if there are cards
          |                                                                       |                 |
          |                                                                       |                 | % if there are no cards available     
          .                                                                       .                 .
    action 2:
          |     AcquireConnectionTurn                                             |                 | % acquire connection
      +-- | <===================================================================  |                 |
      |   .                                                                       .                 . % if legal:
      |   .                                                                       .                 . % referee modifies game state
      +-> .                                                                       .                 . % otherwise:
          .                                                                       .                 . % kick player out
          .                                                                       .                 .
          |   PlayTurn(PlayerGameState state)                                     |                 |
          | --------------------------------------------------------------------------------------> |
          |   ITurn action                                                        |                 |
          | <====================================================================================== |
          |                                                                       |                 |
          .                                                                       .                 .
          .                                                                       .                 . % repeat until a player
          .                                                                       .                 . % has less than 3 rails
          .                                                                       .                 . % or all remaining
          .                                                                       .                 . % players have chosen
          .                                                                       .                 . % to ask for more cards
          .                                                                       .                 . % when there are none
          .                                                                       .                 .
          |                                                                       |                 | % one last round
          |   PlayTurn(PlayerGameState state)                                     |                 |
          | --------------------------------------------------------------------> |                 |
          |   ITurn action                                                        |                 |
          | <===================================================================  |                 |
          .                                                                       .                 .
          .                                                                       .                 .
          .                                                                       .                 .
          .                                                                       .                 .
          |   PlayTurn(PlayerGameState state)                                     |                 |
          | --------------------------------------------------------------------> |                 |
          |   ITurn action                                                        |                 |
          | <==================================================================== |                 |
          |                                                                       |                 |
          .                                                                       .                 .
          .                                                                       .                 .

#### CLOSING

The Closing phase encompasses disconnecting a Player from the game.

    referee                                     player (p_1) . . . player (p_n)
          |                                        |                |
          |                                        |                |
          |       GameOver(bool won)               |                |
          | -------------------------------------> |                | % true means "winner"
          |                                        |                | % false means "loser"
          .                                        .                .
          .                                        .                .
          .                                        .                .
          .                                        .                .
          |       GameOver(bool won)               |                |
          | ------------------------------------------------------> |
          |                                        |                |
          |                                        |                |

#### Convention

+ The % parts on the right are interpretive comments. ~~
+ ------> are calls
+ <====== are returns
+ A missing return arrow means that method must successfully return void.

#### Termination of Interactions

+ An interaction between the referee and any player is discontinued if the player
    + breaks the rules (a "business logic" bug)
    + raises an exception (a safety bug)
    + takes too long for a computation (a DoS bug).
+ We do not worry about a player that exploits shared-memory allocation of data representation. These terminations are not specified in the sequence diagram.
+ Naturally, if a referee has to eliminate the last player, the game is over.