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

        public ProgressDialog(string title, string progressMessage, string successMessage)
        {
            InitializeComponent();
            this.Title = title;
            this.progress.Text = progressMessage;
            this.finished.Text = successMessage;
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
                this.success = true;
                this.ShowFinished();
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
