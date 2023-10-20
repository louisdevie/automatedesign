using AutomateDesign.Core.Documents;
using AutomateDesign.Core.Exceptions;
using AutomateDesign.Core.Users;
using MySql.Data.MySqlClient;
using System.Data;

namespace AutomateDesign.Server.Data.MariaDb.Implementations
{
    public class AutomateDao : BaseDao, IAutomateDao
    {
        public AutomateDao(DatabaseConnector connector) : base(connector) { }

        public DocumentCrypte Create(DocumentCrypte document)
        {
            using MySqlConnection connection = Connect();

            connection.ExecuteNonQuery(
                "INSERT INTO Document (idDoc, document, idUser) VALUES (?, ?, ?)",
            document.IdDoc, document.Document, document.IdUser
            );
            return document.WithId(connection.GetLastInsertId());
        }

        private static DocumentCrypte Hydrate(MySqlDataReader result)
        {
            byte[] document = new byte[32];
            result.GetBytes("document", 0, document, 0, 32);

            return new DocumentCrypte(
                result.GetInt32("idDoc"),
                document,
                result.GetInt32("idUser")
            );
        }

        public void Delete(int id)
        {
            using MySqlConnection connection = Connect();

            connection.ExecuteNonQuery("DELETE FROM `Document` WHERE `idDoc` = ?", id);
        }

        public DocumentCrypte ReadById(int id)
        {
            using MySqlConnection connection = Connect();

            using var result = connection.ExecuteReader(
                "SELECT * FROM `Document` WHERE `idDoc` = ?", id
            );

            if (result.Read())
            {
                return Hydrate(result);
            }
            else
            {
                throw new ResourceNotFoundException("Document inconnu.");
            }
        }

        public void Update(DocumentCrypte document)
        {
            using MySqlConnection connection = Connect();

            connection.ExecuteNonQuery(
                "UPDATE `Document` SET `document` = ?, WHERE `idDoc` = ?",
                document.Document, document.IdDoc
            );
        }
    }
}
