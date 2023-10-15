﻿using AutomateDesign.Client.Model;
using AutomateDesign.Client.Model.Network;
using AutomateDesign.Client.View.Controls;
using System.Diagnostics.Eventing.Reader;
using System.Threading.Tasks;
using System.Windows;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour PasswordResetSuccessView.xaml
    /// </summary>
    public partial class EmailVerificationSuccessView : NavigablePage
    {
        private UsersClient users;
        private Verification verification;

        public EmailVerificationSuccessView(Verification verification)
        {
            this.users = new UsersClient();
            this.verification = verification;

            InitializeComponent();
            this.successMessage.Text = this.verification.SuccessMessage;

            if (this.verification is SignUpEmailVerification signUpEmailVerification)
            {
                this.continueButton.Content = "Commencer";
                this.continueButton.Click += this.AutoSignIn;
            }
            else
            {
                this.continueButton.Content = "Connexion";
                this.continueButton.Click += this.BackToSignIn;
            }
        }

        private void BackToSignIn(object sender, RoutedEventArgs e)
        {
            this.Navigator.Go(new LoginView(), true);
        }

        private void AutoSignIn(object sender, RoutedEventArgs e)
        {
            if (this.verification is SignUpEmailVerification verif)
            {
                this.users.SignInAsync(verif.SignUpEmail, verif.SignUpPassword)
                .ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        MessageBox.Show("Une erreur inconnue s'est produite durant la connexion automatique.", "Erreur");
                        this.Navigator.Go(new LoginView(), true);
                    }
                    else
                    {
                        this.Navigator.Session = new Session(task.Result, verif.SignUpEmail);
                        this.Navigator.Go(new HomeView(), true);
                    }
                },
                TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                MessageBox.Show("Une erreur inconnue s'est produite durant la connexion automatique.", "Erreur");
                this.Navigator.Go(new LoginView(), true);
            }
        }
    }
}