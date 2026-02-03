using MachineLearingInterfaces;
using MachineLearning;
using MachineLearning.Biases;
using MachineLearning.Funcs;
using MachineLearning.Wages;
using SnakeGame.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SnakeGame.UI.Helpers
{
    public static class JsonNetworkReader
    {
        public static Network ReadNetwork(string path)
        {
            string jsonString = File.ReadAllText(path);
            var model = JsonSerializer.Deserialize<NetworkToSaveViewModel>(jsonString);

            var list = new List<ILayer>();

            int i = 1;
            foreach (var item in model.Layers)
            {
                list.Add(new Layer(item, i,  LineralActivationFunc.Create()));
                i++;
            }

            var network = new Network(list, new InitZeroWages(), new InitZeroBiases());
            network.SetWages(model.Wages);

            return network;
        }
    }
}
