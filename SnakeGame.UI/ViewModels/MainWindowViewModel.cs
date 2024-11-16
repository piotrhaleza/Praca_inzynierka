using Genetic.Interfaces;
using Genetic.Operators.Cross;
using Genetic.Operators.Mutate;
using Genetic.Operators.Selection;
using Genetic.SimpleGenetics;
using MachineLearingInterfaces;
using MachineLearingInterfaces.ActivationFunc;
using MachineLearning.Networks;
using MachineLearning;
using SnakeGame.UI.Entities;
using SnakeGame.UI.Enums;
using SnakeGame.UI.Learning;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame.UI.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Readonly poles
        public GameDifficulty[] PossibleDifficulty => new GameDifficulty[] {
            GameDifficulty.Easy,
            GameDifficulty.Medium,
            GameDifficulty.Hard};
        public KindOfActivationFunc[] PossibleActivationFunc => new KindOfActivationFunc[] {
            KindOfActivationFunc.Relu,
            KindOfActivationFunc.Lineral,
            KindOfActivationFunc.Sigmoid };

        #endregion

        #region Private poles
        private ObservableCollection<LayerModel> layers;
        private int tickOfSpeed;
        private int iteration;
        private string iterationText;
        private string theBestSnakeText;
        private KindOfLearning selectedGameMode;
        private int countOfRepeat;
        private MainWindow parent;
        private int maxRepeat;
        #endregion

        #region Constructors
        public MainWindowViewModel(MainWindow parent)
        {
            this.parent = parent;

            Layers = new ObservableCollection<LayerModel>();
           
            SelectedDifficulty = Configuration.DifficultyGame;
            TimeWithoutApple = Configuration.TimeWihoutApple;
            PathToFile = Configuration.PathToFile;
            IsGentics = Configuration.IsGenetics;
            IsPropagation = !Configuration.IsGenetics;
            CountOfLayers = Configuration.Layers.Count();
            CreateNewLayers(Configuration.Layers);
            ActivationFunc = Configuration.ActivationFunc;
            CountOfPopulations = Configuration.PopulationCount;
            CountOfPeopleInPoulation = Configuration.CountPeopleInPopulation;
            CountOfRepeat = Configuration.CounOfRepeat;
            PropablityOfCross = Configuration.PropablityOfCross;
            PropablityOfMutaion = Configuration.PropablityOfMutaion;
            IterationOfPropagation = Configuration.IterationOfPropagation;
        }

        #endregion

        #region Properties
        public int TickOfSpeed
        {
            get => tickOfSpeed;
            set
            {
                var internval = (double)value;
                parent._gameWorld.ChangeInterval(1 / (4 * internval));
                tickOfSpeed = value;
                OnPropertyChanged(nameof(Iteration));
            }
        }
        public int IterationOfPropagation { get; set; }
        public int Iteration
        {
            get => iteration;
            set
            {
                iteration = value;
                OnPropertyChanged(nameof(Iteration));
            }
        }
        public int MaxRepeat
        {
            get => maxRepeat;
            set
            {
                maxRepeat = value;
                OnPropertyChanged(nameof(MaxRepeat));
            }
        }
        public string IterationText
        {
            get => iterationText;
            set
            {
                iterationText = value;
                OnPropertyChanged(nameof(IterationText));
            }
        }
        public string TheBestSnakeText
        {
            get => theBestSnakeText;
            set
            {
                theBestSnakeText = value;
                OnPropertyChanged(nameof(TheBestSnakeText));
            }
        }
        public ObservableCollection<LayerModel> Layers
        {
            get { return layers; }
            set
            {
                OnPropertyChanged(nameof(Layers));
                layers = value;
            }
        }
        public int CountOfRepeat
        {
            get => countOfRepeat;
            set
            {
                countOfRepeat = value;
                OnPropertyChanged(nameof(CountOfRepeat));
            }
        }
        public int PropablityOfCross { get; set; }
        public int PropablityOfMutaion { get; set; }
        public bool IsPropagation { get; set; }
        public bool IsGentics { get; set; }
        public KindOfActivationFunc ActivationFunc { get; set; }
        public GameDifficulty SelectedDifficulty { get; set; }
        public int CountOfPopulations { get; set; }
        public int CountOfPeopleInPoulation { get; set; }

        public int TimeWithoutApple { get; set; }
        public int CountOfLayers { get; set; }
        public string PathToFile { get; set; }
        public string PathToReadFile { get; set; }
        public bool FirstTime { get; set; } = true;
        public GeneticLearning Genetic { get; set; }

       

        #endregion

        #region Public methods
        public void CreateNewLayers(int countofLayers)
        {
            if (FirstTime)
            {
                FirstTime = false;
                return;
            }

            Layers.Clear();


            Layers.Add(new LayerModel() { Name = "Inputs" });

            for (int i = 1; i < countofLayers - 1; i++)
            {
                Layers.Add(new LayerModel() { Name = "Hidden" });
            }
            Layers.Add(new LayerModel() { Name = "Outputs" });
        }
        public void CreateNewLayers(List<int> layers)
        {
            Layers.Clear();


            Layers.Add(new LayerModel() { Name = "Inputs", CountOfNeurons = layers.First() });

            for (int i = 1; i < layers.Count() - 1; i++)
            {
                Layers.Add(new LayerModel() { Name = "Hidden", CountOfNeurons = layers[i] });
            }
            Layers.Add(new LayerModel() { Name = "Outputs", CountOfNeurons = layers.Last() });
        }


        public async Task<IList<double>> StartPropagationTraning(Network network, int countOfRepeat)
        {
            MaxRepeat = countOfRepeat;
            SnakeElement.UiIs = false;

            await Task.Run(() =>
            {
                var propagationLearning = new SnakeLearningNeuralNetwork(network, 50, 10, 10, countOfRepeat,SelectedDifficulty);
                double result = 0;
                for (int i = 0; i < countOfRepeat; i++)
                {
                    try
                    {


                       result += propagationLearning.NetworkLearn();

                        if (i % 10 == 0)
                        {
                            App.Current.Dispatcher.Invoke(() =>
                            {
                                Iteration = i;
                                IterationText = $"{i}/{MaxRepeat}";
                                TheBestSnakeText = $"Najlepszy = {result/10}";
                            });
                            result = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            });


            return network.GetWages().ToList();

        }
        public async Task<IList<double>> StartGeneticTraning(Network network, GeneticPackage package, int countOfRepeat)
        {
            MaxRepeat = countOfRepeat;
            var copyier = new CopyOfNetwork(network);

            var population = new SnakeGeneticPopulation(package.CountOfPeopleInPopulation, package.PropablityOfCross, package.PropablityOfMutaion, package.LentghtOfChromoson, null, new CrossOperatorExchangeWithReplace(), new MutationOperatorChange(), new RouleteSelection());
            Genetic = new GeneticLearning(copyier, population, 50, 10, 10, countOfRepeat,SelectedDifficulty);
            Genetic.IterationEvent += IncreasedIteration;
            SnakeElement.UiIs = false;

            var person = await Genetic.GetBestPerson();
            var bestNetwork = copyier.Copy();

            return person.Value;

        }

        public void IncreasedIteration(int iteration, int best)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                Iteration = iteration;
                IterationText = $"{iteration}/{MaxRepeat}";
                TheBestSnakeText = $"Najlepszy = {best}";
            });
        }
        #endregion

        #region PropertyChanged
        /// <summary>
        /// Zdarzenie obsługujące zmianę wartości właściwości (implementowane przez INotifyPropertyChanged).
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Podnosi zdarzenie PropertyChanged dla konkretnej wałaściwości.
        /// </summary>
        /// <param name="name">Nazwa właściwości.</param>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
