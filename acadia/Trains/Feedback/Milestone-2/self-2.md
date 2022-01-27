# Self-Evaluation Form for Milestone 2

Indicate below each bullet which file/unit takes care of each task:

1. point to the functionality for retrieving the cities from the game-map representation

   - its signature and/or purpose statement
   With our game-map's cities represented as a property of our Map class, we utilize the C# shorthand notation for a public get method. It is not explicitly documented in our purpose statement, but it is a foundational practice within C# that we felt did not need to be documented.
   [link](https://github.ccs.neu.edu/CS4500-F21/acadia/blob/3578ef8cbfbf0184a5ed15a7039a5e953ab0b2ca/Trains/Other/Source/GamePieces/Map.cs#L19-L22)
   - its unit test(s)
   We do not explicitly test our `Map`'s `Cities` property's get accessor, but it is used implicitly within
   [link](https://github.ccs.neu.edu/CS4500-F21/acadia/blob/3578ef8cbfbf0184a5ed15a7039a5e953ab0b2ca/Trains/Other/Tests/GamePiecesTests.cs#L401-L419)

2. point to the functinality for determining all feasible destinations from the game-map representation

   - its signature and/or purpose statement
   [link](https://github.ccs.neu.edu/CS4500-F21/acadia/blob/3578ef8cbfbf0184a5ed15a7039a5e953ab0b2ca/Trains/Other/Source/GamePieces/Map.cs#L116-L121)
   - a basic unit test
   [link](https://github.ccs.neu.edu/CS4500-F21/acadia/blob/3578ef8cbfbf0184a5ed15a7039a5e953ab0b2ca/Trains/Other/Tests/GamePiecesTests.cs#L443-L464)
   - a unit test with at least one pair of cities that aren't connected via some path
   [link](https://github.ccs.neu.edu/CS4500-F21/acadia/blob/3578ef8cbfbf0184a5ed15a7039a5e953ab0b2ca/Trains/Other/Tests/GamePiecesTests.cs#L465-L471)

The ideal feedback for each of these three points is a GitHub
perma-link to the range of lines in a specific file or a collection of
files.

A lesser alternative is to specify paths to files and, if files are
longer than a laptop screen, positions within files are appropriate
responses.

You may wish to add a sentence that explains how you think the
specified code snippets answer the request.

If you did *not* realize these pieces of functionality, say so.
