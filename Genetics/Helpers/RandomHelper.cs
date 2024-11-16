using Genetic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Genetic.Helpers
{
    public static class RandomHelper
    {
        public static int[] GetTwoRandoms(int count)
        {
            if (count < 2) return null;

            var result = new int[2];

            Random ran = new Random();

            if (count == 2)
            {
                result[0] = ran.Next(count);
                result[1] = result[0] == 0 ? 1 : 0;
            }
            else
            {
                result[0] = ran.Next(count);
                result[1] = ran.Next(count);
                while (result[1] == result[0])
                    result[1] = ran.Next(count);
            }
            return result;
        }
    }
}
