﻿using System;
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
    /// Logique d'interaction pour LoginView.xaml
    /// </summary>
    public partial class LoginView : Page
    {
        #region Attributs
        private string email;
        private string password;
        private MainWindow mainWindow;
        private bool isHandlingTextChanged;
        #endregion

        public LoginView(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.email = string.Empty;
            this.password = string.Empty;
            this.isHandlingTextChanged = false;
        }

        private void ConnexionButtonClick(object sender, RoutedEventArgs e)
        {
            this.email = emailBox.Text;
            this.password = passBox.Password;
            this.mainWindow.ChangementFenetre(new HomeView(mainWindow));
        }

        private void passwordOulbieButtonClick(object sender, RoutedEventArgs e)
        {
            this.mainWindow.ChangementFenetre(new PasswordResetView(mainWindow));
        }

        private void pasInscritButtonClick(object sender, RoutedEventArgs e)
        {
            this.mainWindow.ChangementFenetre(new SignUpView(mainWindow));
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
