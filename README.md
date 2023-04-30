# Outliers Calculator

This is a small project I did to learn WPF and the MVVM pattern.
My goal was to create a small application wherein I can put a small data set and automatically determine the outliers using the Q-Dixon method.

The Q-Dixon method is suitable for small data set, mostly around 4-30 data.

The following are capabilities of this application:
1. Determine the outliers in a small data set using Q-Dixon.
2. Enumerate the steps it took while performing the analysis
3. Show all the outliers
4. Show the critical values table that was used. There is a default critical table (Assumed 95% confidence level). It is possible for user to edit the table.
5. Show the scatter plot of the data set before and after the outliers determination. (Uses OxyPlot NuGet).

Note: The default critical table is the one I normally use at work. It may not be similar to what is in the literatures.
