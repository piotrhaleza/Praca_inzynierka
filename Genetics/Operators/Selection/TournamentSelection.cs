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


        public void SelectPopulation(IPopulation population, IFunc func)
        {
            var oldPeople = population.People.ToList();
            var newPopulation = new List<IPerson>();
            double factor = (double) oldPeople.Count() / population.StartingCountPopulation;
            int numberOfPeopleInSmallGroup = (int)factor;

            int countOfBigGroup =(int)(factor - numberOfPeopleInSmallGroup) *population.StartingCountPopulation;

            Random rand = new Random();

            var groups = new List<List<IPerson>>();
            //10 / 10 = 1
            //15 / 10 = 1.5 2,2,2,2,2

            for (int i = 0; i<population.StartingCountPopulation;i++)
            {
                var group = new List<IPerson>();

                if (i<countOfBigGroup)
                    for (int z = 0; z < numberOfPeopleInSmallGroup + 1; z++)
                    {
                        var index = rand.Next(oldPeople.Count);
                        group.Add(oldPeople[index]);
                        oldPeople.RemoveAt(index);
                    }
                else
                    for (int z = 0; z < numberOfPeopleInSmallGroup; z++)
                    {
                        var index = rand.Next(oldPeople.Count);
                        group.Add(oldPeople[index]);
                        oldPeople.RemoveAt(index);
                    }

                groups.Add(group);
            }

            foreach(var group in groups)
                newPopulation.Add(group.FirstOrDefault(x => x.BestValue == group.Max(p => p.BestValue)));


            population.People = newPopulation;
        }

    }
}
