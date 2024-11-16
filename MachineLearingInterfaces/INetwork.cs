using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearingInterfaces
{
    public interface INetwork
    {
        IInitWages InitWages { get; }
        IInitBiases InitBiases { get; }
        ILayer Inputs { get; }
        ILayer Outputs { get; }
        IList<ILayer> Layers { get; }
        int BatchSize { get; }
        double Loss { get; set; }
        double LearningRate { get; set; }
        IList<double> ForwardPropagation(IList<double> inputs);
        bool BackPropagation(IList<double> expected);
        IList<double> TrainModel(IList<double> inputs, IList<double> expected);
        IList<double> GetResults(IList<double> inputs, IList<double> expected);
        void SetWages(IList<double> wages);
        void UpdateWages();
    }
}
