using AutomateDesign.Client.View.Controls;
using AutomateDesign.Client.View.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using AutomateDesign.Client.Model.Network;
using AutomateDesign.Client.Model.Logic.Verifications;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour SignUpView.xaml
    /// </summary>
    public partial class EditPasswordView : NavigablePage
    {
        private int userId;
        private uint secretCode;
        private Verification verification;
        private UsersClient users;

        public bool UserAgreement { get; set; }

        public string Password => this.passBox.Password;

        public string PasswordAgain => this.passBoxConf.Password;

        /// <summary>
        /// Envoyer False lors d'un premier appel a la page
        /// </summary>
        public EditPasswordView(Verification verification, int userId, uint secretCode)
        {
            this.users = new UsersClient();
            this.verification = verification;
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
                        this.Navigator.Go(new EmailVerificationSuccessView(this.verification));
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
