using MachineLearingInterfaces;
using MachineLearning.Biases;
using MachineLearning.Funcs;
using MachineLearning.Wages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.Networks
{
    public class CopyOfNetwork : ICopyNetwork
    {
        private readonly INetwork _template;

        public CopyOfNetwork(INetwork template)
        {
            _template = template;
        }

        public INetwork Copy()
        {
            var list = new List<ILayer>();

            int i = 1;
            foreach (var item in _template.Layers)
            {
                list.Add(new Layer(item.NeuronList.Count(), i, new ReLuActivationFunc()));
                i++;
            }

            return new Network(list, _template.InitWages, _template.InitBiases,10);
        }
    }
}
