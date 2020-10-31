using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AsteroidFieldGenerator.Models;
using AsteroidFieldGenerator.Utility;

namespace AsteroidFieldGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        MainViewModel mainViewModel = MainViewModel.GetInstance();

        String filePath = string.Empty;
        String fileContent = string.Empty;

        bool fileEdited = false;
        
        public static RoutedCommand MenuItemSelected { get; } = new RoutedCommand("MenuItemSelected", typeof(MainWindow));
        private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            switch (e.Parameter)
            {
                case "New":
                    mainViewModel.NewFileCommand.Execute(null);
                    break;
                case "Open":
                    mainViewModel.OpenFileCommand.Execute(null);
                    break;
                case "Save":
                    mainViewModel.SaveFileCommand.Execute(null);
                    break;
                case "Save As...":
                    mainViewModel.SaveAsFileCommand.Execute(null);
                    break;
                case "Exit":
                    if (mainViewModel.IsEdited())
                    {
                        if (MessageBox.Show("This asteroid field was edited and it's not saved, really exit?", "Confirm exit", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            System.Windows.Application.Current.Shutdown();
                        }
                    } else {
                        System.Windows.Application.Current.Shutdown();
                    }
                    break;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = mainViewModel;

            mainViewModel.NewFileCommand.Execute(null);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.fileEdited == true)
            {
                if (MessageBox.Show("This asteroid field was edited and it's not saved, really exit?", "Confirm exit", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
