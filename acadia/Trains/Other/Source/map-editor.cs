using System;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using Trains.Models.GamePieces;

namespace Trains
{
    /// <summary>
    /// Represents a map editor that is used for the visualization of a Map.
    /// </summary>
    public class MapEditor
    {
        // The Map object to display.
        private Map Map { get; }
        // The amount of time that the map visualization window is open, in seconds. Must be a natural number.
        private int VisualizationLength { get; }
        public MapEditor(Map map, int visualizationLength)
        {
            Map = map;
            if (visualizationLength > 0)
            {
                VisualizationLength = visualizationLength;
            }else
            {
                throw new ArgumentException();
            }
        }


        /// <summary>
        /// Visualizes the given Map using an Avalonia Application.
        /// </summary>
        public void Visualize()
        {
            BuildAvaloniaApp().Start(AppMain, null);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();

        // Application entry point. Avalonia is completely initialized.
        public void AppMain(Application app, string[] args)
        {
            App application = app as App;
            application.Initialize();
            ClassicDesktopStyleApplicationLifetime lifetime = new ClassicDesktopStyleApplicationLifetime();
            lifetime.ShutdownMode = ShutdownMode.OnMainWindowClose;
            application.ApplicationLifetime = lifetime;
            application.OnFrameworkInitializationCompleted(Map);
            lifetime.MainWindow.Show();
            // Start the main loop
            CancellationTokenSource cts = new CancellationTokenSource(VisualizationLength * 1000); // takes in time in milliseconds
            application.Run(cts.Token);
        }
    }
}
