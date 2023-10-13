using AutomateDesign.Client.Model;
using AutomateDesign.Client.Model.Network;
using AutomateDesign.Client.View.Helpers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour LoginView.xaml
    /// </summary>
    public partial class LoginView : NavigablePage
    {
        private UsersClient users;
        private bool isHandlingTextChanged;

        public string Email { get; set; }
        public string Password { get => this.passBox.Password; }

        public LoginView()
        {
            this.users = new UsersClient();
            this.isHandlingTextChanged = false;
            this.Email = string.Empty;

            InitializeComponent();
            DataContext = this;

        }

        private void ConnexionButtonClick(object sender, RoutedEventArgs e)
        {
            this.users.SignInAsync(this.Email, this.Password)
            .ContinueWith(task =>
            {
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

        /// <summary>
        /// Autocompletion de l'adresse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutocompletionEmailTextBox(object sender, TextChangedEventArgs e)
        {
            if (e.Changes.Count > 0)
            {
                // Évitez de traiter l'événement lorsqu'il est déjà en cours de traitement.
                if (isHandlingTextChanged) return;
                isHandlingTextChanged = true;

                if (this.Email[^1] == '@')
                {
                    this.Email += "iut-dijon.u-bourgogne.fr";
                    passBox.Focus();
                }

                isHandlingTextChanged = false;
            }
        }
    }
}
