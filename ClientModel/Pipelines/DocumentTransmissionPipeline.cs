using AutomateDesign.Client.Model.Cryptography;
using AutomateDesign.Client.Model.Serialisation;
using AutomateDesign.Core.Documents;
using AutomateDesign.Protos;
using Google.Protobuf;
using Grpc.Core;

namespace AutomateDesign.Client.Model.Pipelines
{
    /// <summary>
    /// Une opération d'envoi d'un document.
    /// </summary>
    public class DocumentTransmissionPipeline : Pipeline
    {
        private IClientStreamWriter<EncryptedDocumentChunk> streamWriter;
        private Task<DocumentIdOnly> asyncResponse;
        private Document payload;

        public DocumentTransmissionPipeline(
            IDocumentSerialiser documentSerialiser,
            IEncryptionMethod encryptionMethod,
            IClientStreamWriter<EncryptedDocumentChunk> streamWriter,
            Task<DocumentIdOnly> asyncResponse,
            Document payload
        )
        : base(documentSerialiser, encryptionMethod)
        {
            this.streamWriter = streamWriter;
            this.asyncResponse = asyncResponse;
            this.payload = payload;
        }

        protected override Task DoExecuteAsync()
        {
            DocumentChannel encryptedChannel = new();
            DocumentChannel decryptedChannel = new();

            return Task.WhenAll(
                this.DocumentSerialiser.SerialiseDocumentAsync(this.payload, decryptedChannel.Writer),
                this.EncryptionMethod.EncryptAsync(decryptedChannel.Reader, encryptedChannel.Writer),
                this.ChannelToStream(encryptedChannel.Reader)
            );
        }
        
        private async Task ChannelToStream(DocumentChannelReader reader)
        {
            byte[] header = await reader.ReadHeaderAsync();
            await this.streamWriter.WriteAsync(
                new EncryptedDocumentChunk { DocumentId = this.payload.Id, Data = ByteString.CopyFrom(header) }
            );

            await foreach (byte[] chunk in reader.ReadAllBodyPartsAsync())
            {
                await this.streamWriter.WriteAsync(
                    new EncryptedDocumentChunk { DocumentId = this.payload.Id, Data = ByteString.CopyFrom(chunk) }
                );
            }
            await this.streamWriter.CompleteAsync();

            await this.asyncResponse;
        }

        /// <summary>
        /// Récupère le nouvel identifiant du document après enregistrement.
        /// </summary>
        /// <returns>Un tâche qui termine avec l'identifiant.</returns>
        public async Task<int> GetNewDocumentId()
        {
            var result = await this.asyncResponse;
            return result.DocumentId;
        }
    }
}