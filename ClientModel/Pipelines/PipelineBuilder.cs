using AutomateDesign.Client.Model.Cryptography;
using AutomateDesign.Client.Model.Serialisation;
using AutomateDesign.Core.Documents;
using AutomateDesign.Protos;
using Grpc.Core;
using Grpc.Net.Client;

namespace AutomateDesign.Client.Model.Pipelines
{
    /// <summary>
    /// Permets de créer et de configurer un <see cref="Pipeline"/>.
    /// </summary>
    public class PipelineBuilder
    {
        private IEncryptionMethod? encryptionMethod;
        private IDocumentSerialiser? documentSerialiser;
        private IAsyncStreamReader<EncryptedDocumentChunk>? serverStream;
        private IClientStreamWriter<EncryptedDocumentChunk>? clientStream;
        private Task? asyncResponse;
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
                throw new InvalidOperationException("Aucun algorithme de chiffrage n'a été configuré.");
            }

            return this.encryptionMethod;
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

            return this.documentSerialiser;
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

            return this.serverStream;
        }

        /// <summary>
        /// Configure le flux requête à utiliser comme sortie.
        /// </summary>
        /// <param name="streamWriter">Un flux requête gRPC.</param>
        /// <param name="asyncResponse">La réponse suivant la fin du flux.</param>
        /// <returns>Le <see cref="PipelineBuilder"/> modifié.</returns>
        public PipelineBuilder UseClientStream(IClientStreamWriter<EncryptedDocumentChunk> streamWriter,
            Task asyncResponse)
        {
            this.clientStream = streamWriter;
            this.asyncResponse = asyncResponse;
            return this;
        }

        private IClientStreamWriter<EncryptedDocumentChunk> RequireClientStream()
        {
            if (this.clientStream == null)
            {
                throw new InvalidOperationException("Aucun flux de requête n'a été configuré.");
            }

            return this.clientStream;
        }

        private Task RequireAsyncResponse()
        {
            if (this.asyncResponse == null)
            {
                throw new InvalidOperationException("Aucun flux de requête n'a été configuré.");
            }

            return this.asyncResponse;
        }

        /// <summary>
        /// Indique les données à envoyer.
        /// </summary>
        /// <param name="payload">Un objet à envoyer au serveur.</param>
        /// <returns>Le <see cref="PipelineBuilder"/> modifié.</returns>
        public PipelineBuilder UsePayload(object payload)
        {
            this.payload = payload;
            return this;
        }

        private object RequirePayload()
        {
            if (this.payload == null)
            {
                throw new InvalidOperationException("Aucune charge utilse n'a été configurée.");
            }

            return this.payload;
        }

        /// <summary>
        /// Construit un pipeline pour recevoir un document.
        /// </summary>
        /// <returns>Un nouveau <see cref="DocumentReceptionPipeline"/>.</returns>
        public DocumentReceptionPipeline BuildDocumentReceptionPipeline()
        {
            return new DocumentReceptionPipeline(
                this.RequireDocumentSerialiser(),
                this.RequireEncryptionMethod(),
                this.RequireServerStream()
            );
        }

        /// <summary>
        /// Construit un pipeline pour recevoir les en-têtes de plusieurs documents.
        /// </summary>
        /// <returns>Un nouveau <see cref="HeadersReceptionPipeline"/>.</returns>
        public HeadersReceptionPipeline BuildHeadersReceptionPipeline()
        {
            return new HeadersReceptionPipeline(
                this.RequireDocumentSerialiser(),
                this.RequireEncryptionMethod(),
                this.RequireServerStream()
            );
        }

        /// <summary>
        /// Construit un pipeline pour envoyer un document.
        /// </summary>
        /// <returns>Un nouveau <see cref="DocumentTransmissionPipeline"/>.</returns>
        public DocumentTransmissionPipeline BuildDocumentTransmissionPipeline()
        {
            var asyncResponse = this.RequireAsyncResponse() as Task<DocumentIdOnly>
                                ?? throw new ArgumentException(
                                    "La réponse à un DocumentTransmissionPipeline doit être un DocumentIdOnly.");

            var payload = this.RequirePayload() as Document
                          ?? throw new ArgumentException(
                              "La charge utile d'un DocumentTransmissionPipeline doit être un Document.");

            return new DocumentTransmissionPipeline(
                this.RequireDocumentSerialiser(),
                this.RequireEncryptionMethod(),
                this.RequireClientStream(),
                asyncResponse,
                payload
            );
        }

        /// <summary>
        /// Construit un pipeline pour envoyer un en-tête seulement.
        /// </summary>
        /// <returns>Un nouveau <see cref="HeaderTransmissionPipeline"/>.</returns>
        public HeaderTransmissionPipeline BuildHeaderTransmissionPipeline()
        {
            throw new NotImplementedException("Not implemented");
        }
    }
}