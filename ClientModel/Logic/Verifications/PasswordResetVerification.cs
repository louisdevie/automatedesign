using AutomateDesign.Client.Model.Network;

namespace AutomateDesign.Client.Model.Logic.Verifications
{
    /// <summary>
    /// Représente une vérification avant une réinitialisation de mot de passe.
    /// </summary>
    public class PasswordResetVerification : Verification
    {
        private int userToVerify;

        /// <summary>
        /// L'identifiant de l'utilisateur à vérifier.
        /// </summary>
        public int UserToVerify => this.userToVerify;

        /// <summary>
        /// Crée une opération de vérification de mot de passe.
        /// </summary>
        /// <param name="userToVerify">L'identifiant de l'utilisateur à vérifier.</param>
        public PasswordResetVerification(int userToVerify)
        : base(
              title: "Vérification par mail",
              successMessage: "Votre mot de passe à bien été changé.",
              continuation: "Connexion"
        )
        {
            this.userToVerify = userToVerify;
        }

        public override Task SendVerificationRequest(IUsersClient client, uint secretCode)
        {
            return client.CheckResetCodeAsync(this.userToVerify, secretCode);
        }
    }
}
