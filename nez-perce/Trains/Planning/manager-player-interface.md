# Manager-Player Interface
## Protocol Overview
Tournament manager has a queue of players that have registered for the tournament. Players can enter the queue by calling joinTournament(IPlayer).

At any time a player can probe the Tournament manager for information such as prize money.

Once the tournament queue is full or a certain time is reached, the tournament starts. The manager informs the player that a game is about to begin by calling __ on each player in the game. The manager then creates the appropriate amount of games for the first round to fit all the players. Each game has its own ref which the manager creates along with the map and starting deck for that game. 


The ref will then setup the game, now having refrences to all the players.

Once ready the manager will call run game on the ref for each table.

Afterwards recieving back the winners from the ref, the manager removes the losers from the players in the tournament. The manager repeats this cycle with the remainin players creating more rounds until one player remains. This last player who is the winner of the last game, is the winner of the tournament.


## Tournament-Manager Interface methods
### bool joinTournament(Player player)
Player registers for a tournament by passing itself as a reference. Returns if the player has entered the queue for the tournament.

### float GetPrize()
Returns the amount of prize money that a player would win should they win the tournament

## New Player Interface methods
### void PrepareForGame(int round)
Informs the player that a game is about to be played by telling it which round of the tournament is about to begin for them.
