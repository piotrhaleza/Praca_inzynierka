using MachineLearning;
using MachineLearningWpfUI.Base;
using MachineLearningWpfUI.Models.Enums;
using MachineLearningWpfUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLearningWpfUI.Views;
using MachineLearning.Funcs.Helpers;

namespace MachineLearningWpfUI.ViewModels
{
    internal class PropertiesViewModel : BaseViewModel
    {
        #region Properties

        public LayerModel EditLayer
        {
            get { return editLayer; }
            set
            {
                editLayer = value;
                OnPropertyChanged();
            }
        }
        private LayerModel editLayer;

        public ObservableCollection<ParameterModel> Parameters
        {
            get { return parameters; }
            set
            {
                parameters = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<ParameterModel> parameters;

        public bool IsOrginal
        {
            get { return isOrginal; }
            set
            {
                isOrginal = value;
                SetPattern();
                OnPropertyChanged();
            }
        }
        private bool isOrginal;
        public string Pattern
        {
            get { return pattern; }
            set
            {
                pattern = value;
                OnPropertyChanged();
            }
        }
        private string pattern;
        public PropertiesWindow Parent { get; set; }
        #endregion
        #region Constructors
        public PropertiesViewModel(LayerModel layer, PropertiesWindow parent)
        {
            isOrginal = false;
            EditLayer = layer;
            Parent= parent;
            Parameters = new ObservableCollection<ParameterModel>();
            int i = 0;
            foreach (var item in layer.Parameters)
                Parameters.Add(new ParameterModel(item.Key, item.Value,i++%2==0, SetPattern));
        }
        private void SetPattern()
        {
            if (IsOrginal)
            {
                string result = editLayer.Pattern;
                FuncCreator.CreateRealFunc(result, parameters.ToDictionary(x => x.Key, x => x.Value));
                foreach (var item in parameters)
                {
                    result = result.Replace(item.Key, item.Value.ToString());
                }
                Pattern = result;
            }

            else
                Pattern = editLayer.Pattern;
        }
        
        #endregion
        #region Commands
        public Command EndCommand
        {
            get
            {
                return new Command(() =>
                {
                    Parent.Close();
                });
            }
        }
        #endregion
    }
}
