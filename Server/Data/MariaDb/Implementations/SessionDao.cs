using AutomateDesign.Core.Users;
using AutomateDesign.Core.Exceptions;
using MySql.Data.MySqlClient;

namespace AutomateDesign.Server.Data.MariaDb.Implementations
{
    public class SessionDao : BaseDao, ISessionDao
    {
        private IUserDao userDao;

        public SessionDao(DatabaseConnector connector) : base(connector) { this.userDao = new UserDao(connector); }

        public Session Create(Session session)
        {
            using MySqlConnection connection = Connect();

            connection.ExecuteNonQuery(
                "INSERT INTO `Session` (`Token`, `LastUse`, `Expiration`, `UserId`) VALUES (?, ?, ?, ?)",
                session.Token, session.LastUse, session.Expiration, session.User.Id
            );

            return session;
        }

        public Session ReadByToken(string token)
        {
            using MySqlConnection connection = Connect();

            using var result = connection.ExecuteReader(
                "SELECT `LastUse`, `Expiration`, `UserId` FROM `Session` WHERE `Token` = ?", token
            );

            if (result.Read())
            {
                return new Session(
                    token,
                    result.GetDateTime("LastUse"),
                    result.GetDateTime("Expiration"),
                    this.userDao.ReadById(result.GetInt32(2))
                );
            }
            else
            {
                throw new ResourceNotFoundException();
            }
        }

        public void UpdateLastUse(Session session)
        {
            using MySqlConnection connection = Connect();

            connection.ExecuteNonQuery(
                "UPDATE `Session` SET `LastUse` = ? WHERE `Token` = ?",
                session.LastUse, session.Token
            );
        }

        public void Delete(string token)
        {
            using MySqlConnection connection = Connect();

            connection.ExecuteNonQuery("DELETE FROM `Session` WHERE `Token` = ?", token);
        }
    }
}
