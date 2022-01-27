using System;
using System.Text.Json;
using xgui.Models;

namespace xgui.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public Graph? Graph { get; set; }

        public MainWindowViewModel()
        {
            // StringBuilder stringBuilder = new();
            // StringWriter stringWriter = new(stringBuilder);
            // string line;
            // while ((line = Console.ReadLine()) != null)
            // {
            //     stringWriter.WriteLine(line);
            // }

            // string json = stringBuilder.ToString();
            const string json = @"{""size"":500,""nodes"":[[500,75],[95,30],[25,30]]}";
            
            var graph = JsonSerializer.Deserialize<Graph>(json);
            Graph = graph ?? throw new InvalidOperationException("Graph cannot be null");
        }
    }
}