using Xunit.Abstractions;

namespace Test_OutliersCalculator
{
    public class QDixon_Tests
    {
        readonly ITestOutputHelper testOutputHelper;
        List<decimal> set1 = new List<decimal>() { 1.23m, 4.56m, 2.34m, 7.89m, 3.45m, -74.2m, 9.12m, 5.67m, 1.23m, 8.76m, 6.54m, 600.22m, 701.22m};
        List<decimal> set2 = new List<decimal>() { 0.87m, 1.23m, 1.53m, 1.75m, 2.14m, 2.23m, 2.65m, 3.01m, 3.42m, 3.68m, 4.11m, 4.59m, 5.02m, 5.47m, 5.89m, 6.25m, 6.84m, 7.26m, 7.69m, 8.12m };
        List<decimal> set3 = new List<decimal>() { 4.2m, 2.6m, 1.7m, 6.8m, 3.9m, 9.1m, 5.5m, 7.3m, 8.2m, 2.1m, 5.7m, 52.0m };

        public QDixon_Tests(ITestOutputHelper outputHelper)
        {
            testOutputHelper = outputHelper;
        }

        [Fact]
        public void TestQDixonNewAverages()
        {
            var qd1 = new QDixon(set1);
            var qd3 = new QDixon(set3);

            Assert.True(qd1.FinalAverage > 0);
            Assert.True(qd3.FinalAverage > 0);
            Assert.Throws<ArgumentException>(() => { var qd2 = new QDixon(set2); } );
            Assert.Equal(qd3.Outliers?.FirstOrDefault(), 52.0m);
            Assert.Equal(qd1.Outliers?.Count, 3);
            Assert.Equal(qd1.Steps?.Count, 2);

            testOutputHelper.WriteLine(qd1.FinalAverage.ToString());
            testOutputHelper.WriteLine($"{qd1.SortedFinalSet?.Count}");
            testOutputHelper.WriteLine(qd3.FinalAverage.ToString());
            testOutputHelper.WriteLine($"{qd3.SortedFinalSet?.Count}");
        }
    }
}