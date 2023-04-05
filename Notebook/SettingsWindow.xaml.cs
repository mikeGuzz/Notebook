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
        public TextWrapping _TextWrapping
        {
            get => (TextWrapping)wrapMode_comboBox.SelectedIndex;
            set => wrapMode_comboBox.SelectedIndex = (int)value;
        }

        public double _FontSize
        {
            get
            {
                switch ((TemplatedFontSize)fontSize_comboBox.SelectedIndex)
                {
                    case TemplatedFontSize.VerySmall:
                        return 8;
                    case TemplatedFontSize.Small:
                        return 12;
                    case TemplatedFontSize.Large:
                        return 18;
                    case TemplatedFontSize.VeryLarge:
                        return 22;
                    default:
                        return 15;
                }
            }
            set
            {
                switch (value)
                {
                    case 8:
                        fontSize_comboBox.SelectedIndex = (int)TemplatedFontSize.VerySmall;
                        break;
                    case 12:
                        fontSize_comboBox.SelectedIndex = (int)TemplatedFontSize.Small;
                        break;
                    case 18:
                        fontSize_comboBox.SelectedIndex = (int)TemplatedFontSize.Large;
                        break;
                    case 22:
                        fontSize_comboBox.SelectedIndex = (int)TemplatedFontSize.VeryLarge;
                        break;
                    default:
                        fontSize_comboBox.SelectedIndex = (int)TemplatedFontSize.Normal;
                        break;
                }
            }
        }

        public SettingsWindow()
        {
            InitializeComponent();
            Setup();

            wrapMode_comboBox.SelectedIndex = 0;
            fontSize_comboBox.SelectedIndex = (int)TemplatedFontSize.Normal;
        }

        public SettingsWindow(MyOrder order)
        {
            InitializeComponent();
            Setup();

            _FontSize = order.FontSize;
            _TextWrapping = order.TextWrapping;
        }

        public SettingsWindow(TextWrapping textWrapping, double fontSize)
        {
            InitializeComponent();
            Setup();

            _FontSize = fontSize;
            _TextWrapping = textWrapping;
        }

        private void Setup()
        {
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            foreach(var ob in Enum.GetNames(typeof(TextWrapping)))
            {
                wrapMode_comboBox.Items.Add(ob);
            }
            foreach (var ob in Enum.GetNames(typeof(TemplatedFontSize)))
            {
                fontSize_comboBox.Items.Add(ob);
            }
        }

        private void ok_button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
