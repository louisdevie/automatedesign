using AutomateDesign.Client.Model.Cryptography;
using AutomateDesign.Client.Model.Serialisation;
using AutomateDesign.Core.Documents;
using AutomateDesign.Protos;
using Google.Protobuf;
using Grpc.Core;
using System.Threading.Channels;

namespace AutomateDesign.Client.Model.Pipelines
{
    public class DocumentTransmissionPipeline : Pipeline
    {
        private IClientStreamWriter<EncryptedDocumentChunk> streamWriter;
        private Document payload;

        public DocumentTransmissionPipeline(
            IDocumentSerialiser documentSerialiser,
            IEncryptionMethod encryptionMethod,
            IClientStreamWriter<EncryptedDocumentChunk> streamWriter,
            Document payload)
        : base(documentSerialiser, encryptionMethod)
        {
            this.streamWriter = streamWriter;
            this.payload = payload;
        }

        public override Task ExecuteAsync()
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
        }
    }
}