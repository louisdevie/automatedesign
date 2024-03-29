﻿using AutomateDesign.Core.Documents;
using AutomateDesign.Core.Exceptions;
using AutomateDesign.Core.Users;
using AutomateDesign.Protos;
using Google.Protobuf;
using MySql.Data.MySqlClient;
using System.Runtime.CompilerServices;

namespace AutomateDesign.Server.Data.MariaDb.Implementations
{
    /// <summary>
    /// Une implémentation de <see cref="IDocumentDao"/> pour MySQL.
    /// </summary>
    public class DocumentDao : BaseDao, IDocumentDao
    {
        public DocumentDao(DatabaseConnector connector) : base(connector) { }

        public void Delete(int userId, int documentId)
        {
            using MySqlConnection connection = Connect();

            int affected = connection.ExecuteNonQuery(
                "DELETE FROM `Document` WHERE `UserId` = ? AND `DocumentId` = ?",
                userId, documentId
            );

            if (affected == 0) throw new ResourceNotFoundException("Le document à supprimer n'a pas été trouvé.");
        }

        public async Task ReadByIdAsync(int userId, int documentId, DocumentChannelWriter document)
        {
            using MySqlConnection connection = Connect();

            using var result = connection.ExecuteReader(
                "SELECT `DocumentId`, `HeaderSize`, `HeaderData`, `BodySize`, `BodyData` FROM `Document` WHERE `UserId` = ? AND `DocumentId` = ?",
                userId, documentId
            );

            if (await result.ReadAsync())
            {
                byte[] buffer = new byte[result.GetUInt32(1)];
                result.GetBytes(2, 0, buffer, 0, buffer.Length);

                await document.WriteHeaderAsync(buffer);

                buffer = new byte[result.GetUInt32(3)];
                result.GetBytes(4, 0, buffer, 0, buffer.Length);

                await document.WriteBodyAsync(buffer);
            }
            else
            {
                throw new ResourceNotFoundException("Le document n'a pas été trouvé.");
            }
        }

        public async IAsyncEnumerable<EncryptedDocumentChunk> ReadAllHeadersAsync(int userId, [EnumeratorCancellation] CancellationToken ct = default)
        {
            using MySqlConnection connection = Connect();

            using var results = connection.ExecuteReader(
                "SELECT `DocumentId`, `HeaderSize`, `HeaderData` FROM `Document` WHERE `UserId` = ?",
                userId
            );

            while (await results.ReadAsync())
            {
                ct.ThrowIfCancellationRequested();
                byte[] buffer = new byte[results.GetUInt32(1)];
                results.GetBytes(2, 0, buffer, 0, buffer.Length);
                yield return new EncryptedDocumentChunk
                {
                    DocumentId = results.GetInt32(0),
                    Data = ByteString.CopyFrom(buffer)
                };
            }
        }

        public IAsyncEnumerable<byte[]> ReadByIdAsync(int userId, int documentId)
        {
            throw new NotImplementedException();
        }

        public async Task<int> UpdateAsync(int userId, int documentId, DocumentChannelReader document)
        {
            using MySqlConnection connection = Connect();

            byte[] header = await document.ReadHeaderAsync();
            byte[] body = await document.ReadBodyAsync();

            connection.ExecuteNonQuery(
                "UPDATE `Document` SET `HeaderSize` = ?, `HeaderData` = ?, `BodySize` = ?, `BodyData` = ? WHERE `DocumentId` = ? AND `UserId` = ?",
                header.Length, header, body.Length, body, documentId, userId
            );

            return documentId;
        }

        public void UpdateHeader(int userId, int documentId, byte[] documentHeader)
        {
            throw new NotImplementedException();
        }

        public async Task<int> CreateAsync(int userId, DocumentChannelReader document)
        {
            using MySqlConnection connection = Connect();

            byte[] header = await document.ReadHeaderAsync();
            byte[] body = await document.ReadBodyAsync();

            connection.ExecuteNonQuery(
                "INSERT INTO `Document` (`UserId`, `HeaderSize`, `HeaderData`, `BodySize`, `BodyData`) VALUES (?, ?, ?, ?, ?)",
                userId, header.Length, header, body.Length, body
            );

            return connection.GetLastInsertId();
        }
    }
}
