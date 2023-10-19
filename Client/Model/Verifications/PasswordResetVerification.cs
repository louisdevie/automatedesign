using AutomateDesign.Client.Model.Network;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model.Verifications
{
    public class PasswordResetVerification : Verification
    {
        private int userToVerify;

        public int UserToVerify => this.userToVerify;

        public PasswordResetVerification(int userToVerify)
        {
            this.userToVerify = userToVerify;
        }

        public override string Title => "Vérification par mail";

        public override string SuccessMessage => "Votre mot de passe à bien été changé.";

        public override Task SendVerificationRequest(UsersClient client, uint secretCode)
        {
            return client.CheckResetCodeAsync(this.userToVerify, secretCode);
        }
    }
}
