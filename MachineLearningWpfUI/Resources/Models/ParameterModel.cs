using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningWpfUI.Models
{
    public class ParameterModel
    {
        public delegate void PerformCalculation();

        public ParameterModel(string key, double value, bool odd, PerformCalculation calculation)
        {
            Calculation = calculation;
            Key = key;
            Value = value;
            Odd = odd;
        }

        public PerformCalculation Calculation;

        private string key;

        public string Key
        {
            get { return key; }
            set {
               
                key = value; }
        }
        private double valueM;

        public double Value
        {
            get { return valueM; }
            set {
                Calculation.Invoke();
                valueM = value; }
        }

        public bool Odd { get; set; }
    }
}
