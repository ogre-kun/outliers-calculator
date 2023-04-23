namespace Lib_OutliersCalculator
{
    public class QDixon
    {
        private decimal[] data_array;
        private decimal[] data_array_sorted;
        private int data_count;
        private Dictionary<int, decimal> criticalvalue_table;
        private static Dictionary<int, decimal> default_crittable = new Dictionary<int, decimal>()
        {
            { 3 , 0.970m },
            { 4 , 0.829m },
            { 5 , 0.710m },
            { 6 , 0.628m },
            { 7 , 0.569m },
            { 8 , 0.608m },
            { 9 , 0.564m },
            { 10 , 0.530m },
            { 11 , 0.502m },
            { 12 , 0.479m },
            { 13 , 0.611m },
        };

        /// <summary>
        /// The steps performed for the Q-Dixon analysis
        /// </summary>
        public List<QDixonStep> Steps { get; private set; } = new List<QDixonStep>();

        /// <summary>
        /// Critical value for N (count) of data. Based on the Q-dixon table
        /// </summary>
        public decimal CriticalValue => criticalvalue_table[data_count];

        /// <summary>
        /// Critical value table
        /// </summary>
        public Dictionary<int, decimal> CriticalTable 
        { 
            get
            {
                return default_crittable;
            }
        }

        /// <summary>
        /// List of the outliers
        /// </summary>
        public List<decimal>? Outliers { get; private set; } = null;

        /// <summary>
        /// The final set of data after removing the outliers, sorted
        /// </summary>
        public List<decimal>? SortedFinalSet { get; private set; } = null;

        /// <summary>
        /// Final average of the sorted final set
        /// </summary>
        public decimal FinalAverage 
        { 
            get 
            { 
                if(SortedFinalSet == null || SortedFinalSet.Count == 0 )
                {
                    throw new ArgumentException("The final sorted data cannot be null or of length zero.");
                }
                return Math.Round(SortedFinalSet.Average(), 3);
            }
        }

        /// <summary>
        /// QDixon constructor that uses the default critical table
        /// </summary>
        /// <param name="data">the list of data</param>
        public QDixon(List<decimal> data) : this(data, default_crittable) { }

        /// <summary>
        /// QDixon constructor
        /// </summary>
        /// <param name="data">the list of data</param>
        /// <param name="critvalue_table">the critical values table</param>
        public QDixon(List<decimal> data, Dictionary<int, decimal> critvalue_table)
        {
            //Throw exception if data count is greater than 30 or less than 3
            if (data.Count < 3 || data.Count > 30)
                throw new ArgumentException($"Input data should be of length less than 30 and greater than 3. Current data count = {data.Count}");

            //Throw exception when N is not in input critical value table
            if (critvalue_table.ContainsKey(data.Count) == false)
                throw new ArgumentException($"Critical value table does not contain entry for count of input data ({data.Count})");

            data_array = data.ToArray();
            data_array_sorted = data.OrderBy(x => x).ToArray<decimal>();
            criticalvalue_table = critvalue_table;
            data_count = data.Count();

            //Execute the QDixon test
            RunQDixon(data_array_sorted, new List<decimal>());
        }

        /// <summary>
        /// The type of test
        /// </summary>
        private enum TestType
        {
            Low,
            High
        }

        /// <summary>
        /// Executes the Q-Dixon test and populates the Outliers list and final set
        /// </summary>
        /// <param name="sorted_data">the input data that is sorted</param>
        /// <param name="outliers">container for the outliers</param>
        private void RunQDixon(decimal[] sorted_data, List<decimal> outliers)
        {
            var N = sorted_data.Length - 1;
            var crit_value = criticalvalue_table[sorted_data.Length];
            var low_test_val = Test(TestType.Low, sorted_data);
            var high_test_val = Test(TestType.High, sorted_data);
            decimal[] new_data_array = null;

            SortedFinalSet = sorted_data.ToList<decimal>();

            if(low_test_val >= crit_value && high_test_val >= crit_value)
            {
                outliers.Add(sorted_data[0]);
                outliers.Add(sorted_data[N]);
                new_data_array = RemoveItems(sorted_data, true, true);
                SortedFinalSet = new_data_array.ToList<decimal>();
                Steps.Add(new QDixonStep("high-low", new_data_array, new List<decimal>() { sorted_data[0], sorted_data[N] }, 
                    low_test_val, high_test_val, crit_value));
                RunQDixon(new_data_array, outliers);
            } else if (low_test_val >= crit_value)
            {
                outliers.Add(sorted_data[0]);
                new_data_array = RemoveItems(sorted_data, true, false);
                SortedFinalSet = new_data_array.ToList<decimal>();
                Steps.Add(new QDixonStep("low", new_data_array, new List<decimal>() { sorted_data[0] }, 
                    low_test_val, high_test_val, crit_value));
                RunQDixon(new_data_array, outliers);
            } else if (high_test_val >= crit_value)
            {
                outliers.Add(sorted_data[N]);
                new_data_array = RemoveItems(sorted_data, false, true);
                SortedFinalSet = new_data_array.ToList<decimal>();
                Steps.Add(new QDixonStep("high", new_data_array, new List<decimal>() { sorted_data[N] }, 
                    low_test_val, high_test_val, crit_value));
                RunQDixon(new_data_array, outliers);
            }
            Outliers = outliers;
            return;
        }

        /// <summary>
        /// Removes the first and/or last items in an array
        /// </summary>
        /// <param name="array">the input array</param>
        /// <param name="removeFirst">if true, removes the first item</param>
        /// <param name="removeLast">if true, removes the last item</param>
        /// <returns>a copy of the array with the specified items removed</returns>
        private decimal[] RemoveItems(decimal[] array, bool removeFirst, bool removeLast)
        {
            int startIndex = removeFirst ? 1 : 0;
            int endIndex = removeLast ? array.Length - 1 : array.Length;
            decimal[] result = new decimal[endIndex - startIndex];
            Array.Copy(array, startIndex, result, 0, result.Length);
            return result;
        }

        /// <summary>
        /// Returns the value to be compared to the critical number
        /// </summary>
        /// <param name="testType">Either high or low test</param>
        /// <param name="sorted_data_array">the sorted data array</param>
        /// <returns></returns>
        private decimal Test(TestType testType, decimal[] sorted_data_array)
        {
            var len = sorted_data_array.Length;
            var indx = GetIndexes(testType, len - 1);
            var nume = (sorted_data_array[indx[0]] - sorted_data_array[indx[1]]);
            var deno = (sorted_data_array[indx[2]] - sorted_data_array[indx[3]]);
            return nume / deno;
        }

        /// <summary>
        /// Gets the indexes or positions of the data to be used in the test
        /// </summary>
        /// <param name="testType">Type of test either low or high</param>
        /// <param name="N">Population count</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Occurs when the case for test is not found</exception>
        private int[] GetIndexes(TestType testType, int N)
        {
            switch (testType, N)
            {
                //Low Tests indexes
                case (TestType.Low, int num) when (num > 3 && num <= 7):
                    return new int[4] { 1, 0, N, 0 };
                case (TestType.Low, int num) when (num >= 8 && num <= 10):
                    return new int[4] { 1, 0, N - 1, 0 };
                case (TestType.Low, int num) when (num >= 11 && num <= 13):
                    return new int[4] { 2, 0, N - 1, 0 };
                case (TestType.Low, int num) when (num >= 14 && num <= 30):
                    return new int[4] { 2, 0, N - 2, 0 };
                //High Tests indexes
                case (TestType.High, int num) when (num > 3 && num <= 7):
                    return new int[4] { N, N - 1, N, 0 };
                case (TestType.High, int num) when (num >= 8 && num <= 10):
                    return new int[4] { N, N - 1, N, 1 };
                case (TestType.High, int num) when (num >= 11 && num <= 13):
                    return new int[4] { N, N - 2, N, 1 };
                case (TestType.High, int num) when (num >= 14 && num <= 30):
                    return new int[4] { N, N - 2, N, 2 };
            }
            throw new ArgumentException("Switch case not found for GetIndexes");
        }
    }
}
