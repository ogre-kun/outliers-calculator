using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf_OutliersCalculator.ViewModels;

namespace Wpf_OutliersCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var qdixonVM = this.Resources["QDixonVM"] as QDixonViewModel;
            if(qdixonVM != null)
            {
                qdixonVM.ErrorOccurred += ViewModel_ErrorOccurred;
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
    }
}
