using AutomateDesign.Client.Model.Logic;
using AutomateDesign.Client.Model.Logic.Verifications;
using System.Threading.Tasks;

namespace AutomateDesign.Client.ViewModel.Users
{
    /// <summary>
    /// Le modèle-vue d'une vérification d'email à l'inscription.
    /// </summary>
    public class SignUpEmailVerificationViewModel : VerificationBaseViewModel
    {
        private SignUpEmailVerification verification;
        private string email;
        private string password;

        public SignUpEmailVerificationViewModel(SignUpEmailVerification verification, string email, string password)
        {
            this.verification = verification;
            this.email = email;
            this.password = password;
        }

        protected override Verification Verification => this.verification;

        public override void DispatchHandler(IVerificationHandler handler) => handler.Handle(this.verification);

        /// <summary>
        /// Se connecte automatiquement à partir des informations renseignées à l'inscription.
        /// </summary>
        /// <returns>Une tâche représentant l'opération, qui termine avec la session nouvellement ouverte.</returns>
        public async Task<Session> AutoSignInAsync()
        {
            return await Users.SignInAsync(email, password);
        }
    }
}