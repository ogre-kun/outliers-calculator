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
        /// Returns the new N after removing outliers
        /// </summary>
        public int NNew => IntermediateArray.Count<decimal>();

        /// <summary>
        /// The outliers produced during this step
        /// </summary>
        public List<decimal> IntermediateOutliers { get; private set; }

        /// <summary>
        /// The result of the low test this step
        /// </summary>
        public decimal LowTestValue { get; private set; }

        /// <summary>
        /// The result of the high test this step
        /// </summary>
        public decimal HighTestValue { get; private set; }

        /// <summary>
        /// The critical value this step
        /// </summary>
        public decimal CritValue { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="step_type"></param>
        /// <param name="array"></param>
        /// <param name="outliers"></param>
        public QDixonStep(string step_type, decimal[] array, List<decimal> outliers, decimal lowTestValue, decimal highTestValue, decimal critvalue)
        {
            StepType = step_type;
            IntermediateArray = array;
            IntermediateOutliers = outliers;
            LowTestValue = lowTestValue;
            HighTestValue = highTestValue;
            CritValue = critvalue;
        }
    }
}
