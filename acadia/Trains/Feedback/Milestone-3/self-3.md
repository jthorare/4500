## Self-Evaluation Form for Milestone 3

Indicate below each bullet which file/unit takes care of each task:

1. explain how your main visualization method/function

   - manages the timed tear down of the visualization window
   https://github.ccs.neu.edu/CS4500-F21/acadia/blob/1dd45dd178c2b74a2ca1e1e97be0b15b4c15ec11/Trains/Other/Source/map-editor.cs#L49   
   The CancellationToken passed to `Application.Run()` indicates when the Application should stop running.
   - calls out to the drawing the graph itself
   https://github.ccs.neu.edu/CS4500-F21/acadia/blob/1dd45dd178c2b74a2ca1e1e97be0b15b4c15ec11/Trains/Other/Source/map-editor.cs#L22-L29

2. point to the functionality for adding cities to the visualized map
We interpreted such functionality to fall under the _Completely, Totally, Optional Fun_ functionality and chose not to implement it.

3. point to the functionality for adding connections to the visualized map
We interpreted such functionality to fall under the _Completely, Totally, Optional Fun_ functionality and chose not to implement it.

The ideal feedback for each of these three points is a GitHub
perma-link to the range of lines in a specific file or a collection of
files.

A lesser alternative is to specify paths to files and, if files are
longer than a laptop screen, positions within files are appropriate
responses.

You may wish to add a sentence that explains how you think the
specified code snippets answer the request.

If you did *not* realize these pieces of functionality, say so.
