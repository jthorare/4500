using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using DynamicData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using xgui.Models;

namespace xgui.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            ReadInput();
        }

        public int Size { get; set; } = 100;
        public IList<Circle> Points { get; set; } = new List<Circle>();
        public IList<Connection> Lines { get; set; } = new List<Connection>();
        public void ReadInput()
        {
            // Read json from stdin into an object
            StreamReader sr = new StreamReader(Console.OpenStandardInput());
            Console.SetIn(sr);
            string input = sr.ReadToEnd();
            if (input != null)
            {
                JsonInput json = JsonConvert.DeserializeObject<JsonInput>(input);
                Size = json.size;

                // create the ellipses from the json list of nodes
                CreateEllipses(json.nodes);
            }

        }

        private void CreateEllipses(IList<IList<int>> nodes)
        {
            // Make all of the Circles
            foreach (IList<int> node in nodes)
            {
                Circle pt = new Circle();
                pt.X = node[0] - 5;
                pt.Y = node[1] - 5;
                Points.Add(pt);
            }

            // Create the list of lines for each point
            for (int start = 0; start < Points.Count; start++)
            {
                Circle pt1 = Points[start];
                for (int end = start + 1; end < Points.Count; end++)
                {
                    Circle pt2 = Points[end];
                    Connection line = new Connection();
                    line.Start = new Point(pt1.X + 5, pt1.Y +5);
                    line.End = new Point(pt2.X + 5, pt2.Y + 5);
                    Lines.Add(line);
                }
            }
        }
    }
}
