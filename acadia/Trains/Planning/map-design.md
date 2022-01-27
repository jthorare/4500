## Our Trains Map Design

The list of actions that we want a referee or player to perform on our map of train routes is as follows:
+ The referee needs to be able to set up the map, which could change game to game.
	+ Add/remove a connection to the map.
	+ Add/remove a city to the map.
+ The referee needs to be able to enforce the rules
	+ Check that connections are not being obtained by a player when it already has an owner
	+ Check that players have sufficient cards of the color corresponding to the color and length of the connection they are attempting to obtain
+ A player should be able to access (but not alter) all of the contents of the map.
+ A player should be able to know whether a connection is owned by another player or is available.
+ A player should be able to claim a connection (as long as it is unowned and the player has enough rails and the right number of the right colored cards).
+ A player should be able to see all of the cities directly connected to a given city.

To accomodate these needs, our map will be represented as a class with an IList<Connection> objects, an IList<City> objects, an integer Width and an integer Height.
+ The integers Width and Height are non-negative values that represent the width and height of the visualized map in pixels.
+ A City is a class representing one city/location on our map. It has integer values X and Y, which must be non-negative and correspond to the City's visual position on the map in pixels.
+ A Connection is a class representing a direct connection between two cities on our map. It has an IList<City> of size 2 (size 2 is invariant) that contains the two endpoint cities of the connection,
a SegmentColor represented by an enumeration of the valid colors used in Trains.com indicating the color of the connection, and a Length represented by an enumeration of the valid connection lengths
indicating the number of segments that the connection is divided into.

+ Compared to a set, a list as the structure that holds a connection's pair of city endpoints allows us to both access its values without prior knowledge of them,
and avoid a need to iterate over the set because there can only be two cities in a connection.
