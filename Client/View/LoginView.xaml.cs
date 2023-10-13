using AutomateDesign.Client.Model;
using AutomateDesign.Client.Model.Network;
using AutomateDesign.Client.View.Helpers;
using System.Threading.Tasks;
using System.Windows;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour LoginView.xaml
    /// </summary>
    public partial class LoginView : NavigablePage
    {
        private UsersClient users;

        public string Email { get; set; }
        public string Password { get => this.passBox.Password; }

        private bool IsFormEnabled
        {
            set
            {
                this.emailBox.IsEnabled = value;
                this.passBox.IsEnabled = value;
                this.signInButton.IsEnabled = value;
            }
        }

        public LoginView()
        {
            this.users = new UsersClient();

            InitializeComponent();
            DataContext = this;

            this.Email = string.Empty;
        }

        private void ConnexionButtonClick(object sender, RoutedEventArgs e)
        {
            this.users.SignInAsync(this.Email, this.Password)
            .ContinueWith(task => {
                if (task.IsFaulted)
                {
                    ErrorMessageBox.Show(task.Exception?.InnerException);
                    this.IsEnabled = true;
                }
                else
                {
                    this.Navigator.Session = new Session(task.Result, this.Email);
                    this.Navigator.Go(new HomeView(), true);
                }
            },
            TaskScheduler.FromCurrentSynchronizationContext());

            this.IsEnabled = false;
        }

        private void passwordOulbieButtonClick(object sender, RoutedEventArgs e)
        {
            this.Navigator.Go(new PasswordResetView());
        }

        private void pasInscritButtonClick(object sender, RoutedEventArgs e)
        {
            this.Navigator.Go(new SignUpView());
        }

    }
}
    