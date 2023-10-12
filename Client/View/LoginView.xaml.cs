using AutomateDesign.Client.Model.Network;
using AutomateDesign.Client.View.Helpers;
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

        public string Email { get; set; }
        public string Password { get => this.passBox.Password; }

        public LoginView()
        {
            this.users = new UsersClient();

            InitializeComponent();
            DataContext = this;

            this.Email = string.Empty;
        }

        private void ConnexionButtonClick(object sender, RoutedEventArgs e)
        {
            this.users.sif
        }

        private void passwordOulbieButtonClick(object sender, RoutedEventArgs e)
        {
            //this.mainWindow.NavigateTo(new PasswordResetView(mainWindow));
        }

        private void pasInscritButtonClick(object sender, RoutedEventArgs e)
        {
            this.Navigator.Go(new SignUpView());
        }

    }
}
    