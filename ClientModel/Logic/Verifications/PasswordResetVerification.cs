using AutomateDesign.Client.Model.Network;

namespace AutomateDesign.Client.Model.Logic.Verifications
{
    /// <summary>
    /// Représente une vérification avant une réinitialisation de mot de passe.
    /// </summary>
    public class PasswordResetVerification : Verification
    {
        private int userToVerify;

        public int UserToVerify => this.userToVerify;

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
