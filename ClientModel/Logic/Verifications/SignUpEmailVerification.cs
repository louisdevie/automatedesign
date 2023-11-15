using AutomateDesign.Client.Model.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model.Logic.Verifications
{
    /// <summary>
    /// Représente une demande d'inscription à valider.
    /// </summary>
    public class SignUpEmailVerification : Verification
    {
        private int userId;

        public SignUpEmailVerification(int userId)
        : base(
              title: "Vérification de votre\nadresse mail",
              successMessage: "Votre adresse mail a bien été vérifiée ! Vous pouvez maintenant utiliser AutomateDesign.",
              continuation: "Commencer"
        )
        {
            this.userId = userId;
        }

        public override Task SendVerificationRequest(IUsersClient client, uint verificationCode)
        {
            return client.VerifyUserAsync(this.userId, verificationCode);
        }
    }
}
