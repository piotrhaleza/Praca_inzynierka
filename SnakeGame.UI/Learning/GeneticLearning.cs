using Genetic.Interfaces;
using SnakeGame.UI.Entities;
using SnakeGame.UI.Enums;
using SnakeGame.UI.Helpers;
using snakeGame.UI.Learning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Genetic.SimpleGenetics;
using Genetic.Operators.Cross;
using Genetic.Operators.Mutate;
using Genetic.Operators.Selection;
using MachineLearning;
using MachineLearingInterfaces;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace SnakeGame.UI.Learning
{
    public delegate void IterationEventHandler(int iteration, int best);

    public class GeneticLearning
    {
        #region Properties

        public event IterationEventHandler IterationEvent;
        public SnakeGeneticPopulation Population { get; set; }
        public ICopyNetwork CopierOfNetwork { get; set; }
        public int ElementSize { get; set; }
        public int RowCount { get; set; }
        public int ColumnCount { get; set; }
        public int NumberOfIteration { get; set; }
        public GameDifficulty Difficulty { get; set; }
        #endregion

        #region Constructors
        public GeneticLearning(ICopyNetwork initNetwork, SnakeGeneticPopulation population, int elementSize, int rowCount, int columnCount, int numberOfIteration,GameDifficulty gameDifficulty)
        {
            CopierOfNetwork = initNetwork;
            Population = population;
            ElementSize = elementSize;
            RowCount = rowCount;
            ColumnCount = columnCount;
            NumberOfIteration = numberOfIteration;
            Difficulty = gameDifficulty;
        }

        #endregion

        public async Task<IPerson> GetBestPerson()
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < NumberOfIteration; i++)
            {
                try
                {
                    Population.CrossPopulation();
                    Population.MutatePopulation();

                    foreach (var person in Population.People)
                    {
                        tasks.Add(BeginGameAsync(person));
                    }
                    await Task.WhenAll(tasks.ToArray());
                    tasks.Clear();

                    Population._select.SelectPopulation(Population, null, 0);
                    var e = string.Join(",", Population.People.Select(x => x.BestValue).OrderBy(x => x));
                    IterationEvent(i, Population.People.Select(x => x.BestValue).Max());
                }
                catch (Exception e)
                {

                }
            }

            var best = Population.People.FirstOrDefault(z => z.BestValue == Population.People.Max(x => x.BestValue));
            return best;
        }
        public Task<bool> BeginGameAsync(IPerson person)
        {
            return Task.Run(() => BeginGame(person));
        }

        public bool BeginGame(IPerson person)
        {
            try
            {
                person.BestValue = 0;

                var snake = new Snake(ElementSize);
                snake.PositionFirstElement(5, 5, MovementDirection.Right);

                Apple apple = null;
                apple = AppleHelper.CreateApple(snake, ElementSize, RowCount, ColumnCount, Difficulty); ;

                var network = CopierOfNetwork.Copy();
                network.SetWages(person.Value);

                int countOfRepeat = 1;


                bool again = true;
                while (again)
                {
                    countOfRepeat++;
                    var inputs = InputsGetter.GetInputs(snake, apple);
                    var result = network.ForwardPropagation(inputs.Values.ToList());
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
                        //string resultText = "O------------O\n";

                        //for (int i = -1; i < 11; i++)
                        //{
                        //    resultText += "|";
                        //    for (int z = -1; z < 11; z++)
                        //        if (apple.X == z && apple.Y == i)
                        //            resultText += "A";
                        //        else if (snake.Elements.Any(x => x.X == z && x.Y == i))
                        //        {
                        //            int number = 1;
                        //            foreach (var item in snake.Elements)
                        //            {
                        //                if (item.X == z && item.Y == i)
                        //                {
                        //                    if (number < 10)
                        //                        resultText += number;
                        //                    else
                        //                        resultText += "X";
                        //                    break;
                        //                }

                        //                number++;
                        //            }
                        //        }
                        //        else
                        //            resultText += "-";

                        //    resultText += "|\n";
                        //}
                        //resultText += "O------------O";
                        countOfRepeat = 0;
                        again = false;
                        person.BestValue += snake.Elements.Count();
                    }
                }
            }
            catch (Exception e)
            {
            }
            return true;
        }
    }
}
