using AutomateDesign.Client.Model.Pipelines;
using System.Windows;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour ProgressPopup.xaml
    /// </summary>
    public partial class ProgressDialog : Window, IPipelineProgress
    {
        private bool success;
        private bool askToContinue;

        /// <summary>
        /// Crée une boîte de dialogue de progression. Un message de succès sera affiché une fois l'opération terminée
        /// et l'utilisateur sera invité à continuer.
        /// </summary>
        /// <param name="title">Le titre de la boîte de dialogue.</param>
        /// <param name="progressMessage">Le message affiché durant l'opération.</param>
        /// <param name="successMessage">Le message affiché une fois l'opération terminée.</param>
        public ProgressDialog(string title, string progressMessage, string successMessage)
        {
            InitializeComponent();
            this.Title = title;
            this.progress.Text = progressMessage;
            this.finished.Text = successMessage;
            this.askToContinue = true;
        }

        /// <summary>
        /// Crée une boîte de dialogue de progression. Elle se fermera automatiquement une fois l'opération terminée.
        /// </summary>
        /// <param name="title">Le titre de la boîte de dialogue.</param>
        /// <param name="progressMessage">Le message affiché durant l'opération.</param>
        public ProgressDialog(string title, string progressMessage)
        {
            InitializeComponent();
            this.Title = title;
            this.progress.Text = progressMessage;
            this.askToContinue = false;
        }

        private void ShowFinished()
        {
            this.progress.Visibility = Visibility.Hidden;
            this.bar.Visibility = Visibility.Hidden;

            this.finished.Visibility = Visibility.Visible;
            this.okButton.Visibility = Visibility.Visible;
        }

        public void Done()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (this.askToContinue)
                {
                    this.success = true;
                    this.ShowFinished();
                }
                else
                {
                    this.DialogResult = true;
                }
            });
        }

        public void Failed(string reason)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.success = false;
                this.finished.Text = reason;
                this.ShowFinished();
            });
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = this.success;
        }
    }
}
