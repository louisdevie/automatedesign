using AutomateDesign.Core.Users;
using AutomateDesign.Protos;
using AutomateDesign.Server.Data;
using AutomateDesign.Server.Middleware.Authentication;
using Google.Protobuf;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

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

        public override async Task GetAllHeaders(Nothing request, IServerStreamWriter<EncryptedDocumentChunk> responseStream, ServerCallContext context)
        {
            User user = context.RequireUser();

            await foreach (var header in this.documentDao.ReadAllHeadersAsync(user.Id, context.CancellationToken))
            {
                await responseStream.WriteAsync(
                    new EncryptedDocumentChunk
                    {
                        Data = ByteString.CopyFrom(header)
                    }
                );
            }
        }

        /// <summary>
        /// Suppréssion d'un automate demandé par l'utiilisateur
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<Nothing> DeleteDocument(DocumentIdOnly request, ServerCallContext context)
        {
            User user = context.RequireUser();
            documentDao.Delete(user.Id,request.DocumentId);

            return Task.FromResult(new Nothing());
        }
    }
}
