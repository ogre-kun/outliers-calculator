using PropertyChanged;
using Wpf_OutliersCalculator.Helpers;
using Lib_OutliersCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Wpf_OutliersCalculator.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class QDixonViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// QDixon object as model
        /// </summary>
        private QDixon? modelQDixon = null;

        /// <summary>
        /// Data set as input in the view
        /// </summary>
        public string InputDataSet { get; set; } = string.Empty;

        /// <summary>
        /// Event raised when a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Event handler for errors
        /// </summary>
        public event Action<string>? ErrorOccurred;

        /// <summary>
        /// Outliers as string
        /// </summary>
        public string Outliers
        {
            get
            {
                if (modelQDixon == null || modelQDixon.Outliers == null || modelQDixon.Outliers.Count == 0)
                    return string.Empty;
                return string.Join(", ", modelQDixon.Outliers.Select(x => x.ToString()));
            }
        }

        /// <summary>
        /// New data set after removing the outliers as string
        /// </summary>
        public string NewDataSet
        {
            get
            {
                if (modelQDixon == null || modelQDixon.SortedFinalSet == null)
                    return string.Empty;
                return string.Join(", ", modelQDixon.SortedFinalSet.Select(x => x.ToString()));
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public QDixonViewModel()
        {
            CalculateCommand = new QDixonCommand(Calculate);
            ResetCommand = new QDixonCommand(Reset);
        }

        /// <summary>
        /// Command to calculate the QDixon
        /// </summary>
        public QDixonCommand CalculateCommand { get; private set; }

        /// <summary>
        /// Command to reset the QDixon data
        /// </summary>
        public QDixonCommand ResetCommand { get; private set; }

        /// <summary>
        /// Convert the data in the input text box to list of decimal for consumption of the QDixon calculator
        /// </summary>
        /// <returns></returns>
        private List<decimal> ConvertInput()
        {
            if (InputDataSet == null || InputDataSet == string.Empty)
            {
                throw new Exception("Input data empty.");
            }

            try
            {
                return InputDataSet.
                        Trim().
                        Split(',').
                        Select(x => decimal.Parse(x)).
                        ToList<decimal>();
            }
            catch (Exception)
            {
                throw new Exception("Input data error.");
            }        
        }

        /// <summary>
        /// Performs the QDixon from the input data set and stores the data in the modelQDixon object
        /// </summary>
        private void Calculate() 
        {
            try
            {
                var dataList = ConvertInput();
                modelQDixon = new QDixon(dataList);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NewDataSet)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Outliers)));
            }
            catch (Exception e)
            {
                OnErrorOccurred(e.Message);
            }
        }

        /// <summary>
        /// Resets the QDixon object
        /// </summary>
        private void Reset()
        {
            modelQDixon = null;
            InputDataSet = string.Empty;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NewDataSet)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Outliers)));
        }


        /// <summary>
        /// Error occurred event handler
        /// </summary>
        /// <param name="errorMessage"></param>
        private void OnErrorOccurred(string errorMessage)
        {
            ErrorOccurred?.Invoke(errorMessage);
        }
    }
}
