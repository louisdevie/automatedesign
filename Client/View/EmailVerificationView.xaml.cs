using AutomateDesign.Client.Model.Network;
using AutomateDesign.Client.View.Helpers;
using AutomateDesign.Core.Users;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour EmailVerificationView.xaml
    /// </summary>
    public partial class EmailVerificationView : NavigablePage
    {
        private UsersClient users;
        private int userToVerify;
        private bool verifyingPasswordReset;

        public EmailVerificationView(int userToVerify, bool verifyingPasswordReset)
        {
            this.users = new UsersClient();
            this.userToVerify = userToVerify;
            this.verifyingPasswordReset = verifyingPasswordReset;

            InitializeComponent();

        }

        public bool IsFormEnabled
        {
            set
            {
                this.codeVerifBox.IsEnabled = value;
                this.confirmButton.IsEnabled = value;
            }
        }

        private void ConfirmerVerifButtonClick(object sender, RoutedEventArgs e)
        {
            uint code = UInt32.Parse(this.codeVerifBox.Text);

            if (this.verifyingPasswordReset)
            {
                this.users.CheckResetCodeAsync(this.userToVerify, code)
                .ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        ErrorMessageBox.Show(task.Exception?.InnerException);
                        this.IsFormEnabled = true;
                    }
                    else
                    {
                        this.Navigator.Go(new EditPasswordView(this.userToVerify, code));
                    }
                },
                TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                this.users.VerifyUserAsync(this.userToVerify, code)
                .ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        ErrorMessageBox.Show(task.Exception?.InnerException);
                        this.IsFormEnabled = true;
                    }
                    else
                    {
                        // TODO: Connexion auto
                    }
                },
                TaskScheduler.FromCurrentSynchronizationContext());
            }

            this.IsFormEnabled = false;
        }

        /// <summary>
        /// Verifie que les caractère entrer soient bien des chiffres et non des lettres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char c in e.Text)
            {
                if (!char.IsDigit(c)) // Vérifie si le caractère n'est pas un chiffre
                {
                    e.Handled = true; // Ignore le caractère non numérique
                    break;
                }
            }
        }


    }
}
