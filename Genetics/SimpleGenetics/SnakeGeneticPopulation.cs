using Genetic.Helpers;
using Genetic.Interfaces;
using GeneticInterfaces.Interfaces;
using GeneticInterfaces.Interfaces.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic.SimpleGenetics
{
    public class SnakeGeneticPopulation : IPopulation
    {
        #region Public Properties

        
        public IList<IPerson> People { get; set; }

        public int StartingCountPopulation { get; set; }
        #endregion

        #region Readonly
        public readonly int _lentghChromoson;
        public readonly double _propabiltyOfCross;
        public readonly double _propabilityOfMutation;
        public readonly IFunc _func;
        public readonly ICrossOperator _cross;
        public readonly IMutateOperator _mutate;
        public readonly ISelectionOperator _select;
        #endregion

        #region Constructors

        public SnakeGeneticPopulation(int countOfPeople, double propabiltyOfCross, double propabilityOfMutation,int lentghChromoson, 
            IFunc func,
            ICrossOperator cross, 
            IMutateOperator mutate,
            ISelectionOperator select)
        {
            _propabiltyOfCross = propabiltyOfCross;
            _propabilityOfMutation = propabilityOfMutation;
            _cross = cross;
            _mutate = mutate;
            _select = select;
            _func = func;
            _lentghChromoson = lentghChromoson;
            StartingCountPopulation = countOfPeople;
            People = new List<IPerson>();
            for (int i = 0; i < countOfPeople; i++)
                People.Add(new SnakeGenticPerson(i));
        }
        #endregion

        #region IPopulation implemetion

        public void CrossPopulation()
        {
          _cross.Cross(this,_propabiltyOfCross);
        }
        public void MutatePopulation()
        {
           _mutate.Mutate(this, _propabilityOfMutation);
        }
        public void SelectPopulation()
        {
           _select.SelectPopulation(this,_func);
        }
        public string GetTheBestToString()
        {
            var theBest = People.FirstOrDefault(x => _func.GetResult(x.ToDouble()) == People.Max(z => _func.GetResult(z.ToDouble())));
            return $"{_func.GetResult(theBest.ToDouble())} {theBest.ToDouble()}";
        }
        public IPerson GetTheBest()
        {
            var theBest = People.FirstOrDefault(x => x.BestValue == People.Max(z => z.BestValue));
            return theBest;
        }
        public void Init(Func<double, double> func)
        {
            foreach(var item in People)
                item.Init(func, _lentghChromoson);
        }
        #endregion

        #region ToString

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var item in People)
            {
                builder.Append($"Osobnik {item.Id}: {item.ToString()}  {item.ToDouble()}\n");
            }
            return builder.ToString();
        }

      
        #endregion
    }
}
