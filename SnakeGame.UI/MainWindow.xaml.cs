using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MachineLearning.Biases;
using MachineLearning.Wages;
using MachineLearning;
using MachineLearningsUi;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using SnakeGame.UI.Entities;
using MachineLearningWpfUI;
using MachineLearingInterfaces;
using MachineLearingInterfaces.ActivationFunc;
using MachineLearning.Funcs;
using SnakeGame.UI.Enums;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using SnakeGame.UI.ViewModels;
using System.IO;
using System.Text.Json;
using System.Windows.Documents;
using System.Linq;
using System.Configuration;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;

namespace SnakeGame.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties
        public MainWindowViewModel ViewModel { get; set; }
        public int Iteration { get; set; }
        public int Deaths { get; set; }
        public int Best { get; set; }

        #endregion

        #region Constructors
        public MainWindow()
        {
            ViewModel = new MainWindowViewModel(this);
            this.DataContext = ViewModel;
        }

        #endregion

        #region Clicks

        private void StartClick(object sender, RoutedEventArgs e)
        {
            this.DialogHost.IsOpen = true;
            StartGameHost.Visibility = Visibility.Visible;
            StartBtn.IsEnabled = false;
        }
        private void StopClick(object sender, RoutedEventArgs e)
        {
            StopGame();
        }
        public void StopGame()
        {
            StartBtn.IsEnabled = true;
            OptionsBtn.IsEnabled = true;
            TrainingBtn.IsEnabled = true;

            this.DialogHost.IsOpen = true;
            this.ViewModel.UpdateResultTestingSerries();

            this.ResultGameHost.Visibility = Visibility.Visible;

            this._gameWorld.Stop();
        }
        private async void StartGameClick(object sender, RoutedEventArgs e)
        {
            var filePath = ViewModel.PathToReadFile;

            this.DialogHost.IsOpen = false;
            StartGameHost.Visibility = Visibility.Collapsed;
            _gameWorld.InitlizeNetwork(filePath);
            await _gameWorld.InitializeGame(ViewModel.SelectedDifficulty, ViewModel.IterationOfTestingGame);

            StartBtn.IsEnabled = false;
        }
        private void CancelGameClick(object sender, RoutedEventArgs e)
        {
            this.DialogHost.IsOpen = false;
            StartGameHost.Visibility = Visibility.Collapsed;
            StartBtn.IsEnabled = true;
        }
        private void EndTestingClick(object sender, RoutedEventArgs e)
        {
            StartBtn.IsEnabled = true;

            this.DialogHost.IsOpen = false;
            this.ResultGameHost.Visibility = Visibility.Collapsed;

        }

        private void OptionsClick(object sender, RoutedEventArgs e)
        {
            StartBtn.IsEnabled = false;
            OptionsBtn.IsEnabled = false;
            TrainingBtn.IsEnabled = false;
            this.DialogHost.IsOpen = true;
            this.OptionsHost.Visibility = Visibility.Visible;

        }
        private void SaveClick(object sender, RoutedEventArgs e)
        {
            StartBtn.IsEnabled = true;
            OptionsBtn.IsEnabled = true;
            TrainingBtn.IsEnabled = true;
            this.DialogHost.IsOpen = false;

            this.OptionsHost.Visibility = Visibility.Collapsed;

        }
        private async void StopTrainingClick(object sender, RoutedEventArgs e)
        {
            this.DialogHost.IsOpen = false;
            this.ViewModel.ContinueLearning = false;
            StartBtn.IsEnabled = true;
            OptionsBtn.IsEnabled = true;
            TrainingGameHost.Visibility = Visibility.Collapsed;
            StartBtn.IsEnabled = true;
        }
        private async void StopTrainingClick2(object sender, RoutedEventArgs e)
        {
            this.DialogHost.IsOpen = false;
            StartBtn.IsEnabled = true;
            OptionsBtn.IsEnabled = true;
            TrainingGameHost.Visibility = Visibility.Collapsed;
            StartBtn.IsEnabled = true;
        }
        private async void StartTrainingClick(object sender, RoutedEventArgs e)
        {
            this.DialogHost.IsOpen = true;
            StartBtn.IsEnabled = false;
            OptionsBtn.IsEnabled = false;
            TrainingGameHost.Visibility = Visibility.Visible;
            StartBtn.IsEnabled = false;
        }
        private async void StartTraingRealClick(object sender, RoutedEventArgs e)
        {
            var oldpath = ViewModel.PathToFile;

            this.ViewModel.ContinueLearning = true;
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            var network = InitialzeNetwork();
            var package = new GeneticPackage()
            {
                CountOfPeopleInPopulation = ViewModel.CountOfPeopleInPoulation,
                LentghtOfChromoson = network.GetCountOfWages(),
                PropablityOfCross = ((double)ViewModel.PropablityOfCross / 100),
                PropablityOfMutaion = ((double)ViewModel.PropablityOfMutaion / 100),
            };

            IList<double> wages;

            if (ViewModel.IsPropagation)
                wages = await ViewModel.StartPropagationTraning(InitialzeNetwork(), ViewModel.IterationOfPropagation);
            else
                wages = await ViewModel.StartGeneticTraning(InitialzeZeroNetwork(), package, ViewModel.CountOfRepeat);

            if (this.ViewModel.ContinueLearning == false)
                return;

            if (wages != null)
            {
                NetworkToSaveViewModel model = new NetworkToSaveViewModel()
                {
                    Layers = GetLayersToSave(),
                    Wages = wages.ToList()
                };
                string jsonString = JsonSerializer.Serialize(model);
                File.WriteAllText(ViewModel.PathToFile, jsonString);
            }
            this.ViewModel.ContinueLearning = true;
            StartBtn.IsEnabled = true;
            OptionsBtn.IsEnabled = true;
            StartBtn.IsEnabled = true;

        }

        private List<int> GetLayersToSave()
        {
            var result = new List<int>();
            foreach (var item in ViewModel.Layers)
            {
                result.Add(item.CountOfNeurons);
            }
            return result;
        }

        private void BtnFileOpen_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    var file = fileDialog.FileName;
                    PathToFile.Text = file;
                    PathToFile.ToolTip = file;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    PathToFile.Text = null;
                    PathToFile.ToolTip = null;
                    break;
            }
        }
        private void BtnReadLoadWages_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    var file = fileDialog.FileName;
                    ViewModel.PathToReadFile = file;
                    FileToRead.Text = file;
                    FileToRead.ToolTip = file;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    ViewModel.PathToReadFile = null;
                    FileToRead.Text = null;
                    FileToRead.ToolTip = null;
                    break;
            }
        }
        private void BtnReadFileOpen_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    var file = fileDialog.FileName;
                    ViewModel.PathToReadFile = file;
                    FileToRead.Text = file;
                    FileToRead.ToolTip = file;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    ViewModel.PathToReadFile = null;
                    FileToRead.Text = null;
                    FileToRead.ToolTip = null;
                    break;
            }
        }
        #endregion

        #region Handle Events
        protected override void OnContentRendered(EventArgs e)
        {
            _gameWorld = new GameWorld(this);
            base.OnContentRendered(e);
        }
        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (_gameWorld != null)
                switch (e.Key)
                {
                    case Key.W:
                        _gameWorld.UpdateMovementDirection(_gameWorld.Snake, MovementDirection.Up);
                        break;
                    case Key.A:
                        _gameWorld.UpdateMovementDirection(_gameWorld.Snake, MovementDirection.Left);
                        break;
                    case Key.S:
                        _gameWorld.UpdateMovementDirection(_gameWorld.Snake, MovementDirection.Down);
                        break;
                    case Key.D:
                        _gameWorld.UpdateMovementDirection(_gameWorld.Snake, MovementDirection.Right);
                        break;
                }
        }
        public void UpdateScore()
        {
            Iteration++;
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            System.Windows.Controls.TextBox textbox = (System.Windows.Controls.TextBox)sender;

            if (int.TryParse(textbox.Text, out int countOfLayer))
                ViewModel.CreateNewLayers(countOfLayer);
        }

        #endregion

        #region Private Methods
        private Network InitialzeNetwork()
        {
            var list = new List<ILayer>();

            int i = 1;
            foreach (var item in ViewModel.Layers)
            {
                list.Add(new Layer(item.CountOfNeurons, i, LineralActivationFunc.Create()));
                i++;
            }

            return new Network(list, new InitXawierWages(), new InitZeroBiases());
        }
        private Network InitialzeZeroNetwork()
        {
            var list = new List<ILayer>();

            int i = 1;
            foreach (var item in ViewModel.Layers)
            {
                list.Add(new Layer(item.CountOfNeurons, i, LineralActivationFunc.Create()));
                i++;
            }

            return new Network(list, new InitZeroWages(), new InitZeroBiases());
        }

        #endregion

        internal GameWorld _gameWorld { get; set; }






    }
}
