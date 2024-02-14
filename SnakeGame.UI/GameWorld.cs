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

namespace SnakeGame.UI
{
    class GameWorld
    {
        private MainWindow parent;

        public int ElementSize { get; private set; }
        public int ColumnCount { get; private set; }
        public int RowCount { get; private set; }
        public double GameAreaWidth { get; private set; }
        public double GameAreaHeight { get; private set; }

        Random _randoTron;
        Network Network;
        public Apple Apple { get; set; }
        public Snake Snake { get; set; }
        DispatcherTimer _gameLoopTimer;
        public bool IsRunning { get; set; }

        #region Constructors
        public GameWorld(MainWindow mainWindow)
        {
            this.parent = mainWindow;
            _randoTron = new Random(DateTime.Now.Millisecond / (DateTime.Now.Second + 1));
        }
        #endregion

        #region Initialize
        public void InitializeGame(Network network)
        {
            InitializeArea();
            DrawGameWorld();
            InitializeSnake();
            InitializeTimer();
            Network = network;
            IsRunning = true;
        }
        private void InitializeTimer()
        {
            var interval = TimeSpan.FromSeconds(0.001).Ticks;
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
            Snake.PositionFirstElement(ColumnCount, RowCount, MovementDirection.Right);
        }
        private void InitializeArea()
        {
            ElementSize = 50;
            GameAreaWidth = parent.GameWorld.ActualWidth;
            GameAreaHeight = parent.GameWorld.ActualHeight;
            ColumnCount = (int)GameAreaWidth / ElementSize;
            RowCount = (int)GameAreaHeight / ElementSize;
        }
        #endregion

        private void MainGameLoop(object sender, EventArgs e)
        {
            if (Apple != null)
            {
                NetworkLearn();
                parent.UpdateScore();
                CheckCollision();
                parent.Iteration++;
            }
            CreateApple();
            Draw();
        }

        private void NetworkLearn()
        {
            var previousdirect = Snake.MovementDirection;
            UpdateMovementDirection(previousdirect);
            var inputs = InputsGetter.GetInputs(Snake, Apple);
            var expected = GetNewExpected(inputs);
            Network.TrainModel(inputs.Values.ToList(), expected);
            var result = Network.TrainModel(inputs.Values.ToList(), expected);
            var direct = DirectionHelper.GetDirect(result, Snake.MovementDirection);
            UpdateMovementDirection(direct);
            Snake.MoveSnake();
            if (Snake.CollisionWithSelf() || CollisionWithWorldBounds())
            {
                var c = Network.TrainModel(inputs.Values.ToList(), expected);
            }
        }

      
       
        private List<double> GetNewExpected(Dictionary<SnakeInputs, double> inputs)
        {
            List<double> result = new List<double> { 0.5, 0.5, 0.5 };

            if (inputs[SnakeInputs.LeftToWall] == 1)
            {
                result[0] = 0;
                result[2] = result[2] != 0 ? 0.6 : 0;
            }
            if (inputs[SnakeInputs.ForwardToWall] == 1)
            {
                result[1] = 0;
            }
            if (inputs[SnakeInputs.RightToWall] == 1)
            {
                result[2] = 0;
                result[0] = result[0] !=0? 0.6 :0;
            }

            if (inputs[SnakeInputs.LeftToApple] == 1)
            {
                result[0] = 1;
            }
            else if (inputs[SnakeInputs.ForwardToApple] == 1)
            {
                result[1] = 1;
            }
            else if (inputs[SnakeInputs.RightToApple] == 1)
            {
                result[2] = result[2] != 0 ? 1 : 0; ;
            }

            if (inputs[SnakeInputs.LeftToSnake] ==1)
            {
                result[0] = 0;
            }
            if (inputs[SnakeInputs.ForwardToSnake] == 1)
            {
                result[1] = 0;
            }
            if (inputs[SnakeInputs.RightToSnake] == 1)
            {
                result[2] = 0;
            }

            if (inputs[SnakeInputs.LeftToWall] == 0.5 && !(result[1] == 0 && result[2] ==0))
            {
                result[0] = 0;
                result[2] = result[2] != 0 ? 0.6 : 0;
            }
            if (inputs[SnakeInputs.ForwardToWall] == 0.5 && !(result[0] == 0 && result[2] == 0))
            {
                result[1] = 0;
            }
            if (inputs[SnakeInputs.RightToWall] == 0.5 && !(result[1] == 0 && result[0] == 0))
            {
                result[2] = 0;
                result[0] = result[0] != 0 ? 0.6 : 0;
            }

            if (!result.Any(x => x == 1))
            {
                if (result[1] == 0)
                {
                    var same = result[0] == result[2];
                    if (same)
                    {
                        Random random = new Random();
                        var exp = random.Next(0, 1);

                        if (exp == 1)
                            result[0] = 1;
                        else
                            result[2] = 1;
                    }
                    else
                    {
                        if (result[0] > result[2])
                            result[0] = 1;
                        else
                            result[2] = 1;
                    }
                }
                else if(result.Any(x=> x == 0.6))
                {

                    if (result[0] > result[2])
                        result[0] = 1;
                    else
                        result[2] = 1;
                }
                else
                {
                    result[1] = 1;
                }

            }


            for (int i = 0; i < result.Count(); i++)
                result[i] = result[i] == 0.5 ? 0 : result[i];
           

            return result;
        }

      

        #region Draw

        private void Draw()
        {
            DrawSnake();
            DrawApple();
        }

        public void DrawGameWorld()
        {
            int i = 0;
            for (; i < 10; i++)
                parent.GameWorld.Children.Add(GenerateHorizontalWorldLine(i));
            int j = 0;
            for (; j < 10; j++)
                parent.GameWorld.Children.Add(GenerateVerticalWorldLine(j));
            parent.GameWorld.Children.Add(GenerateVerticalWorldLine(j));
            parent.GameWorld.Children.Add(GenerateHorizontalWorldLine(i));
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
        private void CheckCollision()
        {
            if (CollisionWithApple())
                ProcessCollisionWithApple();
            if (Snake.CollisionWithSelf() || CollisionWithWorldBounds())
            {
                parent.Deaths++;
                parent.Best = parent.Best < Snake.Elements.Count() ? Snake.Elements.Count() : parent.Best;

                Snake = new Snake(ElementSize);
                Snake.PositionFirstElement(ColumnCount / 2, RowCount / 2, MovementDirection.Left);
                parent.GameWorld.Children.Clear();
                if (!(Apple == null || Snake == null || Snake.Head == null))
                    parent.GameWorld.Children.Remove(Apple.UIElement);
                Apple = null;

                DrawGameWorld();
            }
        }

        private bool CollisionWithApple()
        {
            if (Apple == null || Snake == null || Snake.Head == null)
                return false;
            SnakeElement head = Snake.Head;
            return (head.RealX == Apple.RealX && head.RealY == Apple.RealY);
        }

        private void ProcessCollisionWithApple()
        {
            parent.GameWorld.Children.Remove(Apple.UIElement);
            Apple = null;
            Snake.Grow();
        }

       
        private bool CollisionWithWorldBounds()
        {
            if (Snake == null || Snake.Head == null)
                return false;
            var snakeHead = Snake.Head;
            return (snakeHead.RealX > GameAreaWidth - ElementSize ||
                snakeHead.RealY > GameAreaHeight - ElementSize ||
                snakeHead.RealX < 0 || snakeHead.RealY < 0);
        }

        #endregion

        #region Update
        private void CreateApple()
        {
            if (Apple != null)
                return;

            var x = _randoTron.Next(1, RowCount-1);
            var y = _randoTron.Next(1, ColumnCount-1);

            while(Snake.Elements.Any(z=>z.X -1 == x && z.Y-1 == y))
            {
                x++;
                if (x == 9)
                {
                    y++;
                    x = 1;
                }
                if (y == 9)
                {
                    y=1;
                }

            }
            Apple = new Apple(ElementSize)
            {
                RealX = x * ElementSize,
                RealY = y * ElementSize
            };
        }

        internal void UpdateMovementDirection(MovementDirection movementDirection)
        {
            if (Snake != null)
                Snake.UpdateMovementDirection(movementDirection);
        }
        #endregion
    }

}
