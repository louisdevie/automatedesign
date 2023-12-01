using MySql.Data.MySqlClient;
using System.Runtime.CompilerServices;

namespace AutomateDesign.Server.Data.MariaDb.Implementations
{
    public class DocumentDao : BaseDao, IDocumentDao
    {
        public DocumentDao(DatabaseConnector connector) : base(connector) { }

        public int CreateAsync(int userId, IAsyncEnumerable<byte[]> documentChunks)
        {
            throw new NotImplementedException();
        }

        public void Delete(int userId, int documentId)
        {
            throw new NotImplementedException();
        }

#pragma warning disable CS1998 // Cette méthode async n'a pas d'opérateur 'await' et elle s'exécutera de façon synchrone
        public async IAsyncEnumerable<byte[]> ReadAllHeadersAsync(int userId, [EnumeratorCancellation] CancellationToken ct = default)
        {
            using MySqlConnection connection = Connect();

            using var results = connection.ExecuteReader(
                "SELECT `HeaderSize`, `HeaderData` FROM `Document` WHERE `UserId` = ?",
                userId
            );

            while (results.Read())
            {
                ct.ThrowIfCancellationRequested();
                byte[] buffer = new byte[results.GetUInt32(0)];
                results.GetBytes(1, 0, buffer, 0, buffer.Length);
                yield return buffer;
            }
            yield break;
        }
#pragma warning restore CS1998

        public IAsyncEnumerable<byte[]> ReadByIdAsync(int userId, int documentId)
        {
            throw new NotImplementedException();
        }

        public void UpdateAsync(int userId, int documentId, IAsyncEnumerable<byte[]> documentChunks)
        {
            throw new NotImplementedException();
        }

        public void UpdateHeader(int userId, int documentId, byte[] documentHeader)
        {
            throw new NotImplementedException();
        }
    }
}
