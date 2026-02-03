using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic.Interfaces
{
    public interface IPopulation
    {
        int StartingCountPopulation { get; set; }
        IList<IPerson> People { get; set; }
        void CrossPopulation();
        void MutatePopulation();
        void SelectPopulation();
        string GetTheBestToString();
        IPerson GetTheBest();
        void Init(Func<double, double> func);
    }
}
