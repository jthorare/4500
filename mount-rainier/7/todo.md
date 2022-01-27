#### Todo
+ [X] Create CL test runner & Xtest script
+ [X] Referee needs to end the game if, after a complete cycle of turns, there are no changes.
+ [X] Single point of Player based error handling.
+ [x] There are magic constants in the code that should be constants we can easily change if the spec changes J
+ [X] The A* algorithm gets called multiple times in GetRankings()
+ [x] Unit Tests
  +  [x] Hold10Strategy.ChooseDestination()
  +  [x] BuyNow.PlayTurn()
  +  [x] More tests PlayerGameState.CanClaim() at least 2 for eachtrue and false
  +  [x] Visualization 
    +  [x] needs to handle offsetting lines
    +  [x] city name where city is on edge of map
    +  [x] connection
+ [x] App cancellation token impl

+  [x] Add/Update Documentation
   +  [x] Referee
   +  [x] RefereeGameState
   +  [X] PlayerGameState
   +  [X] Deck
   +  [X] PlayerInventory
   +  [X] TrainMap

+ [X] Player
    + [X] Add Player Constructor that takes in filepath and loads a Dynamic Strategy

