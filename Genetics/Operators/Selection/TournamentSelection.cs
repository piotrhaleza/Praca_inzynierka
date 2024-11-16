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
    public class TournamentSelection : ISelectionOperator
    {

        public TournamentSelection()
        {
            
        }


        public void SelectPopulation(IPopulation population, IFunc func,int numberOfElite = 0)
        {
            var oldPeople = population.People.ToList();
            var newPopulation = new List<IPerson>();
            var countOfPeopleInGroup = oldPeople.Count() / population.StartingCountPopulation;
            Random rand = new Random();


            for(int i = 0; i<population.StartingCountPopulation;i++)
            {
                var group = new List<IPerson>();

                for(int z = 0; z < countOfPeopleInGroup; z++)
                {
                    var index = rand.Next(oldPeople.Count);
                    group.Add(oldPeople[index]);
                    oldPeople.RemoveAt(index);
                }

                newPopulation.Add(group.FirstOrDefault(x => x.BestValue == group.Max(p => p.BestValue)));
            }

            population.People = newPopulation;
        }

    }
}
