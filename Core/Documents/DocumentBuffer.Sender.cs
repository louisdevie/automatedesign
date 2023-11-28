using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AutomateDesign.Core.Documents
{
    public partial class DocumentBuffer
    {
        /// <summary>
        /// Envoie des données dans un <see cref="DocumentBuffer"/>. Cette classe ne doit pas être partagée par plusieurs threads.
        /// </summary>
        public class Sender
        {
            private ChannelWriter<DocumentChunk> underlying;
            private MemoryStream chunkBuffer;
            private int lastChunkSent;

            internal Sender(ChannelWriter<DocumentChunk> writer)
            {
                this.underlying = writer;
                this.chunkBuffer = new MemoryStream();
                this.lastChunkSent = NO_CHUNK_YET;
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
            public async ValueTask SendHeaderAsync(byte[] headerData, CancellationToken cancellationToken = default)
            {
                if (this.lastChunkSent != NO_CHUNK_YET) throw new InvalidOperationException("L'en-tête à déjà été envoyé.");
                if (this.chunkBuffer.Length > 0) throw new InvalidOperationException("Certaines parties de l'en-tête ont déjà été envoyées.");
                await this.underlying.WriteAsync(new DocumentChunk(HEADER_CHUNK, headerData), cancellationToken);
                this.lastChunkSent = HEADER_CHUNK;
            }

            /// <summary>
            /// Envoie une partie des données d'en-tête à la suite des données déjà envoyées. <br/>
            /// Cette méthode ne peut pas être appelée si l'en-tête à déjà été envoyé. <br/>
            /// Si vous savez que cette partie est la dernière, vous pouvez appeler <see cref="FinishSendingHeaderAsync"/> à la suite.
            /// </summary>
            /// <param name="otherHeaderData">Les données binaires à rajouter.</param>
            /// <param name="cancellationToken">Un jeton pouvant être utilisé pour annuler l'opération.</param>
            /// <returns>Une tâche représentant l'opération.</returns>
            /// <exception cref="InvalidOperationException"></exception>
            public async ValueTask SendHeaderPartAsync(byte[] otherHeaderData, CancellationToken cancellationToken = default)
            {
                if (this.lastChunkSent != NO_CHUNK_YET) throw new InvalidOperationException("L'en-tête à déjà été envoyé.");
                await this.chunkBuffer.WriteAsync(otherHeaderData, cancellationToken);
            }

            /// <summary>
            /// Indique qu'il n'y aura pas d'autres données d'en-tête evoyées avec <see cref="SendHeaderPartAsync"/>. <br/>
            /// Cette méthode n'a pas besoin d'être appelée quand <see cref="SendHeaderAsync"/> est utilisée.
            /// </summary>
            /// <param name="cancellationToken">Un jeton pouvant être utilisé pour annuler l'opération.</param>
            /// <returns>Une tâche représentant l'opération.</returns>
            /// <exception cref="InvalidOperationException"></exception>
            public async ValueTask FinishSendingHeaderAsync(CancellationToken cancellationToken = default)
            {
                if (this.lastChunkSent != NO_CHUNK_YET) throw new InvalidOperationException("L'en-tête à déjà été envoyé.");
                await this.underlying.WriteAsync(new DocumentChunk(HEADER_CHUNK, this.chunkBuffer.ToArray()), cancellationToken);
                this.chunkBuffer.SetLength(0);
                this.lastChunkSent = HEADER_CHUNK;
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
            public async ValueTask SendBodyAsync(byte[] bodyData, CancellationToken cancellationToken = default)
            {
                if (this.lastChunkSent > HEADER_CHUNK) throw new InvalidOperationException("Le corps du document a déjà commencé à être envoyé.");
                await this.SendBodyPartAsync(bodyData, cancellationToken);
                await this.FinishSendingBodyAsync(cancellationToken);
            }

            /// <summary>
            /// Envoie une partie de l'automate à la suite des données déjà présentes.
            /// </summary>
            /// <param name="otherBodyData">Les données binaires à rajouter.</param>
            /// <param name="cancellationToken">Un jeton pouvant être utilisé pour annuler l'opération.</param>
            /// <returns>Une tâche représentant l'opération.</returns>
            /// <exception cref="InvalidOperationException"></exception>
            public async ValueTask SendBodyPartAsync(byte[] otherBodyData, CancellationToken cancellationToken = default)
            {
                if (this.lastChunkSent < HEADER_CHUNK) throw new InvalidOperationException("L'en-tête n'a pas encore été envoyé.");
                int written = 0;
                while (written < otherBodyData.Length)
                {
                    // on utilise la place restante dans le tampon

                    int spaceToWrite = (int)Math.Min(otherBodyData.Length - written, ChunkSize - this.chunkBuffer.Length);
                    this.chunkBuffer.Write(otherBodyData, written, spaceToWrite);
                    written += spaceToWrite;

                    if (this.chunkBuffer.Length == ChunkSize)
                    {
                        // si on a rempli le tampon, on l'envoie tout de suite

                        await this.underlying.WriteAsync(new DocumentChunk(this.lastChunkSent++, this.chunkBuffer.ToArray()), cancellationToken);
                        this.chunkBuffer.SetLength(0);
                    }
                }
            }

            /// <summary>
            /// Indique qu'il n'y aura pas d'autres données de l'automate evoyées avec <see cref="SendBodyPartAsync"/>. <br/>
            /// Cette méthode n'a pas besoin d'être appelée quand <see cref="SendBodyAsync"/> est utilisée.
            /// </summary>
            /// <param name="cancellationToken">Un jeton pouvant être utilisé pour annuler l'opération.</param>
            /// <returns>Une tâche représentant l'opération.</returns>
            /// <exception cref="InvalidOperationException"></exception>
            public async ValueTask FinishSendingBodyAsync(CancellationToken cancellationToken = default)
            {
                await this.underlying.WriteAsync(new DocumentChunk(this.lastChunkSent++, this.chunkBuffer.ToArray()), cancellationToken);
                this.chunkBuffer.SetLength(0);
                this.underlying.TryComplete();
            }

            #endregion
        }
    }
}
