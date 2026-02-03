using MachineLearingInterfaces;
using MachineLearning.Biases;
using MachineLearning.errors;
using MachineLearning.Wages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
    public class Network2 : Network
    {
        public Network2(IList<ILayer> layers, IInitWages initWages, IInitBiases initBiases, int batchSize) : base(layers, new InitHeWages(), new InitRandomBiases())
        {
        }
    }

    public class Network : INetwork
    {
        #region Readonly Properties
        public readonly int _batchSize;
        public readonly IList<ILayer> _layers;
        private readonly IInitWages _initWages;
        private readonly IInitBiases _initBiases;
        #endregion

        #region Public Properties
        public IInitWages InitWages => _initWages;
        public IInitBiases InitBiases => _initBiases;
        public ILayer Inputs => Layers.FirstOrDefault();
        public ILayer Outputs => Layers.LastOrDefault();
        public IList<ILayer> Layers => _layers;
        public int BatchSize => _batchSize;
        public double Loss { get; set; }
        public double LearningRate { get; set; }
        #endregion

        #region Constructors

        public Network(IList<ILayer> layers, IInitWages initWages, IInitBiases initBiases)
        {
            _layers = PrepareLayers(layers);

            _initWages = initWages;
            _initBiases = initBiases;

            initWages?.Init(this);
            initBiases?.Init(this);

            LearningRate = 0.0005;
        }
        public Network(IInitWages initWages, IInitBiases initBiases, int batchSize)
        {
            _batchSize = batchSize;

            LearningRate = 0.1;
        }
        #endregion

        #region Public Methods
        public void SetWages(IList<double> wages)
        {
            int i = 0;

            foreach (var item in Layers)
                foreach (var dict in item.Wages)
                    foreach (var wage in dict)
                        wage.Value.Value = wages[i++];

        }
        public IEnumerable<double> GetWages()
        {
            foreach (var item in Layers)
                foreach (var dict in item.Wages)
                    foreach (var wage in dict)
                        yield return wage.Value.Value;
        }

        public IList<ILayer> PrepareLayers(IList<ILayer> layers)
        {
            for (int i = 0; i < layers.Count(); i++)
            {
                if (i != layers.Count() - 1)
                    layers[i].NextLayer = layers[i + 1];
                if (i != 0)
                    layers[i].PreviousLayer = layers[i - 1];
            }

            return layers;
        }

        public IList<double> ForwardPropagation(IList<double> inputs)
        {
            for (int i = 0; i < inputs.Count(); i++)
                Inputs.NeuronList[i].Value = inputs[i];

            foreach (var layer in Layers.Skip(1))
                foreach (var neuron in layer.NeuronList)
                    layer.CalculateNextNeuron(neuron);

            return Outputs.NeuronValues.ToList();
        }
        public bool BackPropagation(IList<double> expected)
        {
            BackPropagationOutputs(expected);

            foreach (var layer in Layers.Reverse().Skip(1))
                BackPropagationHiddenLayer(layer);

            return true;
        }
        public IList<double> TrainModel(IList<double> inputs, IList<double> expected)
        {
            ForwardPropagation(inputs);
            BackPropagation(expected);
            return Outputs.NeuronValues.ToList();
        }

        public IList<double> GetResults(IList<double> inputs, IList<double> expected)
        {
            ForwardPropagation(inputs);
            return Outputs.NeuronValues.ToList();
        }

        public int GetCountOfWages()
        {
            int result = 0;
            for (int i = 0; i < this.Layers.Count() - 1; i++)
                result += Layers[i].NeuronList.Count() * Layers[i + 1].NeuronList.Count();
            return result;
        }
        #endregion

        #region Private Methods
        private List<double> GetSumOfGradtients(IEnumerable<Dictionary<INeuron, IWage>> dicts)
        {
            List<double> sum = new List<double>();

            foreach (var neuron_wages in dicts)
                sum.Add(neuron_wages.Sum(x => x.Value.Error));

            return sum;
        }

        private void BackPropagationHiddenLayer(ILayer layer)
        {
            if (layer.PreviousLayer == null)
                return;

            foreach (var previousNeuron in layer.PreviousLayer.NeuronList)
            {
                foreach (var neuron in layer.NeuronList)
                {
                    
                    var total_error = neuron.Wages.Sum(wage => wage.Value.Value * wage.Value.Error);
                    previousNeuron.Wages[neuron].Error = total_error * Outputs.PreviousLayer.ActivationFunc.Derivative(neuron.Value);
                }
            }
        }
        private void BackPropagationOutputs(IList<double> expected)
        {
            foreach (var previousNeuron in Outputs.PreviousLayer.NeuronList)
            {
                foreach (var neuron in Outputs.NeuronList)
                {
                    var wage = previousNeuron.Wages[neuron];

                    wage.Error = (neuron.Value - expected[neuron.Id]) * Outputs.PreviousLayer.ActivationFunc.Derivative(neuron.Value);
                }
            }
        }

        public void UpdateWages()
        {
            foreach (var layer in Layers)
            {
                foreach (var item in layer.NeuronList)
                    foreach (var wage in item.Wages.Select(x => x.Value))
                        wage.Value = wage.Value - LearningRate * wage.Error * item.Value;
            }
        }


        #endregion
    }
}
