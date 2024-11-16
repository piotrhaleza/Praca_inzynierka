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
    public class CrossOperatorExchangeWithReplace : ICrossOperator
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
                    var crossingIndex = rand.Next((int)pair.first.Value.Count() - 2) + 1;

                    population.People.Add(CrossingPerson(pair.first, pair.second, crossingIndex, population.People.Max(x=>x.Id)));
                    population.People.Add( CrossingPerson(pair.second, pair.first, crossingIndex, population.People.Max(x => x.Id)));

                }
            }
        }

        public IPerson CrossingPerson(IPerson firstPerson, IPerson secondPerson, int crossingIndex,int maxId)
        {
            double[] array = new double[firstPerson.Value.Count()];

            for (int i = 0; i < crossingIndex; i++)
                array[i] = firstPerson.Value[i];

            for (int i = crossingIndex; i < array.Length; i++)
                array[i] = secondPerson.Value[i];

            return new SnakeGenticPerson(maxId +1, firstPerson.Value.Count(), array) { BestValue = firstPerson.BestValue };
        }
    }
}
