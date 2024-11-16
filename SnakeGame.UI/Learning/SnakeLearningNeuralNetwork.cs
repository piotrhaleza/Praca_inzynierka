using MachineLearning;
using SnakeGame.UI.Entities;
using SnakeGame.UI.Helpers;
using snakeGame.UI.Learning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Genetic.Interfaces;
using SnakeGame.UI.Enums;
using Genetic.SimpleGenetics;
using MachineLearingInterfaces;

namespace SnakeGame.UI.Learning
{
    public  class SnakeLearningNeuralNetwork
    {
        #region Properties

        public event IterationEventHandler IterationEvent;
        public SnakeGeneticPopulation Population { get; set; }
        public Network Network { get; set; }
        public int ElementSize { get; set; }
        public int RowCount { get; set; }
        public int ColumnCount { get; set; }
        public int NumberOfIteration { get; set; }
        public GameDifficulty Difficulty { get; set; }
        #endregion

        #region Constructors
        public SnakeLearningNeuralNetwork(Network network, int elementSize, int rowCount, int columnCount, int numberOfIteration,GameDifficulty difficulty)
        {
            Network = network;
            ElementSize = elementSize;
            RowCount = rowCount;
            ColumnCount = columnCount;
            NumberOfIteration = numberOfIteration;
            Difficulty = difficulty;
        }

        #endregion

        public int NetworkLearn()
        {

            var snake = new Snake(ElementSize);
            snake.PositionFirstElement(5, 5, MovementDirection.Right);

            Apple apple = null;
            apple = AppleHelper.CreateApple(snake, ElementSize, RowCount, ColumnCount, Difficulty); ;

            int countOfRepeat = 1;

            bool again = true;
            while (again)
            {
                countOfRepeat++;
                var inputs = InputsGetter.GetInputs(snake, apple);
                var expected = OutputGetter.GetExpectedOutputs(inputs);
                var result = Network.TrainModel(inputs.Values.ToList(), expected);
                Network.UpdateWages();
                var oldDirect = snake.MovementDirection;
                var direct = DirectionHelper.GetDirect(result, snake.MovementDirection);

                snake.UpdateMovementDirection(direct);
                snake.MoveSnake();


                var resultCollision = CollisionHelper.SnakeHadCollision(snake, apple, ElementSize, countOfRepeat);

                if (resultCollision == KindOfCollision.AppleCollision)
                {
                    countOfRepeat = 0;
                    apple = AppleHelper.CreateApple(snake, ElementSize, RowCount, ColumnCount, Difficulty); ;
                    snake.Grow();
                }
                else if (resultCollision != KindOfCollision.NoCollision)
                {
                    countOfRepeat = 0;
                    again = false;
                }
            }

            return snake.Elements.Count();
        }
    }
}
