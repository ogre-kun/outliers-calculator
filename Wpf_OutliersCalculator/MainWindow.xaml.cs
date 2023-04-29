using Lib_OutliersCalculator;
using System.Collections.Generic;
using System.Windows;
using Wpf_OutliersCalculator.ViewModels;
using Wpf_OutliersCalculator.Views;

namespace Wpf_OutliersCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel? mainWindowVM;
        private QDixonCriticalTableViewModel? qdixonCriticalTableVM;
        private QDixonStepsViewModel? qdixonStepsVM;
        private QDixonPlotViewModel? qdixonPlotVM;

        private Dictionary<int, decimal>? userSuppliedCritTable = null;

        public MainWindow()
        {
            InitializeComponent();

            mainWindowVM = this.Resources["QDixonVM"] as MainWindowViewModel;
            if(mainWindowVM != null)
            {
                mainWindowVM.ErrorOccurred += ViewModel_ErrorOccurred;
                mainWindowVM.ShowCriticalTableClicked += ViewModel_ShowCriticalTableClicked;
                mainWindowVM.ShowStepsClicked += ViewModel_ShowStepsTableClicked;
                mainWindowVM.ShowPlotClicked += ViewModel_ShowPlotClicked;
            }
        }

        /// <summary>
        /// Shows a message box with details of the error from the view model
        /// </summary>
        /// <param name="errormsg"></param>
        private void ViewModel_ErrorOccurred(string errormsg)
        {
            MessageBox.Show(errormsg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }


        /// <summary>
        /// Opens a new window for the QDixon Critical table, triggered when the button is clicked
        /// </summary>
        private void ViewModel_ShowCriticalTableClicked()
        {
            var critTable = mainWindowVM?.ModelQDixon?.CriticalTable;
            qdixonCriticalTableVM = new QDixonCriticalTableViewModel(critTable);
            var critTableWindow = new QDixonCritTable(qdixonCriticalTableVM)
            {
                Owner = this
            };

            //Set location of crit table window
            double offsetX = 0;
            double offsetY = 0;
            critTableWindow.Left = offsetX + this.Left + this.Width;
            critTableWindow.Top = offsetY + this.Top;

            critTableWindow.DataG.ItemsSource = qdixonCriticalTableVM.CriticalValueTable;
            critTableWindow.DataContext = qdixonCriticalTableVM;
            critTableWindow.CriticalTableUserUpdated += ViewModel_UserCritTableUpdated;

            //Open window as modal
            critTableWindow.ShowDialog();
        }


        /// <summary>
        /// Opens an new window when the show steps button is clicked
        /// </summary>
        private void ViewModel_ShowStepsTableClicked()
        {
            var stepsWindow = new QDixonSteps
            {
                Owner = this
            };
            var steps = mainWindowVM?.ModelQDixon?.Steps;

            //Set location of crit table window
            double offsetX = 0;
            double offsetY = 0;
            stepsWindow.Left = offsetX + this.Left + this.Width;
            stepsWindow.Top = offsetY + this.Top;

            qdixonStepsVM = new QDixonStepsViewModel(steps);
            stepsWindow.StepsTable.ItemsSource = qdixonStepsVM.StepsTable.DefaultView;

            stepsWindow.ShowDialog();
        }

        /// <summary>
        /// Opens a new window to show the plot
        /// </summary>
        private void ViewModel_ShowPlotClicked()
        {
            var plotWindow = new QDixonPlot
            {
                Owner = this
            };
            var outliers = mainWindowVM?.ModelQDixon?.Outliers;
            var newset = mainWindowVM?.ModelQDixon?.SortedFinalSet;
            qdixonPlotVM = new QDixonPlotViewModel(outliers, newset);
            plotWindow.DataContext = qdixonPlotVM;

            //Set location of crit table window
            double offsetX = 0;
            double offsetY = 0;
            plotWindow.Left = offsetX + this.Left + this.Width;
            plotWindow.Top = offsetY + this.Top;

            plotWindow.ShowDialog();
        }

        /// <summary>
        /// Handles when user critical table has been supplied
        /// </summary>
        /// <param name="newTable">user supplied critical table</param>
        private void ViewModel_UserCritTableUpdated(Dictionary<int, decimal> newTable)
        {
            mainWindowVM?.ModelQDixon?.UpdateCritTable(newTable);
            mainWindowVM?.UpdateUserCritTable(newTable);
        }
    }
}
