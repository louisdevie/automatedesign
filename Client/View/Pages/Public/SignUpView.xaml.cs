using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using AutomateDesign.Client.View.Controls;
using AutomateDesign.Client.View.Helpers;
using AutomateDesign.Client.Model.Network;
using AutomateDesign.Client.ViewModel.Users;
using AutomateDesign.Client.Model.Logic.Exceptions;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour SignUpView.xaml
    /// </summary>
    public partial class SignUpView : NavigablePage
    {
        private SignUpViewModel viewModel;

        public SignUpView()
        {
            this.viewModel = new();

            DataContext = this.viewModel;
            InitializeComponent();
            EmailInputHelper.AttachTo(this.emailBox)
                            .AfterAutocompletion += this.EmailInputHelper_AfterAutocompletion;
        }

        private void EmailInputHelper_AfterAutocompletion()
        {
            passBox.Focus();
        }

        /// <summary>
        /// Boutton déclenchant la procedure d'inscription
        /// Si le mot de passe est incorrect ne fait rien et l'indique à l'utilisateur sinon renvoie vers la page de vérification d'email
        /// </summary>
        private async void ContinueButtonClick(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;

            try
            {
                var signUpVM = await this.viewModel.SignUpAsync();
                this.Navigator.Go(new EmailVerificationView(signUpVM));
            }
            catch (InvalidInputsException ex)
            {
                // si le champ invalide est la case à cocher, on colore son label en rouge
                if (ex.IsInputInvalid(nameof(SignUpViewModel.WarningRead)))
                {
                    this.checkBoxText.Foreground = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    ErrorMessageBox.Show(ex);
                }
                this.IsEnabled = true;
            }
            catch (Exception error)
            {
                ErrorMessageBox.Show(error);
                this.IsEnabled = true;
            }
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            this.Navigator.Back();
        }

        public override void OnWentBackToThis()
        {
            this.IsEnabled = true;
        }
    }
}
