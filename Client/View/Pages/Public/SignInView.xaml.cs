using AutomateDesign.Client.View.Controls;
using AutomateDesign.Client.View.Helpers;
using AutomateDesign.Client.View.Navigation;
using Grpc.Core;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AutomateDesign.Client.Model.Network;
using AutomateDesign.Client.ViewModel.Users;
using AutomateDesign.Client.Model.Logic;
using System;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour LoginView.xaml
    /// </summary>
    public partial class SignInView : NavigablePage
    {
        private SignInViewModel viewModel;

        public override WindowPreferences Preferences => new(WindowPreferences.WindowSize.Small, WindowPreferences.ResizeMode.MinimizeOnly);

        public SignInView()
        {
            this.viewModel = new SignInViewModel();

            InitializeComponent();

            DataContext = this.viewModel;
            EmailInputHelper.AttachTo(this.emailBox)
                            .AfterAutocompletion += this.EmailInputHelper_AfterAutocompletion;
            this.viewModel.Password.Bind(this.passBox);
        }

        private void EmailInputHelper_AfterAutocompletion()
        {
            this.passBox.Focus();
        }

        private async void SignInButtonClick(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;

            try
            {
                this.Navigator.Session = await this.viewModel.SignInAsync();
                this.Navigator.Go(new HomeView(), true);
            }
            catch(RpcException rpce)
            {
                if (rpce.StatusCode == StatusCode.FailedPrecondition)
                {
                    // l'utilisateur n'a pas encore validé son adresse mail
                    try
                    {
                        var verificationVM = await this.viewModel.RetryEmailVerificationAsync();

                        this.Navigator.Go(new EmailVerificationView(verificationVM));
                    }
                    catch (Exception error)
                    {
                        ErrorMessageBox.Show(error);
                        this.viewModel.ClearPassword();
                        this.IsEnabled = true;
                    }
                }
                else
                {
                    ErrorMessageBox.Show(rpce);
                    this.viewModel.ClearPassword();
                    this.IsEnabled = true;
                }
            }
            catch (Exception error)
            {
                ErrorMessageBox.Show(error);
                this.viewModel.ClearPassword();
                this.IsEnabled = true;
            }
        }

        private void PasswordForgottenButtonClick(object sender, RoutedEventArgs e)
        {
            this.Navigator.Go(new PasswordResetView());
        }

        private void NoAccountButtonClick(object sender, RoutedEventArgs e)
        {
            this.Navigator.Go(new SignUpView());
        }

        private void PasswordBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.SignInButtonClick(sender, new RoutedEventArgs());
            }
        }

        public override void OnNavigatedToThis(bool clearedHistory)
        {
            this.viewModel.ClearPassword();
        }

        public override void OnWentBackToThis()
        {
            this.viewModel.ClearPassword();
        }
    }
}
