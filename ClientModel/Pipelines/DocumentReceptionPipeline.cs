using System.Threading.Tasks.Sources;
using AutomateDesign.Client.Model.Cryptography;
using AutomateDesign.Client.Model.Serialisation;
using AutomateDesign.Core.Documents;
using AutomateDesign.Protos;
using Grpc.Core;

namespace AutomateDesign.Client.Model.Pipelines
{
    public class DocumentReceptionPipeline : Pipeline
    {
        private readonly IAsyncStreamReader<EncryptedDocumentChunk> stream;
        private int documentId;
        private Task<Document>? asyncResult;

        public DocumentReceptionPipeline(
            IDocumentSerialiser documentSerialiser,
            IEncryptionMethod encryptionMethod,
            IAsyncStreamReader<EncryptedDocumentChunk> stream
        )
            : base(documentSerialiser, encryptionMethod)
        {
            this.stream = stream;
            this.documentId = DocumentHeader.UnsavedId;
        }

        protected override Task DoExecuteAsync()
        {
            DocumentChannel encryptedChannel = new();
            DocumentChannel decryptedChannel = new();

            this.asyncResult = this.DocumentSerialiser.DeserialiseDocumentAsync(decryptedChannel.Reader);

            return Task.WhenAll(
                this.asyncResult,
                this.EncryptionMethod.DecryptAsync(encryptedChannel.Reader, decryptedChannel.Writer),
                this.StreamToChannel(encryptedChannel.Writer)
            );
        }

        private async Task StreamToChannel(DocumentChannelWriter pipelineInput)
        {
            await this.stream.MoveNext();
            this.documentId = this.stream.Current.DocumentId;
            await pipelineInput.WriteHeaderAsync(this.stream.Current.Data.ToByteArray());

            while (await this.stream.MoveNext())
            {
                await pipelineInput.WriteBodyPartAsync(this.stream.Current.Data.ToByteArray());
            }

            await pipelineInput.FinishWritingBodyAsync();
        }

        /// <summary>
        /// Récupère le document reçu une fois le pipeline terminé.
        /// </summary>
        /// <returns>Un tâche qui termine avec le document.</returns>
        /// <exception cref="InvalidOperationException">Si le pipeline n'a pas encore été lancé.</exception>
        public async Task<Document> GetDocument()
        {
            if (this.asyncResult == null)
            {
                throw new InvalidOperationException("Le pipeline de réception n'a pas encore été démarré.");
            }

            Document document = await this.asyncResult;
            document.Header.Id = this.documentId;
            return document;
        }
    }
}