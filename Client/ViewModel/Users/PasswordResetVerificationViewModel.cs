using AutomateDesign.Client.Model.Logic.Verifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.ViewModel.Users
{
    /// <summary>
    /// Le modèle-vue d'une vérification d'email pour réinitialiser le mot de passe.
    /// </summary>
    public class PasswordResetVerificationViewModel : VerificationBaseViewModel
    {
        private PasswordResetVerification verification;

        public PasswordResetVerificationViewModel(PasswordResetVerification verification)
        {
            this.verification = verification;
        }

        protected override Verification Verification => this.verification;

        public override void DispatchHandler(IVerificationHandler handler) => handler.Handle(this.verification);
    }
}
