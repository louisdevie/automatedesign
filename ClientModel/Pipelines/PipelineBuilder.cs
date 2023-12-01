using AutomateDesign.Client.Model.Cryptography;
using AutomateDesign.Client.Model.Serialisation;
using AutomateDesign.Protos;
using Grpc.Core;

namespace AutomateDesign.Client.Model.Pipelines
{
    public class PipelineBuilder
    {
        private IEncryptionMethod? encryptionMethod;
        private IDocumentSerialiser? documentSerialiser;
        private IAsyncStreamReader<EncryptedDocumentChunk>? serverStream;
        private IClientStreamWriter<EncryptedDocumentChunk>? clientStream;
        private object? payload;

        /// <summary>
        /// Choisis l'algorithme à utiliser pour (dé)chiffrer les données.
        /// </summary>
        /// <param name="encryptionMethod">L'<see cref="IEncryptionMethod"/> à utiliser.</param>
        /// <returns>Le <see cref="PipelineBuilder"/> modifié.</returns>
        public PipelineBuilder UseEncryptionMethod(IEncryptionMethod encryptionMethod)
        {
            this.encryptionMethod = encryptionMethod;
            return this;
        }

        private IEncryptionMethod RequireEncryptionMethod()
        {
            if (this.encryptionMethod == null)
            {
                throw new InvalidOperationException("Aucun algorithme d'encryption n'a été configuré.");
            }
            else
            {
                return this.encryptionMethod;
            }
        }

        /// <summary>
        /// Choisis le sérialiseur à utiliser.
        /// </summary>
        /// <param name="documentSerialiser">Le <see cref="IDocumentSerialiser"/> à utiliser.</param>
        /// <returns>Le <see cref="PipelineBuilder"/> modifié.</returns>
        public PipelineBuilder UseDocumentSerialiser(IDocumentSerialiser documentSerialiser)
        {
            this.documentSerialiser = documentSerialiser;
            return this;
        }

        private IDocumentSerialiser RequireDocumentSerialiser()
        {
            if (this.documentSerialiser == null)
            {
                throw new InvalidOperationException("Aucun sérialiseur n'a été configuré.");
            }
            else
            {
                return this.documentSerialiser;
            }
        }

        /// <summary>
        /// Configure le flux réponse à utiliser comme source de données.
        /// </summary>
        /// <param name="streamReader">Un flux réponse gRPC.</param>
        /// <returns>Le <see cref="PipelineBuilder"/> modifié.</returns>
        public PipelineBuilder UseServerStream(IAsyncStreamReader<EncryptedDocumentChunk> streamReader)
        {
            this.serverStream = streamReader;
            return this;
        }

        private IAsyncStreamReader<EncryptedDocumentChunk> RequireServerStream()
        {
            if (this.serverStream == null)
            {
                throw new InvalidOperationException("Aucun flux de réponse n'a été configuré.");
            }
            else
            {
                return this.serverStream;
            }
        }

        /// <summary>
        /// Configure le flux requête à utiliser comme sortie.
        /// </summary>
        /// <param name="streamReader">Un flux requête gRPC.</param>
        /// <returns>Le <see cref="PipelineBuilder"/> modifié.</returns>
        public PipelineBuilder UseClientStream(IClientStreamWriter<EncryptedDocumentChunk> streamWriter)
        {
            this.clientStream = streamWriter;
            return this;
        }

        /// <summary>
        /// Indique les données à envoyer.
        /// </summary>
        /// <param name="payload">Un objet à envoyer au serveur.</param>
        /// <returns>Le <see cref="PipelineBuilder"/> modifié.</returns>
        public PipelineBuilder UsePayload(object? payload)
        {
            this.payload = payload;
            return this;
        }

        public DocumentReceptionPipeline BuildDocumentReceptionPipeline()
        {
            throw new NotImplementedException("Not implemented");
        }

        public HeadersReceptionPipeline BuildHeadersReceptionPipeline()
        {
            return new HeadersReceptionPipeline(
                this.RequireDocumentSerialiser(),
                this.RequireEncryptionMethod(),
                this.RequireServerStream()
            );
        }

        public DocumentTransmissionPipeline BuildDocumentTransmissionPipeline()
        {
            throw new NotImplementedException("Not implemented");
        }

        public HeaderTransmissionPipeline BuildHeaderTransmissionPipeline()
        {
            throw new NotImplementedException("Not implemented");
        }

    }
}
