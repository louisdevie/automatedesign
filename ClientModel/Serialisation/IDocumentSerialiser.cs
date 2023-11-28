using AutomateDesign.Core.Documents;
using System.Threading.Channels;

namespace AutomateDesign.Client.Model.Serialisation
{
    /// <summary>
    /// Permets de (dé)sérialiser des automates.
    /// </summary>
    public interface IDocumentSerialiser
    {
        /// <summary>
        /// Sérialise un document entier (automate et métadonnées).
        /// </summary>
        /// <param name="document">Le document à sérialiser.</param>
        /// <param name="output">Le tampon dans lequel envoyer les données sérialisées.</param>
        /// <returns>Une tâche représentant l'opération.</returns>
        Task SerialiseDocumentAsync(Document document, DocumentBuffer.Sender output);

        /// <summary>
        /// Sérialise uniquement un en-tête de document.
        /// </summary>
        /// <param name="header">L'en-tête à sérialiser.</param>
        /// <param name="output">Le tampon dans lequel envoyer les données sérialisées.</param>
        /// <returns>Une tâche représentant l'opération.</returns>
        Task SerialiseHeaderAsync(DocumentHeader header, DocumentBuffer.Sender output);

        /// <summary>
        /// Désérialise un document entier (automate et métadonnées).
        /// </summary>
        /// <param name="input">Le tampon depuis lequel récupérer les données à désérialiser.</param>
        /// <returns>Un automate avec son en-tête.</returns>
        Task<Document> DeserialiseDocumentAsync(DocumentBuffer.Receiver input);

        /// <summary>
        /// Désérialise une liste d'en-têtes.
        /// </summary>
        /// <param name="input">Le tampon depuis lequel récupérer les données à désérialiser.</param>
        /// <returns>Une liste d'en-têtes.</returns>
        IAsyncEnumerable<DocumentHeader> DeserialiseHeadersAsync(ChannelReader<byte[]> input);
    }
}
