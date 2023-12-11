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
    public class DocumentChannel
    {
        internal record Chunk(int index, byte[] data);

        internal const int NO_CHUNK_YET = -2;
        internal const int HEADER_CHUNK = -1;

        /// <summary>
        /// La taille d'un bloc du corps du document.
        /// </summary>
        public static int ChunkSize => 16_384; // 16Kio

        private DocumentChannelReader reader;
        private DocumentChannelWriter writer;

        /// <summary>
        /// L'extrémité en lecture du canal.
        /// </summary>
        public DocumentChannelReader Reader => this.reader;

        /// <summary>
        /// L'extrémité en écriture du canal.
        /// </summary>
        public DocumentChannelWriter Writer => this.writer;

        /// <summary>
        /// Crée un nouveau <see cref="DocumentChannel"/> avec un lecteur et un consommateur.
        /// </summary>
        public DocumentChannel()
        {
            Channel<Chunk> channel = Channel.CreateUnbounded<Chunk>();
            this.reader = new(channel.Reader);
            this.writer = new(channel.Writer);
        }
    }
}
