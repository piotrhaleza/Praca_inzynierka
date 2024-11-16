using Genetic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticInterfaces.Interfaces.Operators
{
    public interface ISelectionOperator
    {
        void SelectPopulation(IPopulation population, IFunc func, int numberOfElite = 0);
    }
}
