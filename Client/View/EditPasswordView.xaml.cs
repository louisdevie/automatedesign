using AutomateDesign.Client.Model;
using AutomateDesign.Client.Model.Network;
using AutomateDesign.Client.View.Controls;
using AutomateDesign.Client.View.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour SignUpView.xaml
    /// </summary>
    public partial class EditPasswordView : NavigablePage
    {
        private int userId;
        private uint secretCode;
        private UsersClient users;

        public bool UserAgreement { get; set; }

        public string Password => this.passBox.Password;

        public string PasswordAgain => this.passBoxConf.Password;

        /// <summary>
        /// Envoyer False lors d'un premier appel a la page
        /// </summary>
        public EditPasswordView(int userId, uint secretCode)
        {
            this.users = new UsersClient();
            this.userId = userId;
            this.secretCode = secretCode;

            DataContext = this;
            InitializeComponent();
        }

        private void ContinueButtonClick(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(this.Password))
            {
                MessageBox.Show("Veuillez saisir un mot de passe", "Erreur");
            }
            else if (this.Password != this.PasswordAgain)
            {
                MessageBox.Show("Les mots de passe ne correspondent pas", "Erreur");
            }
            else if (!this.UserAgreement)
            {
                this.checkBoxText.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                this.users.ChangePasswordWithResetCodeAsync(this.userId, this.Password, this.secretCode)
                .ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        ErrorMessageBox.Show(task.Exception?.InnerException);
                        this.IsEnabled = true;
                    }
                    else
                    {
                        this.Navigator.Go(new EmailVerificationSuccessView(new PasswordResetVerification()));
                    }
                },
                TaskScheduler.FromCurrentSynchronizationContext());

                this.IsEnabled = false;
            }
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            this.Navigator.Back();
        }
    }
}
