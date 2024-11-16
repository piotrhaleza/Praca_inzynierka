using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using SnakeGame.UI.Enums;
using MachineLearingInterfaces.ActivationFunc;

namespace SnakeGame.UI
{
    internal static class Configuration
    {
        public static GameDifficulty DifficultyGame { get; }
        public static int TimeWihoutApple  { get; }
        public static string PathToFile { get; }
        public static bool IsGenetics { get; }
        public static List<int> Layers { get; }
      
        public static int IterationOfPropagation { get; }
        public static KindOfActivationFunc ActivationFunc { get; }
        public static int PopulationCount { get; }
        public static int CountPeopleInPopulation { get; }
        public static int CounOfRepeat { get; }
        public static int PropablityOfCross { get; }
        public static int PropablityOfMutaion { get; }
        static Configuration()
        {
            DifficultyGame = (GameDifficulty)Enum.Parse(typeof(GameDifficulty), ConfigurationManager.AppSettings["DifficultyGame"]);
            TimeWihoutApple = int.Parse(ConfigurationManager.AppSettings["TimeWihoutApple"]);
            PathToFile = ConfigurationManager.AppSettings["PathToFile"];
            IsGenetics = "Genetics" == ConfigurationManager.AppSettings["LearningMode"];
            Layers = new List<int>();

            int i = 1;
            while (true)
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains($"Layer_{i}"))
                    Layers.Add(int.Parse(ConfigurationManager.AppSettings[$"Layer_{i}"]));
                else
                    break;
                i++;

            }

            ActivationFunc = (KindOfActivationFunc)Enum.Parse(typeof(KindOfActivationFunc), ConfigurationManager.AppSettings["ActivationFunc"]); 
            PopulationCount = int.Parse(ConfigurationManager.AppSettings["PopulationCount"]);
            CountPeopleInPopulation = int.Parse(ConfigurationManager.AppSettings["CountPeopleInPopulation"]);
            CounOfRepeat = int.Parse(ConfigurationManager.AppSettings["CounOfRepeat"]);
            PropablityOfCross = int.Parse(ConfigurationManager.AppSettings["PropablityOfCross"]);
            PropablityOfMutaion = int.Parse(ConfigurationManager.AppSettings["PropablityOfMutaion"]);
            IterationOfPropagation = int.Parse(ConfigurationManager.AppSettings["IterationOfPropagation"]);
        }
    }
}
