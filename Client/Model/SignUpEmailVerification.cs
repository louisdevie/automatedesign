using AutomateDesign.Client.Model.Network;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model
{
    public class SignUpEmailVerification : Verification
    {
        private int userId;
        private string signUpEmail;
        private string signUpPassword;

        public string SignUpEmail => this.signUpEmail;

        public string SignUpPassword => this.signUpPassword;

        public override string Title => "Vérification de votre\nadresse mail";

        public override string SuccessMessage => "Votre adresse mail a bien été vérifiée ! Vous pouvez maintenant utiliser AutomateDesign.";

        public SignUpEmailVerification(string signUpEmail, string signUpPassword, int userId)
        {
            this.signUpEmail = signUpEmail;
            this.signUpPassword = signUpPassword;
            this.userId = userId;
        }

        public override Task SendVerificationRequest(UsersClient client, uint verificationCode)
        {
            return client.VerifyUserAsync(this.userId, verificationCode);
        }
    }
}
