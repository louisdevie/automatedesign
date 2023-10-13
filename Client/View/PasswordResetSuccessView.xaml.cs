using AutomateDesign.Client.View.Helpers;
using System.Windows;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour PasswordResetSuccessView.xaml
    /// </summary>
    public partial class PasswordResetSuccessView : NavigablePage
    {
        public PasswordResetSuccessView()
        {
            InitializeComponent();
        }

        private void BackToSignIn(object sender, RoutedEventArgs e)
        {
            this.Navigator.Go(new LoginView(), true);
        }
    }
}
