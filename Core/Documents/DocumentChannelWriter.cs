using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AutomateDesign.Core.Documents
{
    /// <summary>
    /// Envoie des données dans un <see cref="DocumentChannel"/>. Cette classe ne doit pas être partagée par plusieurs threads.
    /// </summary>
    public class DocumentChannelWriter
    {
        private ChannelWriter<DocumentChannel.Chunk> underlying;
        private MemoryStream chunkBuffer;
        private int lastChunkSent;

        internal DocumentChannelWriter(ChannelWriter<DocumentChannel.Chunk> writer)
        {
            this.underlying = writer;
            this.chunkBuffer = new MemoryStream();
            this.lastChunkSent = DocumentChannel.NO_CHUNK_YET;
        }

        #region Manipulation de l'en-tête

        /// <summary>
        /// Envoie les données d'en-tête. <br/>
        /// Cette méthode ne peut pas être appelée si des données ont déjà été envoyées.
        /// </summary>
        /// <param name="headerData">Les données binaires de l'en-tête du document.</param>
        /// <param name="cancellationToken">Un jeton pouvant être utilisé pour annuler l'opération.</param>
        /// <returns>Une tâche représentant l'opération.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async ValueTask WriteHeaderAsync(byte[] headerData, CancellationToken cancellationToken = default)
        {
            if (this.lastChunkSent != DocumentChannel.NO_CHUNK_YET) throw new InvalidOperationException("L'en-tête à déjà été envoyé.");
            if (this.chunkBuffer.Length > 0) throw new InvalidOperationException("Certaines parties de l'en-tête ont déjà été envoyées.");
            await this.underlying.WriteAsync(new DocumentChannel.Chunk(DocumentChannel.HEADER_CHUNK, headerData), cancellationToken);
            this.lastChunkSent = DocumentChannel.HEADER_CHUNK;
        }

        /// <summary>
        /// Envoie une partie des données d'en-tête à la suite des données déjà envoyées. <br/>
        /// Cette méthode ne peut pas être appelée si l'en-tête à déjà été envoyé. <br/>
        /// Si vous savez que cette partie est la dernière, vous pouvez appeler <see cref="FinishWriteingHeaderAsync"/> à la suite.
        /// </summary>
        /// <param name="otherHeaderData">Les données binaires à rajouter.</param>
        /// <param name="cancellationToken">Un jeton pouvant être utilisé pour annuler l'opération.</param>
        /// <returns>Une tâche représentant l'opération.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async ValueTask WriteHeaderPartAsync(byte[] otherHeaderData, CancellationToken cancellationToken = default)
        {
            if (this.lastChunkSent != DocumentChannel.NO_CHUNK_YET) throw new InvalidOperationException("L'en-tête à déjà été envoyé.");
            await this.chunkBuffer.WriteAsync(otherHeaderData, cancellationToken);
        }

        /// <summary>
        /// Indique qu'il n'y aura pas d'autres données d'en-tête evoyées avec <see cref="WriteHeaderPartAsync"/>. <br/>
        /// Cette méthode n'a pas besoin d'être appelée quand <see cref="WriteHeaderAsync"/> est utilisée.
        /// </summary>
        /// <param name="cancellationToken">Un jeton pouvant être utilisé pour annuler l'opération.</param>
        /// <returns>Une tâche représentant l'opération.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async ValueTask FinishWritingHeaderAsync(CancellationToken cancellationToken = default)
        {
            if (this.lastChunkSent != DocumentChannel.NO_CHUNK_YET) throw new InvalidOperationException("L'en-tête à déjà été envoyé.");
            await this.underlying.WriteAsync(new DocumentChannel.Chunk(DocumentChannel.HEADER_CHUNK, this.chunkBuffer.ToArray()), cancellationToken);
            this.chunkBuffer.SetLength(0);
            this.lastChunkSent = DocumentChannel.HEADER_CHUNK;
        }

        #endregion

        #region Manipulation de l'automate

        /// <summary>
        /// Envoie les données de l'automate. <br/>
        /// Cette méthode ne peut pas être appelée si des données ont déjà été envoyées.
        /// </summary>
        /// <param name="bodyData">Les données binaires de l'automate.</param>
        /// <param name="cancellationToken">Un jeton pouvant être utilisé pour annuler l'opération.</param>
        /// <returns>Une tâche représentant l'opération.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async ValueTask WriteBodyAsync(byte[] bodyData, CancellationToken cancellationToken = default)
        {
            if (this.lastChunkSent > DocumentChannel.HEADER_CHUNK) throw new InvalidOperationException("Le corps du document a déjà commencé à être envoyé.");
            await this.WriteBodyPartAsync(bodyData, cancellationToken);
            await this.FinishWritingBodyAsync(cancellationToken);
        }

        /// <summary>
        /// Envoie une partie de l'automate à la suite des données déjà présentes.
        /// </summary>
        /// <param name="otherBodyData">Les données binaires à rajouter.</param>
        /// <param name="cancellationToken">Un jeton pouvant être utilisé pour annuler l'opération.</param>
        /// <returns>Une tâche représentant l'opération.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async ValueTask WriteBodyPartAsync(byte[] otherBodyData, CancellationToken cancellationToken = default)
        {
            if (this.lastChunkSent < DocumentChannel.HEADER_CHUNK) throw new InvalidOperationException("L'en-tête n'a pas encore été envoyé.");
            int written = 0;
            while (written < otherBodyData.Length)
            {
                // on utilise la place restante dans le tampon

                int spaceToWrite = (int)Math.Min(otherBodyData.Length - written, DocumentChannel.ChunkSize - this.chunkBuffer.Length);
                this.chunkBuffer.Write(otherBodyData, written, spaceToWrite);
                written += spaceToWrite;

                if (this.chunkBuffer.Length == DocumentChannel.ChunkSize)
                {
                    // si on a rempli le tampon, on l'envoie tout de suite

                    await this.underlying.WriteAsync(new DocumentChannel.Chunk(this.lastChunkSent++, this.chunkBuffer.ToArray()), cancellationToken);
                    this.chunkBuffer.SetLength(0);
                }
            }
        }

        /// <summary>
        /// Indique qu'il n'y aura pas d'autres données de l'automate evoyées avec <see cref="WriteBodyPartAsync"/>. <br/>
        /// Cette méthode n'a pas besoin d'être appelée quand <see cref="WriteBodyAsync"/> est utilisée.
        /// </summary>
        /// <param name="cancellationToken">Un jeton pouvant être utilisé pour annuler l'opération.</param>
        /// <returns>Une tâche représentant l'opération.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async ValueTask FinishWritingBodyAsync(CancellationToken cancellationToken = default)
        {
            await this.underlying.WriteAsync(new DocumentChannel.Chunk(this.lastChunkSent++, this.chunkBuffer.ToArray()), cancellationToken);
            this.chunkBuffer.SetLength(0);
            this.underlying.TryComplete();
        }

        #endregion
    }
}

