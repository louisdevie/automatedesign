using AutomateDesign.Core.Users;

namespace AutomateDesign.Server.Data
{
    public interface ISessionDao
    {
        /// <summary>
        /// Ajoute la session fournit a la bdd
        /// </summary>
        /// <param name="session">session fournit</param>
        /// <returns>la session avec son identifiant</returns>
        public Session Create(Session session);

        /// <summary>
        /// Renvoie la session correspondant au token fournit
        /// </summary>
        /// <param name="token">token de la session cherche</param>
        /// <returns>la session lie au token</returns>
        public Session ReadByToken(string token);

        /// <summary>
        /// Mets à jour la date de dernière utilisation.
        /// </summary>
        /// <param name="session">La session à mettre à jour.</param>
        public void UpdateLastUse(Session session);

        /// <summary>
        /// Supprime de la bdd la session a partir du token fournit
        /// </summary>
        /// <param name="token">token de la session a supprimer</param>
        public void Delete(string token);
    }
}
