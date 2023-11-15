using AutomateDesign.Client.Model;
using AutomateDesign.Client.Model.Verifications;
using AutomateDesign.Client.View.Controls;
using AutomateDesign.Client.View.Helpers;
using AutomateDesign.Client.View.Navigation;
using Grpc.Core;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AutomateDesign.Client.Model.Network;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour LoginView.xaml
    /// </summary>
    public partial class LoginView : NavigablePage
    {
        private UsersClient users;
        private EmailInputHelper emailInputHelper;

        public string Email { get; set; }
        public string Password { get => this.passBox.Password; }

        public override WindowPreferences Preferences => new(WindowPreferences.WindowSize.Small, WindowPreferences.ResizeMode.MinimizeOnly);

        public LoginView()
        {
            this.users = new UsersClient();
            this.Email = string.Empty;

            InitializeComponent();
            this.emailInputHelper = new(this.emailBox);
            this.emailInputHelper.AfterAutocompletion += this.EmailInputHelper_AfterAutocompletion;
            DataContext = this;
        }

        private void EmailInputHelper_AfterAutocompletion()
        {
            this.passBox.Focus();
        }

        private void SignInButtonClick(object sender, RoutedEventArgs e)
        {
            TaskScheduler ts = TaskScheduler.FromCurrentSynchronizationContext();

            this.users.SignInAsync(this.Email, this.Password)
            .ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    if (task.Exception?.InnerException is RpcException rpce && rpce.StatusCode == StatusCode.FailedPrecondition)
                    {
                        this.users.SignUpAsync(this.Email, this.Password)
                        .ContinueWith(task =>
                        {
                            if (task.IsFaulted)
                            {
                                ErrorMessageBox.Show(task.Exception?.InnerException);
                                this.IsEnabled = true;
                            }
                            else
                            {
                                this.Navigator.Go(new EmailVerificationView(new SignUpEmailVerification(this.Email, this.Password, task.Result)));
                            }
                        },
                        ts);
                    }
                    else
                    {
                        ErrorMessageBox.Show(task.Exception?.InnerException);
                        this.IsEnabled = true;
                    }
                }
                else
                {
                    this.Navigator.Session = new Session(task.Result.Token, task.Result.UserId, this.Email);
                    this.Navigator.Go(new HomeView(), true);
                }
            }, ts);

            this.IsEnabled = false;
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
                this.SignInButtonClick(sender, e);
            }
        }
    }
}
