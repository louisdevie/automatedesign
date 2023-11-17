using AutomateDesign.Core.Documents;

namespace AutomateDesign.Client.Model.Serialisation
{
    internal interface IDocumentSerialiser
    {
        IAsyncEnumerable<byte[]> SerialiseDocumentAsync(Document document);

        Task<byte[]> SerialiseHeaderAsync(Document document);

        Task<Document> DeserialiseDocumentAsync(IAsyncEnumerable<byte[]> stream);

        IAsyncEnumerable<DocumentHeader> DeserialiseHeadersAsync(IAsyncEnumerable<byte[]> stream);
    }
}
