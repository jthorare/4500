start : Boolean
Map : Map [Spec](https://www.ccs.neu.edu/home/matthias/4500-f21/3.html#%28tech._map%29)
End : Boolean
Setup : {"Map" : Map [Spec](https://www.ccs.neu.edu/home/matthias/4500-f21/3.html#%28tech._map%29),
		"rails" : Number,
		"Cards" : [Colors,..,..]}
Pick : {"Destination 1" : Destination [spec](https://www.ccs.neu.edu/home/matthias/4500-f21/5.html#%28tech._destination%29),
		"Destination 2" : Destination [spec](https://www.ccs.neu.edu/home/matthias/4500-f21/5.html#%28tech._destination%29)}
PlayerResponse : none //none for more cards
				or Acquired [spec](https://www.ccs.neu.edu/home/matthias/4500-f21/5.html#%28tech._acquired%29) //To claim a connection.
play : 	ThisPlayer [spec](https://www.ccs.neu.edu/home/matthias/4500-f21/5.html#%28tech._thisplayer%29)

