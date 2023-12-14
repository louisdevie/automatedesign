using AutomateDesign.Core.Random;

namespace AutomateDesign.Core.Users
{
    /// <summary>
    /// Représente une session.
    /// </summary>
    public class Session
    {
        private string token;
        private DateTime lastUse;
        private DateTime expiration;
        private User user;

        /// <summary>
        /// La durée maximum d'une session (24 heures).
        /// </summary>
        public static readonly TimeSpan LIFETIME = TimeSpan.FromHours(24);

        /// <summary>
        /// La durée maximum d'une session sans activité (30 minutes).
        /// </summary>
        public static readonly TimeSpan INACTIVITY_TIMEOUT = TimeSpan.FromMinutes(30);

        /// <summary>
        /// L'identifiant de la session.
        /// </summary>
        public string Token => this.token;

        /// <summary>
        /// La dernière utilisation de la session.
        /// </summary>
        public DateTime LastUse => this.lastUse;

        /// <summary>
        /// Le moment auquel la session expirera.
        /// </summary>
        public DateTime Expiration => this.expiration;

        /// <summary>
        /// La durée depuis la dernière utilisation de la session.
        /// </summary>
        public TimeSpan UnusedSince => DateTime.UtcNow.Subtract(this.lastUse);

        /// <summary>
        /// L'utilisateur qui a ouvert la session.
        /// </summary>
        public User User => this.user;

        /// <summary>
        /// Indique si la session a expiré ou non, en prenant en compte l'inactivité.
        /// </summary>
        public bool Expired => this.UnusedSince > INACTIVITY_TIMEOUT || this.expiration < DateTime.UtcNow;

        /// <summary>
        /// Crée une session existante.
        /// </summary>
        /// <param name="token">Le jeton de la session.</param>
        /// <param name="user">L'utilisateur qui a ouvert la session.</param>
        public Session(string token, DateTime lastUse, DateTime expiration, User user)
        {
            this.token = token;
            this.lastUse = lastUse;
            this.expiration = expiration;
            this.user = user;
        }

        /// <summary>
        /// Crée une session pour un utilisateur.
        /// </summary>
        /// <param name="user">L'utilisateur pour qui créer la session.</param>
        /// <returns>Une nouvelle session avec un identifiant aléatoire.</returns>
        public Session(User user)
        {
            var rtg = new RandomTextGenerator(new BasicRandomProvider());
            this.token = rtg.AlphaNumericString(20);
            this.lastUse = DateTime.UtcNow;
            this.expiration = DateTime.UtcNow + LIFETIME;
            this.user = user;
        }

        /// <summary>
        /// Mets à jour l'heure d'accès de la session.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> si la session à bien été mise à jour,
        /// ou <see langword="false"/> si la session a expiré depuis le dernier accès.
        /// </returns>
        public bool Refresh()
        {
            if (this.Expired) return false;

            this.lastUse = DateTime.UtcNow;
            return true;
        }
    }
}
