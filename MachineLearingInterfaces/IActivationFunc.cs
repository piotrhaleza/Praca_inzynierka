using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearingInterfaces.ActivationFunc
{
    public enum KindOfActivationFunc
    {
        Sigmoid = 0,
        Lineral = 1,
        Tanh =2,
        Relu = 3,
        SoftMax = 4,
    }

    public interface IActivationFunc
    {
        double Activate(double input);

        double Derivative(double input);

        Dictionary<string, double> Parameters { get; set; }

        KindOfActivationFunc KindActivation {get;}

    }

    public abstract class ActivationFunc : IActivationFunc
    {
        public Dictionary<string, double> Parameters { get; set; }

        public abstract KindOfActivationFunc KindActivation {get;}

        public abstract double Activate(double input);

        public abstract double Derivative(double input);
        
    }

}
