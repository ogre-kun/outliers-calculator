using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Wpf_OutliersCalculator.Helpers
{
    public class QDixonCommand : ICommand
    {
        private Action Command;
        public event EventHandler? CanExecuteChanged = (o, e) => { };

        public bool CanExecute(object? parameter)
        {
            return Command != null;
        }

        public void Execute(object? parameter)
        {
            Command?.Invoke();
        }

        public QDixonCommand(Action command)
        {
            Command = command;
        }
    }
}
