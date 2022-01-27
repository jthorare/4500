# Map Design

## Data Definition

### Map

A game Map will store Locations and Connections between these Locations. It also has a Width and Height dimension in pixels.

### Location

A Location has a Name and a list of all Connections it leads to. It also has a Coordinate (x, y pair) that represents its location on the map.

### Connection

A Connection has a Name, a Set of two Locations it connects, a number of rail Segments connecting the two Locations, the Color of the Connection, and a possible Player who owns the Connection.

## Functional Definition

There are several actions that can be performed on a game Map.

### GetDimensions()

Get the dimensions of the board as a pair of (width, height)

### GetAllConnections()

Get a Set of Connection that represents all Connections on the map.

### GetConnections(Location)

Get a Set of Connections that represent all Connections connected to the requested Location.

### GetAllLocations()

Get a Set of Location that represent all Location on the map.

### ClaimConnection(Connection, Player)

Mark a connection as owned by a player.

## Additional Notes

It is assumed that whether marking a connection claimed by a player is valid will be handled by the referee and the referee will validate whether the connection can be claimed before allowing the player to claim it.
