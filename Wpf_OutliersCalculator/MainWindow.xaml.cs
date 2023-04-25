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
        private QDixonViewModel? qdixonVM;
        private QDixonCriticalTableViewModel? qdixonCriticalTableVM;
        private QDixonStepsViewModel? qdixonStepsVM;

        public MainWindow()
        {
            InitializeComponent();

            qdixonVM = this.Resources["QDixonVM"] as QDixonViewModel;
            if(qdixonVM != null)
            {
                qdixonVM.ErrorOccurred += ViewModel_ErrorOccurred;
                qdixonVM.ShowCriticalTableClicked += ViewModel_ShowCriticalTableClicked;
                qdixonVM.ShowStepsClicked += ViewModel_ShowStepsTableClicked;
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
            var critTableWindow = new QDixonCritTable();
            var critTable = qdixonVM?.ModelQDixon?.CriticalTable;

            critTableWindow.Owner = this;

            //Set location of crit table window
            double offsetX = 0;
            double offsetY = 0;
            critTableWindow.Left = offsetX + this.Left + this.Width;
            critTableWindow.Top = offsetY + this.Top;

            qdixonCriticalTableVM = new QDixonCriticalTableViewModel(critTable);
            critTableWindow.DataG.ItemsSource = qdixonCriticalTableVM.CriticalValueTable;

            //Open window as modal
            critTableWindow.ShowDialog();
        }


        /// <summary>
        /// Opens an new window when the show steps button is clicked
        /// </summary>
        private void ViewModel_ShowStepsTableClicked()
        {
            var stepsWindow = new QDixonSteps();
            var steps = qdixonVM?.ModelQDixon?.Steps;

            stepsWindow.Owner = this;

            //Set location of crit table window
            double offsetX = 0;
            double offsetY = 0;
            stepsWindow.Left = offsetX + this.Left + this.Width;
            stepsWindow.Top = offsetY + this.Top;

            qdixonStepsVM = new QDixonStepsViewModel(steps);
            stepsWindow.StepsTable.ItemsSource = qdixonStepsVM.StepsTable.DefaultView;

            stepsWindow.ShowDialog();
        }
    }
}
