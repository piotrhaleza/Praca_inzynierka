using Genetic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic.Helpers
{
    public static class PairHelper
    {
        public static IList<(IPerson first, IPerson second)> GetPairs(IList<IPerson> people)
        {
            IList<(IPerson, IPerson)> pair = new List<(IPerson, IPerson)>();
            var oldPeople = people.ToList();

            while (oldPeople.Count > 1)
            {
                var pairIndex = RandomHelper.GetTwoRandoms(oldPeople.Count());
                if (pairIndex != null)
                {
                    var newPerson1 = oldPeople[pairIndex[0]];
                    var newPerson2 = oldPeople[pairIndex[1]]; ;

                    pair.Add((newPerson1, newPerson2));
                    oldPeople.Remove(newPerson1);
                    oldPeople.Remove(newPerson2);
                }
            }

            return pair;
        }
    }
}
