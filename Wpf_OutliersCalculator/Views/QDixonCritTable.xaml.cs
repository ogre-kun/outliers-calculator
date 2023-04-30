using System;
using System.Collections.Generic;
using System.Windows;
using Wpf_OutliersCalculator.Helpers;
using Wpf_OutliersCalculator.ViewModels;

namespace Wpf_OutliersCalculator.Views
{
    /// <summary>
    /// Interaction logic for QDixonCritTable.xaml
    /// </summary>
    public partial class QDixonCritTable : Window
    {
        /// <summary>
        /// Event when critical table user is updated
        /// </summary>
        public event Action<Dictionary<int, decimal>> CriticalTableUserUpdated;

        /// <summary>
        /// Reference to the viewmodel of this window
        /// </summary>
        public QDixonCriticalTableViewModel ViewModel { get; private set; }

        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="viewmodel"></param>
        public QDixonCritTable(QDixonCriticalTableViewModel viewmodel)
        {
            ViewModel = viewmodel;
            ViewModel.SaveButtonClicked += ViewModel_SaveButtonClicked;
            InitializeComponent();
        }

        /// <summary>
        /// Callback when save button is clicked
        /// </summary>
        private void ViewModel_SaveButtonClicked()
        {
            //Create the new dictionary
            Dictionary<int, decimal> newCritTableDictionary = new();

            //Converts all of the current item in the DataGrid (as updated by user) into a dictionary
            //It is then passed on to a an event CriticalTableUserUpdated
            foreach(var item in DataG.Items)
            {
                int N;
                try
                {
                    var n = ((QDixonCriticalTableRow)item).N;
                    int.TryParse(n, out N);
                }
                catch (Exception)
                {
                    continue;
                }
                decimal.TryParse(((QDixonCriticalTableRow)item).Value, out decimal Value);
                newCritTableDictionary.Add(N, Value);
            }
            //Pass the new table to the event
            CriticalTableUserUpdated?.Invoke(newCritTableDictionary);
        }
    }
}
