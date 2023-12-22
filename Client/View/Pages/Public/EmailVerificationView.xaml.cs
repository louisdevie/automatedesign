using AutomateDesign.Client.View.Controls;
using AutomateDesign.Client.View.Helpers;
using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AutomateDesign.Client.Model.Network;
using AutomateDesign.Client.Model.Logic.Verifications;
using AutomateDesign.Client.ViewModel.Users;

namespace AutomateDesign.Client.View.Pages
{
    /// <summary>
    /// Logique d'interaction pour EmailVerificationView.xaml
    /// </summary>
    public partial class EmailVerificationView : NavigablePage, IVerificationHandler
    {
        private VerificationBaseViewModel viewModel;

        public EmailVerificationView(VerificationBaseViewModel verification)
        {
            this.viewModel = verification;

            this.DataContext = this.viewModel;
            InitializeComponent();
        }

        public override void OnNavigatedToThis(bool clearedHistory)
        {
            FormattedText formattedText = new(
                "0000", CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface(
                    this.codeVerifBox.FontFamily,
                    this.codeVerifBox.FontStyle,
                    this.codeVerifBox.FontWeight,
                    this.codeVerifBox.FontStretch
                ),
                this.codeVerifBox.FontSize, Brushes.Black,
                VisualTreeHelper.GetDpi(this).PixelsPerDip
            );
            this.codeVerifBox.Width = formattedText.Width + 24;
        }

        public override void OnWentBackToThis()
        {
            this.Navigator.Back();
        }

        private async void VerifyCode()
        {
            this.IsEnabled = false;

            try
            {
                await this.viewModel.SendVerificationRequestAsync();
                viewModel.DispatchHandler(this);
            }
            catch (Exception error)
            {
                ErrorMessageBox.Show(error);
                this.IsEnabled = true;
            }
        }

        /// <summary>
        /// Verifie que les caractère entrer soient bien des chiffres et non des lettres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumbersOnly(object sender, TextCompositionEventArgs e)
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

        private void CodeInputTextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.codeVerifBox.Text.Length == 4)
            {
                this.VerifyCode();
            }
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            this.Navigator.Back();
        }

        public void Handle(SignUpEmailVerification verification)
        {
            this.Navigator.Go(new EmailVerificationSuccessView(this.viewModel));
        }

        public void Handle(PasswordResetVerification verification)
        {
            this.Navigator.Go(new NewPasswordView(this.viewModel, verification.UserToVerify, this.viewModel.CodeValue));
        }
    }
}
