using AutomateDesign.Client.View.Controls;
using AutomateDesign.Client.View.Helpers;
using System.Threading.Tasks;
using System.Windows;
using AutomateDesign.Client.Model.Network;
using AutomateDesign.Client.ViewModel.Users;
using System;
using AutomateDesign.Client.Model.Logic.Verifications;

namespace AutomateDesign.Client.View.Pages
{
    /// <summary>
    /// Logique d'interaction pour PasswordResetView.xaml
    /// </summary>
    public partial class PasswordResetView : NavigablePage
    {
        private AskForPasswordResetViewModel viewModel;

        public PasswordResetView()
        {
            this.viewModel = new();

            DataContext = this.viewModel;
            InitializeComponent();

            EmailInputHelper.AttachTo(this.emailBox);
        }

        private async void ResetPasswordButtonClick(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;

            try
            {
                int userId = await this.viewModel.AskForPasswordResetAsync();
                this.Navigator.Go(
                    new EmailVerificationView(
                        new PasswordResetVerificationViewModel(
                            new PasswordResetVerification(userId)
                        )
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
            this.Navigator.Back();
        }

        public override void OnWentBackToThis()
        {
            this.IsEnabled = true;
        }
    }
}
