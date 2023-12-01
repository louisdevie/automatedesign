using AutomateDesign.Client.Model.Logic;
using AutomateDesign.Core.Documents;
using AutomateDesign.Protos;
using Grpc.Core;
using System;


namespace AutomateDesign.Client.Model.Network
{
    public class DocumentsClient : Client, IDocumentsClient
    {
        public async Task DeleteDocument(Session session, int documentId)
        {
            using var channel = this.OpenChannel();
            var document = new Documents.DocumentsClient(channel);

            document.DeleteDocument(
                new DocumentIdOnly
                {
                    DocumentId = documentId
                },
                CallOptionsFromSession(session)
                
            );
        }

        public IAsyncEnumerable<DocumentHeader> GetAllHeader(Session session)
        {
            throw new NotImplementedException();
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
