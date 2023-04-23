using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Notebook
{
    public enum TemplatedFontSize { VerySmall, Small, Normal, Large, VeryLarge };

    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public Dictionary<TemplatedFontSize, double> sizeStands { get; set; } = new Dictionary<TemplatedFontSize, double>()
        {
            { TemplatedFontSize.VerySmall, 10 },
            { TemplatedFontSize.Small, 14 },
            { TemplatedFontSize.Normal, 18 },
            { TemplatedFontSize.Large, 22 },
            { TemplatedFontSize.VeryLarge, 26 },
        };

        public TextWrapping _TextWrapping
        {
            get => (TextWrapping)wrapMode_comboBox.SelectedIndex;
            set => wrapMode_comboBox.SelectedIndex = (int)value;
        }

        public double _FontSize
        {
            get
            {
                if(sizeStands.TryGetValue((TemplatedFontSize)fontSize_comboBox.SelectedIndex, out var n))
                    return n;
                throw new InvalidOperationException();
            }
            set
            {
                foreach(var ob in sizeStands)
                {
                    if(ob.Value == value)
                    {
                        fontSize_comboBox.SelectedIndex = (int)ob.Key;
                        return;
                    }
                }
            }
        }

        public Brush BackColor
        {
            get => backColor_border.Background;
            set
            {
                if (value.GetType() != typeof(SolidColorBrush))
                    return;
                backColor_border.Background = value;
            }
        }

        public Brush ForeColor
        {
            get => foreColor_border.Background;
            set
            {
                if (value.GetType() != typeof(SolidColorBrush))
                    return;
                foreColor_border.Background = value;
            }
        }

        public SettingsWindow()
        {
            InitializeComponent();

            foreach (var ob in Enum.GetNames(typeof(TextWrapping)))
            {
                wrapMode_comboBox.Items.Add(ob);
            }
            fontSize_comboBox.SelectedIndex = (int)TemplatedFontSize.Normal;
            foreach (var ob in Enum.GetNames(typeof(TemplatedFontSize)))
            {
                fontSize_comboBox.Items.Add(ob);
            }
            wrapMode_comboBox.SelectedIndex = 0;
        }

        private void ok_button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void foreColor_button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new MyColorDialog();
            dialog.Color = ((SolidColorBrush)foreColor_border.Background).Color;
            if (dialog.ShowDialog() == true)
            {
                foreColor_border.Background = new SolidColorBrush(dialog.Color);
            }
        }

        private void backColor_button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new MyColorDialog();
            dialog.Color = ((SolidColorBrush)backColor_border.Background).Color;
            if (dialog.ShowDialog() == true)
            {
                backColor_border.Background = new SolidColorBrush(dialog.Color);
            }
        }
    }
}
