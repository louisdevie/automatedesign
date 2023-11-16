using AutomateDesign.Client.View.Controls;
using AutomateDesign.Client.View.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using AutomateDesign.Client.Model.Network;
using AutomateDesign.Client.Model.Logic.Verifications;
using AutomateDesign.Client.ViewModel.Users;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour SignUpView.xaml
    /// </summary>
    public partial class NewPasswordView : NavigablePage
    {
        private VerificationBaseViewModel verification;
        private PasswordResetViewModel viewModel;

        /// <summary>
        /// Envoyer False lors d'un premier appel a la page
        /// </summary>
        public NewPasswordView(VerificationBaseViewModel verification, int userId, uint secretCode)
        {
            this.verification = verification;
            this.viewModel = new PasswordResetViewModel(userId, secretCode);

            DataContext = this.viewModel;
            InitializeComponent();

            this.viewModel.Password.Bind(this.passBox);
            this.viewModel.PasswordAgain.Bind(this.passBoxConf);
        }

        private async void ContinueButtonClick(object sender, RoutedEventArgs e)
        {
            if (!this.viewModel.PasswordsNotEmpty)
            {
                MessageBox.Show("Veuillez saisir un mot de passe", "Erreur");
            }
            else if (!this.viewModel.PasswordsMatch)
            {
                MessageBox.Show("Les mots de passe ne correspondent pas", "Erreur");
            }
            else if (!this.viewModel.UserAgreement)
            {
                this.checkBoxText.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                this.IsEnabled = false;

                try
                {
                    await this.viewModel.ResetPasswordAsync();
                    this.Navigator.Go(new EmailVerificationSuccessView(this.verification));
                }
                catch (Exception ex)
                {
                    ErrorMessageBox.Show(ex);
                    this.IsEnabled = true;
                }
            }
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            this.Navigator.Back();
        }
    }
}
