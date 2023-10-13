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
    /// Logique d'interaction pour EmailVerificationView.xaml
    /// </summary>
    public partial class EmailVerificationView : Page
    {
        #region Attributs
        private MainWindow mainWindow;
        private bool inscription;
        #endregion

        /// <summary>
        /// Mettre True si incription, False si modification mot de passe en cas de perte
        /// </summary>
        /// <param name="mainWindow"></param>
        /// <param name="inscription"></param>
        public EmailVerificationView(MainWindow mainWindow, bool inscription)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.inscription = inscription;
        }

        private void ConfirmerVerifButtonClick(object sender, RoutedEventArgs e)
        {
            // TEMPORAIRE
            if (inscription)
            {
                this.mainWindow.ChangementFenetre(new LoginView(mainWindow));
            }
            else {
                this.mainWindow.ChangementFenetre(new EditPasswordView(mainWindow));
            }
        }

        /// <summary>
        /// Verifie que les caractères entrés soient bien des chiffres et non des lettres
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
