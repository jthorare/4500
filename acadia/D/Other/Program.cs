using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using System;
using System.Threading;
using xgui.Views;

namespace xgui
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args)
         => BuildAvaloniaApp().Start(AppMain, args);
        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();

        // Application entry point. Avalonia is completely initialized.
        static void AppMain(Application app, string[] args)
        {
            // A cancellation token source that will be used to stop the main loop
            var cts = new CancellationTokenSource(3000);

            app.Initialize();
            IApplicationLifetime lifetime = new ClassicDesktopStyleApplicationLifetime();
            app.ApplicationLifetime = lifetime;
            app.OnFrameworkInitializationCompleted();
            (lifetime as ClassicDesktopStyleApplicationLifetime).MainWindow.Show();
            // Start the main loop
            app.Run(cts.Token);
        }
    }
}
