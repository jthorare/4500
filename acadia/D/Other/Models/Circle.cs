using Avalonia;
using System.Collections.Generic;
using System.ComponentModel;

namespace xgui.Models
{
    public class Circle : AvaloniaObject, INotifyPropertyChanged
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
