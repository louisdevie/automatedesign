using AutomateDesign.Client.Model.Verifications;
using AutomateDesign.Client.Model.Network;
using AutomateDesign.Client.View.Controls;
using AutomateDesign.Client.View.Helpers;
using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour EmailVerificationView.xaml
    /// </summary>
    public partial class EmailVerificationView : NavigablePage
    {
        private UsersClient users;
        private Verification verification;

        public EmailVerificationView(Verification verification)
        {
            this.users = new UsersClient();
            this.verification = verification;

            InitializeComponent();

            this.titleLabel.Text = this.verification.Title;
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

        private void VerifyCode()
        {
            uint code = UInt32.Parse(this.codeVerifBox.Text);

            this.verification.SendVerificationRequest(this.users, code)
            .ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    ErrorMessageBox.Show(task.Exception?.InnerException);
                    this.IsEnabled = true;
                }
                else if (this.verification is PasswordResetVerification prv)
                {
                    this.Navigator.Go(new EditPasswordView(prv, prv.UserToVerify, code));
                }
                else if (this.verification is SignUpEmailVerification)
                {
                    this.Navigator.Go(new EmailVerificationSuccessView(this.verification));
                }
            },
            TaskScheduler.FromCurrentSynchronizationContext());

            this.IsEnabled = false;
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
    }
}
