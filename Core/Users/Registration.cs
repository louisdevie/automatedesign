using AutomateDesign.Core.Random;

namespace AutomateDesign.Core.Users
{
    /// <summary>
    /// Une demande d'inscription.
    /// </summary>
    public class Registration
    {
        private string verificationCode;
        private User user;

        /// <summary>
        /// L'utilisateur concerné par la demande d'inscription.
        /// </summary>
        public User User => this.user;

        /// <summary>
        /// Crée une demande d'inscription existante.
        /// </summary>
        /// <param name="verificationCode">Le code de vérification.</param>
        /// <param name="user">L'utilisateru qui demande à s'inscrire.</param>
        public Registration(string verificationCode, User user)
        {
            this.verificationCode = verificationCode;
            this.user = user;
        }

        /// <summary>
        /// Crée une nouvelle demande d'inscription.
        /// </summary>
        /// <param name="user">L'utilisateur pour qui créer la demande d'inscription.</param>
        public Registration(User user)
        {
            var rtg = new RandomTextGenerator(new BasicRandomProvider());
            this.verificationCode = rtg.AlphaNumericString(30);
            this.user = user;
        }
    }
}
