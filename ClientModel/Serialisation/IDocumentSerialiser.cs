using AutomateDesign.Core.Documents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
