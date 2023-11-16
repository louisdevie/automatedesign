using AutomateDesign.Client.Model.Network;

namespace AutomateDesign.Client.Model.Logic.Verifications
{
    /// <summary>
    /// Représente une opération à valider.
    /// </summary>
    public abstract class Verification
    {
        private string title, successMessage, continuation;

        /// <summary>
        /// Le nom de l'opération.
        /// </summary>
        public string Title => this.title;

        /// <summary>
        /// Un message décrivant à afficher une fois l'opération réussie.
        /// </summary>
        public string SuccessMessage => this.successMessage;

        /// <summary>
        /// Le texte invitant l'utilisateur à continuer après avoir effectué l'opération.
        /// </summary>
        public string Continuation => this.continuation;

        /// <summary>
        /// Crée une opération à vérifier.
        /// </summary>
        /// <param name="title">Le nom de l'opération.</param>
        /// <param name="successMessage">Un message décrivant à afficher une fois l'opération réussie.</param>
        /// <param name="continuation">Le texte invitant l'utilisateur à continuer après avoir effectué l'opération.</param>
        public Verification(string title, string successMessage, string continuation)
        {
            this.title = title;
            this.successMessage = successMessage;
            this.continuation = continuation;
        }

        /// <summary>
        /// Envoie une requête pour effectuer la vérification.
        /// </summary>
        /// <param name="client">Le client à utiliser pour envoyer la requête.</param>
        /// <param name="verificationCode">Le code de vérification à utiliser.</param>
        /// <returns></returns>
        public abstract Task SendVerificationRequest(IUsersClient client, uint verificationCode);
    }
}
