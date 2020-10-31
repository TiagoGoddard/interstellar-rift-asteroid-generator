using AsteroidFieldGenerator.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Input;


namespace AsteroidFieldGenerator
{
    class MainViewModel : ViewModelBase
    {

        private static MainViewModel _instance;

        public static MainViewModel GetInstance()
        {
            if (_instance == null)
            {
                _instance = new MainViewModel();
            }
            return _instance;
        }

        private ObservableCollection<Asteroid> _asteroids = new ObservableCollection<Asteroid>();
        public ObservableCollection<Asteroid> Asteroids
        {
            get => _asteroids;
            set => SetProperty(ref _asteroids, value);
        }

        private string _filePath;
        private string _fileContent;
        private bool _fileEdited;
        public string FilePath
        {
            get => _filePath;
            set => SetProperty(ref _filePath, value);
        }
        public string FileContent
        {
            get => _fileContent;
            set => SetProperty(ref _fileContent, value);
        }
        public bool FileEdited
        {
            get => _fileEdited;
            set => SetProperty(ref _fileEdited, value);
        }
        public bool IsEdited()
        {
            return FileEdited;
        }

        private readonly DelegateCommand _openFileCommand;
        public ICommand OpenFileCommand => _openFileCommand;

        private readonly DelegateCommand _saveFileCommand;
        public ICommand SaveFileCommand => _saveFileCommand;

        private readonly DelegateCommand _saveAsFileCommand;
        public ICommand SaveAsFileCommand => _saveAsFileCommand;

        private readonly DelegateCommand _newFileCommand;
        public ICommand NewFileCommand => _newFileCommand;

        private MainViewModel()
        {
            _openFileCommand = new DelegateCommand(OnOpenFile, CanOpenFile);
            _saveFileCommand = new DelegateCommand(OnSaveFile, CanSaveFile);
            _saveAsFileCommand = new DelegateCommand(OnSaveAsFile, CanSaveAsFile);
            _newFileCommand = new DelegateCommand(OnNewFile, CanNewFile);
        }

        private void OnOpenFile(object commandParameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                //Get the path of specified file
                FilePath = openFileDialog.FileName;

                //Read the contents of the file into a stream
                var fileStream = openFileDialog.OpenFile();
                StreamReader reader = new StreamReader(fileStream);
                FileContent = reader.ReadToEnd();

                try
                {
                    Asteroids = JsonSerializer.Deserialize<ObservableCollection<Asteroid>>(FileContent);
                    FileEdited = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid JSON file!", "AsteroifFieldGenerator - File Import", MessageBoxButton.OK, MessageBoxImage.Error);
                    Console.WriteLine("Exception caught: {0}", ex);
                }
            }

            _openFileCommand.InvokeCanExecuteChanged();
        }
        private bool CanOpenFile(object commandParameter)
        {
            return true;
        }

        private void OnNewFile(object commandParameter)
        {
            if (FileEdited == true)
            {
                if (MessageBox.Show("This asteroid field was edited and it's not saved, really create a new one?", "Confirm delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Asteroids = new ObservableCollection<Asteroid>();
                }
            }
            else
            {
                Asteroids = new ObservableCollection<Asteroid>();
            }

            _newFileCommand.InvokeCanExecuteChanged();
        }
        private bool CanNewFile(object commandParameter)
        {
            return true;
        }

        public void saveAsFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                RestoreDirectory = true
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                FilePath = saveFileDialog.FileName;
                saveFile();
            }
        }
        public void saveFile()
        {
            if (String.Equals(FilePath, string.Empty))
            {
                saveAsFile();
            }
            else
            {
                FileContent = JsonSerializer.Serialize<ObservableCollection<Asteroid>>(Asteroids);
                File.WriteAllText(FilePath, FileContent);
                FileEdited = false;
            }
        }

        private void OnSaveFile(object commandParameter)
        {
            saveFile();
            _saveFileCommand.InvokeCanExecuteChanged();
        }
        private bool CanSaveFile(object commandParameter)
        {
            return true;
        }

        private void OnSaveAsFile(object commandParameter)
        {
            saveAsFile();
            _saveAsFileCommand.InvokeCanExecuteChanged();
        }
        private bool CanSaveAsFile(object commandParameter)
        {
            return true;
        }

        public void RemoveAsteroid(int position)
        {
            FileEdited = true;
            Asteroids.RemoveAt(position);
        }

        public void UpdateAsteroid(Asteroid asteroid, int position)
        {
            FileEdited = true;
            Asteroids[position] = asteroid;
        }

        public void AddAsteroid(Asteroid asteroid)
        {
            FileEdited = true;
            Asteroids.Add(asteroid);
        }

        public int GetAsteroidCount()
        {
            return Asteroids.Count;
        }
        public Asteroid GetAsteroid(int position)
        {
            return Asteroids[position];
        }
    }
}
