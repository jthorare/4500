using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace xtests.xvisualize.Views
{
    /// <summary>
    /// The main window of the program
    /// </summary>
    public class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
//#if DEBUG
//            this.AttachDevTools();
//#endif
        }

        /// <summary>
        /// Initialize the MainWindow component.
        /// </summary>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}