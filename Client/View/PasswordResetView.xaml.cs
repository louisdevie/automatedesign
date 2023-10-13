using AutomateDesign.Client.Model.Network;
using AutomateDesign.Client.View.Helpers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour PasswordResetView.xaml
    /// </summary>
    public partial class PasswordResetView : NavigablePage
    {
        private UsersClient users;

        public string Email { get; set; }

        public PasswordResetView()
        {
            this.users = new UsersClient();
            this.Email = string.Empty;

            InitializeComponent();
            DataContext = this;
        }

        private bool IsFormEnabled
        {
            set
            {
                this.emailBox.IsEnabled = value;
                this.checkBoxButton.IsEnabled = value;
                this.continueButton.IsEnabled = value;
            }
        }

        private void resetPasswordBUttonClick(object sender, RoutedEventArgs e)
        {
            if (!(this.checkBoxButton.IsChecked ?? false))
            {
                this.checkBoxText.Foreground = new SolidColorBrush(Colors.Red);
            } 
            else
            {
                this.users.ResetPasswordAsync(this.Email)
                .ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        ErrorMessageBox.Show(task.Exception?.InnerException);
                        this.IsEnabled = true;
                    }
                    else
                    {
                        this.Navigator.Go(new EmailVerificationView(task.Result, true));
                    }
                },
                TaskScheduler.FromCurrentSynchronizationContext());

                this.IsEnabled = false;
            }
        }
    }
}
