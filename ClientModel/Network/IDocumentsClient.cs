using AutomateDesign.Client.Model.Logic;
using AutomateDesign.Client.Model.Pipelines;
using AutomateDesign.Core.Documents;

namespace AutomateDesign.Client.Model.Network
{
    public interface IDocumentsClient
    {
        /// <summary>
        /// Récupère tous les en-têtes des documents d'un utilisateur.
        /// </summary>
        /// <param name="session">Les informations de l'utilisateur connecté.</param>
        /// <returns>Un <see cref="HeadersReceptionPipeline"/> qui représente l'opération.</returns>
        HeadersReceptionPipeline GetAllHeaders(Session session);

        /// <summary>
        /// Récupère un document appartenant à un utilisateur.
        /// </summary>
        /// <param name="session">Les informations de l'utilisateur connecté.</param>
        /// <param name="id">L'identifiant du document à récupérer.</param>
        /// <returns>Un <see cref="DocumentReceptionPipeline"/> qui représente l'opération.</returns>
        DocumentReceptionPipeline GetDocument(Session session, int id);

        /// <summary>
        /// Enregistre uniquement les métadonnées d'un document.
        /// </summary>
        /// <param name="session">Les informations de l'utilisateur connecté.</param>
        /// <param name="header">Les informations à mettre à jour.</param>
        /// <returns>Une tâche représentant l'opération qui termine avec le nouvel identifiant du document.</returns>
        Task<int> SaveHeader(Session session, DocumentHeader header);

        /// <summary>
        /// Enregistre un document entier.
        /// </summary>
        /// <param name="session">Les informations de l'utilisateur connecté.</param>
        /// <param name="document">Les informations à mettre à jour.</param>
        /// <returns>Un <see cref="DocumentTransmissionPipeline"/> qui représente l'opération.</returns>
        DocumentTransmissionPipeline SaveDocument(Session session, Document document);

        /// <summary>
        /// Supprime un document définitivement.
        /// </summary>
        /// <param name="session">Les informations de l'utilisateur connecté.</param>
        /// <param name="documentId">L'identifiant du document à supprimer.</param>
        /// <returns>Une tâche représentant l'opération.</returns>
        Task DeleteDocument(Session session, int documentId);
    }
}
