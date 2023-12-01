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
            using var channel = this.OpenChannel();
            var document = new Documents.DocumentsClient(channel);

            await document.DeleteDocumentAsync(
                new DocumentIdOnly
                {
                    DocumentId = documentId
                },
                CallOptionsFromSession(session)
            );
        }

        public HeadersReceptionPipeline GetAllHeaders(Session session)
        {
            using var channel = this.OpenChannel();
            var client = new Documents.DocumentsClient(channel);

            var ssCall = client.GetAllHeaders(new Nothing(), CallOptionsFromSession(session));

            return new PipelineBuilder()
                .UseEncryptionMethod(GetDefaultEncryptionMethodWithKey(session.UserEncryptionKey))
                .UseDocumentSerialiser(GetDefaultDocumentSerialiser())
                .UseServerStream(ssCall.ResponseStream)
                .BuildHeadersReceptionPipeline();
        }

        public Task<int> SaveDocument(Session session, Document document)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveHeader(Session session, DocumentHeader header)
        {
            throw new NotImplementedException();
        }
    }
}
