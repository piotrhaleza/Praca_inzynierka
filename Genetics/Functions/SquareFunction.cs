using Genetic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic.functions
{
    public class SquareFunction : IFunc
    {
        public double a { get; set; }
        public double b { get; set; }
        public double c { get; set; }

        public SquareFunction(double a, double b, double c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public double GetResult(double x)
        {
            return a * Math.Pow(x, 2) + b * Math.Pow(x, 1) + c;
        }
    }
}
