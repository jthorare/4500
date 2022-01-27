using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using xtests.xvisualize.ViewModels;
using xtests.xvisualize.Views;

namespace xtests.xvisualize
{
    /// <inheritdoc cref="Application"/>
    /// <summary>
    /// Represents the main <see cref="Application"/> of the program.
    /// </summary>
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        /// <inheritdoc cref="OnFrameworkInitializationCompleted"/>
        /// <summary>
        /// Handles initialization of the application after the framework has initialized
        /// </summary>
        public override void OnFrameworkInitializationCompleted()
        {
            // Get the object representing the desktop application
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Create the main window
                desktop.MainWindow = new MainWindow
                {
                    // Create data for the window
                    DataContext = new MainWindowViewModel()
                };
            }
            
            base.OnFrameworkInitializationCompleted();
        }
    }
}