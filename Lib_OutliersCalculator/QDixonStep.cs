using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib_OutliersCalculator
{
    public class QDixonStep
    {
        /// <summary>
        /// Type of step in the q-dixon process, low removal, high removal or low-high removal
        /// </summary>
        public string StepType { get; private set; }
        
        /// <summary>
        /// The intermediate array during the step
        /// </summary>
        public decimal[] IntermediateArray { get; private set; }

        /// <summary>
        /// The outliers produced during this step
        /// </summary>
        public List<decimal> IntermediateOutliers { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="step_type"></param>
        /// <param name="array"></param>
        /// <param name="outliers"></param>
        public QDixonStep(string step_type, decimal[] array, List<decimal> outliers)
        {
            StepType = step_type;
            IntermediateArray = array;
            IntermediateOutliers = outliers;
        }
    }
}
