using AutomateDesign.Client.View.Controls;
using System.Threading.Tasks;
using System.Windows;
using AutomateDesign.Client.Model.Logic.Verifications;
using AutomateDesign.Client.ViewModel.Users;
using System;

namespace AutomateDesign.Client.View.Pages
{
    /// <summary>
    /// Logique d'interaction pour PasswordResetSuccessView.xaml
    /// </summary>
    public partial class EmailVerificationSuccessView : NavigablePage, IVerificationHandler
    {
        private VerificationBaseViewModel? viewModel;

        /// <summary>
        /// Crée une vue de succès à partir d'une opération qui à été vérifiée.
        /// </summary>
        /// <param name="verification">L'opération vérifiée avec succès.</param>
        public EmailVerificationSuccessView(VerificationBaseViewModel verification)
        {
            this.viewModel = verification;

            DataContext = this.viewModel;
            InitializeComponent();

            this.continueButton.Click += ContinueButtonClickDispatch;
        }

        /// <summary>
        /// Crée une vue de succès à partir d'informations précises.
        /// </summary>
        /// <param name="successMessage">Le message indiquant le résultat de l'opération.</param>
        /// <param name="continuationText">Le texte du bouton invitant l'utilisateur à continuer.</param>
        /// <param name="continuationAction">L'action à effectuer quand l'utilisateur continue.</param>
        public EmailVerificationSuccessView(string successMessage, string continuationText, RoutedEventHandler continuationAction)
        {
            this.viewModel = null;

            DataContext = new { SuccessMessage = successMessage, Continuation = continuationText };
            InitializeComponent();

            this.continueButton.Click += continuationAction;
        }

        private void ContinueButtonClickDispatch(object sender, RoutedEventArgs e)
        {
            this.viewModel?.DispatchHandler(this);
        }

        public void Handle(SignUpEmailVerification verification)
        {
            var signUpVM = this.viewModel as SignUpEmailVerificationViewModel;

            signUpVM?.AutoSignInAsync()
            .ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    MessageBox.Show("Une erreur inconnue s'est produite durant la connexion automatique.", "Erreur");
                    this.Navigator.Go(new SignInView(), true);
                }
                else
                {
                    this.Navigator.Session = task.Result;
                    this.Navigator.Go(new HomeView(), true);
                }
            },
            TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void Handle(PasswordResetVerification verification)
        {
            this.Navigator.Go(new SignInView(), true);
        }
    }
}
