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
    public delegate bool IterationEventHandler(int iteration, int best, int worst, double average, List<KindOfCollision> collisons);

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

                    Population.SelectPopulation();
                    var bestResult = Population.People.Select(x => x.BestValue).Max();
                    var worstResult = Population.People.Select(x => x.BestValue).Min();
                    var averageResult = (double)(Population.People.Sum(x => x.BestValue)) / Population.People.Count();
                    var collisions = Population.People.Select(x => x.KindOfLose.Convert()).ToList();
                    if (!IterationEvent(i, bestResult, worstResult, averageResult, collisions))
                        return null;

                }
                catch (Exception e)
                {

                }
            }

            var best = Population.GetTheBest();
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
                        person.KindOfLose = resultCollision.Convert();
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
