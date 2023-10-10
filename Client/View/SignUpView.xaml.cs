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
        #endregion

        /// <summary>
        /// Envoyer False lors d'un premier appel a la page
        /// </summary>
        /// <param name="passwordIncorrect">Vrai si demande d'inscription avec 2 mot de passe differents</param>
        public SignUpView(bool passwordIncorrect, MainWindow main)
        {
            this.mainWindow = main;
            DataContext = this;  
            InitializeComponent();
            this.email = string.Empty;
            this.password = string.Empty;
            this.passwordConf = string.Empty;
            this.checkBox = false;
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
            if (password != passwordConf)
            {
                this.messageErreurMDP.Visibility = Visibility.Visible;
            } else if (!this.checkBox){
                this.checkBoxText.Foreground = new SolidColorBrush(Colors.Red);
            } else {
                // TEMPORAIRE !
                mainWindow.ChangementFenetre(new EmailVerificationView(mainWindow));
            }
        }
    }
}
