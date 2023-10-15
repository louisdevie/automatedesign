using AutomateDesign.Client.Model.Network;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model
{
    public class PasswordResetVerification : Verification
    {
        public override string Title => "Vérification par mail";

        public override string SuccessMessage => "Votre mot de passe à bien été changé.";

        public override Task SendVerificationRequest(UsersClient client, int userId, uint secretCode)
        {
            return client.CheckResetCodeAsync(userId, secretCode);
        }
    }
}
