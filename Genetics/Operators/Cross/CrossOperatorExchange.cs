using Genetic.Helpers;
using Genetic.Interfaces;
using Genetic.SimpleGenetics;
using GeneticInterfaces.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic.Operators.Cross
{
    public class CrossOperatorTwoPointCrossover : ICrossOperator
    {
        public void Cross(IPopulation population, double propability)
        {
            var pairs = PairHelper.GetPairs(population.People);
            Random rand = new Random();

            foreach (var pair in pairs)
            {
                bool cross = rand.NextDouble() <= propability;

                if (cross)
                {
                    var index1 = rand.Next((int)pair.first.Value.Count() - 1); 
                    var index2 = rand.Next((int)pair.first.Value.Count() - 1); 

                    while (index1 == index2)
                        index2 = rand.Next((int)pair.first.Value.Count() - 1);

                    var indexes = new List<int>() { index1, index2 };
                    var orderIndexes = indexes.OrderBy(x => x);

                    population.People.Add(CrossingPerson(population, pair.first, pair.second, index1,index2, population.People.Max(x=>x.Id)));
                    population.People.Add( CrossingPerson(population, pair.second, pair.first, index1, index2, population.People.Max(x => x.Id)));

                }
            }
        }

        private IPerson CrossingPerson(IPopulation population, IPerson firstPerson, IPerson secondPerson, int index1, int index2, int maxId)
        {
         
            double[] array = new double[firstPerson.Value.Count()];

            for (int i = 0; i < index1; i++)
                array[i] = firstPerson.Value[i];

            for (int i = index1; i < index2; i++)
                array[i] = secondPerson.Value[i];

            for (int i = index2; i < array.Length; i++)
                array[i] = firstPerson.Value[i];

            return new SnakeGenticPerson(maxId + 1, array) { BestValue = firstPerson.BestValue };
        }
    }
}
