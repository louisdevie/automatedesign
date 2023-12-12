using System;
using System.Windows.Input;

namespace AutomateDesign.Client.ViewModel
{
    public class ActionCommand : ICommand
    {
        private Action action;

        public event EventHandler? CanExecuteChanged;

        public ActionCommand(Action action)
        {
            this.action = action;
        }

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            this.action();
        }
    }
}
