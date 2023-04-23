namespace Wpf_OutliersCalculator.Helpers
{
    public class QDixonCriticalTableRow
    {
        /// <summary>
        /// The number indicating how may items in a population
        /// </summary>
        public string N { get; init; }

        /// <summary>
        /// The critical value corresponding to the N itesm of a population
        /// </summary>
        public string Value { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="n">N number of items</param>
        /// <param name="value">Critical value</param>
        public QDixonCriticalTableRow(string n, string value)
        {
            N = n;
            Value = value;
        }
    }
}
