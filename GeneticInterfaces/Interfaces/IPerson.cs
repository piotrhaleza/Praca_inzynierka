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
        double ToDouble();
        double MinValue { get;}
        double MaxValue { get;}
        int BestValue { get; set; }
    }
}
