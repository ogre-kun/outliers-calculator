using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public List<QDixonStep> Steps { get; private set; }

        /// <summary>
        /// Critical value for N (count) of data. Based on the Q-dixon table
        /// </summary>
        public decimal CriticalValue => criticalvalue_table[data_count];

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
                return SortedFinalSet.Average();
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
                throw new ArgumentException("Input data should be of length less than 30 and greater than 3");

            //Throw exception when N is not in input critical value table
            if (critvalue_table.ContainsKey(data.Count) == false)
                throw new ArgumentException("Critical value table does not countain entry for count of input data (N)");

            data_array = data.ToArray();
            data_array_sorted = data.OrderBy(x => x).ToArray<decimal>();
            criticalvalue_table = critvalue_table;
            data_count = data.Count();

            //Execute the QDixon test
            ExecuteQDixon(data_array_sorted, new List<decimal>());
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
        /// Executes the Q-Dixon test and populates the Outliers list and final 
        /// </summary>
        /// <param name="sorted_data">the input data that is sorted</param>
        /// <param name="outliers">container for the outliers</param>
        private void ExecuteQDixon(decimal[] sorted_data, List<decimal> outliers)
        {
            var N = sorted_data.Length;
            var crit_value = criticalvalue_table[N];
            var low_test_val = Test(TestType.Low, sorted_data);
            var high_test_val = Test(TestType.High, sorted_data);
            decimal[] new_data_array;

            if(low_test_val >= crit_value && high_test_val >= crit_value)
            {
                outliers.Add(sorted_data[0]);
                outliers.Add(sorted_data[N]);
                new_data_array = RemoveFirstLast(sorted_data);
                Steps.Add(new QDixonStep("high-low", new_data_array, new List<decimal>() { sorted_data[0], sorted_data[N] }));
                ExecuteQDixon(new_data_array, outliers);
            } else if (low_test_val >= crit_value)
            {
                outliers.Add(sorted_data[0]);
                new_data_array = RemoveFirst(sorted_data);
                Steps.Add(new QDixonStep("low", new_data_array, new List<decimal>() { sorted_data[0] }));
                ExecuteQDixon(new_data_array, outliers);
            } else if (high_test_val >= crit_value)
            {
                outliers.Add(sorted_data[N]);
                new_data_array = RemoveLast(sorted_data);
                Steps.Add(new QDixonStep("high", new_data_array, new List<decimal>() { sorted_data[N] }));
                ExecuteQDixon(new_data_array, outliers);
            }
            Outliers = outliers;
            SortedFinalSet = sorted_data.ToList<decimal>();
            return;
        }

        /// <summary>
        /// Removes the first and last item in an array
        /// </summary>
        /// <param name="array"></param>
        /// <returns>Copy of the array with first and last items removed</returns>
        private decimal[] RemoveFirstLast(decimal[] array)
        {
            decimal[] result = new decimal[array.Length - 2];
            Array.Copy(array, 1, result, 0, result.Length);
            return result;
        }


        /// <summary>
        /// Removes the first item in the array
        /// </summary>
        /// <param name="array"></param>
        /// <returns>Copy of the array with the first item removed</returns>
        private decimal[] RemoveFirst(decimal[] array)
        {
            decimal[] result = new decimal[array.Length -1];
            Array.Copy(array, 1, result, 0, array.Length - 1);
            return result;
        }

        /// <summary>
        /// Removes the last item in the array
        /// </summary>
        /// <param name="array"></param>
        /// <returns>Copy of the array with the last item removed</returns>
        private decimal[] RemoveLast(decimal[] array)
        {
            decimal[] result = new decimal[array.Length - 1];
            Array.Copy(array, result, result.Length);
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
            var indx = GetIndexes(testType, len);
            var nume = (sorted_data_array[indx[0]] - sorted_data_array[indx[1]]);
            var deno = (sorted_data_array[indx[2]] - sorted_data_array[indx[3]]);
            return nume / deno;
        }

        /// <summary>
        /// Gets the indexes or positions of the data to be used in the test
        /// </summary>
        /// <param name="testType">Type of test either low or high</param>
        /// <param name="N">Populaiton count</param>
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
