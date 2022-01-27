## Tournament Manager Design
+ TournamentManager(int numberOfEliminationBrackets) - Constructs a Tournament Manager where a Player can lose numberOfEliminationBrackets number of times before being eliminated from the Tournament
+ IList<IDictionary<IPlayer,int>> PlayTournament(IList<IPlayer> playersInTournament) - Plays a tournament
  + Returns: IList<IDictionary<IPlayer,int>> representing The results of each game in the tournament where index 0 is the first game played and Ranking.Count - 1 index is the Final game

#### PLAYER API EXPANSION
+ void LossesToBeEliminated(int losses) - Tells the Player how many losses are possible in this tournament before they are eliminated
  + losses - How many games can the Player win before they are eliminated. 1 means that if the Player loses a single game, they are eliminated.
+ void WonTournament(bool win) - Tells the Player if they won the tournament
  + win - If the Player won the tournament (true) or not (false)


### REFEREE/MANAGER -> PLAYER PROTOCOL

#### TOURNAMENT SETUP

The Tournament Setup phase informs each Player of how many losses they are permitted before being eliminated from
the tournament according to the tournament style.


    Starting a Tournament
    manager                                                                      player (p_1) . . . player (p_n)
          |                                                                       |                 |
          |   LossesToBeEliminated(int losses)                                    |                 | % player receives:
          | --------------------------------------------------------------------> |                 | % - losses possible before elimination
          .                                                                       .                 .
          .                                                                       .                 . % repeat down age
          .                                                                       .                 .
          |   LossesToBeEliminated(int losses)                                    |                 |
          | --------------------------------------------------------------------------------------->|

### For every game in the tournament:

#### GAME SETUP

The Game Setup phase sets up each Player with their initial Destination, ColoredCard, and rails.

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

#### GAME TURNS

The Game Turn phase consists of the Referee communicating with the Players and updating their information to carry out the game and complete turns.

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

#### GAME CLOSING

The Game Closing phase encompasses disconnecting a Player from the game.

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

#### PLAYER-MANAGER PROTOCOL

		  
    Ending a Tournament
          |   WonTournament(bool win)                                             |                 | % player receives:
          | --------------------------------------------------------------------> |                 | % - true means "winner"
          |                                                                       |                 | % - false means "loser"
          .                                                                       .                 .
          .                                                                       .                 . % repeat down age
          .                                                                       .                 .
          |   WonTournament(bool win)                                             |                 |
          | --------------------------------------------------------------------------------------->|


#### CONVENTION

+ The % parts on the right are interpretive comments. ~~
+ ------> are calls
+ <====== are returns
+ A missing return arrow means that method must successfully return void.

#### TERMINATION OF INTERACTIONS

+ An interaction between the referee and any player is discontinued if the player
    + breaks the rules (a "business logic" bug)
    + raises an exception (a safety bug)
    + takes too long for a computation (a DoS bug).
+ We do not worry about a player that exploits shared-memory allocation of data representation. These terminations are not specified in the sequence diagram.
+ Naturally, if a referee has to eliminate the last player, the game is over.