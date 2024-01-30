using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using AutomateDesign.Client.Model.Logic.Exceptions;

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

        /// <summary>
        /// Crée un nouveau <see cref="PasswordResetViewModel"/> pour un utilisateur spécifique.
        /// </summary>
        /// <param name="userId">L'identifiant de l'utilisateur qui veut réinitialiser son mot de passe.</param>
        /// <param name="verificationCode">Le code de vérification saisi et vérifié au préalable.</param>
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
            this.ThrowIfInputsAreInvalid();

            if (!this.UserAgreement)
            {
                throw new InvalidInputsException(nameof(UserAgreement));
            }
            else
            {
                await Users.ChangePasswordWithResetCodeAsync(this.userId, this.PasswordValue, this.verficationCode);
            }
        }
    }
}
