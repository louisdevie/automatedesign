using AutomateDesign.Client.Model.Cryptography;
using AutomateDesign.Client.Model.Logic;
using AutomateDesign.Client.Model.Pipelines;
using AutomateDesign.Client.Model.Serialisation;
using AutomateDesign.Core.Documents;
using AutomateDesign.Protos;
using Grpc.Core;
using System;


namespace AutomateDesign.Client.Model.Network
{
    /// <summary>
    /// Une implémentation de <see cref="IDocumentsClient"/> qui utilise le service gRPC.
    /// </summary>
    public class DocumentsClient : Client, IDocumentsClient
    {
        private static IEncryptionMethod GetDefaultEncryptionMethodWithKey(byte[] userKey)
        {
            return new AesCbcEncryptionMethod(userKey);
        }

        private static IDocumentSerialiser GetDefaultDocumentSerialiser()
        {
            return new JsonDocumentSerialiser();
        }

        public async Task DeleteDocument(Session session, int documentId)
        {
            var document = new Documents.DocumentsClient(this.Channel);

            await document.DeleteDocumentAsync(
                new DocumentIdOnly { DocumentId = documentId },
                CallOptionsFromSession(session)
            );
        }

        public HeadersReceptionPipeline GetAllHeaders(Session session)
        {
            var client = new Documents.DocumentsClient(this.Channel);

            var ssCall = client.GetAllHeaders(new Nothing(), CallOptionsFromSession(session));

            return new PipelineBuilder()
                   .UseEncryptionMethod(GetDefaultEncryptionMethodWithKey(session.UserEncryptionKey))
                   .UseDocumentSerialiser(GetDefaultDocumentSerialiser())
                   .UseServerStream(ssCall.ResponseStream)
                   .BuildHeadersReceptionPipeline();
        }

        public DocumentReceptionPipeline GetDocument(Session session, int id)
        {
            var client = new Documents.DocumentsClient(this.Channel);

            var ssCall = client.GetDocument(
                new DocumentIdOnly { DocumentId = id },
                CallOptionsFromSession(session)
            );

            return new PipelineBuilder()
                   .UseEncryptionMethod(GetDefaultEncryptionMethodWithKey(session.UserEncryptionKey))
                   .UseDocumentSerialiser(GetDefaultDocumentSerialiser())
                   .UseServerStream(ssCall.ResponseStream)
                   .BuildDocumentReceptionPipeline();
        }

        public DocumentTransmissionPipeline SaveDocument(Session session, Document document)
        {
            var client = new Documents.DocumentsClient(this.Channel);

            var csCall = client.SaveDocument(CallOptionsFromSession(session));

            return new PipelineBuilder()
                   .UseEncryptionMethod(GetDefaultEncryptionMethodWithKey(session.UserEncryptionKey))
                   .UseDocumentSerialiser(GetDefaultDocumentSerialiser())
                   .UseClientStream(csCall.RequestStream, csCall.ResponseAsync)
                   .UsePayload(document)
                   .BuildDocumentTransmissionPipeline();
        }

        public Task<int> SaveHeader(Session session, DocumentHeader header)
        {
            throw new NotImplementedException();
        }
    }
}