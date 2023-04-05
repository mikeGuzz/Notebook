using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Notebook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly static string productName = "Notepad";
        private readonly static string saveFileFilter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
        private readonly static string settingsFileName = "settings.xml";

        private string filePath = string.Empty;

        private string fileName => File.Exists(filePath) ? System.IO.Path.GetFileNameWithoutExtension(filePath) : "Untitled";

        public MainWindow()
        {
            InitializeComponent();
            Title = GetTitle();

            if (File.Exists(settingsFileName))
            {
                try
                {
                    using (var s = File.OpenRead(settingsFileName))
                    {
                        var serializer = new XmlSerializer(typeof(MyOrder));
                        if (serializer.Deserialize(s) is MyOrder order)
                        {
                            textBox.TextWrapping = order.TextWrapping;
                            textBox.FontSize = order.FontSize;
                        }
                        else
                        {
                            s.Close();
                            File.Delete(settingsFileName);
                        }
                    }
                }
                catch
                {
                    File.Delete(settingsFileName);
                }
            }
            else
            {
                textBox.TextWrapping = TextWrapping.Wrap;
                textBox.FontSize = 15;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!IsSaved())
            {
                var res = MessageBox.Show($"Do you want to save changes to {filePath}?", GetTitle(), MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (res == MessageBoxResult.Yes)
                {
                    if (!Save())
                        e.Cancel = true;
                }
                else if (res == MessageBoxResult.Cancel)
                    e.Cancel = true;
            }
        }

        private bool IsSaved()
        {
            if(File.Exists(filePath))
            {
                try
                {
                    using (var reader = File.OpenText(filePath))
                    {
                        return reader.ReadToEnd() == textBox.Text;
                    }
                }
                catch
                {
                    return false;
                }
            }
            return string.IsNullOrEmpty(textBox.Text);
        }

        private string GetTitle()
        {
            return fileName + $" - {productName}";
        }

        //Save file

        private bool Save(string filePath)
        {
            try
            {
                using (var writer = File.CreateText(filePath))
                {
                    writer.WriteLine(textBox.Text);
                    this.filePath = filePath;
                    Title = GetTitle();
                    return true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", GetTitle(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return false;
        }

        private bool Save()
        {
            if (File.Exists(filePath))
                return Save(filePath);
            else
                return SaveAsNew();
        }

        private bool SaveAsNew()
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = saveFileFilter;
            if(dialog.ShowDialog() == true)
            {
                return Save(dialog.FileName);
            }
            return false;
        }


        private void save_button_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void saveAs_button_Click(object sender, RoutedEventArgs e)
        {
            SaveAsNew();
        }

        // Open file

        private bool Open(string filePath)
        {
            if (!File.Exists(filePath))
                return false;
            try
            {
                using (var reader = File.OpenText(filePath))
                {
                    textBox.Text = reader.ReadToEnd();
                    this.filePath = filePath;
                    Title = GetTitle();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", GetTitle(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return false;
        }

        private void Open()
        {
            if (!IsSaved())
            {
                var res = MessageBox.Show($"Do you want to save changes to {filePath}?", GetTitle(), MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (res == MessageBoxResult.Yes)
                {
                    if (!Save())
                        return;
                }
                else if (res == MessageBoxResult.Cancel)
                    return;
            }
            var dialog = new OpenFileDialog();
            dialog.Filter = saveFileFilter;
            if (dialog.ShowDialog() == true)
            {
                Open(dialog.FileName);
            }
        }

        private void open_button_Click(object sender, RoutedEventArgs e)
        {
            Open();
        }

        //New file

        private void New()
        {
            textBox.Text = string.Empty;
            filePath = "Untitled";
            Title = GetTitle();
        }

        private void new_button_Click(object sender, RoutedEventArgs e)
        {
            if (!IsSaved())
            {
                var res = MessageBox.Show($"Do you want to save changes to {filePath}?", GetTitle(), MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (res == MessageBoxResult.Yes)
                {
                    if (!Save())
                        return;
                }
                else if (res == MessageBoxResult.Cancel)
                    return;
            }
            New();
        }

        //settings

        private void SaveSettings()
        {
            try
            {
                using (var s = File.Create(settingsFileName))
                {
                    var serializer = new XmlSerializer(typeof(MyOrder));
                    serializer.Serialize(s, new MyOrder(textBox.TextWrapping, textBox.FontSize));
                }
            }
            catch
            {
                if(File.Exists(settingsFileName))
                    File.Delete(settingsFileName);
            }
        }

        private void settings_button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SettingsWindow(textBox.TextWrapping, textBox.FontSize);
            if(dialog.ShowDialog() == true)
            {
                textBox.TextWrapping = dialog._TextWrapping; 
                textBox.FontSize = dialog._FontSize;
                SaveSettings();
            }
        }
    }
}
