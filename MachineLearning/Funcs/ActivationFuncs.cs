using MachineLearingInterfaces.ActivationFunc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace MachineLearning.Funcs
{
    public partial class SigmoidActivationFunc : ActivationFunc
    {
        public override KindOfActivationFunc KindActivation => KindOfActivationFunc.Sigmoid;

        public SigmoidActivationFunc(double a = 1, double b = 0, double DenominatorBias = 1, double MeterBias = 1)
        {
            Parameters = new Dictionary<string, double>();
            Parameters.Add("a", a);
            Parameters.Add("b", b);
            Parameters.Add("d", DenominatorBias);
            Parameters.Add("m", MeterBias);
        }

        public override double Activate(double input)
        {
            return (Parameters["m"]) / (Math.Exp(-input * Parameters["a"]) + Parameters["d"]) + Parameters["b"];
        }

        public override double Derivative(double input)
        {
            return Activate(input) * (1.0 - Activate(input));
        }
    }

    public partial class TanhActivationFunc : ActivationFunc
    {
        public override KindOfActivationFunc KindActivation => KindOfActivationFunc.Tanh;

        public static double TanhActivation(double x)
        {
            return (Math.Exp(x) - Math.Exp(-x)) / (Math.Exp(x) + Math.Exp(-x));
        }

        public override double Activate(double input)
        {
            return Math.Tanh(input);
        }

        public override double Derivative(double input)
        {
            double coshValue = Math.Cosh(input);
            return 1.0 / (coshValue * coshValue);
        }
    }

    public partial class ReLuActivationFunc : ActivationFunc
    {
        public override KindOfActivationFunc KindActivation => KindOfActivationFunc.Relu;

        public ReLuActivationFunc(double a = 1, double b = 0)
        {
            Parameters = new Dictionary<string, double>();
            Parameters.Add("a", a);
            Parameters.Add("b", b);
        }

        public override double Activate(double input)
        {
            return Math.Max(0, Parameters["a"] * input + Parameters["b"]);
        }

        public override double Derivative(double input)
        {
            if (input >= 0)
                return Parameters["a"];
            else
                return 0;
        }
    }

    public partial class LineralActivationFunc : ActivationFunc
    {
        public override KindOfActivationFunc KindActivation => KindOfActivationFunc.Lineral;

        private LineralActivationFunc(double a = 1, double b = 0)
        {
            Parameters = new Dictionary<string, double>();
            Parameters.Add("a", a);
            Parameters.Add("b", b);
        }

        public override double Activate(double input)
        {
            return Parameters["a"] * input + Parameters["b"];
        }

        public override double Derivative(double input)
        {
            return Parameters["a"];
        }

    }

    public partial class SoftmaxActivationFunc : ActivationFunc
    {
        public override KindOfActivationFunc KindActivation => KindOfActivationFunc.SoftMax;

        public override double Activate(double input)
        {
            return 1.0 / (1.0 + Math.Exp(-input));
        }

        public override double Derivative(double input)
        {
            throw new NotImplementedException();
        }
    }
}
