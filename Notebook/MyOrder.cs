using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Notebook
{
    public class MyOrder
    {
        public TextWrapping TextWrapping { get; set; }
        public double FontSize { get; set; }
        public Color BackColor { get; set; }
        public Color ForeColor { get; set; }

        public MyOrder() { }

        public MyOrder(TextWrapping textWrapping, double fontSize, Color backColor, Color foreColor)
        {
            TextWrapping = textWrapping;
            FontSize = fontSize;
            ForeColor = foreColor;
            BackColor = backColor;
        }
    }
}
