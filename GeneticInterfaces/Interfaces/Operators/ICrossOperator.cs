using Genetic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticInterfaces.Interfaces
{
    public interface ICrossOperator
    {
        void Cross(IPopulation population, double propability);
    }
}
