## Self-Evaluation Form for Milestone 6

Indicate below each bullet which file/unit takes care of each task:

1. In the player component, identify the following pieces of functionality:

  - `setup`, the function/method for receiving the game map etc.     
https://github.ccs.neu.edu/CS4500-F21/acadia/blob/a34fd9d05970aaf4b32f2990f38039e1eedca445/Trains/Other/Source/Models/GameEntities/Player.cs#L61-L66
  - `pick`, the function/method for picking destinations from given alternatives     
https://github.ccs.neu.edu/CS4500-F21/acadia/blob/a34fd9d05970aaf4b32f2990f38039e1eedca445/Trains/Other/Source/Models/GameEntities/Player.cs#L76-L80
  - `play`, the function/method for taking a turn    
https://github.ccs.neu.edu/CS4500-F21/acadia/blob/a34fd9d05970aaf4b32f2990f38039e1eedca445/Trains/Other/Source/Models/GameEntities/Player.cs#L82-L85
  - `more_cards`, the function/method for receiving more cards (if available)    
https://github.ccs.neu.edu/CS4500-F21/acadia/blob/a34fd9d05970aaf4b32f2990f38039e1eedca445/Trains/Other/Source/Models/GameEntities/Player.cs#L68-L74
  - `win`, the function/method for receiving information about the outcome of the game    
https://github.ccs.neu.edu/CS4500-F21/acadia/blob/a34fd9d05970aaf4b32f2990f38039e1eedca445/Trains/Other/Source/Models/GameEntities/Player.cs#L87


2. In the referee component, identify the following pieces of major functionality:

  - a start-up phase, i.e., for setting up players with maps and destinations     
The set-up phase within our Referee.PlayGame method: https://github.ccs.neu.edu/CS4500-F21/acadia/blob/a34fd9d05970aaf4b32f2990f38039e1eedca445/Trains/Other/Source/Models/GameEntities/Referee.cs#L62-L64     
Referee.SetUpPlayers method itself: https://github.ccs.neu.edu/CS4500-F21/acadia/blob/a34fd9d05970aaf4b32f2990f38039e1eedca445/Trains/Other/Source/Models/GameEntities/Referee.cs#L509-L532      
Referee.SetPlayerDestinations method itself: https://github.ccs.neu.edu/CS4500-F21/acadia/blob/a34fd9d05970aaf4b32f2990f38039e1eedca445/Trains/Other/Source/Models/GameEntities/Referee.cs#L534-L551      
  - a play-turns phase, i.e., for running the game proper      
https://github.ccs.neu.edu/CS4500-F21/acadia/blob/a34fd9d05970aaf4b32f2990f38039e1eedca445/Trains/Other/Source/Models/GameEntities/Referee.cs#L293-L318
  - a shut down phase, i.e., for informing players of the outcome      
Compute score and inform players of outcome within Referee.PlayGame: https://github.ccs.neu.edu/CS4500-F21/acadia/blob/a34fd9d05970aaf4b32f2990f38039e1eedca445/Trains/Other/Source/Models/GameEntities/Referee.cs#L68-L69     
Referee.Score method itself: https://github.ccs.neu.edu/CS4500-F21/acadia/blob/a34fd9d05970aaf4b32f2990f38039e1eedca445/Trains/Other/Source/Models/GameEntities/Referee.cs#L99-L117     
Referee.InformPlayersFinalStatus method itself: https://github.ccs.neu.edu/CS4500-F21/acadia/blob/a34fd9d05970aaf4b32f2990f38039e1eedca445/Trains/Other/Source/Models/GameEntities/Referee.cs#L81-L97

3. In the referee component, identify the following pieces of scoring functionality and their unit tests: 

  - the functionality for granting points for segments per connection      
https://github.ccs.neu.edu/CS4500-F21/acadia/blob/a34fd9d05970aaf4b32f2990f38039e1eedca445/Trains/Other/Source/Models/GameEntities/Referee.cs#L142-L162     
No unit tests.
  - the functionality for granting points for longest path     
The below methods get called here in our Referee.Score method: https://github.ccs.neu.edu/CS4500-F21/acadia/blob/a34fd9d05970aaf4b32f2990f38039e1eedca445/Trains/Other/Source/Models/GameEntities/Referee.cs#L111      
No unit tests.      
Method that returns a list of players with the longest path: https://github.ccs.neu.edu/CS4500-F21/acadia/blob/a34fd9d05970aaf4b32f2990f38039e1eedca445/Trains/Other/Source/Models/GameEntities/Referee.cs#L164-L182      
No unit tests.     
Method that actually grants the points: https://github.ccs.neu.edu/CS4500-F21/acadia/blob/a34fd9d05970aaf4b32f2990f38039e1eedca445/Trains/Other/Source/Models/GameEntities/Referee.cs#L119-L140     
No unit tests.     
  - the functionality for granting points for destinations connected      
https://github.ccs.neu.edu/CS4500-F21/acadia/blob/a34fd9d05970aaf4b32f2990f38039e1eedca445/Trains/Other/Source/Models/GameEntities/Referee.cs#L184-L201     
No unit tests.

4. In the referee component, identify the functionality for ranking players     
https://github.ccs.neu.edu/CS4500-F21/acadia/blob/a34fd9d05970aaf4b32f2990f38039e1eedca445/Trains/Other/Source/Models/GameEntities/Referee.cs#L99-L117

5. In the referee component, identify the functionality for eliminating misbehaving players      
https://github.ccs.neu.edu/CS4500-F21/acadia/blob/a34fd9d05970aaf4b32f2990f38039e1eedca445/Trains/Other/Source/Models/GameEntities/Referee.cs#L478-L487

The ideal feedback for each of these three points is a GitHub
perma-link to the range of lines in a specific file or a collection of
files.

A lesser alternative is to specify paths to files and, if files are
longer than a laptop screen, positions within files are appropriate
responses.

You may wish to add a sentence that explains how you think the
specified code snippets answer the request.

If you did *not* realize these pieces of functionality, say so.
