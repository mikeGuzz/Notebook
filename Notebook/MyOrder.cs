using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Notebook
{
    public class MyOrder
    {
        public TextWrapping TextWrapping { get; set; }
        public double FontSize { get; set; }

        public MyOrder() { }

        public MyOrder(TextWrapping textWrapping, double fontSize)
        {
            TextWrapping = textWrapping;
            FontSize = fontSize;
        }
    }
}
