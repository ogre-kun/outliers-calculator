using System.Data;
using System.Windows.Documents;
using Wpf_OutliersCalculator.Views;
using Lib_OutliersCalculator;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System;

namespace Wpf_OutliersCalculator.ViewModels
{

    /// <summary>
    /// View model for the QDixonSteps window
    /// </summary>
    public class QDixonStepsViewModel
    {
        /// <summary>
        /// Reference to the DataTable
        /// </summary>
        public DataTable StepsTable { get; private set; } = new();

        /// <summary>
        /// Constructor for the view model
        /// </summary>
        /// <param name="steps">list of QDixonStep</param>
        public QDixonStepsViewModel(List<QDixonStep> steps)
        {
            GenerateStepsTable(steps);
        }

        /// <summary>
        /// Generate the QDixon Steps data table for the list of QDixon steps
        /// </summary>
        /// <param name="steps">QDixonStep, contains details about the performed QDixon step</param>
        private void GenerateStepsTable(List<QDixonStep> steps)
        {
            //Add column and each of their headers and types
            StepsTable.Columns.Add("Step No", typeof(int));
            StepsTable.Columns.Add("Crit Value", typeof(decimal));
            StepsTable.Columns.Add("Low Test", typeof(decimal));
            StepsTable.Columns.Add("High Test", typeof(decimal));
            StepsTable.Columns.Add("Type", typeof(string));
            StepsTable.Columns.Add("New Set", typeof (string));
            StepsTable.Columns.Add("N", typeof(int));
            StepsTable.Columns.Add("Outliers", typeof(string));

            //Add rows to the step table
            int i = 1;
            foreach(var step in steps)
            {
                StepsTable.Rows.Add(
                    i++,
                    step.CritValue,
                    Math.Round(step.LowTestValue,2),
                    Math.Round(step.HighTestValue,2),
                    step.StepType,
                    string.Join(" ", step.IntermediateArray.Select(x => x.ToString())),
                    step.NNew,
                    string.Join(" ", step.IntermediateOutliers.Select(x => x.ToString()))
                );
            }
        }
    }
}
