
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.Funcs.Helpers
{
    public static class FuncCreator
    {
        public static string CreateRealFunc(string pattern, IDictionary<string, double> arguments)
        {
            string result = pattern;
            foreach (var item in arguments)
            {
                result = result.Replace(item.Key, item.Value.ToString());
            }

            return pattern;

            return null;
        }
    }
}
