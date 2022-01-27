# Player Protocol Design
## Overview of Player-Manager Interface order
### Sign-in
Tournament manager has a queue of players that have registered for the tournament. This is done by calling joinTournament(IPlayer).

Once the tournament is full or a certain time is reached the tournament starts, the manager then creates the appropriate amount of tables for the first round to fit all the players. Each table has its own ref which the manager creates along with the map and starting deck for that table. 

The ref will then setup the game, now having refrences to all the players.

Once ready the manager will call run game on the ref for each table.

Afterwards recieving back the winners from the ref, the manager removes the losers from the players in the tournament. The manager repeats this cycle with the remainin players creating more rounds until one player remains. This last player who is the winner of the last game, is the winner of the tournament.

## Overview of Player-Referee Interface method order

### Setup
When the setup phase begin, for each player, in the order of play, the referee will ask the player to select 2 destinations. This is done by calling Player.ChooseDestinations(Set<IDestination> destinations) where destinations is a set of 5 possible destinations that a player can choose from. The player will then return the two destinations that they would like to keep. The referee will validate, make note of this, and move to the next player.

When all players have chosen their destinations, the In-Progress phase begins.

### In-Progress
When the game is in progress the referee will tell each player, in the order of play, to make a move. The referee does this by calling Player.PlayTurn(PlayerGameState gameState) passing along the current game state. The player then returns a PlayerResponse which tells the referee what move the player would like to make. The referee will validate that this move is possible and make note of the player's move.

When a player finishes their move, an updated version of a PlayerGameState is passed to every signed-in player. This is done by calling Player.RecieveGameState(PlayerGameState gameState) and passing along the updated state.

The In-Progress phase is over when one of the players' inventory has fewer than 3 rail cards. The game is now over and the referee must communicate final scores to all players.

### Final Scores
When the game is over the referee gives each player the final scores and their position in the game. This is done by calling the Player.RecieveScore(List<Tuple<string, uint> podium, uint position) method. podium is an ordered list of (playerName, score) and position is the player's position on that podium. (This is to differentiate in the case that another player has the same name as you).


