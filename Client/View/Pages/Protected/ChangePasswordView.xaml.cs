using AutomateDesign.Client.Model.Verifications;
using AutomateDesign.Client.View.Controls;
using AutomateDesign.Client.View.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using AutomateDesign.Client.Model.Network;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour SignUpView.xaml
    /// </summary>
    public partial class ChangePasswordView : NavigablePage
    {
        private UsersClient users;

        public string CurrentPassword => this.currentPasswordBox.Password;

        public string NewPassword => this.newPasswordBox.Password;

        public string NewPasswordAgain => this.newPasswordAgainBox.Password;

        /// <summary>
        /// Envoyer False lors d'un premier appel a la page
        /// </summary>
        public ChangePasswordView()
        {
            this.users = new UsersClient();

            DataContext = this;
            InitializeComponent();
        }

        private void ContinueButtonClick(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(this.CurrentPassword))
            {
                MessageBox.Show("Veuillez saisir votre mot de passe actuel", "Erreur");
            }
            else if (String.IsNullOrEmpty(this.NewPassword))
            {
                MessageBox.Show("Veuillez saisir un nouveau mot de passe", "Erreur");
            }
            else if (this.NewPassword != this.NewPasswordAgain)
            {
                MessageBox.Show("Les mots de passe ne correspondent pas", "Erreur");
            }
            else
            {
                this.users.ChangePasswordAsync(this.Navigator.Session!.UserId, this.NewPassword, this.CurrentPassword)
                .ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        ErrorMessageBox.Show(task.Exception?.InnerException);
                        this.IsEnabled = true;
                    }
                    else
                    {
                        this.Navigator.Go(new EmailVerificationSuccessView(new PasswordChangeVerification()));
                    }
                },
                TaskScheduler.FromCurrentSynchronizationContext());

                this.IsEnabled = false;
            }
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            this.Navigator.Window.Close();
        }
    }
}
