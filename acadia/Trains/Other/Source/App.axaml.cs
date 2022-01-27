using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Trains.Models.GamePieces;
using Trains.ViewModels;
using Trains.Views;

namespace Trains
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void OnFrameworkInitializationCompleted(Map map)
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(map),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
