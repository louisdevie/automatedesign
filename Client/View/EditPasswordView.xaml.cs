using AutomateDesign.Client.Model.Network;
using AutomateDesign.Client.View.Helpers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour SignUpView.xaml
    /// </summary>
    public partial class EditPasswordView : NavigablePage
    {
        private int userId;
        private uint secretCode;
        private UsersClient users;

        public bool UserAgreement { get; set; }

        public string Password => this.passBox.Password;

        public string PasswordAgain => this.passBoxConf.Password;

        /// <summary>
        /// Envoyer False lors d'un premier appel a la page
        /// </summary>
        public EditPasswordView(int userId, uint secretCode)
        {
            this.users = new UsersClient();
            this.userId = userId;
            this.secretCode = secretCode;

            DataContext = this;
            InitializeComponent();
        }

        private void ConfirmerInscriptionButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.Password != this.PasswordAgain)
            {
                this.messageErreurMDP.Visibility = Visibility.Visible;
            }
            else if (!this.UserAgreement)
            {
                this.checkBoxText.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                this.users.ChangePasswordWithResetCodeAsync(this.userId, this.Password, this.secretCode)
                .ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        ErrorMessageBox.Show(task.Exception?.InnerException);
                        this.IsEnabled = true;
                    }
                    else
                    {
                        this.Navigator.Go(new PasswordResetSuccessView());
                    }
                },
                TaskScheduler.FromCurrentSynchronizationContext());

                this.IsEnabled = false;
            }
        }
    }
}
