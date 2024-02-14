using MachineLearingInterfaces.ActivationFunc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.Funcs
{
    public partial class SigmoidActivationFunc
    {
        public static string OrginalPattern => "f(x) = 1/(1+e^x)";
        public static string RealPattern => "f(x) = {m}/({b}+e^({a}*x)) + {d}";
        public static Dictionary<string, double> OrginalParameters => new Dictionary<string, double>() { { "a", 1 }, };
    }
    public partial class TanhActivationFunc
    {
        public static string OrginalPattern => "f(x) = (e^x – e^-x) / (e^x + e^-x)";
        public static string RealPattern => "f(x) = m/(b+e^(a*x)) + d";
        public static Dictionary<string, double> OrginalParameters => new Dictionary<string, double>() { { "a", 1 }, };
    }
    public partial class ReLuActivationFunc
    {
        public static string OrginalPattern => "f(x) = ax + b (f(x) < 0 = 0)";
        public static string RealPattern => "f(x) = {a}*x + {b}";
        public static Dictionary<string, double> OrginalParameters => new Dictionary<string, double>() { { "a", 1 }, };
    }

    public partial class LineralActivationFunc
    {
        public static LineralActivationFunc Create(int a = 1, int b = 0)
        {
            return new LineralActivationFunc(a, b);
        }
        public static string OrginalPattern => "f(x) = a*x + b";
        public static string RealPattern => "f(x) = {a}*x + {b}";
        public static Dictionary<string, double> OrginalParameters => new Dictionary<string, double>() { { "a", 1 }, { "b", 0 } };
    }

    public partial class SoftmaxActivationFunc
    {
        public static string OrginalPattern => "Nie obsłużone";
        public static string RealPattern => "f(x) = {a}*x + {b}";
        public static Dictionary<string, double> OrginalParameters => new Dictionary<string, double>() { { "a", 1 }, };
    }
}
