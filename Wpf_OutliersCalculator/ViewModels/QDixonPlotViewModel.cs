using System;
using System.Collections.Generic;
using System.Linq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Wpf_OutliersCalculator.ViewModels
{
    public class QDixonPlotViewModel
    {
        /// <summary>
        /// The plot model containing all the data points
        /// </summary>
        public PlotModel OriginalPlotModel { get; private set; } = new PlotModel { Title = "Original Set" };
        
        /// <summary>
        /// The plot model excluding the outliers
        /// </summary>
        public PlotModel NewPlotModel { get; private set; } = new PlotModel { Title = "New Set" };

        public QDixonPlotViewModel(List<decimal> outliers, List<decimal> newSet)
        {
            CreatePlotModels(outliers, newSet);
        }

        /// <summary>
        /// Record to hold a data and indicate whether it is outlier or not
        /// </summary>
        /// <param name="value">data value</param>
        /// <param name="isOutlier">if it is outlier or not</param>
        record data_point (decimal value, bool isOutlier);


        /// <summary>
        /// Places data in the plot models
        /// </summary>
        /// <param name="outliers">list of outliers</param>
        /// <param name="newSet">list of the values in the new set (excluding outliers)</param>
        private void CreatePlotModels(List<decimal> outliers, List<decimal> newSet)
        {
            //Define the original data plot model
            OriginalPlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Position" });
            OriginalPlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Data Value" });
            NewPlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Position" });
            NewPlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Data Value" });

            var OutlierSeries = new ScatterSeries
            {
                MarkerType = MarkerType.Cross,
                MarkerSize = 5,
                MarkerStroke = OxyColors.Red,
                MarkerStrokeThickness = 1.0,
                MarkerFill = OxyColors.Red,
            };
            var NewSetSeries = new ScatterSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 5,
                MarkerStroke = OxyColors.Blue,
                MarkerStrokeThickness = 1.0,
                MarkerFill = OxyColors.Blue,
            };
            var NewSetSeriesCopy = new ScatterSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 5,
                MarkerStroke = OxyColors.Blue,
                MarkerStrokeThickness = 1.0,
                MarkerFill = OxyColors.Blue,
            };

            //A container to put all data point and note if they are outlier or not
            var OGPoints = new List<data_point>();

            //Put the outliers and the new set in a list
            outliers?.ForEach(x => OGPoints.Add(new data_point(x, true)));
            newSet.ForEach(x => OGPoints.Add(new data_point(x, false)));

            var OGAverage = Math.Round(OGPoints.Select(x => x.value).Average(), 2);
            var NewAverage = Math.Round(newSet.Average(), 2);

            //Define horizontal series from the data averages
            var horizontalOGAverage = new LineSeries
            {
                Color = OxyColors.Green,
                LineStyle = LineStyle.Dash,
                StrokeThickness = 1,
                Points = { new DataPoint(0, (double)OGAverage), 
                    new DataPoint(OGPoints.Count + 1, (double)OGAverage) }
            };
            var horizontalNewAverage = new LineSeries
            {
                Color = OxyColors.Green,
                LineStyle = LineStyle.Dash,
                StrokeThickness = 1,
                Points = { new DataPoint(0, (double)NewAverage), 
                    new DataPoint(newSet.Count + 1, (double)NewAverage) }
            };

            //Sort the original points
            OGPoints = OGPoints.OrderBy(point => point.value).ToList();

            int i = 1, j = 1;
            foreach (var ogpoint in  OGPoints)
            {
                if(ogpoint.isOutlier)
                    OutlierSeries.Points.Add(new ScatterPoint(i++, (double) ogpoint.value));
                else
                {
                    NewSetSeries.Points.Add(new ScatterPoint(i++, (double)ogpoint.value));
                    NewSetSeriesCopy.Points.Add(new ScatterPoint(j++, (double)ogpoint.value));
                }
            }

            //Place the outliers and the new set series in a plot
            OriginalPlotModel.Series.Add(OutlierSeries);
            OriginalPlotModel.Series.Add(NewSetSeries);
            NewPlotModel.Series.Add(NewSetSeriesCopy);

            //Add the horizontal data series
            OriginalPlotModel.Series.Add(horizontalOGAverage);
            NewPlotModel.Series.Add(horizontalNewAverage);
        }
    }
}
