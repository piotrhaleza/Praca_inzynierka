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
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;

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
        private string theBestSnakeTextTest;
        private string theWorstSnakeTextTest;
        private string theAverageSnakeTextTest;
        private string theBestSnakeText;
        private string theWorstSnakeText;
        private string theAverageSnakeText;
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
            IterationOfTestingGame = Configuration.IterationOfTestingGame;
            
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Średnia seri",
                    Values = new ChartValues<ObservablePoint>()
                },
                 new LineSeries
                {
                    Title = "Najlepszy w seri",
                    Values = new ChartValues<ObservablePoint>()
                },
                    new LineSeries
                {
                    Title = "Najgorszy w seri",
                    Values = new ChartValues<ObservablePoint>()
                },

            };

            SeriesCollectionTesting = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Wynik podejścia",
                    Values = new ChartValues<int>()
                },

            };

            CollysionSeriesTesting = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Kolizje",
                    Values = new ChartValues<int> { }
                }
            };


            Labels = new[] { "Kolizja z ścianą", "Kolizja z ogonem", "Koniec czasu" };


            CollysionSeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Dotknięcie ogona",
                    Values = new ChartValues<ObservablePoint>()
                },
                 new LineSeries
                {
                    Title = "Dotknięcie ściany",
                    Values = new ChartValues<ObservablePoint>()
                },
                    new LineSeries
                {
                    Title = "Czas",
                    Values = new ChartValues<ObservablePoint>()
                },

            };


        }

        #endregion

        #region Properties
        public string[] Labels { get; set; }
        public SeriesCollection SeriesCollectionTesting { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public SeriesCollection CollysionSeriesCollection { get; set; }
        public SeriesCollection CollysionSeriesTesting{ get; set; }
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
        public int IterationOfTestingGame { get; set; }
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
        public string TheBestSnakeTextTest
        {
            get => theBestSnakeTextTest;
            set
            {
                theBestSnakeTextTest = value;
                OnPropertyChanged(nameof(TheBestSnakeTextTest));
            }
        }
        public string TheAverageSnakeTextTest
        {
            get => theBestSnakeTextTest;
            set
            {
                theBestSnakeTextTest = value;
                OnPropertyChanged(nameof(TheAverageSnakeTextTest));
            }
        }
        public string TheWorstSnakeTextTest
        {
            get => theBestSnakeTextTest;
            set
            {
                theBestSnakeTextTest = value;
                OnPropertyChanged(nameof(TheWorstSnakeTextTest));
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
        public string TheAverageSnakeText
        {
            get => theBestSnakeText;
            set
            {
                theBestSnakeText = value;
                OnPropertyChanged(nameof(TheAverageSnakeText));
            }
        }
        public string TheWorstSnakeText
        {
            get => theBestSnakeText;
            set
            {
                theBestSnakeText = value;
                OnPropertyChanged(nameof(TheWorstSnakeText));
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
        public bool ContinueLearning { get; set; }
        public List<int> Results { get; set; }
        public List<KindOfCollision> CollisionsResult { get; set; }

        #endregion

        #region Public methods
        public void UpdateResultTestingSerries()
        {
            TheAverageSnakeTextTest = $"Średni wynik = {((double)Results.Sum()) / Results.Count()}";
            TheBestSnakeTextTest = $"Najlepszy wynik = {Results.Max()}";
            TheWorstSnakeTextTest = $"Najgorszy wynik = {Results.Min()}";


            SeriesCollectionTesting[0].Values.Clear();
            CollysionSeriesTesting[0].Values.Clear();
            foreach (var item in Results)
            {
                SeriesCollectionTesting[0].Values.Add(item);
            }
         
            CollysionSeriesTesting[0].Values.Add(CollisionsResult.Count(x=> x == KindOfCollision.WallCollision));
            CollysionSeriesTesting[0].Values.Add(CollisionsResult.Count(x => x == KindOfCollision.SnakeCollision));
            CollysionSeriesTesting[0].Values.Add(CollisionsResult.Count(x => x == KindOfCollision.TimeCollision));
        }
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


            SeriesCollection[0].Values.Clear();
            SeriesCollection[1].Values.Clear();
            SeriesCollection[2].Values.Clear();


            CollysionSeriesCollection[0].Values.Clear();
            CollysionSeriesCollection[1].Values.Clear();
            CollysionSeriesCollection[2].Values.Clear();

            await Task.Run(() =>
            {
                var propagationLearning = new SnakeLearningNeuralNetwork(network, 50, 10, 10, countOfRepeat, SelectedDifficulty);
                double averageResult = 0;
                double worstResult = -1;
                double bestResult = 0;
                List<KindOfCollision> collisons = new List<KindOfCollision>();
                for (int i = 0; i < countOfRepeat; i++)
                {
                    if (ContinueLearning == false)
                        break;


                    try
                    {


                        var result = propagationLearning.NetworkLearn(out var collision);
                        collisons.Add(collision);
                        if (worstResult > result || worstResult == -1)
                            worstResult = result;
                        if (bestResult < result)
                            bestResult = result;
                        averageResult += result;
                        
                        if (i % 20 == 0)
                        {
                            App.Current.Dispatcher.Invoke(() =>
                            {
                                SeriesCollection[0].Values.Add(new ObservablePoint(i, averageResult / 20));
                                SeriesCollection[1].Values.Add(new ObservablePoint(i, bestResult));
                                SeriesCollection[2].Values.Add(new ObservablePoint(i, worstResult));

                                if (SeriesCollection[0].Values.Count > 20)
                                    SeriesCollection[0].Values.RemoveAt(0);
                                if (SeriesCollection[1].Values.Count > 20)
                                    SeriesCollection[1].Values.RemoveAt(0);
                                if (SeriesCollection[2].Values.Count > 20)
                                    SeriesCollection[2].Values.RemoveAt(0);


                                CollysionSeriesCollection[0].Values.Add(new ObservablePoint(i, collisons.Count(x => x == KindOfCollision.SnakeCollision)));
                                CollysionSeriesCollection[1].Values.Add(new ObservablePoint(i, collisons.Count(x => x == KindOfCollision.WallCollision)));
                                CollysionSeriesCollection[2].Values.Add(new ObservablePoint(i, collisons.Count(x => x == KindOfCollision.TimeCollision)));

                                if (CollysionSeriesCollection[0].Values.Count > 20)
                                    CollysionSeriesCollection[0].Values.RemoveAt(0);
                                if (CollysionSeriesCollection[1].Values.Count > 20)
                                    CollysionSeriesCollection[1].Values.RemoveAt(0);
                                if (CollysionSeriesCollection[2].Values.Count > 20)
                                    CollysionSeriesCollection[2].Values.RemoveAt(0);

                                collisons.Clear();

                                OnPropertyChanged("SeriesCollection");
                                OnPropertyChanged("CollysionSeriesCollection");

                                Iteration = i;
                                IterationText = $"{i}/{MaxRepeat}";
                                TheBestSnakeText = $"Najlepszy = {bestResult}";
                                TheWorstSnakeText = $"Najgorszy = {worstResult}";
                                TheAverageSnakeText = $"Średnia = {averageResult / 20}";

                                result = 0;
                                averageResult = 0;
                                bestResult = 0;
                                worstResult = -1;
                            });
                        }
                      
                    }
                    catch (Exception ex)
                    {
                    }
                }
            });



            Iteration = 0;
            IterationText = $"";
            TheBestSnakeText = $"";
            TheWorstSnakeText = $"";
            TheAverageSnakeText = $"";


            return network.GetWages().ToList();

        }
        public async Task<IList<double>> StartGeneticTraning(Network network, GeneticPackage package, int countOfRepeat)
        {

            SeriesCollection[0].Values.Clear();
            SeriesCollection[1].Values.Clear();
            SeriesCollection[2].Values.Clear();


            CollysionSeriesCollection[0].Values.Clear();
            CollysionSeriesCollection[1].Values.Clear();
            CollysionSeriesCollection[2].Values.Clear();

            MaxRepeat = countOfRepeat;
            var copyier = new CopyOfNetwork(network);

            var population = new SnakeGeneticPopulation(package.CountOfPeopleInPopulation, package.PropablityOfCross, package.PropablityOfMutaion, package.LentghtOfChromoson, null, new CrossOperatorTwoPointCrossover(), new MutationOperatorChange(), new TournamentSelection());

            population.Init(rand => (rand * 2 - 1) * Math.Sqrt(2.0 / (network.Inputs.Size + network.Outputs.Size)));

            Genetic = new GeneticLearning(copyier, population, 50, 10, 10, countOfRepeat, SelectedDifficulty);
            Genetic.IterationEvent += IncreasedIteration;
            SnakeElement.UiIs = false;

            var person = await Genetic.GetBestPerson();

            if (person == null)
                return null;

            var bestNetwork = copyier.Copy();

            return person.Value;

        }

        public bool IncreasedIteration(int iteration, int bestResult,int worstResult, double averageResult,List<KindOfCollision> collisons)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                SeriesCollection[0].Values.Add(new ObservablePoint(iteration, averageResult));
                SeriesCollection[1].Values.Add(new ObservablePoint(iteration, bestResult));
                SeriesCollection[2].Values.Add(new ObservablePoint(iteration, worstResult));

                if (SeriesCollection[0].Values.Count > 20)
                    SeriesCollection[0].Values.RemoveAt(0);
                if (SeriesCollection[1].Values.Count > 20)
                    SeriesCollection[1].Values.RemoveAt(0);
                if (SeriesCollection[2].Values.Count > 20)
                    SeriesCollection[2].Values.RemoveAt(0);


                CollysionSeriesCollection[0].Values.Add(new ObservablePoint(iteration, collisons.Count(x => x == KindOfCollision.SnakeCollision)));
                CollysionSeriesCollection[1].Values.Add(new ObservablePoint(iteration, collisons.Count(x => x == KindOfCollision.WallCollision)));
                CollysionSeriesCollection[2].Values.Add(new ObservablePoint(iteration, collisons.Count(x => x == KindOfCollision.TimeCollision)));

                if (CollysionSeriesCollection[0].Values.Count > 20)
                    CollysionSeriesCollection[0].Values.RemoveAt(0);
                if (CollysionSeriesCollection[1].Values.Count > 20)
                    CollysionSeriesCollection[1].Values.RemoveAt(0);
                if (CollysionSeriesCollection[2].Values.Count > 20)
                    CollysionSeriesCollection[2].Values.RemoveAt(0);

                collisons.Clear();

                OnPropertyChanged("SeriesCollection");
                OnPropertyChanged("CollysionSeriesCollection");

                Iteration = iteration;
                IterationText = $"{iteration}/{MaxRepeat}";
                TheBestSnakeText = $"Najlepszy = {bestResult}";
                TheWorstSnakeText = $"Najgorszy = {worstResult}";
                TheAverageSnakeText = $"Średnia = {averageResult}";


               
              
            });
            return ContinueLearning;
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
