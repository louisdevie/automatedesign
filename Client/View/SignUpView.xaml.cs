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
        }

        /// <summary>
        /// Boutton déclenchant la procedure d'inscription
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmerInscriptionButtonClick(object sender, RoutedEventArgs e)
        {
            this.email = emailBox.Text;
            this.password = passBox.Password;
            this.passwordConf = passBoxConf.Password;
            if (passBox != passBoxConf)
            {
                this.messageErreurMDP.Visibility = Visibility.Visible;
            } else
            {
                // TEMPORAIRE !
                mainWindow.ChangementFenetre(new LoginView(mainWindow));
            }
        }

        private void ConfirmerInscriptionButtonClick(object sender, RoutedEventArgs e)
        {
            this.email = emailBox.Text;
            this.password = passBox.Password;
            this.passwordConf = passBoxConf.Password;
            if (passBox != passBoxConf)
            {
                this.messageErreurMDP.Visibility = Visibility.Visible;
            } else
            {
                // TEMPORAIRE !
                mainWindow.ChangementFenetre(new LoginView(mainWindow));
            }
        }
    }
}
