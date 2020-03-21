using System;
using System.Windows.Input;

namespace ParallelSearch.Mvvm
{
    /// <summary>
    /// Implementation of the <see cref="ICommand"/> interface.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        private readonly Action action;

        /// <summary>
        /// Creates a new instance of <see cref="DelegateCommand"/>.
        /// </summary>
        /// <param name="action">Passed action to be executed when the command is executed.</param>
        public DelegateCommand(Action action)
        {
            this.action = action;
        }

        /// <summary>
        /// Event handler for the 'can-execute state has changed' event.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Determines whether the command can be executed.
        /// </summary>
        /// <param name="parameter">Parameter that is not used in this implementation.</param>
        /// <returns>Always <c>true</c>, since this feature is not needed for our cases.</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Executes the command with the given parameters.
        /// </summary>
        /// <param name="parameter">Parameter that is not used in this implementation.</param>
        public void Execute(object parameter)
        {
            this.action();
        }
    }
}