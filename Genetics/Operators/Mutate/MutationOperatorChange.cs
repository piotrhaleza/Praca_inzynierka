using Genetic.Helpers;
using Genetic.Interfaces;
using Genetic.SimpleGenetics;
using GeneticInterfaces.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic.Operators.Mutate
{
    public class MutationOperatorChange : IMutateOperator
    {
        public void Mutate(IPopulation population, double propability)
        {
            List<IPerson> newPeople = new List<IPerson>();
            int maxId = population.People.Max(x => x.Id);
            Random rand = new Random();
            foreach (var person in population.People)
            {

                var mutation = rand.NextDouble() < propability;
                if (mutation)
                {
                    var index = rand.Next(person.Value.Count());

                    var resultList = new List<double>();
                    for (int i = 0; i < person.Value.Count(); i++)
                        if (i == index)
                        {
                            var minus = rand.NextDouble() > 0.5 ? -1 : 1;
                            var power =rand.Next(0, 1);
                            var diff = minus * rand.NextDouble() / Math.Pow(10, power);
                            resultList.Add(person.Value[i] + diff);
                        }
                        else
                            resultList.Add(person.Value[i]);
            
                    newPeople.Add(new SnakeGenticPerson(++maxId, resultList));
                }
            }

            foreach (var person in newPeople)
            {
                population.People.Add(person);
            }

        }
    }
}
