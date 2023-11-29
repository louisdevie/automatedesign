using AutomateDesign.Client.Model.Logic;
using AutomateDesign.Core.Documents;
using AutomateDesign.Protos;

namespace AutomateDesign.Client.Model.Network
{
    public class DocumentsClient : Documents.DocumentsClient, IDocumentsClient
    {
        public Task DeleteDocument(Session session, int documentId)
        {
            throw new NotImplementedException();
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
