using Genetic.Interfaces;
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
        public IList<double> Value { get; set; }
        public int Id { get; set; }
        public double MinValue => -0.3;
        public double MaxValue => 0.6;
        public int BestValue { get; set; }
        #endregion

        #region Constructors
        public SnakeGenticPerson(int id, IPerson person)
        {
            Id = id;
            Value = person.Value;
            BestValue = person.BestValue;
        }

        public SnakeGenticPerson(int id,int lenght, IList<double> value = null,Random rand =null)
        {
            Id = id;

            if (value == null)
            {
                rand = rand==null? new Random():rand;
                Value = new double[lenght];
                for (int i = 0; i < lenght; i++)
                {
                    Value[i] = rand.NextDouble() * MaxValue + MinValue;
                }
            }
            else
                Value = value;
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
