using Wpf_OutliersCalculator.Helpers;
using System.Collections.Generic;

namespace Wpf_OutliersCalculator.ViewModels
{
    public class QDixonCriticalTableViewModel
    {
        private Dictionary<int, decimal> criticalValueTable;
        private List<QDixonCriticalTableRow> criticalValueRows = new List<QDixonCriticalTableRow>();


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

    }
}
