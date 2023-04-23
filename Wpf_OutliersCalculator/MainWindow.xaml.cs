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

        public MainWindow()
        {
            InitializeComponent();

            qdixonVM = this.Resources["QDixonVM"] as QDixonViewModel;
            if(qdixonVM != null)
            {
                qdixonVM.ErrorOccurred += ViewModel_ErrorOccurred;
                qdixonVM.ShowCriticalTableClicked += ViewModel_ShowCriticalTableClicked;
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
    }
}
