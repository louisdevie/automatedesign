using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AutomateDesign.Client.Model.Network;
using AutomateDesign.Client.View.Helpers;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour SignUpView.xaml
    /// </summary>
    public partial class SignUpView : NavigablePage
    {
        private UsersClient users;
        private bool isHandlingTextChanged;

        public SignUpView()
        {
            this.users = new UsersClient();
            this.isHandlingTextChanged = false;

            InitializeComponent();
        }

        private bool IsFormEnabled
        {
            set
            {
                this.signUpButton.IsEnabled = value;
                this.emailBox.IsEnabled = value;
                this.passBox.IsEnabled = value;
                this.passBoxConf.IsEnabled = value;
            }
        }

        /// <summary>
        /// Boutton déclenchant la procedure d'inscription
        /// Si le mot de passe est incorrect ne fait rien et l'indique à l'utilisateur sinon renvoie vers la page de vérification d'email
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmerInscriptionButtonClick(object sender, RoutedEventArgs e)
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
                        this.Navigator.Go(new EmailVerificationView(task.Result, false));
                    }
                },
                TaskScheduler.FromCurrentSynchronizationContext());

                this.IsEnabled = false;
            }
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

                if (emailBox.Text[^1] == '@')
                {
                    emailBox.Text += "iut-dijon.u-bourgogne.fr";
                    passBox.Focus();
                }

                isHandlingTextChanged = false;
            }
        }
    }
}
