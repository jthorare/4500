using System.Collections.Generic;
using System.Linq;
using Trains.Models.GamePieces;
using Trains.Util;

namespace Trains.ViewModels
{

    /// <summary>
    /// A Class that represents the logic for visualizing a Trains.com Map
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// The Map to be visualized in a window
        /// </summary>
        public Map GameMap { get; }

        // COLOR CONSTANTS/MORE

        /// <summary>
        /// Constructor that for this MainWindowViewModel that visualizes a Window.
        /// <param name="map">The Map object this MainWindowViewModel should visualize</param>
        /// </summary>
        public MainWindowViewModel(Map map)
        {
            GameMap = map;

        }
    }
}
