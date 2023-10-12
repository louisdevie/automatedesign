using AutomateDesign.Core.Users;

namespace AutomateDesign.Server.Data
{
    public interface ISessionDao
    {
        public Session Create(Session session);

        public Session ReadByToken(string token);

        /// <summary>
        /// Mets à jour la date de dernière utilisation.
        /// </summary>
        /// <param name="session">La session à mettre à jour.</param>
        public void UpdateLastUse(Session session);

        public void Delete(string token);
    }
}
