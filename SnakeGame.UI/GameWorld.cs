using System;
using SnakeGame.UI.Entities;
using System.Windows.Threading;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System.Linq;
using MachineLearning;
using System.Collections.Generic;
using MachineLearning.Wages;
using MachineLearning.Biases;
using System.Windows.Forms;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows;
using System.Windows.Documents;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Xml.Linq;
using System.IO;
using SnakeGame.UI.Enums;
using SnakeGame.UI.Helpers;
using snakeGame.UI.Learning;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using SnakeGame.UI.Learning;
using Genetic.SimpleGenetics;
using Genetic.Operators.Cross;
using Genetic.Operators.Mutate;
using Genetic.Operators.Selection;
using Genetic.Interfaces;
using MachineLearingInterfaces;
using MachineLearning.Funcs;
using static System.Windows.Forms.AxHost;
using System.Threading.Tasks;
using System.Data;
using MachineLearning.Networks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Text.Json;
using SnakeGame.UI.ViewModels;

namespace SnakeGame.UI
{
    class GameWorld
    {

        private MainWindow parent;

        public static int ElementSize { get; set; } = 50;
        public static int ColumnCount { get; set; }
        public static int RowCount { get; set; }
        public static double GameAreaWidth { get; set; }
        public static double GameAreaHeight { get; set; }

        Random _randoTron;
        INetwork Network;
        public Apple Apple { get; set; }
        public Snake Snake { get; set; }
        DispatcherTimer _gameLoopTimer;
        public bool IsRunning { get; set; }
        SnakeGeneticPopulation population { get; set; }
        ICopyNetwork initialzeNetwork { get; set; }
        GameDifficulty Difficulty { get; set; }
        #region Constructors
        public GameWorld(MainWindow mainWindow)
        {
            this.parent = mainWindow;
            _randoTron = new Random(DateTime.Now.Millisecond / (DateTime.Now.Second + 1));
        }
        #endregion

        #region Initialize

        public async Task InitializeGame(GameDifficulty difficulty)
        {
            InitializeArea();
            DrawGameWorld();
            InitializeSnake();
            InitializeApple();
            InitializeTimer();
            IsRunning = true;
            Difficulty = difficulty;
        }
        public void Stop()
        {
            parent.GameWorld.Children.Clear();
            if (_gameLoopTimer != null)
                _gameLoopTimer.Stop();
        }
        public void InitlizeGenticsNetwork(string path)
        {
            Network = JsonNetworkReader.ReadNetwork(path);
            SnakeElement.UiIs = true;
          
        }

        public void ChangeInterval(double time)
        {
            if(_gameLoopTimer.IsEnabled && time == 0.25)
            {
                _gameLoopTimer.Stop();
            }

            if (!_gameLoopTimer.IsEnabled && time != 0.25)
            {
                _gameLoopTimer.Start();
            }

            var interval = TimeSpan.FromSeconds(time).Ticks;
            if (_gameLoopTimer != null)
                _gameLoopTimer.Interval = TimeSpan.FromTicks(interval);
        }
        private void InitializeTimer()
        {
            var interval = TimeSpan.FromSeconds(0.1).Ticks;
            _gameLoopTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromTicks(interval)
            };
            _gameLoopTimer.Tick += MainGameLoop;
            _gameLoopTimer.Start();
        }
        private void InitializeSnake()
        {
            Snake = new Snake(ElementSize);
            Snake.PositionFirstElement(4, 4, MovementDirection.Right);
        }
        private void InitializeApple()
        {
            Apple = AppleHelper.CreateApple(Snake, ElementSize, RowCount, ColumnCount,Difficulty); ;
        }
        private void InitializeArea()
        {
            GameAreaWidth = parent.GameWorld.ActualWidth;
            GameAreaHeight = parent.GameWorld.ActualHeight;
            ColumnCount = (int)GameAreaWidth / ElementSize;
            RowCount = (int)GameAreaHeight / ElementSize;
        }
        #endregion

        private void MainGameLoop(object sender, EventArgs e)
        {

            var inputs = InputsGetter.GetInputs(Snake, Apple);
            var expected = OutputGetter.GetExpectedOutputs(inputs);
            var result = Network.ForwardPropagation(inputs.Values.ToList());
            //network.updatewages();
            var direct = DirectionHelper.GetDirect(result, Snake.MovementDirection);
            maxRepat++;
            UpdateMovementDirection(Snake, direct);

            if (GameLoop())
            {

            }
        }
        public int maxRepat = 0;
        public bool GameLoop()
        {
            bool result = false;


            Snake.MoveSnake();
            parent.UpdateScore();
            result = CheckCollision();

            if (Apple == null)
                Apple = AppleHelper.CreateApple(Snake, ElementSize, RowCount, ColumnCount,Difficulty); ;

            Draw();

            return result;
        }


        #region Draw

        private void Draw()
        {
            DrawApple();
            DrawSnake();
          
        }

        public void DrawGameWorld()
        {
            for (int i = 0; i <= 10; i++)
                parent.GameWorld.Children.Add(GenerateHorizontalWorldLine(i));
            for (int j = 0; j <= 10; j++)
                parent.GameWorld.Children.Add(GenerateVerticalWorldLine(j));
        }
        private void DrawSnake()
        {
            foreach (var snakeElement in Snake.Elements)
            {
                if (!parent.GameWorld.Children.Contains(snakeElement.UIElement))
                    parent.GameWorld.Children.Add(snakeElement.UIElement);
                Canvas.SetLeft(snakeElement.UIElement, snakeElement.RealX + 2);
                Canvas.SetTop(snakeElement.UIElement, snakeElement.RealY + 2);
            }
        }

        private void DrawApple()
        {
            if (Snake.Elements.Any(element => element.X == Apple.X && element.Y == Apple.Y))
            {
                (Apple.UIElement as Rectangle).Fill = new SolidColorBrush(Color.FromArgb(100, 255, 0, 0));
            }
            else
            {
                (Apple.UIElement as Rectangle).Fill = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            }

            if (!parent.GameWorld.Children.Contains(Apple.UIElement))
            parent.GameWorld.Children.Add(Apple.UIElement);
            Canvas.SetLeft(Apple.UIElement, Apple.RealX + 2);
            Canvas.SetTop(Apple.UIElement, Apple.RealY + 2);
        }
        private Line GenerateVerticalWorldLine(int j)
        {
            return new Line
            {
                Stroke = Brushes.Black,
                X1 = j * ElementSize,
                Y1 = 0,
                X2 = j * ElementSize,
                Y2 = ElementSize * RowCount
            };
        }

        private Line GenerateHorizontalWorldLine(int i)
        {
            return new Line
            {
                Stroke = Brushes.Black,
                X1 = 0,
                Y1 = i * ElementSize,
                X2 = ElementSize * ColumnCount,
                Y2 = i * ElementSize
            };
        }
        #endregion

        #region Collision
        private bool CheckCollision()
        {


            if (Snake != null)
            {
                if (CollisionHelper.CollisionWithApple(Snake, Apple))
                    ProcessCollisionWithApple();
                if (maxRepat > 200 || Snake.CollisionWithSelf() || CollisionHelper.CollisionWithWorldBounds(Snake, ElementSize))
                {
                    maxRepat = 0;
                    parent.Deaths++;
                    parent.Best = Snake.Elements.Count();
                    Snake = new Snake(ElementSize);
                    Snake.PositionFirstElement(ColumnCount / 2, RowCount / 2, MovementDirection.Left);
                    parent.GameWorld.Children.Clear();
                    if (!(Apple == null || Snake == null || Snake.Head == null))
                        parent.GameWorld.Children.Remove(Apple.UIElement);
                    Apple = null;

                    DrawGameWorld();
                    return true;
                }
            }
            return false;
        }

        private bool CollisionWithApple(Snake snake, Apple apple)
        {
            if (apple == null || snake == null || snake.Head == null)
                return false;
            SnakeElement head = snake.Head;
            return (head.RealX == apple.RealX && head.RealY == apple.RealY);
        }

        private void ProcessCollisionWithApple()
        {
            parent.GameWorld.Children.Remove(Apple.UIElement);
            maxRepat = 0;
            Apple = null;
            Snake.Grow();
        }

        #endregion

        #region Update
        internal void UpdateMovementDirection(Snake Snake, MovementDirection movementDirection)
        {
            if (Snake != null)
                Snake.UpdateMovementDirection(movementDirection);
        }
        #endregion
    }

}
