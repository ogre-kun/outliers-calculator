using System;
using System.Windows.Input;

namespace Wpf_OutliersCalculator.Helpers
{
    /// <summary>
    /// Command class for the QDixon implementation
    /// </summary>
    public class QDixonCommand : ICommand
    {
        private Action Command;

        /// <summary>
        /// Event implementation for the ICommand interface
        /// </summary>
        public event EventHandler? CanExecuteChanged = (o, e) => { };

        /// <summary>
        /// Can execute implementation for the ICommand interface
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object? parameter)
        {
            return Command != null;
        }

        /// <summary>
        /// Execute the Action, implementation for ICommand interface
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object? parameter)
        {
            Command?.Invoke();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="command">Action delegate to be executed in this command</param>
        public QDixonCommand(Action command)
        {
            Command = command;
        }
    }
}
