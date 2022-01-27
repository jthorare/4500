using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using System.Threading;

namespace xtests.xvisualize
{
    class Program
    {
        /// <summary>
        /// How to handle timeout in avalonia is found from https://github.com/AvaloniaUI/Avalonia/wiki/Application-lifetimes
        /// and adapted to our 10s timeout.
        /// </summary>
        public static void Main()
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
        public static void AppMain(Application app, string[] args)
        {
            App application = app as App;
            application.Initialize();
            ClassicDesktopStyleApplicationLifetime lifetime = new ClassicDesktopStyleApplicationLifetime();
            lifetime.ShutdownMode = ShutdownMode.OnMainWindowClose;
            application.ApplicationLifetime = lifetime;
            application.OnFrameworkInitializationCompleted();
            lifetime.MainWindow.Show();
            int visualizationLengthInSeconds = 10;
            // Start the main loop
            CancellationTokenSource cts = new CancellationTokenSource(visualizationLengthInSeconds * 1000); // CTS takes in time in milliseconds
            application.Run(cts.Token);
        }
    }
}