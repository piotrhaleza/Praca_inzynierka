using GeneticInterfaces.Interfaces;
using MachineLearingInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic.Interfaces
{
    public interface IPerson
    {
        IList<double> Value { get; set; }
        int Id { get; set; }
        int BestValue { get; set; }
        void Init(Func<double, double> func, int lentghChromoson);
        double ToDouble();
        KindOfLose KindOfLose { get; set; }
    }
}
