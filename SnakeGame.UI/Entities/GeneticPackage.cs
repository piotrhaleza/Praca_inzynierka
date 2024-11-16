using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.UI.Entities
{
    public class GeneticPackage
    {
        public int CountOfPopulation { get; set; }
        public int CountOfPeopleInPopulation { get; set; }
        public double PropablityOfCross { get; set; }
        public double PropablityOfMutaion { get; set; }
        public int LentghtOfChromoson { get; set; }
    }
}
