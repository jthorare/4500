## Project Structure
+ acadia
	+ 3 :
		+ xmap : test harness for milestone 3 that consumes on STDIN a 
		[JSON input containing a Map and two City names](https://www.ccs.neu.edu/home/matthias/4500-f21/3.html); writes to STDOUT
		whether there is a connection between the two cities indicated by the passed city names. **run xmap by redirecting JSON input
		into a `./xmap` call from the acadia/3/ directory. ex: `./xmap < Tests/1-in.json` should write to STDOUT the value that can be found in 1-out.json**
		+ Tests : directory that holds test cases for `xmap` each made up of a pair: an input n-in.json and output n-out.json, where n is an integer between 1 and the number of tests
	+ 4 :
		+ xvisualize : visualization harness for milestone 4 that consumes on STDIN a JSON input containing a [Map](https://www.ccs.neu.edu/home/matthias/4500-f21/3.html#%28tech._map%29) object;
		pops up a window that visualizes the given Map, and closes it after 10s. **run it from acadia/Trains/4/ using `./xvisualize < Vis/1-in.json`, or provide it a different valid, well-formed JSON input.**
		+ Vis : directory that holds tests ([Map](https://www.ccs.neu.edu/home/matthias/4500-f21/3.html#%28tech._map%29) JSON inputs) for `xvisualize`
	+ 5 :
		+ xlegal : test harness for milestone 5 that consumes on STDIN a JSON input containing [Map](https://www.ccs.neu.edu/home/matthias/4500-f21/3.html#%28tech._map%29),
		[PlayerState](https://www.ccs.neu.edu/home/matthias/4500-f21/5.html#%28tech._playerstate%29), and [Acquired](https://www.ccs.neu.edu/home/matthias/4500-f21/5.html#%28tech._acquired%29)
		objects; writes to STDOUT whether the requested action is legal according to the rules with respect to the given map and state.
		**run xlegal by redirecting JSON input into a `./xlegal` call from the acadia/5/ directory. ex: `./xlegal < Tests/1-in.json` should write to STDOUT the value that can be found in 1-out.json**
		+ Tests : directory that holds test cases for `xlegal` each made up of a pair: an input n-in.json and output n-out.json, where n is an integer between 1 and the number of tests
	+ 6 : 
		+ xstrategy : test harness for milestone 6 that consumes on STDIN a JSON input containing [Map](https://www.ccs.neu.edu/home/matthias/4500-f21/3.html#%28tech._map%29)
		and [PlayerState](https://www.ccs.neu.edu/home/matthias/4500-f21/5.html#%28tech._playerstate%29) objects; writes to STDOUT a JSON 
		[Action](https://www.ccs.neu.edu/home/matthias/4500-f21/6.html#%28tech._action%29) object corresponding to the given PlayerState's Strategy's expected Action in the context of the given Map.
		**run xstrategy by redirecting JSON input into a `./xstrategy` call from the acadia/6/ directory. ex: `./xstrategy < Tests/1-in.json` should write to STDOUT the Action that can be found in 1-out.json**
		+ Tests : directory that holds test cases for `xstrategy` each made up of a pair: an input n-in.json and output n-out.json, where n is an integer between 1 and the number of tests
	+ B : exploratory assignment xhead
	+ C : exploratory assignment xjson
	+ D : exploratory assignment xgui
	+ E : exploratory assignment xtcp
	+ Trains
		+ Admin :
			+ link-to-referee.txt
			+ link-to-referee-game-state.txt
		+ Common : 
			+ link-to-map.txt
			+ link-to-player-game-state.txt
		+ Editor : 
			+ link-to-map-editor.txt
		+ Other : directory that holds all auxiliary files used in development
			+ Source : directory that holds all source code
				+ App.axaml & App.axaml.cs : corresponding files used by Avalonia to set-up and launch the Avalonia application
				+ map-editor.cs : class that is used for map visualization (milestone 3 programming task)
				+ Models : directory that holds our data representations used in Trains.com and the business logic involving them
					+ GameEntities : directory that holds our data repreentations for the entities that take part in Trains.com (player, referee, tournament manager)
					+ GamePieces : directory that holds our data representations for game pieces used in the Trains.com game and the business logic involving them
					+ GameStates : directory that holds our data representations for the player and referee game states used in the Trains.com game and the business logic involving them
					+ Strategies : directory that holds our data representations for different strategies used by players in deciding their actions and the business logic involving them
					+ TurnTypes : directory that holds our data representations for the different actions a player can take on their turn and the business logic involving them
				+ Program.cs : entry point (main) for our Trains project
				+ Util : directory that holds any miscellaneous utilities used in the source code
					+ AvaloniaConverters : directory that holds all converters used by the View to display data from the ViewModel
					+ Comparers : directory that holds comparers used for operations involving comparison between game pieces
					+ Constant.cs : holds constant values used throughout the project
					+ Json : directory that holds files relevant to the JSON input being used throughout our integration tests
					+ Utilities.cs : holds non-class-specific functionality that comes up throughout the project
				+ ViewModels : directory that holds the instance data from our Models that is used in Trains.com; accessible from our Views
				+ Views : directory that holds our graphical display of Trains.com using data from the ViewModels through the AvaloniaUI bindings
			+ Tests : directory that holds our tests
				+ IntegrationTests : directory that holds the source code and executable for the integration test
					+ IntegrationObjects : directory that holds the source code for the integration test harnesses listed below
						+ Xlegal.cs : file that holds xlegal test harness functionality
						+ Xmap.cs : file that holds xlegal test harness functionality
						+ Xstrategy.cs : file that holds xstrategy test harness functionality
						+ Xvisualize.cs : file that holds xvisualize test harness functionality
					+ IntegrationPub : directory that holds our published integration test harness used to run the integration tests from each milestone and associated dependency/configuration files
					+ Program.cs : entry point (main) for the test harness's executable
				+ TestRunner : directory that holds TestRunner project used for xtest
					+ Program.cs : entry point (main) for TestRunner project
				+ Unit : directory that holds our unit tests
					+ TrainsModelsGamePiecesTests.cs : unit tests for files in acadia/Trains/Other/Source/Models/GamePieces/
					+ TrainsUtilConverterTests.cs : unit tests for files in acadia/Trains/Other/Source/Util/Converters/
					+ XmapJsonTests.cs : unit tests for files in acadia/Trains/Other/Tests/Xmap/Json/
					+ test-pub : directory that holds our published Tests project; used to run our tests in acadia/Trains/xtest
					+ nunit-console-runner : directory that contains the executable used to run our tests and associated dependency/configuration files
		+ Planning : directory that contains milestone design tasks
		+ Player :
			+ link-to-player.txt
			+ link-to-strategy.txt
		+ **xtest : script that runs all of our unit tests and prints an overview of the results; run it by calling `./xtest` from the Trains directory**

Note that directories/files excluded from the above breakdown is boilerplate from project creation and AvaloniaUI set-up or running.

## Milestone 6

The overall purpose of milestone 6 is to:
+ implement player (player.cs) and referee (referee.cs) classes 
+ design the tournament manager component (manager-player-interface.md)
+ create a test harness (xstrategy) 

## Milestone 5

The overall purpose of milestone 5 is to:
+ implement the Hold-10 and Buy-Now deterministic strategies (strategy.cs) used by players
+ design the interaction protocol between the referee and the player(s) (player-protocol.md)
+ create a test harness (xlegal) 

## Milestone 4

The overall purpose of milestone 4 is to:
+ implement the game states used for players (player-game-state.cs) and the referee (referee-game-state.cs)
+ design the API for the player component and its accompanying protocol (player-interface.md)
+ create a visualization harness (xvisualize) that consumes its JSON input from STDIN and pops up a window visualizing the input map for 10 seconds

## Milestone 3

The overall purpose of milestone 3 is to:   
+ implement a map visualizer (map-editor.cs) that consumes a data representation of a map and displays the map
+ design data representations (game-state.md) for the state of the game for both referees and players
+ create a test harness (xmap) that consumes its JSON input from STDIN and produces its results to STDOUT along with 3 test cases

## Milestone 2

The overall purpose of milestone 2 for Trains.com is to:
+ First, to design and implement a data representation for maps of train connections (Map.cs)
+ Secondly, to describe a plan (visual.md) for a program that visualizes the map in enough detail such that if the coding was outsourced, we could expect a working
program that we can call according to our specification and that draws the map mostly as desired
