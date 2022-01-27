## Map Visualization Design Plan

#### GOAL: Create a program that _visualizes_ a Map using the AvaloniaUI framework using the MVVM pattern, specifically in the MainWindowView and MainWindowViewModel.

Using AvaloniaUI, the entry point to Trains.com will be handled by an App class. As a result, we expect these programs to not be able to run by themselves but to act as components in an AvaloniaUI MVVM project.

The MainWindowViewModel should have a single Constructor that takes in a Map and assigns the following properties:
+ GameMap is a Map object that is assigned the given Map.

We define a Map to be made up of a List of City, Cities, a List of Connection, Connections, and integers Width and Height as described in map-design.md.
A Map should be represented visually by an [Avalonia.Controls.Canvas](http://reference.avaloniaui.net/api/Avalonia.Controls/Canvas/) with its List of City and Connection drawn in the following order and in the following specifications:
+ Width and Height are natural numbers that represent the width and height, respectively, in pixels of the background that makes up the base of the Map.
+ A City represents a city on our map, and is made up of integers X and Y, and string Name.
	+ A City should be represented visually as an [Avalonia.Controls.Shapes.Path](http://reference.avaloniaui.net/api/Avalonia.Controls.Shapes/Path/) with an [Avalonia.Media.MediumPurple](http://reference.avaloniaui.net/api/Avalonia.Media/Colors/) [Avalonia.Media.EllipseGeometry](http://reference.avaloniaui.net/api/Avalonia.Media/EllipseGeometry/) with a 10px width and 10px height.
	+ X and Y are natural numbers that represent the x and y position in pixels, respectively, of a City upon the background.
	+ The City's Name should be displayed in fontsize=16 [Avalonia.Controls.TextBlock](http://reference.avaloniaui.net/api/Avalonia.Controls/TextBlock/) centered on the center of its corresponding Ellipse. It should also only use all letters of the English alphabet plus space, dot, and comma and have at most 25 ASCII characters.
+ A Connection represents a direct connection between two City on our map, and is made up of a List of City, Cities, a GamePieceColor, Color, and a Length, Segments.
	+ Cities is a List of size 2 that consists of the two City representing the endpoints of the Connection.
	+ Length is a defined range of lengths that represents the number of segments that makes up a Connection. A Length is one of:
		+ Length.Three, which is equivalent to the integer 3.
		+ Length.Four, which is equivalent to the integer 4.
		+ Length.Five, which is equivalent to the integer 5.
	+ GamePieceColor is a defined range of colors that a Connection can be. A GamePieceColor is one of:
		+ GamePieceColor.Blue, which is represented using [Avalonia.Media.DodgerBlue](http://reference.avaloniaui.net/api/Avalonia.Media/Colors/9A355459)
		+ GamePieceColor.Red, which is represented using [Avalonia.Media.Red](http://reference.avaloniaui.net/api/Avalonia.Media/Colors/F7A7BFDE)
		+ GamePieceColor.Green, which is represented using [Avalonia.Media.LightGreen](http://reference.avaloniaui.net/api/Avalonia.Media/Colors/D195FFC2)
		+ GamePieceColor.White, which is represented using [Avalonia.Media.GhostWhite](http://reference.avaloniaui.net/api/Avalonia.Media/Colors/618A0D09)
	+ A Connection should be represented visually by a single [Avalonia.Controls.Shapes.Line](http://reference.avaloniaui.net/api/Avalonia.Controls.Shapes/Line/) with a [StrokeDashArray](http://reference.avaloniaui.net/api/Avalonia.Controls.Shapes/Shape/0B8F4357)
		bound to the properties of "SegmentLength, GapSize" in the ViewModel and the Color property set to the correct Color using the mapping above. A Connection should be placed on the Canvas with StartPoint="Cities[0].X,Cities[0].Y" and EndPoint="Cities[1].X, Cities[2].Y"
		+ SegmentLength = (Math.Sqrt((Connection.CityPair[0].Y - Connection.CityPair[1].Y)^2 + (Connection.CityPair[0].X - Connection.CityPair[1].X)^2)) / (int)Connection.Length)) - (GapSize * (((int)Connection.Length) - 1)) / (int)Connection.Length)"
		+ GapSize = 0.05 * length of line (i.e. Math.Sqrt((Connection.CityPair[0].Y - Connection.CityPair[1].Y)^2 + (Connection.CityPair[0].X - Connection.CityPair[1].X)^2)))
		
Additional Details:
+ The window in which the Map is drawn should not be resizable.
+ The drawing of the Map should never time out; it should close manually, requiring user interaction.