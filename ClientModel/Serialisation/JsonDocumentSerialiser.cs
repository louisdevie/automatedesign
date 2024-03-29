﻿using AutomateDesign.Client.Model.Serialisation.Dto;
using AutomateDesign.Core.Documents;
using System.Text.Json;
using System.Threading.Channels;

namespace AutomateDesign.Client.Model.Serialisation
{
    /// <summary>
    /// Une implémentation de <see cref="IDocumentSerialiser"/> qui sérialise les données en JSON et UTF-8.
    /// </summary>
    public class JsonDocumentSerialiser : IDocumentSerialiser
    {
        public async Task<Document> DeserialiseDocumentAsync(DocumentChannelReader input)
        {
            DocumentHeaderDto header;
            DocumentBodyDto body;

            using (MemoryStream stream = new(await input.ReadHeaderAsync()))
            {
                header = await JsonSerializer.DeserializeAsync<DocumentHeaderDto>(stream)
                    ?? throw new NullReferenceException();
            }

            using (MemoryStream stream = new(await input.ReadBodyAsync()))
            {
                body = await JsonSerializer.DeserializeAsync<DocumentBodyDto>(stream)
                       ?? throw new NullReferenceException();
            }

            // reconstruction du document complet
            return body.MapToModel(header.MapToModel());
        }

        public async IAsyncEnumerable<DocumentHeader> DeserialiseHeadersAsync(ChannelReader<byte[]> input)
        {
            await foreach (var headerBytes in input.ReadAllAsync())
            {
                DocumentHeaderDto header;
                using (MemoryStream stream = new(headerBytes))
                {
                    header = await JsonSerializer.DeserializeAsync<DocumentHeaderDto>(stream)
                        ?? throw new NullReferenceException();
                }
                yield return header!.MapToModel();
            }
        }

        public async Task SerialiseDocumentAsync(Document document, DocumentChannelWriter output)
        {
            await SerialiseHeaderAsync(document.Header, output);

            using MemoryStream stream = new();
            await JsonSerializer.SerializeAsync(stream, DocumentBodyDto.MapFromModel(document));
            await output.WriteBodyAsync(stream.ToArray());
        }

        public async Task SerialiseHeaderAsync(DocumentHeader header, DocumentChannelWriter output)
        {
            using MemoryStream stream = new();
            await JsonSerializer.SerializeAsync(stream, DocumentHeaderDto.MapFromModel(header));
            await output.WriteHeaderAsync(stream.ToArray());
        }
    }
}
