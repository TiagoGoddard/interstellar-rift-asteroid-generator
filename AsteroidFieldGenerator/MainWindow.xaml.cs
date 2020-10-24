using System;
using System.Collections.Generic;
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
using AsteroidFieldGenerator.Models;
using Microsoft.Win32;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AsteroidFieldGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        AsteroidField asteroidField = new AsteroidField();

        String filePath = string.Empty;
        String fileContent = string.Empty;
        bool fileEdited = false;

        readonly bool isDebugMode = false;

        private void openFile() {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                //Get the path of specified file
                this.filePath = openFileDialog.FileName;

                //Read the contents of the file into a stream
                var fileStream = openFileDialog.OpenFile();
                StreamReader reader = new StreamReader(fileStream);
                this.fileContent = reader.ReadToEnd();
            }
            try
            {
                this.asteroidField = JsonSerializer.Deserialize<AsteroidField>(this.fileContent);
                fileEdited = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid JSON file!", "AsteroifFieldGenerator - File Import", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine("Exception caught: {0}", ex);
            }
        }
        private void saveAsFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                RestoreDirectory = true
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                this.filePath = saveFileDialog.FileName;
                this.fileContent = JsonSerializer.Serialize(asteroidField);
                File.WriteAllText(this.filePath, this.fileContent);
                fileEdited = false;
            }
        }
        private void saveFile()
        {
            if (String.Equals(this.filePath,string.Empty)) {
                saveAsFile();
            } else {
                this.fileContent = JsonSerializer.Serialize<AsteroidField>(asteroidField);
                File.WriteAllText(this.filePath, this.fileContent);
                fileEdited = false;
            }
        }
        public static RoutedCommand MenuItemSelected { get; } = new RoutedCommand("MenuItemSelected", typeof(MainWindow));
        private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            switch (e.Parameter)
            {
                case "New":
                    if(this.asteroidField.asteroids.Count > 0 && this.filePath == string.Empty)
                    {
                        if(MessageBox.Show("This asteroid field is not empty, and it's not saved, really delete it?", "Confirm delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            this.asteroidField = new AsteroidField();
                            this.filePath = string.Empty;
                            this.fileContent = string.Empty;
                        }
                    } else
                    {
                        this.asteroidField = new AsteroidField();
                    }
                    break;
                case "Open":
                    openFile();
                    break;
                case "Save":
                    saveFile();
                    break;
                case "Save As...":
                    saveAsFile();
                    break;
                case "Exit":
                    System.Windows.Application.Current.Shutdown();
                    break;
            }

            if(isDebugMode) { 
                SelectedMenuTextBlock.Text = $"Selected Item: {e.Parameter}";
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            if (System.Diagnostics.Debugger.IsAttached)
            {
                isDebugMode = true;
            }
            asteroidField = new AsteroidField();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
