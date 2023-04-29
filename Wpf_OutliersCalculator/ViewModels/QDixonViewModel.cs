using Wpf_OutliersCalculator.Helpers;
using Lib_OutliersCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace Wpf_OutliersCalculator.ViewModels
{
    public class QDixonViewModel : INotifyPropertyChanged
    {
        private QDixon? modelQDixon = null;

        /// <summary>
        /// QDixon object as model
        /// </summary>
        public QDixon? ModelQDixon => modelQDixon;

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
        /// Raised event to show the critical event button is clicked
        /// </summary>
        public event Action? ShowCriticalTableClicked;

        /// <summary>
        /// Raised event when the click show steps button is clicked
        /// </summary>
        public event Action? ShowStepsClicked;

        /// <summary>
        /// Raised event when the button to show plot is clicked
        /// </summary>
        public event Action? ShowPlotClicked;

        /// <summary>
        /// Outliers as string
        /// </summary>
        public string Outliers
        {
            get
            {
                if (modelQDixon == null || modelQDixon.Outliers == null || modelQDixon.Outliers.Count == 0)
                    return string.Empty;
                return string.Join(",", modelQDixon.Outliers.Select(x => x.ToString()));
            }
        }

        /// <summary>
        /// Whether to enable the steps button depending on the data in steps list
        /// </summary>
        public bool StepsEnabled => modelQDixon?.Steps.Count > 0;

        /// <summary>
        /// Whether to enable or disable the Critical Table button
        /// </summary>
        public bool CritTableButtonEnabled => modelQDixon != null;

        /// <summary>
        /// Whether the plot button is disabled or enabled
        /// </summary>
        public bool PlotButtonEnabled => modelQDixon != null;

        /// <summary>
        /// Average of the new data set;
        /// </summary>
        public string NewDataSetAverage => modelQDixon?.FinalAverage.ToString();

        /// <summary>
        /// New data set after removing the outliers as string
        /// </summary>
        public string NewDataSet
        {
            get
            {
                if (modelQDixon == null || modelQDixon.SortedFinalSet == null)
                    return string.Empty;
                return string.Join(",", modelQDixon.SortedFinalSet.Select(x => x.ToString()));
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public QDixonViewModel()
        {
            CalculateCommand = new QDixonCommand(Calculate);
            ResetCommand = new QDixonCommand(Reset);
            ShowStepsCommand = new QDixonCommand(ShowSteps);
            ShowCriticalTableCommand = new QDixonCommand(ShowCriticalTable);
            ShowPlotCommand = new QDixonCommand(ShowPlot);
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
        /// Command to show the QDixon steps
        /// </summary>
        public QDixonCommand ShowStepsCommand { get; private set; }

        /// <summary>
        /// Command to show the critical table
        /// </summary>
        public QDixonCommand ShowCriticalTableCommand { get; private set; }

        /// <summary>
        /// Command to show the plot
        /// </summary>
        public QDixonCommand ShowPlotCommand { get; private set; }

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
                char[] delimiters = { ',',' ' };
                return InputDataSet.
                        Trim().
                        Split(delimiters).
                        Select(x => decimal.Parse(x)).
                        ToList<decimal>();
            }
            catch (Exception e)
            {
                throw new Exception("Input data error. Separate individual data with EITHER spaces or commas only.");
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
                PropertyChanges();
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
            PropertyChanges();
        }


        /// <summary>
        /// Shows the steps to the QDixon
        /// </summary>
        private void ShowSteps()
        {
            ShowStepsClicked?.Invoke();
        }

        /// <summary>
        /// Raises the ShowPlot button clicked event
        /// </summary>
        private void ShowPlot()
        {
            ShowPlotClicked?.Invoke();
        }


        /// <summary>
        /// Error occurred event handler
        /// </summary>
        /// <param name="errorMessage"></param>
        private void OnErrorOccurred(string errorMessage)
        {
            ErrorOccurred?.Invoke(errorMessage);
        }

        /// <summary>
        /// Contains all property changes invocations
        /// </summary>
        private void PropertyChanges()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NewDataSet)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Outliers)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StepsEnabled)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NewDataSetAverage)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CritTableButtonEnabled)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InputDataSet)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PlotButtonEnabled)));
        }

        /// <summary>
        /// Handler for show critical table command
        /// </summary>
        private void ShowCriticalTable()
        {
            ShowCriticalTableClicked?.Invoke();
        }
    }
}
