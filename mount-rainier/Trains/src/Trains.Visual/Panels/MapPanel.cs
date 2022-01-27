using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Trains.Common.Map;

namespace Trains.Visual.Panels
{
    public class MapPanel : Panel
    {
        public TrainsMap Map
        {
            set => SetValue(MapProperty, value);
            get => GetValue(MapProperty);
        }

        public static readonly StyledProperty<TrainsMap> MapProperty =
            AvaloniaProperty.Register<MapPanel, TrainsMap>(nameof(TrainsMap));

        public MapPanel()
        {
            Background ??= new SolidColorBrush(Colors.Black);
        }
    }
}