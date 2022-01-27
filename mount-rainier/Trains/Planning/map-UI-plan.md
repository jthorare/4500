Map Plan

The class that will create the UI representation of the map will take the following inputs:
- Our Map class
- The size of the UI Window

It will then process the data as follows:
- Create window of the correct size.
- Take the list of cities and draw them in the correct spots on the map.
- Loop through connections drawn to map with correct colors and rails
- Using the Model, View, ViewModel architecture used in the AvaloniaUI library for a canvas object.
This will create an accurate canvas representation of current Map giving us the following output:
- A correctly sized window with all cities displayed with correctly colored connections and rails

Method definition we are expecting is as follows:
Void DrawMap(IMap m, (int x, int y));
