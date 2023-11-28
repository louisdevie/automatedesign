using System.Collections.Concurrent;
using System.Threading.Channels;

namespace AutomateDesign.Core.Documents
{
    /// <summary>
    /// Une classe fournissant un mécanisme pour transférer des données binaires
    /// représentant un document entre producteurs et consommateurs. <br/>
    /// Elle consiste de plusieurs blocs: un en-tête de taille variable suivi par
    /// les données de l'automate, découpé en blocs de <see cref="ChunkSize"/> octets.
    /// </summary>
    public static partial class DocumentBuffer
    {
        internal record DocumentChunk(int index, byte[] data);

        /// <summary>
        /// La taille d'un bloc du corps du document.
        /// </summary>
        public static int ChunkSize => 16_384; // 16Kio

        private const int NO_CHUNK_YET = -2;
        private const int HEADER_CHUNK = -1;

        /// <summary>
        /// Crée un nouveau <see cref="DocumentBuffer"/> avec un producteur et un consommateur.
        /// </summary>
        /// <returns>Un couple contenant le consommateur et le producteur respectivement.</returns>
        public static (Receiver, Sender) CreateSpsc()
        {
            Channel<DocumentChunk> channel = Channel.CreateUnbounded<DocumentChunk>();
            Receiver reader = new(channel.Reader);
            Sender writer = new(channel.Writer);
            return (reader, writer);
        }
    }
}
