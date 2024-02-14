using MachineLearingInterfaces;
using MachineLearingInterfaces.ActivationFunc;
using MachineLearning.Funcs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningWpfUI.Models
{
    public class LayerModel : INotifyPropertyChanged
    {
        #region Private fileds
        private int id;
        private ActivationFuncEnum kindActivation;
        private string pattern;
        private string realPattern { get; set; }
        #endregion

        #region Public Properties

        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                OnPropertyChanged(nameof(Odd));
                OnPropertyChanged();
            }
        }
        
        public ActivationFuncEnum KindActivation
        {
            get
            {
                return kindActivation;
            }
            set
            {
                kindActivation = value;

                switch (kindActivation)
                {
                    case ActivationFuncEnum.Sigmoid:
                        Pattern = SigmoidActivationFunc.OrginalPattern;
                        Parameters = SigmoidActivationFunc.OrginalParameters;
                        break;
                    case ActivationFuncEnum.Lineral:
                        Pattern = LineralActivationFunc.OrginalPattern;
                        Parameters = LineralActivationFunc.OrginalParameters; 
                        break;
                    case ActivationFuncEnum.Tanh:
                        Pattern = TanhActivationFunc.OrginalPattern;
                        Parameters = TanhActivationFunc.OrginalParameters;
                        break;
                    case ActivationFuncEnum.ReLu:
                        Pattern = ReLuActivationFunc.OrginalPattern;
                        Parameters = ReLuActivationFunc.OrginalParameters;
                        break;
                    case ActivationFuncEnum.SoftMax:
                        Pattern = SoftmaxActivationFunc.OrginalPattern;
                        Parameters = SoftmaxActivationFunc.OrginalParameters; 
                        break;
                    default:
                        break;
                }

                OnPropertyChanged();
            }
        }

        public Dictionary<string,double> Parameters { get; set; }

        public string Pattern
        {
            get
            {
                return pattern;
            }
            set
            {
                pattern = value;
                OnPropertyChanged(nameof(Odd));
                OnPropertyChanged();
            }
        }
       
        public string RealPattern
        {
            get
            {
                return realPattern;
            }
            set
            {
                realPattern = value;
                OnPropertyChanged();
            }
        }
       
        public bool Odd => Id % 2 == 0;

        public int NumberOfNeurons { get; set; }
        #endregion

        #region Constructors
        public LayerModel(int id)
        {
            KindActivation = ActivationFuncEnum.Lineral;
            Id = id;
            NumberOfNeurons = 1;
        }

        public LayerModel(ILayer layer)
        {
            if (layer.ActivationFunc != null)
            {
                KindActivation = layer.ActivationFunc.KindActivation.Map();
            }
            Id = layer.Id;
            NumberOfNeurons = layer.NeuronList.Count;
        }
        #endregion

        #region Static Methods
        public static IEnumerable<LayerModel> Map(IList<ILayer> layers)
        {
            foreach (var layer in layers)
                yield return new LayerModel(layer);
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
