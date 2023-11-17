using AutomateDesign.Core.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model.Serialisation
{
    public class DocumentSerializer : IDocumentSerialiser
    {
        public Task<Core.Documents.Document> DeserialiseDocumentAsync(IAsyncEnumerable<byte[]> stream)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<DocumentHeader> DeserialiseHeadersAsync(IAsyncEnumerable<byte[]> stream)
        {
            throw new NotImplementedException();
        }

        public async IAsyncEnumerable<byte[]> SerialiseDocumentAsync(Core.Documents.Document document)
        {
            yield return await SerialiseHeaderAsync(document.Header);

            // Sérialiser la classe Document en bytes au format UTF-8
            byte[] jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(document);

            yield return jsonUtf8Bytes;

            yield break;
        }

        public Task<byte[]> SerialiseHeaderAsync(DocumentHeader header)
        {
            // Sérialiser de DocumentHeader (nom, date de création automate) en bytes au format UTF-8
            byte[] jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(header);
            
            return Task.FromResult(jsonUtf8Bytes);
        }
    }
}
