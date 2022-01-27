## Self-Evaluation Form for Milestone 5

Indicate below each bullet which file/unit takes care of each task:

1. the general interface/type/signatures for strategies
https://github.ccs.neu.edu/CS4500-F21/acadia/blob/5e42fa388acec706b81d9a30900b39f93db6c46e/Trains/Other/Source/Models/Strategies/IStrategy.cs#L10-L28


2. the common container/abstract class (see Fundamentals II)  for the buy algorithm; in an FP approach, the common algoritnm
https://github.ccs.neu.edu/CS4500-F21/acadia/blob/5e42fa388acec706b81d9a30900b39f93db6c46e/Trains/Other/Source/Models/Strategies/Strategy.cs#L12-L55


3. the method/function for setting-up decisions, plus unit tests 
Method: https://github.ccs.neu.edu/CS4500-F21/acadia/blob/5e42fa388acec706b81d9a30900b39f93db6c46e/Trains/Other/Source/Models/Strategies/Strategy.cs#L15-L31
Tests:
https://github.ccs.neu.edu/CS4500-F21/acadia/blob/5e42fa388acec706b81d9a30900b39f93db6c46e/Trains/Other/Tests/Unit/TrainsModelsStrategiesTests.cs#L12-L22
https://github.ccs.neu.edu/CS4500-F21/acadia/blob/5e42fa388acec706b81d9a30900b39f93db6c46e/Trains/Other/Tests/Unit/TrainsModelsStrategiesTests.cs#L34-L44


4. the method/function for take-turn decisions, plus unit tests 
Method: https://github.ccs.neu.edu/CS4500-F21/acadia/blob/5e42fa388acec706b81d9a30900b39f93db6c46e/Trains/Other/Source/Models/Strategies/Strategy.cs#L33-L49
Tests:
https://github.ccs.neu.edu/CS4500-F21/acadia/blob/5e42fa388acec706b81d9a30900b39f93db6c46e/Trains/Other/Tests/Unit/TrainsModelsStrategiesTests.cs#L24-L32
https://github.ccs.neu.edu/CS4500-F21/acadia/blob/5e42fa388acec706b81d9a30900b39f93db6c46e/Trains/Other/Tests/Unit/TrainsModelsStrategiesTests.cs#L46-L55


5. the methods/functions for lexical-order comparisions of destinations, plus unit tests
Method: https://github.ccs.neu.edu/CS4500-F21/acadia/blob/5e42fa388acec706b81d9a30900b39f93db6c46e/Trains/Other/Source/Util/Comparers/LexicoDestinationComparer.cs#L9-L33
Tests: We do not have unit tests.


6. the methods/functions for lexical-order comparisions of connections, plus unit tests 
Method: https://github.ccs.neu.edu/CS4500-F21/acadia/blob/5e42fa388acec706b81d9a30900b39f93db6c46e/Trains/Other/Source/Util/Comparers/LexicoConnectionComparer.cs#L9-L37
Tests: We do not have unit tests.



The ideal feedback for each of these three points is a GitHub
perma-link to the range of lines in a specific file or a collection of
files.

A lesser alternative is to specify paths to files and, if files are
longer than a laptop screen, positions within files are appropriate
responses.

You may wish to add a sentence that explains how you think the
specified code snippets answer the request.

If you did *not* realize these pieces of functionality, say so.
