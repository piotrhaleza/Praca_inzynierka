using Genetic.Interfaces;
using Genetic.SimpleGenetics;
using GeneticInterfaces.Interfaces.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Genetic.Operators.Selection
{
    public class RouleteSelection: ISelectionOperator
    {
        
        public void SelectPopulation(IPopulation population, IFunc func,int numberOfElite = 0)
        {
            var probability = GetPropability(population.People, func);
            var sum = probability.Values.Sum();
            List<IPerson> newPersons = new List<IPerson>();
            int id = 0;

            if (numberOfElite > 0)
            {
                var orderProbability = probability.OrderByDescending(x => x.Value).ToList();

                for (int k = 0; k < numberOfElite; k++)
                {
                    var newPerson = population.People.FirstOrDefault(x => x.Id == orderProbability[k].Key) as SnakeGenticPerson;
                    newPersons.Add(new SnakeGenticPerson(id, newPerson));
                    id++;
                }

            }
            Random random = new Random();
            for (int k = numberOfElite; k < population.StartingCountPopulation; k++)
            {
                int radomValue = random.Next(0,sum);
                int floor = 0;

                foreach (var item in probability)
                {
                    if (radomValue >= floor && radomValue < floor + item.Value)
                    {
                        var newPerson = population.People.FirstOrDefault(x => x.Id == item.Key) as SnakeGenticPerson;
                        newPersons.Add(new SnakeGenticPerson(id, newPerson));
                        break;
                    }
                    else
                        floor += item.Value;
                }
                id++;
            }
            population.People  = newPersons;
        }

        private Dictionary<int, int> GetPropability(IList<IPerson> Value, IFunc func)
        {
            return Value.ToDictionary(x => x.Id,x=> x.BestValue);
        }
    }
}
