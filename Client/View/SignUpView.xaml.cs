using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour SignUpView.xaml
    /// </summary>
    public partial class SignUpView : Page
    {
        #region Attributs
        private string email;
        private string password;
        private string passwordConf;
        private MainWindow mainWindow;
        private bool checkBox;
        private bool isHandlingTextChanged;

        #endregion

        public SignUpView(MainWindow main)
        {
            this.mainWindow = main;
            DataContext = this;  
            InitializeComponent();
            this.email = string.Empty;
            this.password = string.Empty;
            this.passwordConf = string.Empty;
            this.checkBox = false;
            this.isHandlingTextChanged = false;
        }

        /// <summary>
        /// Boutton déclenchant la procedure d'inscription
        /// Si le mot de passe est incorrect ne fait rien et l'indique à l'utilisateur sinon renvoie vers la page de vérification d'email
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmerInscriptionButtonClick(object sender, RoutedEventArgs e)
        {
            this.email = emailBox.Text;
            this.password = passBox.Password;
            this.passwordConf = passBoxConf.Password;
            this.checkBox = this.checkBoxButton.IsChecked.Value;
            if (password != passwordConf) {
                this.messageErreurMDP.Visibility = Visibility.Visible;
            } else if (!this.checkBox){
                this.checkBoxText.Foreground = new SolidColorBrush(Colors.Red);
            } else {
                // TEMPORAIRE !
                mainWindow.ChangementFenetre(new EmailVerificationView(mainWindow, true));
            }
        }

        /// <summary>
        /// Autocompletion de l'adresse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutocompletionEmailTextBox(object sender, TextChangedEventArgs e)
        {
            if (isHandlingTextChanged)
            {
                return; // Évitez de traiter l'événement lorsqu'il est déjà en cours de traitement.
            }

            if (emailBox.Text.Length > 0)
            {
                isHandlingTextChanged = true;

                // Un caractère a été entré, vous pouvez maintenant exécuter votre fonction.
                foreach (char c in emailBox.Text)
                {
                    if (c == '@')
                    {
                        this.email = emailBox.Text + "iut-dijon.u-bourgogne.fr";
                        emailBox.Text = this.email;
                        passBox.Focus();
                    }
                }

                isHandlingTextChanged = false;
            }
        }
    }
}
