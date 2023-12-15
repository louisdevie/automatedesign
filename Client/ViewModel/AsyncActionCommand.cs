using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AutomateDesign.Client.ViewModel
{
    /// <summary>
    /// Une implémentation de <see cref="ICommand"/> qui appelle un méthode asynchrone en arrière-plan une seule fois.
    /// </summary>
    public class OnceAsyncCommand : ICommand
    {
        private readonly Func<Task?> action;
        private bool canExecute;
        private readonly bool canRetry;
        private readonly object executeLock;

        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Crée une commande qui s'exécutera en arrière-plan une seule fois.
        /// </summary>
        /// <param name="action">Le travail à effectuer quand la commande est déclenchée.</param>
        /// <param name="canRetry">Indique si l'action peut être effectuée à nouveau si elle échoue.</param>
        public OnceAsyncCommand(Func<Task?> action, bool canRetry)
        {
            this.action = action;
            this.canExecute = true;
            this.canRetry = canRetry;
            this.executeLock = new object();
        }

        public bool CanExecute(object? parameter) => this.canExecute;

        public void Execute(object? parameter)
        {
            lock (this.executeLock)
            {
                if (this.canExecute)
                {
                    Task.Run(this.action).ContinueWith(this.ActionFinished, TaskScheduler.FromCurrentSynchronizationContext());
                    this.canExecute = false;
                    this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void ActionFinished(Task action)
        {
            if (action.IsFaulted || action.IsCanceled)
            {
                lock (this.executeLock)
                {
                    this.canExecute = this.canRetry;
                    this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}
