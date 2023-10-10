using AutomateDesign.Core;
using MySql.Data.MySqlClient;

namespace AutomateDesign.Server.Data.Implementation
{
    public class SessionDao : DatabaseConnector, ISessionDao
    {
        public SessionDao(ConfigurationService configurationService) : base(configurationService)
        {

        }

        public void Create(Session item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            string query = $"INSERT INTO Session (Token, DateTimeLastUse, DateTimeExpired) VALUES ('{item.Token}', '{item.DateTimeLastUse}', '{item.DateTimeExpired}')";

            using (MySqlConnection connection = Connection)
            {
                OpenConnection();
                ExecuteQuery(query);
            }
        }

        public void Delete(int key)
        {
            string query = $"DELETE FROM Session WHERE IdSession = {key}";

            using (MySqlConnection connection = Connection)
            {
                OpenConnection();
                ExecuteQuery(query);
            }
        }

        public IEnumerable<Session> Read()
        {
            throw new NotImplementedException();
        }

        public Session Read(int key)
        {
            Session session = null;
            string query = $"SELECT * FROM Session WHERE IdSession = {key}";

            using (MySqlConnection connection = Connection)
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader.Read())
                {
                    session = new Session
                    {
                        IdSession = Convert.ToInt32(dataReader["IdUser"]),
                        DateTimeLastUse = Convert.ToDateTime(dataReader["DateTimeLastUse"]),
                        DateTimeExpired = Convert.ToDateTime(dataReader["DateTimeExpired"])
                    };
                }
                dataReader.Close();
            }
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

            string query = $"UPDATE Session SET DateTimeLastUse = '{item.DateTimeLastUse}' WHERE IdSession = {key}";

            using (MySqlConnection connection = Connection)
            {
                OpenConnection();
                ExecuteQuery(query);
            }
        }
    }
}
