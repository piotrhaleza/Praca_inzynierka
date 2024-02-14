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

namespace SnakeGame.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties

        public Network Network { get; set; }
        public int Iteration { get; set; }
        public int Deaths { get; set; }
        public int Best { get; set; }
        #endregion

        public MainWindow()
        {
            Network = InitialzeNetwork();
            InitializeComponent();
        }

        private Network InitialzeNetwork()
        {
            var inputs = new Layer(9,1, LineralActivationFunc.Create());
            var hiddenfirsLayer = new Layer(16,2, LineralActivationFunc.Create());
            var hidden2firsLayer = new Layer(12, 3, LineralActivationFunc.Create());
            
            var output = new Layer(3,3);

            var list = new List<ILayer>() { inputs, hiddenfirsLayer, hidden2firsLayer, output };

            return  new Network(list, new InitXawierWages(), new InitZeroBiases(), 10);
        }

        private GameWorld _gameWorld;

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
                        _gameWorld.UpdateMovementDirection(MovementDirection.Up);
                        break;
                    case Key.A:
                        _gameWorld.UpdateMovementDirection(MovementDirection.Left);
                        break;
                    case Key.S:
                        _gameWorld.UpdateMovementDirection(MovementDirection.Down);
                        break;
                    case Key.D:
                        _gameWorld.UpdateMovementDirection(MovementDirection.Right);
                        break;
                }
        }

        private void RestartClick(object sender, RoutedEventArgs e)
        {
            Network = InitialzeNetwork();
            Iteration = 0; 
            Deaths = 0;
            Best = 0;
         
            _gameWorld = new GameWorld(this);
            GameWorld.Children.Clear();
            if (!_gameWorld.IsRunning)
            {
                _gameWorld.InitializeGame(Network);
                StartBtn.IsEnabled = false;
            }
        }

       

        private void StartClick(object sender, RoutedEventArgs e)
        {
            if (!_gameWorld.IsRunning)
            {
                _gameWorld.InitializeGame(Network);
                StartBtn.IsEnabled = false;
            }
        }

        private void OptionsClick(object sender, RoutedEventArgs e)
        {
            StartBtn.IsEnabled = !StartBtn.IsEnabled;
            RestartBtn.IsEnabled = !RestartBtn.IsEnabled;
            this.DialogHost.IsOpen = !this.DialogHost.IsOpen;
        }

        public void UpdateScore()
        {
            ApplesLbl.Content = $"i: {Iteration}";
            ScoreLbl.Content = $"lose: {Deaths}";
            LevelLbl.Content = $"best: {Best}";
        }
    }
}
