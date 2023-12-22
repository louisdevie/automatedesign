using AutomateDesign.Core.Documents;
using AutomateDesign.Core.Users;
using AutomateDesign.Protos;
using AutomateDesign.Server.Data;
using AutomateDesign.Server.Middleware.Authentication;
using Google.Protobuf;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Channels;

namespace AutomateDesign.Server.Services
{
    [Authorize]
    public class DocumentsService : Documents.DocumentsBase
    {
        private IDocumentDao documentDao;

        public DocumentsService(IDocumentDao documentDao)
        {
            this.documentDao = documentDao;
        }

        public override async Task GetAllHeaders(Nothing request,
            IServerStreamWriter<EncryptedDocumentChunk> responseStream, ServerCallContext context)
        {
            User user = context.RequireUser();

            await foreach (var header in this.documentDao.ReadAllHeadersAsync(user.Id, context.CancellationToken))
            {
                await responseStream.WriteAsync(header);
            }
        }

        public override async Task GetDocument(DocumentIdOnly request,
            IServerStreamWriter<EncryptedDocumentChunk> responseStream, ServerCallContext context)
        {
            User user = context.RequireUser();
            DocumentChannel channel = new();

            Task daoTask = this.documentDao.ReadByIdAsync(user.Id, request.DocumentId, channel.Writer);

            var header = await channel.Reader.ReadHeaderAsync();
            await responseStream.WriteAsync(new EncryptedDocumentChunk
                { DocumentId = request.DocumentId, Data = ByteString.CopyFrom(header) }
            );

            await foreach (var chunk in channel.Reader.ReadAllBodyPartsAsync())
            {
                await responseStream.WriteAsync(new EncryptedDocumentChunk
                    { DocumentId = request.DocumentId, Data = ByteString.CopyFrom(chunk) }
                );
            }

            await daoTask;
        }

        public override async Task<DocumentIdOnly> SaveDocument(
            IAsyncStreamReader<EncryptedDocumentChunk> requestStream, ServerCallContext context)
        {
            User user = context.RequireUser();
            bool headerChunk = true;
            DocumentChannel channel = new();
            Task<int> daoTask = Task.FromResult(DocumentHeader.UnsavedId);

            while (await requestStream.MoveNext())
            {
                if (headerChunk)
                {
                    // traitement du premier morceau

                    int documentId = requestStream.Current.DocumentId;

                    if (documentId == DocumentHeader.UnsavedId)
                    {
                        // nouveau document

                        daoTask = documentDao.CreateAsync(user.Id, channel.Reader);
                    }
                    else
                    {
                        // document existant

                        daoTask = documentDao.UpdateAsync(user.Id, documentId, channel.Reader);
                    }

                    await channel.Writer.WriteHeaderAsync(requestStream.Current.Data.ToArray());

                    headerChunk = false;
                }
                else
                {
                    await channel.Writer.WriteBodyPartAsync(requestStream.Current.Data.ToArray());
                }
            }

            await channel.Writer.FinishWritingBodyAsync();

            return new DocumentIdOnly { DocumentId = await daoTask };
        }

        /// <summary>
        /// Suppression d'un automate demandé par l'utilisateur
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<Nothing> DeleteDocument(DocumentIdOnly request, ServerCallContext context)
        {
            User user = context.RequireUser();
            documentDao.Delete(user.Id, request.DocumentId);

            return Task.FromResult(new Nothing());
        }
    }
}