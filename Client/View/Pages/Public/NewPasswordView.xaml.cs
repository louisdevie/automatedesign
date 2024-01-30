using AutomateDesign.Client.View.Controls;
using AutomateDesign.Client.View.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using AutomateDesign.Client.Model.Network;
using AutomateDesign.Client.Model.Logic.Verifications;
using AutomateDesign.Client.ViewModel.Users;
using AutomateDesign.Client.Model.Logic.Exceptions;

namespace AutomateDesign.Client.View.Pages
{
    /// <summary>
    /// Logique d'interaction pour SignUpView.xaml
    /// </summary>
    public partial class NewPasswordView : NavigablePage
    {
        private VerificationBaseViewModel verification;
        private PasswordResetViewModel viewModel;

        /// <summary>
        /// Crée une nouvelle page permettant de choisir un nouveau mot de passe.
        /// </summary>
        /// <param name="verification">Un opération de réinitialisation de mot de passe vérifiée.</param>
        /// <param name="userId">L'identifiant de l'utilisateur qui veut changer son mot de passe.</param>
        /// <param name="secretCode">Le code secret saisi précedemment.</param>
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
            this.IsEnabled = false;

            try
            {
                await this.viewModel.ResetPasswordAsync();
                this.Navigator.Go(new EmailVerificationSuccessView(this.verification));
            }
            catch (InvalidInputsException ex)
            {
                // si le champ invalide est la case à cocher, on colore son label en rouge
                if (ex.IsInputInvalid(nameof(PasswordResetViewModel.UserAgreement)))
                {
                    this.checkBoxText.Foreground = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    ErrorMessageBox.Show(ex);
                }
                this.IsEnabled = true;
            }
            catch (Exception ex)
            {
                ErrorMessageBox.Show(ex);
                this.IsEnabled = true;
            }
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            this.Navigator.Back();
        }
    }
}
