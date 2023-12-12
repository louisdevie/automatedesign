using AutomateDesign.Client.View.Controls;
using AutomateDesign.Client.View.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows;
using AutomateDesign.Client.Model.Network;
using AutomateDesign.Client.ViewModel.Users;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour SignUpView.xaml
    /// </summary>
    public partial class ChangePasswordView : NavigablePage
    {
        private ChangePasswordViewModel viewModel;

        /// <summary>
        /// Envoyer False lors d'un premier appel a la page
        /// </summary>
        public ChangePasswordView(ChangePasswordViewModel viewModel)
        {
            this.viewModel = viewModel;

            DataContext = this;
            InitializeComponent();

            this.viewModel.CurrentPassword.Bind(this.currentPasswordBox);
            this.viewModel.Password.Bind(this.newPasswordBox);
            this.viewModel.Password.Bind(this.newPasswordAgainBox);
        }

        private async void ContinueButtonClick(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;

            try
            {
                await this.viewModel.ChangePasswordAsync();
                this.Navigator.Go(
                    new EmailVerificationSuccessView(
                        successMessage: "Votre mot de passe à bien été changé.",
                        continuationText: "Terminer",
                        continuationAction: (sender, e) => this.Navigator.ParentWindow.Close()
                    )
                );
            }
            catch (Exception error)
            {
                ErrorMessageBox.Show(error);
                this.IsEnabled = true;
            }
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            this.Navigator.ParentWindow.Close();
        }
    }
}
