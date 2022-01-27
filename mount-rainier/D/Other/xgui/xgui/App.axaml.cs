using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using xgui.Models;
using xgui.ViewModels;
using xgui.Views;

namespace xgui
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var context = new MainWindowViewModel();
                desktop.MainWindow = new MainWindow
                {
                    CanResize = false,
                    DataContext = context,
                    Width = context.Graph.Size,
                    Height = context.Graph.Size
                };
                
                Task.Run(() =>
                {
                    Thread.Sleep(3000);
                    desktop.Shutdown();
                });
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}