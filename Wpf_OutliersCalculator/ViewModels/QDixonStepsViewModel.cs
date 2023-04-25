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
    public class QDixonStepsViewModel
    {
        public DataTable StepsTable { get; private set; } = new();

        public QDixonStepsViewModel(List<QDixonStep> steps)
        {
            GenerateStepsTable(steps);
        }

        private void GenerateStepsTable(List<QDixonStep> steps)
        {
            StepsTable.Columns.Add("Step No", typeof(int));
            StepsTable.Columns.Add("Crit Value", typeof(decimal));
            StepsTable.Columns.Add("Low Test", typeof(decimal));
            StepsTable.Columns.Add("High Test", typeof(decimal));
            StepsTable.Columns.Add("Type", typeof(string));
            StepsTable.Columns.Add("New Set", typeof (string));
            StepsTable.Columns.Add("N", typeof(int));
            StepsTable.Columns.Add("Outliers", typeof(string));

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
