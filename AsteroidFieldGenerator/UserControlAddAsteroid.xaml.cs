using AsteroidFieldGenerator.Models;
using AsteroidFieldGenerator.Utility;
using MaterialDesignThemes.Wpf.Transitions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interação lógica para UserControlAddAsteroid.xam
    /// </summary>
    public partial class UserControlAddAsteroid : UserControl
    {
        MainViewModel mainViewModel = MainViewModel.GetInstance();
        public static RoutedCommand OptionSelected { get; } = new RoutedCommand("OptionSelected", typeof(MainWindow));

        private ObservableDictionary<ResourceType.RareResource, AsteroidResource> rareAsteroidsList;
        private ObservableDictionary<ResourceType.CommonResource, AsteroidResource> commonAsteroidsList;
        private Asteroid asteroid { get; set; }
        private Asteroid originalAsteroid { get; set; }

        public bool isEditing { get; set; }
        public bool isRare { get; set; }
        public ResourceType.Type Type { get; set; }
        public String TypeName { get; set; }

        private readonly DelegateCommand _editAsteroidCommand;
        public ICommand EditAsteroidCommand => _editAsteroidCommand;

        private static UserControlAddAsteroid _instance;

        public static UserControlAddAsteroid GetInstance()
        {
            return _instance;
        }

        public UserControlAddAsteroid()
        {
            _instance = this;

            InitializeComponent();
            this.DataContext = this;

            _editAsteroidCommand = new DelegateCommand(OnEditAsteroid, CanEditAsteroid);

            NewAsteroid();
        }
        public void ToogleResourceType(bool doing)
        {
            isRare = doing;
            if (isRare)
            {
                Type = ResourceType.Type.Rare;
                ResourceComboBox.ItemsSource = Enum.GetValues(typeof(ResourceType.RareResource));
            }
            else
            {
                Type = ResourceType.Type.Common;
                ResourceComboBox.ItemsSource = Enum.GetValues(typeof(ResourceType.CommonResource));
            }
            ResourceComboBox.SelectedItem = null;
            TypeName = Type.ToString();
        }
        private void UpdateResourceList()
        {
            ListViewCommonAsteroids.ItemsSource = commonAsteroidsList;
            ListViewRareAsteroids.ItemsSource = rareAsteroidsList;
        }
        private void DoubleTextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            // Use SelectionStart property to find the caret position.
            // Insert the previewed text into the existing text in the textbox.
            var fullText = textBox.Text.Insert(textBox.SelectionStart, e.Text);

            // If parsing is successful, set Handled to false
            e.Handled = !double.TryParse(fullText, out _);
        }
        private void NumericTextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            // Use SelectionStart property to find the caret position.
            // Insert the previewed text into the existing text in the textbox.
            var fullText = textBox.Text.Insert(textBox.SelectionStart, e.Text);

            // If parsing is successful, set Handled to false
            e.Handled = !int.TryParse(fullText, out _);
        }

        public void NewAsteroid()
        {
            commonAsteroidsList = new ObservableDictionary<ResourceType.CommonResource, AsteroidResource>();
            rareAsteroidsList = new ObservableDictionary<ResourceType.RareResource, AsteroidResource>();

            asteroid = new Asteroid();

            ResourceComboBox.SelectedItem = null;
            MinAmountTextBox.Text = null;
            MaxAmountTextBox.Text = null;
            TextBlockTitle.Text = "New Asteroid";
            ChanceTextBox.Text = null;

            isEditing = false;
            originalAsteroid = null;

            UpdateResourceList();
        }

        private void OnEditAsteroid(object commandParameter)
        {
            asteroid = (Asteroid) commandParameter;
            originalAsteroid = (Asteroid)commandParameter;

            foreach (KeyValuePair<string, AsteroidResource> commonEntry in asteroid.minMaxResources)
            {
                ResourceType.CommonResource res = (ResourceType.CommonResource)System.Enum.Parse(typeof(ResourceType.CommonResource), commonEntry.Key);
                if (commonAsteroidsList.ContainsKey(res))
                {
                    commonAsteroidsList[res] = commonEntry.Value;
                }
                else
                {
                    commonAsteroidsList.Add(res, commonEntry.Value);
                }
            }
            foreach (KeyValuePair<string, AsteroidResource> rareEntry in asteroid.minMaxRareResources)
            {
                ResourceType.RareResource res = (ResourceType.RareResource)System.Enum.Parse(typeof(ResourceType.RareResource), rareEntry.Key);
                if (rareAsteroidsList.ContainsKey(res))
                {
                    rareAsteroidsList[res] = rareEntry.Value;
                }
                else
                {
                    rareAsteroidsList.Add(res, rareEntry.Value);
                }
            }

            TextBlockTitle.Text = "Edit Asteroid";
            ResourceComboBox.SelectedItem = null;
            MinAmountTextBox.Text = null;
            MaxAmountTextBox.Text = null;
            ChanceTextBox.Text = asteroid.chance.ToString();

            isEditing = true;

            _editAsteroidCommand.InvokeCanExecuteChanged();
        }
        private bool CanEditAsteroid(object commandParameter)
        {
            return true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            isRare = false;
            ToogleResourceType(isRare);
            UpdateResourceList();
        }

        private void ToggleType_Click(object sender, RoutedEventArgs e)
        {
            ToogleResourceType((bool)(sender as ToggleButton).IsChecked);
        }

        private void DeleteCommonButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var c = ((KeyValuePair<ResourceType.CommonResource, AsteroidResource>) btn.DataContext).Key as ResourceType.CommonResource?;
            if (c != null) { 
                commonAsteroidsList.Remove((ResourceType.CommonResource) c);
            }
        }

        private void DeleteRareButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var d = ((KeyValuePair<ResourceType.RareResource, AsteroidResource>)btn.DataContext).Key as ResourceType.RareResource?;
            if (d != null)
            {
                rareAsteroidsList.Remove((ResourceType.RareResource) d);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceComboBox.SelectedItem == null)
            {
                MessageBox.Show("Resource must be defined!", "AsteroifFieldGenerator - Add Asteriod", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (MinAmountTextBox.Text == null || MinAmountTextBox.Text == "")
                {
                    MessageBox.Show("Min Amount must be defined!", "AsteroifFieldGenerator - Add Asteriod", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (MaxAmountTextBox.Text == null || MaxAmountTextBox.Text == "")
                    {
                        MessageBox.Show("Max Amount must be defined!", "AsteroifFieldGenerator - Add Asteriod", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        int valMin = int.Parse(MinAmountTextBox.Text);
                        int valMax = int.Parse(MaxAmountTextBox.Text);

                        if (valMin >= valMax)
                        {
                            MessageBox.Show("Min Amount must be less than the Max Amount!", "AsteroifFieldGenerator - Add Asteriod", MessageBoxButton.OK, MessageBoxImage.Error);
                        } else { 
                            if (ResourceComboBox.SelectedItem != null)
                            {
                                if (Type == ResourceType.Type.Common)
                                {
                                    if (commonAsteroidsList.ContainsKey((ResourceType.CommonResource)ResourceComboBox.SelectedItem))
                                    {
                                        commonAsteroidsList[(ResourceType.CommonResource)ResourceComboBox.SelectedItem] = new AsteroidResource(valMin, valMax);
                                    }
                                    else
                                    {
                                        commonAsteroidsList.Add((ResourceType.CommonResource)ResourceComboBox.SelectedItem, new AsteroidResource(valMin, valMax));
                                    }
                                }
                                else
                                {
                                    if (rareAsteroidsList.ContainsKey((ResourceType.RareResource)ResourceComboBox.SelectedItem))
                                    {
                                        rareAsteroidsList[(ResourceType.RareResource)ResourceComboBox.SelectedItem] = new AsteroidResource(valMin, valMax);
                                    }
                                    else
                                    {
                                        rareAsteroidsList.Add((ResourceType.RareResource)ResourceComboBox.SelectedItem, new AsteroidResource(valMin, valMax));
                                    }
                                }

                                UpdateResourceList();
                            }
                        }
                    }
                }
            }
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            switch (e.Parameter)
            {
                case "Save":
                    if (ChanceTextBox.Text == null || ChanceTextBox.Text == "") { 
                        asteroid.chance = 0;
                    } else { 
                        asteroid.chance = decimal.Parse(ChanceTextBox.Text);
                    }

                    foreach (KeyValuePair<ResourceType.CommonResource, AsteroidResource> commonEntry in commonAsteroidsList)
                    {
                        if (asteroid.minMaxResources.ContainsKey(commonEntry.Key.ToString())) {
                            asteroid.minMaxResources[commonEntry.Key.ToString()] = commonEntry.Value;
                        } else {
                            asteroid.minMaxResources.Add(commonEntry.Key.ToString(), commonEntry.Value);
                        }
                    }
                    foreach (KeyValuePair<ResourceType.RareResource, AsteroidResource> rareEntry in rareAsteroidsList)
                    {
                        if (asteroid.minMaxRareResources.ContainsKey(rareEntry.Key.ToString()))
                        {
                            asteroid.minMaxRareResources[rareEntry.Key.ToString()] = rareEntry.Value;
                        }
                        else
                        {
                            asteroid.minMaxRareResources.Add(rareEntry.Key.ToString(), rareEntry.Value);
                        }
                    }

                    if (asteroid.chance > 0) { 
                        if(asteroid.minMaxRareResources.Count > 0 || asteroid.minMaxResources.Count > 0)
                        {
                            if(isEditing)
                            {
                                mainViewModel.UpdateAsteroid(originalAsteroid, asteroid);
                            } else
                            {
                                mainViewModel.AddAsteroid(asteroid);
                            }
                            Transitioner.MovePreviousCommand.Execute(null, null);
                            NewAsteroid();
                        } else
                        {
                            MessageBox.Show("Can't add an empty Asteroid!", "AsteroifFieldGenerator - Save Asteroid in Field", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Can't add an Asteroid with 0 chance!", "AsteroifFieldGenerator - Save Asteroid in Field", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
                default:
                    Transitioner.MovePreviousCommand.Execute(null, null);
                    NewAsteroid();
                    break;
            }
        }
    }
}