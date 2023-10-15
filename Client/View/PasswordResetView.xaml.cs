using AutomateDesign.Client.Model;
using AutomateDesign.Client.Model.Network;
using AutomateDesign.Client.View.Controls;
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
        private EmailInputHelper emailInputHelper;

        public string Email { get; set; }

        public PasswordResetView()
        {
            this.users = new UsersClient();
            this.Email = string.Empty;

            InitializeComponent();
            this.emailInputHelper = new(this.emailBox);
            DataContext = this;
        }

        private void ResetPasswordButtonClick(object sender, RoutedEventArgs e)
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
                       this.Navigator.Go(new EmailVerificationView(task.Result, new PasswordResetVerification()));
                   }
               },
               TaskScheduler.FromCurrentSynchronizationContext());

            this.IsEnabled = false;

        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            this.Navigator.Back();
        }
    }
}
