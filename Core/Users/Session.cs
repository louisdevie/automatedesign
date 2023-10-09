using AutomateDesign.Core.Random;

namespace AutomateDesign.Core.Users
{
    /// <summary>
    /// Représente une session.
    /// </summary>
    public class Session
    {
        private uint token;
        private User user;

        /// <summary>
        /// L'identifiant de la session.
        /// </summary>
        public uint Token => this.token;

        /// <summary>
        /// L'utilisateur qui a ouvert la session.
        /// </summary>
        public User User => this.user;

        /// <summary>
        /// Crée une session existante.
        /// </summary>
        /// <param name="token">Le jeton de la session.</param>
        /// <param name="user">L'utilisateur qui a ouvert la session.</param>
        public Session(uint token, User user)
        {
            this.token = token;
            this.user = user;
        }

        /// <summary>
        /// Crée une session pour un utilisateur.
        /// </summary>
        /// <param name="user">L'utilisateur pour qui créer la session.</param>
        /// <returns>Une nouvelle session avec un identifiant aléatoire.</returns>
        public Session(User user)
        {
            var random = new BasicRandomProvider();
            this.token = random.NextUInt();
            this.user = user;
        }
    }
}
