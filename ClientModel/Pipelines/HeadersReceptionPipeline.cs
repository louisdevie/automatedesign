using AutomateDesign.Client.Model.Cryptography;
using AutomateDesign.Client.Model.Serialisation;
using AutomateDesign.Core.Documents;
using AutomateDesign.Protos;
using Grpc.Core;
using System.Threading.Channels;

namespace AutomateDesign.Client.Model.Pipelines
{
    /// <summary>
    /// Une opération de réception de plusieurs en-têtes de document.
    /// </summary>
    public class HeadersReceptionPipeline : Pipeline
    {
        private IAsyncStreamReader<EncryptedDocumentChunk> stream;

        public HeadersReceptionPipeline(
            IDocumentSerialiser documentSerialiser,
            IEncryptionMethod encryptionMethod,
            IAsyncStreamReader<EncryptedDocumentChunk> stream
        ) : base(documentSerialiser, encryptionMethod)
        {
            this.stream = stream;
        }

        public delegate void HeaderReceivedEventHandler(DocumentHeader header);

        /// <summary>
        /// Déclenché quand un en-tête est reçu à travers le pipeline.
        /// </summary>
        public event HeaderReceivedEventHandler? OnHeaderReceived;

        public override Task ExecuteAsync()
        {
            Channel<byte[]> encryptedChannel = Channel.CreateUnbounded<byte[]>();
            Channel<byte[]> decryptedChannel = Channel.CreateUnbounded<byte[]>();

            return Task.WhenAll(
                this.DeserialiseAndNotifyAsync(decryptedChannel.Reader),
                this.EncryptionMethod.DecryptUnstructuredAsync(encryptedChannel.Reader, decryptedChannel.Writer),
                this.StreamToChannel(encryptedChannel.Writer)
            );
        }

        private async Task StreamToChannel(ChannelWriter<byte[]> pipelineInput)
        {
            while(await this.stream.MoveNext())
            {
                await pipelineInput.WriteAsync(this.stream.Current.Data.ToByteArray());
            }
            pipelineInput.Complete();
        }

        private async Task DeserialiseAndNotifyAsync(ChannelReader<byte[]> serialiserInput)
        {
            await foreach (DocumentHeader header in this.DocumentSerialiser.DeserialiseHeadersAsync(serialiserInput))
            {
                this.OnHeaderReceived?.Invoke(header);
            }
        }
    }
}