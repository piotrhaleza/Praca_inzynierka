using Genetic.Interfaces;
using GeneticInterfaces.Interfaces;
using MachineLearingInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic.SimpleGenetics
{
    public class SnakeGenticPerson : IPerson
    {

        #region Public Properties
        public IPopulation Population { get; set; }
        public IList<double> Value { get; set; }
        public int Id { get; set; }
        public int BestValue { get; set; }
        public KindOfLose KindOfLose { get; set; }
        #endregion

        #region Static methods
        public static SnakeGenticPerson Copy(int id, IPerson person, IPopulation population)
        {
            return new SnakeGenticPerson(id, person, population);
        }
        #endregion

        #region Constructors
        private SnakeGenticPerson(int id, IPerson person, IPopulation population)
        {
            Id = id;
            Value = person.Value;
            BestValue = person.BestValue;
        }

        public SnakeGenticPerson(int id, IList<double> value = null)
        {
            Id = id;
            Value = value;
        }
        #endregion

        #region Public Methods

        public void Init(Func<double, double> func, int lentghChromoson)
        {
            var rand = new Random();
            Value = new double[lentghChromoson];
            for (int i = 0; i < lentghChromoson; i++)
            {
                Value[i] = func(rand.NextDouble());
            }
        }

        #endregion

        #region ToValue methods

        public double ToDouble()
        {
            double result = 0;

            for (int i = Value.Count() - 1; i >= 0; i--)
                result += Value[i] == 1 ? Math.Pow(2, i) : 0;

            return result;
        }
        public override string ToString()
        {
            return BestValue.ToString();
        }

        #endregion
    }
}
