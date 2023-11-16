using AutomateDesign.Core.Documents;
using AutomateDesign.Server.EncryptedData;
using System.Reflection.Metadata;

namespace AutomateDesign.Server.Data
{
    public interface IDocumentDao
    {
        /// <summary>
        /// Enregistre un nouveal automate.
        /// </summary>
        /// <param name="userId">L'identifiant de l'utilisateur à qui appartient l'automate.</param>
        /// <param name="documentChunks">Les morceux de l'automate chiffré.</param>
        /// <returns>L'identifiant du nouvel automate.</returns>
        public int CreateAsync(int userId, IAsyncChunkSource documentChunks);

        /// <summary>
        /// Récupère l'automate correspondant à un identifiant.
        /// </summary>
        /// <param name="documentId">L'identifiant de l'automate à lire.</param>
        /// <returns>L'automate chiffré.</returns>
        public IAsyncChunkSource ReadById(int documentId);

        /// <summary>
        /// Récupère tous les automates d'un utilisateur.
        /// </summary>
        /// <returns>Les métadonnées des automates. Chaque morceau doit correspondre à un automate.</returns>
        public IAsyncChunkSource ReadAllHeaders();

        /// <summary>
        /// Enregistre un automate existant.
        /// </summary>
        /// <param name="documentId">L'identifiant de l'automate à mettre à jour.</param>
        /// <param name="documentChunks">Les morceux de l'automate chiffré.</param>
        public void Update(int documentId, IAsyncChunkSource documentChunks);

        /// <summary>
        /// Supprimme un automate.
        /// </summary>
        /// <param name="documentId">L'identifiant de l'automate à supprimmer.</param>
        public void Delete(int documentId);
    }
}
