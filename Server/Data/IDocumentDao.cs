using AutomateDesign.Core.Documents;
using AutomateDesign.Protos;
using System.Reflection.Metadata;

namespace AutomateDesign.Server.Data
{
    /// <summary>
    /// Permet d'accéder aux données des documents.
    /// </summary>
    public interface IDocumentDao
    {
        /// <summary>
        /// Enregistre un nouveal automate.
        /// </summary>
        /// <param name="userId">L'identifiant de l'utilisateur à qui appartient l'automate.</param>
        /// <param name="documentChunks">L'automate chiffré.</param>
        /// <returns>L'identifiant du nouvel automate.</returns>
        public Task<int> CreateAsync(int userId, DocumentChannelReader document);

        /// <summary>
        /// Récupère l'automate correspondant à un identifiant.
        /// </summary>
        /// <param name="documentId">L'identifiant de l'automate à lire.</param>
        /// <returns>L'automate chiffré.</returns>
        public IAsyncEnumerable<byte[]> ReadByIdAsync(int userId, int documentId);

        /// <summary>
        /// Récupère tous les automates d'un utilisateur.
        /// </summary>
        /// <returns>Les métadonnées des automates. Chaque morceau doit correspondre à un automate.</returns>
        public IAsyncEnumerable<EncryptedDocumentChunk> ReadAllHeadersAsync(int userId, CancellationToken ct = default);

        /// <summary>
        /// Enregistre un automate existant.
        /// </summary>
        /// <param name="documentId">L'identifiant de l'automate à mettre à jour.</param>
        /// <param name="document">L'automate chiffré.</param>
        public Task<int> UpdateAsync(int userId, int documentId, DocumentChannelReader document);

        /// <summary>
        /// Enregistre les métadonnées un automate existant.
        /// </summary>
        /// <param name="documentId">L'identifiant de l'automate à mettre à jour.</param>
        /// <param name="documentHeader">Les métadonnées de l'automate.</param>
        public void UpdateHeader(int userId, int documentId, byte[] documentHeader);

        /// <summary>
        /// Supprimme un automate.
        /// </summary>
        /// <param name="documentId">L'identifiant de l'automate à supprimmer.</param>
        public void Delete(int userId, int documentId);
    }
}
