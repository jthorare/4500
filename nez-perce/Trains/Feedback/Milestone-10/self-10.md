## Self-Evaluation Form for Milestone 10

Indicate below each bullet which file/unit takes care of each task.

The `remote proxy patterns` and `server-client` implementation calls for several
different design-implementation tasks. Point to each of the following:

1. the implementation of the `remote-proxy-player`
	The file RemotePlayer.cs at the path /nez-perce/Trains/src/Trains/Remote/RemotePlayer.cs is the remote-proxy-player implementation.

	With one sentence explain how it satisfies the player interface.
	It implements the IPlayer interface, and converts the arguments from the Referee or Manager and converts them into our JSON format and sends that over a NetworkStream.


2. the unit tests for the `remote-proxy-player`
	We do not have unit tests for the remote-proxy-player implementation.


3. the `server` and especially the following two pieces of factored-out
   functionality:
	 nez-perce/Trains/src/Trains/Remote/Server.cs contains the server implementation.

   - signing up enough players in at most two rounds of waiting
	 The Server.SignUpPlayers() method handles signing up all the players for the tournament.
   - signing up a single player (connect, check name, create proxy)
	 The Server.SignUpPlayer() method handles signing up a single player.

4. the `remote-proxy-manager-referee`

	With one sentence, explain how it deals with all calls from the manager and referee on the server side.  
	Our remote proxy-manager-referee is located  at /nez-perce/Trains/src/Trains/Remote/ServerProxy.cs
	It is used to send and receive the json over the NetworkStream and convert that into RemoteFunction object for use in deciding how to respond.



The ideal feedback for each of these three points is a GitHub
perma-link to the range of lines in a specific file or a collection of
files.

A lesser alternative is to specify paths to files and, if files are
longer than a laptop screen, positions within files are appropriate
responses.

You may wish to add a sentence that explains how you think the
specified code snippets answer the request.

If you did *not* realize these pieces of functionality, say so.

