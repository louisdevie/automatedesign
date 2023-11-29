using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AutomateDesign.Core.Documents
{
    /// <summary>
    /// Récupère des données depuis un <see cref="DocumentChannel"/>. Cette classe ne doit pas être partagée par plusieurs threads.
    /// </summary>
    public class DocumentChannelReader
    {
        private ChannelReader<DocumentChannel.Chunk> underlying;
        private Queue<DocumentChannel.Chunk> receivedAhead;
        private int lastChunkConsumed;

        internal DocumentChannelReader(ChannelReader<DocumentChannel.Chunk> reader)
        {
            this.underlying = reader;
            this.receivedAhead = new();
            this.lastChunkConsumed = DocumentChannel.NO_CHUNK_YET;
        }

        private async Task<DocumentChannel.Chunk> PeekAsync(CancellationToken cancellationToken = default)
        {
            if (this.receivedAhead.Count == 0)
            {
                this.receivedAhead.Enqueue(await this.underlying.ReadAsync(cancellationToken));
            }
            return this.receivedAhead.Peek();
        }

        private async Task<int> PeekIndexAsync(CancellationToken cancellationToken = default)
        {
            return (await this.PeekAsync(cancellationToken)).index;
        }

        private async Task<DocumentChannel.Chunk> TakeAsync(CancellationToken cancellationToken = default)
        {
            if (this.receivedAhead.Count == 0)
            {
                this.receivedAhead.Enqueue(await this.underlying.ReadAsync(cancellationToken));
            }
            DocumentChannel.Chunk chunk = this.receivedAhead.Dequeue();
            this.lastChunkConsumed = chunk.index;
            return chunk;
        }

        #region Manipulation de l'en-tête

        /// <summary>
        /// Reçois l'en-tête du document une fois qu'il a été envoyé en entier.
        /// </summary>
        /// <param name="cancellationToken">Un jeton permettant d'annuler l'opération.</param>
        /// <returns>Une tâche représentant l'opération, qui termine avec les données de l'en-tête.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<byte[]> ReadHeaderAsync(CancellationToken cancellationToken = default)
        {
            if (await this.PeekIndexAsync(cancellationToken) > DocumentChannel.HEADER_CHUNK)
            {
                throw new InvalidOperationException("L'en-tête à déjà été lu.");
            }
            else
            {
                DocumentChannel.Chunk headerChunk = await this.TakeAsync(cancellationToken);
                return headerChunk.data;
            }
        }

        #endregion

        #region Manipulation de l'automate

        /// <summary>
        /// Reçois le corps du document (l'automate) une fois qu'il a été envoyé en entier.
        /// </summary>
        /// <param name="cancellationToken">Un jeton permettant d'annuler l'opération.</param>
        /// <returns>Une tâche représentant l'opération, qui termine avec les données de l'automate.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<byte[]> ReadBodyAsync(CancellationToken cancellationToken = default)
        {
            if (this.lastChunkConsumed != DocumentChannel.HEADER_CHUNK) throw new InvalidOperationException("L'en-tête n'a pas encore été lu.");

            using MemoryStream buffer = new();

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    DocumentChannel.Chunk chunk = await this.TakeAsync(cancellationToken);
                    await buffer.WriteAsync(chunk.data, cancellationToken);
                }
                catch (ChannelClosedException)
                {
                    break;
                }
            }

            return buffer.ToArray();
        }

        /// <summary>
        /// Reçois les différents morceaux de l'automate.
        /// </summary>
        /// <param name="cancellationToken">Un jeton permettant d'annuler l'opération.</param>
        /// <returns>Un énumérable asynchrone par lequel arrivent les morceaux de l'automate.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async IAsyncEnumerable<byte[]> ReadAllBodyPartsAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (this.lastChunkConsumed != DocumentChannel.HEADER_CHUNK) throw new InvalidOperationException("L'en-tête n'a pas encore été lu.");

            while (!cancellationToken.IsCancellationRequested)
            {
                DocumentChannel.Chunk chunk;
                try
                {
                    chunk = await this.TakeAsync(cancellationToken);
                }
                catch (ChannelClosedException)
                {
                    break;
                }
                yield return chunk.data;
            }
        }

        #endregion
    }
}
