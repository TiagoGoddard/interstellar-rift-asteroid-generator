using AsteroidFieldGenerator.Models;
using MaterialDesignThemes.Wpf.Transitions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AsteroidFieldGenerator
{
    /// <summary>
    /// Interação lógica para UserControlAsteroids.xam
    /// </summary>
    public partial class UserControlAsteroids : UserControl
    {
        MainViewModel mainViewModel = MainViewModel.GetInstance();
         
        public UserControlAsteroids()
        {
            InitializeComponent();
            DataContext = mainViewModel;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var c = btn.DataContext as Asteroid;
            if (c != null)
            {
                UserControlAddAsteroid userControlAdd = UserControlAddAsteroid.GetInstance();
                userControlAdd.EditAsteroidCommand.Execute(c);
                Transitioner.MoveNextCommand.Execute(null, null);
            }
        }
    }


}
