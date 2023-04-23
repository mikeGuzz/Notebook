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
    /// <summary>
    /// Interaction logic for MyColorDialog.xaml
    /// </summary>
    public partial class MyColorDialog : Window
    {
        public byte R
        {
            get => (byte)r_slider.Value;
            set
            {
                r_slider.Value = value;
                Update();
            }
        }

        public byte G
        {
            get => (byte)g_slider.Value;
            set
            {
                g_slider.Value = value;
                Update();
            }
        }

        public byte B
        {
            get => (byte)b_slider.Value;
            set
            {
                b_slider.Value = value;
                Update();
            }
        }

        public Color Color
        {
            get => Color.FromRgb(R, G, B);
            set
            {
                r_slider.Value = value.R;
                g_slider.Value = value.G;
                b_slider.Value = value.B;
                Update();
            }
        }

        public MyColorDialog()
        {
            InitializeComponent();

            r_slider.Value = r_slider.Maximum;
            g_slider.Value = g_slider.Maximum;
            b_slider.Value = b_slider.Maximum;

            Update();
        }

        private void UpdateRGB()
        {
            rgb_textBox.Text = $"{R}, {G}, {B}";
        }

        private void UpdateCMYK()
        {
            float rf = R / 255F;
            float gf = G / 255F;
            float bf = B / 255F;

            var clampCmyk = (float value) => (value < 0 || float.IsNaN(value)) ? 0 : value;

            float k = clampCmyk(1 - Math.Max(Math.Max(rf, gf), bf));
            float c = clampCmyk((1 - rf - k) / (1 - k));
            float m = clampCmyk((1 - gf - k) / (1 - k));
            float y = clampCmyk((1 - bf - k) / (1 - k));

            cmyk_textBox.Text = $"{Math.Round(c * 100)}%, {Math.Round(m * 100)}%, {Math.Round(y * 100)}%, {Math.Round(k * 100)}%";
        }

        private void UpdateHEX()
        {
            Color color = Color.FromRgb(R, G, B);
            hex_textBox.Text = "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }

        private void Update()
        {
            UpdateRGB();
            UpdateCMYK();
            UpdateHEX();
            preview_border.Background = new SolidColorBrush(Color);
        }

        private void r_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Update();
            r_textBlock.Text = R.ToString();
        }
        
        private void g_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Update();
            g_textBlock.Text = G.ToString();
        }

        private void b_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Update();
            b_textBlock.Text = B.ToString();
        }

        private void rgb_textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var arr = rgb_textBox.Text.Split(',', ';');
            var showWarn = (string value) => { MessageBox.Show($"Invalid value: '{value}'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error); UpdateRGB(); };
            if (arr.Length != 3)
            {
                showWarn(rgb_textBox.Text);
                return;
            }
            byte[] colors = new byte[3];
            for(int i = 0; i < arr.Length; i++)
            {
                if (byte.TryParse(arr[i], out var n))
                    colors[i] = n;
                else
                {
                    showWarn(arr[i]);
                    return;
                }
            }
            r_slider.Value = colors[0];
            g_slider.Value = colors[1];
            b_slider.Value = colors[2];

            Update();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return)
                return;
            if (rgb_textBox.IsFocused || cmyk_textBox.IsFocused || hex_textBox.IsFocused)
                ok_button.Focus();
            else
                ok_button_Click(sender, e);
        }

        private void ok_button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void cmyk_textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var arr = cmyk_textBox.Text.Split(',');
            var showWarn = (string value) => { MessageBox.Show($"Invalid value: '{value}'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error); UpdateCMYK(); };
            if (arr.Length != 4)
            {
                showWarn(cmyk_textBox.Text);
                return;
            }
            float[] colors = new float[4];
            for (int i = 0; i < arr.Length; i++)
            {
                var p = arr[i].Contains('%');
                if (float.TryParse(!p ? arr[i] : arr[i].Replace('%', ' '), out var n))
                    colors[i] = p ? n / 100f : n;
                else
                {
                    showWarn(arr[i]);
                    return;
                }
            }
            r_slider.Value = byte.MaxValue * (1 - colors[0]) * (1 - colors.Last());
            g_slider.Value = byte.MaxValue * (1 - colors[1]) * (1 - colors.Last());
            b_slider.Value = byte.MaxValue * (1 - colors[2]) * (1 - colors.Last());

            Update();
        }

        private void hex_textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            System.Drawing.Color c;
            try
            {
                c = System.Drawing.ColorTranslator.FromHtml(hex_textBox.Text[0] != '#' ? '#' + hex_textBox.Text : hex_textBox.Text);
            }
            catch
            {
                MessageBox.Show($"Invalid value: '{hex_textBox.Text}'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                UpdateHEX();
                return;
            }

            r_slider.Value = c.R;
            g_slider.Value = c.G;
            b_slider.Value = c.B;

            Update();
        }
    }
}
