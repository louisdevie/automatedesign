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
        #region Attributs
        private string email;
        private string password;
        #endregion

        public LoginView()
        {
            InitializeComponent();

            this.email = string.Empty;
            this.password = string.Empty;
        }

        private void ConnexionButtonClick(object sender, RoutedEventArgs e)
        {
            this.email = emailBox.Text;
            this.password = passBox.Password;
            //this.mainWindow.NavigateTo(new HomeView(mainWindow));
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
    