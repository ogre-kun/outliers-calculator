using Wpf_OutliersCalculator.Helpers;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Runtime.CompilerServices;

namespace Wpf_OutliersCalculator.ViewModels
{
    /// <summary>
    /// View model for the QDixon Critical Table window
    /// </summary>
    public class QDixonCriticalTableViewModel : INotifyPropertyChanged
    {
        private Dictionary<int, decimal> criticalValueTable;
        private List<QDixonCriticalTableRow> criticalValueRows = new List<QDixonCriticalTableRow>();

        /// <summary>
        /// Property changed event handler
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Event raised when Save button is clicked
        /// </summary>
        public event Action SaveButtonClicked;

        /// <summary>
        /// State of the data grid if read only
        /// </summary>
        public bool IsDataGridReadOnly { get; private set; } = true;

        /// <summary>
        /// State if Data Grid user can add rows
        /// </summary>
        public bool CanUserAdd => !IsDataGridReadOnly;

        /// <summary>
        /// State of the edit button if enabled
        /// </summary>
        public bool IsEditButtonEnabled => IsDataGridReadOnly;

        /// <summary>
        /// State of the save button
        /// </summary>
        public bool IsSaveButtonEnabled { get; }

        /// <summary>
        /// Command for the edit button
        /// </summary>
        public QDixonCommand EditButtonCommand { get; private set; }

        public QDixonCommand SaveButtonCommand { get; private set; }

        /// <summary>
        /// Table of critical values expressed as list of QDixonCriticalTableRows
        /// </summary>
        public List<QDixonCriticalTableRow> CriticalValueTable => criticalValueRows;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="table">Critical value table</param>
        public QDixonCriticalTableViewModel(Dictionary<int, decimal> table)
        {
            criticalValueTable = table;
            EditButtonCommand = new QDixonCommand(OnEditButtonClicked);
            SaveButtonCommand = new QDixonCommand(OnSaveButtonClicked);
            ConvertDictionaryToList();
        }

        /// <summary>
        /// Converts the input dictionary of critical table to a list of QDixonCriticalTableRows
        /// </summary>
        private void ConvertDictionaryToList()
        {
            foreach(var kvp in criticalValueTable) 
            { 
                criticalValueRows.Add( new QDixonCriticalTableRow(kvp.Key.ToString(), kvp.Value.ToString()) );
            }
        }

        /// <summary>
        /// Handles when edit button is clicked
        /// </summary>
        private void OnEditButtonClicked()
        {
            IsDataGridReadOnly = false;
            InvokePropertyChanges();
        }

        /// <summary>
        /// Handles when the save button is clicked
        /// </summary>
        private void OnSaveButtonClicked()
        {
            SaveButtonClicked?.Invoke();
            IsDataGridReadOnly = true;
            InvokePropertyChanges();
        }

        /// <summary>
        /// Invokes all property changes
        /// </summary>
        private void InvokePropertyChanges()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsDataGridReadOnly)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanUserAdd)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEditButtonEnabled)));
        }
    }
}
