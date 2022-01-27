#### Logical Interaction
###### Manager Player Interactions

    The arrows connecting Proxy to Client players is indicating what the Server is doing with the TCPClient.GetStream() stream
    manager                   (Proxy) player (p_1) . . . player (p_n)               (Client) player (p_1) . . . player (p_n)
      |                                |                 |                                                   |                 | % for n <= MAX_PLAYERS
      |                                |                 |                                                   |                 |
      |                                |                 |                                                   |                 |
      |                                |                 |                                                   |                 |  
      |     start(Boolean)             |                 |  NetworkStream.Write(byte[], 0, byte[].Length)    |                 | % true means the tournament
      | -----------------------------> |                 | ----------------------------------------------->  |                 | % is about to start
      |     Map                        |                 |   NetworkStream.Read(Span<byte>)                  |                 |
      | <============================  |                 | <===============================================  |                 |  % player submits a map
      .                                .                 .                                                   |                 |  % in response
      .                                .                 .
      .                                .                 .
      |     start(Boolean)             |                 |  NetworkStream.Write(byte[], 0, byte[].Length)    |                 | 
      | ---------------------------------------------------------------------------------------------------------------------> |
      |     Map                        |                 |   NetworkStream.Read(Span<byte>)                  |                 |
      | <===================================================================================================================== |  

    manager                   (Proxy) player (p_1) . . . player (p_n)               (Client) player (p_1) . . . player (p_n)
      |                                |                 |                                                   |                 | % for n <= MAX_PLAYERS
      |                                |                 |                                                   |                 |
      |     end(Boolean)               |                 |  NetworkStream.Write(byte[], 0, byte[].Length)    |                 | % true means "winner"
      | -----------------------------> |                 | ----------------------------------------------->  |                 | % false means "loser"
      |                                |                 |                                                   |                 |
      |                                |                 |                                                   |                 |
      |                                |                 |                                                   |                 |
      |                                |                 |                                                   |                 |
      |     end(Boolean)               |                 |  NetworkStream.Write(byte[], 0, byte[].Length)    |                 | 
      | ---------------------------------------------------------------------------------------------------------------------> |

###### Referee-Player Interactions
     
    referee                      (Proxy) player (p_1) . . . player (p_n)                         (Client)   player (p_1) . . . player (p_n)
      |                                |                 |                                                   |                 |
      |     setup(map,r,cards)         |                 |  NetworkStream.Write(byte[], 0, byte[].Length)    |                 | % the map for this game
      | -----------------------------> |                 |  -------------------------------------------->    |                 | % the number of rails
      |                                |                 |                                                   |                 |
      |     pick(destinations[])       |                 |  NetworkStream.Write(byte[], 0, byte[].Length)    |                 | % given these 5 destinations,
      | -----------------------------> |                 |  -------------------------------------------->    |                 | % where does the player
      |     destinations[]             |                 |   NetworkStream.Read(Span<byte>)                  |                 | % want to go (return 3)
      | <============================= |                 |  <============================================    |                 |
      |                                |                 |                                                   |                 |
      .                                .                 .                                                   .                 .
      .                                .                 .                                                   .                 . % repeat down age
      .                                .                 .                                                   .                 .
      |                                |                 |                                                   |                 |
      |     setup(map,r,cards)         |                 |  NetworkStream.Write(byte[], 0, byte[].Length)    |                 | 
      | ---------------------------------------------------------------------------------------------------------------------> |
      |                                |                 |                                                   |                 |
      |                                |                 |                                                   |                 |
      |     pick(destinations[])       |                 |  NetworkStream.Write(byte[], 0, byte[].Length)    |                 | 
      | ---------------------------------------------------------------------------------------------------------------------> |    
      |                                |                 |
      |     destinations[]             |                 |   NetworkStream.Read(Span<byte>)                  |                 |
      | <===================================================================================================================== | 

Playing Turns

       referee                      (Proxy) player (p_1) . . . player (p_n)                         (Client)   player (p_1) . . . player (p_n)
          |                                |                 |                                                   |                 |
          |   play(state)                  |                 |  NetworkStream.Write(byte[], 0, byte[].Length)    |                 | % - current state 
          | -----------------------------> |                 |  -------------------------------------------->    |                 |
    action 1:                              |                 |                                                   |                 |
          |     more_cards                 |                 |   NetworkStream.Read(Span<byte>)                  |                 |  % request cards
          | <============================  |                 | <==============================================   |                 |
          |     more(cards[])              |                 |  NetworkStream.Write(byte[], 0, byte[].Length)    |                 |
          | -----------------------------> |                 |  -------------------------------------------->    |                 | % if there are cards
          |                                |                 |
          |                                |                 | % no cards available     
    action 2:
          |     Connection                 |                 |   NetworkStream.Read(Span<byte>)                  |                 |
      +-- | <============================  |                 | <==============================================   |                 | % acquire connection
      |   .                                .                 . % if legal:
      |   .                                .                 . % referee modifies game state
      +-> .                                .                 . % otherwise:
          .                                .                 . % kick player out
          .                                .                 .                                                   .                 .
          |   play(state)                  |                 |
          | -----------------------------------------------> | ------------------------------------------------------------------> |
          |     action                     |                 |
          | <=============================================== | <=================================================================> |
          |                                |                 |                                                   |                 |
          .                                .                 .                                                   .                 .
          .                                .                 .                                                   .                 . play until end condition:
          .                                .                 .                                                   .                 .
          .                                .                 .                                                   .                 . % When one of the player’s number of
          .                                .                 .                                                   .                 . % rails drops to 2, 1, or 0 at the  
          .                                .                 .                                                   .                 . % end of a turn, each of the all    
          .                                .                 .                                                   .                 . % other remaining players get to    
          .                                .                 .                                                   .                 . % take one more turn.               
          .                                .                 .                                                   .                 .
          .                                .                 .                                                   .                 . % The game also ends if every       
          .                                .                 .                                                   .                 . % remaining player has had an       
          .                                .                 .                                                   .                 . % opportunity to play a turn and the
          .                                .                 .                                                   .                 .
          |   play(state)                  |                 |  NetworkStream.Write(byte[], 0, byte[].Length)    |                 | % - current state 
          |  ----------------------------------------------> |  -----------------------------------------------> |                 |
          |     action                     |                 |  NetworkStream.Read(Span<byte>)                   |                 |
          | <=============================================== | <===============================================  |                 |
          |                                |                 |                                                   |                 |
          .                                .                 .                                                   .                 .
          .                                .                 .                                                   .                 .
          |   play(state)                  |                 |   NetworkStream.Write(byte[], 0, byte[].Length)   |                 |
          | -----------------------------------------------> | ------------------------------------------------------------------> |
          |     action                     |                 |  NetworkStream.Read(Span<byte>)                   
          | <=============================================== | <=================================================================> |
          .                                .                 .
          .                                .                 .
          .                                .                 .


Scoring a Game

       referee                      (Proxy) player (p_1) . . . player (p_n)                         (Client)   player (p_1) . . . player (p_n)
      |                                |                 |                                                   |                 |
      |                                |                 |
      |     win(Boolean)               |                 |  NetworkStream.Write(byte[], 0, byte[].Length)    |                 | 
      | ------------------------------------------------------------------------------------------------->   |                 |% true means "winner"
      |                                |                 |                                                   |                 |% false means "loser"
      .                                .                 .                                                   .                 .
      .                                .                 .                                                   .                 .
      .                                .                 .                                                   .                 .
      .                                .                 .                                                   .                 .
      |   win(Boolean)                 |                 |   NetworkStream.Write(byte[], 0, byte[].Length)   |                 |
      | -----------------------------------------------> | ------------------------------------------------------------------> |
      |                                |                 |                                                   |                 |
      |                                |                 |                                                   |                 |

###### These are the formats for the JSON string that will occupy the byte[] or the Span<byte> of the NetworkStream method call. From the given Referee call.

+ The following are JSON objects that will be referenced below. Their definitions are linked.
+ Map : Map defined in the [spec](https://www.ccs.neu.edu/home/matthias/4500-f21/3.html#%28tech._map%29)
+ Color: Color defined in the [spec](https://www.ccs.neu.edu/home/matthias/4500-f21/trains.html#%28tech._color%29)
+ Acquired : none //none for more cards
				or Acquired defined in the [spec](https://www.ccs.neu.edu/home/matthias/4500-f21/5.html#%28tech._acquired%29) //To claim a connection.
+ Destination defined in the  [spec](https://www.ccs.neu.edu/home/matthias/4500-f21/5.html#%28tech._destination%29)
+ ThisPlayer defined in the [spec](https://www.ccs.neu.edu/home/matthias/4500-f21/5.html#%28tech._thisplayer%29)
+ Action defined in the [spec](https://www.ccs.neu.edu/home/matthias/4500-f21/6.html)

###### The following says what side of each call will read/write to/from the NetworkStream in a JSON converted to a byte[] using UTF-8 encoding.
+ Manager calling Player.Start() : Boolean
+ Result of Manager calling Player.Start() : Map
+ Manager calling Player.End() : Boolean
+ Referee calling Player.Setup() : {"Map" : Map,
		+ "rails" : Number,
		+ "Cards" : [Colors,..,..]}
+ Referee calling Player.Pick() : A JSON array of JSON Destination
+ Result of Referee calling Player.Pick() : A JSON array of JSON Destination
+ Referee calling Player.Play() : ThisPlayer
+ action : Action
+ Referee calling Player.Win() : Boolean