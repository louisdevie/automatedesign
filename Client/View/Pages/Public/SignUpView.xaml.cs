using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AutomateDesign.Client.Model;
using AutomateDesign.Client.Model.Network;
using AutomateDesign.Client.View.Controls;
using AutomateDesign.Client.View.Helpers;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour SignUpView.xaml
    /// </summary>
    public partial class SignUpView : NavigablePage
    {
        private UsersClient users;
        private EmailInputHelper emailInputHelper;

        public SignUpView()
        {
            this.users = new UsersClient();

            InitializeComponent();
            this.emailInputHelper = new(this.emailBox);
            this.emailInputHelper.AfterAutocompletion += this.EmailInputHelper_AfterAutocompletion;
        }

        private void EmailInputHelper_AfterAutocompletion()
        {
            passBox.Focus();
        }

        /// <summary>
        /// Boutton déclenchant la procedure d'inscription
        /// Si le mot de passe est incorrect ne fait rien et l'indique à l'utilisateur sinon renvoie vers la page de vérification d'email
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContinueButtonClick(object sender, RoutedEventArgs e)
        {
            string email = emailBox.Text;
            string password = passBox.Password;
            string passwordAgain = passBoxConf.Password;
            bool warningRead = this.checkBoxButton.IsChecked ?? false;

            if (password != passwordAgain)
            {
                MessageBox.Show("Les mots de passe ne correspondent pas", "Erreur");
            }
            else if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password))
            {
                MessageBox.Show("Veuillez saisir une addresse mail et un mot de passe", "Erreur");
            }
            else if (!warningRead)
            {
                this.checkBoxText.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                this.users
                .SignUpAsync(email, password)
                .ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        ErrorMessageBox.Show(task.Exception?.InnerException);
                        this.IsEnabled = true;
                    }
                    else
                    {
                        this.Navigator.Go(new EmailVerificationView(new SignUpEmailVerification(email, password, task.Result)));
                    }
                },
                TaskScheduler.FromCurrentSynchronizationContext());

                this.IsEnabled = false;
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
