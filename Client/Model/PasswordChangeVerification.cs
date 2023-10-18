using AutomateDesign.Client.Model.Network;
using System;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model
{
    internal class PasswordChangeVerification : Verification
    {
        // pas utilisé
        public override string Title => throw new NotImplementedException();

        public override string SuccessMessage => "Votre mot de passe à bien été changé.";

        // pas utilisé
        public override Task SendVerificationRequest(UsersClient client, uint secretCode)
        {
            throw new NotImplementedException();
        }
    }
}
