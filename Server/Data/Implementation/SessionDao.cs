using AutomateDesign.Core;
using AutomateDesign.Core.Users;
using MySql.Data.MySqlClient;

namespace AutomateDesign.Server.Data.Implementation
{
    public class SessionDao : BaseDao, ISessionDao
    {
        private IUserDao userDao;

        public SessionDao(DatabaseConnector connector) : base(connector) { this.userDao = new UserDao(connector); }

        public void Create(Session item)
        {
            using MySqlConnection connection = Connect();

            connection.ExecuteNonQuery(
                "INSERT INTO Session (Token, DateTimeLastUse, DateTimeExpired) VALUES ('?', '?', '?')",
                item.Token, item.LastUse, item.Expiration
                );


        }

        public void Delete(int key)
        {
            string query = $"DELETE FROM Session WHERE IdSession = {key}";

            using MySqlConnection connection = Connect();

            connection.ExecuteNonQuery("DELETE FROM Session WHERE IdSession = ?", key);            
        }

        public IEnumerable<Session> Read()
        {
            throw new NotImplementedException();
        }

        public Session Read(int key)
        {
            Session session = null;
            string query = $"SELECT * FROM Session WHERE IdSession = {key}";

            using MySqlConnection connection = Connect();
            
                
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader.Read())
                {
                User user = this.userDao.Read(Convert.ToInt32(dataReader["IdUser"]));
                session = new Session(
                    dataReader.GetString("Token"),
                    Convert.ToDateTime(dataReader["DateTimeLastUse"]),
                    Convert.ToDateTime(dataReader["DateTimeExpired"]),
                    user
                    );               
                }
                dataReader.Close();
            
            return session;
        }

        /// <summary>
        /// Ne peux modifier que la date de dernière utilisation
        /// </summary>
        /// <param name="key">id Session</param>
        /// <param name="item">Session</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Update(int key, Session item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            using MySqlConnection connection = Connect();
            
                connection.ExecuteNonQuery("UPDATE Session SET DateTimeLastUse = '?' WHERE IdSession = ?",
                    item.LastUse,key);
            
        }
    }
}
