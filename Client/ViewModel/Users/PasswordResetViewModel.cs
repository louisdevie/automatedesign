using System.Threading.Tasks;

namespace AutomateDesign.Client.ViewModel.Users
{
    /// <summary>
    /// Le modèle-vue d'une réinitialisation de mot de passe.
    /// </summary>
    public class PasswordResetViewModel : NewPasswordBaseViewModel
    {
        private int userId;
        private uint verficationCode;
        private bool userAgreement;

        /// <summary>
        /// Si l'utilisateur est bien d'accord pour réeinitialiser son mot de passe.
        /// </summary>
        public bool UserAgreement
        {
            get => this.userAgreement;
            set {
                this.userAgreement = value;
                this.NotifyPropertyChanged();
            }
        }

        public PasswordResetViewModel(int userId, uint verificationCode)
        {
            this.userId = userId;
            this.verficationCode = verificationCode;
        }

        /// <summary>
        /// Réinitialise le mot de passe de l'utilisateur.
        /// </summary>
        /// <returns>Une tâche représentant l'opération.</returns>
        public async Task ResetPasswordAsync()
        {
            await Users.ChangePasswordWithResetCodeAsync(this.userId, this.PasswordValue, this.verficationCode);
        }
    }
}
