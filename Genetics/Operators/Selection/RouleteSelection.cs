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
        
        public void SelectPopulation(IPopulation population, IFunc func)
        {
            List<IPerson> newPersons = new List<IPerson>();
            int id = 0;

           
            var sum = population.People.Sum(x=>x.BestValue);
            var probability = population.People.ToDictionary(x=> x.Id,x=>(double)(x.BestValue)/sum);

            Random random = new Random();
            for (int k = 0; k < population.StartingCountPopulation; k++)
            {
                double radomValue = random.NextDouble();
                double floor = 0;

                foreach (var item in probability)
                {
                    if (radomValue >= floor && radomValue < floor + item.Value)
                    {
                        var newPerson = population.People.FirstOrDefault(x => x.Id == item.Key) as SnakeGenticPerson;
                        newPersons.Add(SnakeGenticPerson.Copy(id, newPerson, population));
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
