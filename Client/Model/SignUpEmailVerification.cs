using AutomateDesign.Client.Model.Network;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model
{
    public class SignUpEmailVerification : Verification
    {
        private string signUpEmail;
        private string signUpPassword;

        public string SignUpEmail => this.signUpEmail;

        public string SignUpPassword => this.signUpPassword;

        public override string Title => "Vérification de votre\nadresse mail";

        public override string SuccessMessage => "Votre adresse mail a bien été vérifiée ! Vous pouvez maintenant utiliser AutomateDesign.";

        public SignUpEmailVerification(string signUpEmail, string signUpPassword)
        {
            this.signUpEmail = signUpEmail;
            this.signUpPassword = signUpPassword;
        }

        public override Task SendVerificationRequest(UsersClient client, int userId, uint verificationCode)
        {
            return client.VerifyUserAsync(userId, verificationCode);
        }
    }
}
