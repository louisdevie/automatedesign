using AutomateDesign.Core;
using MySql.Data.MySqlClient;
using System.Runtime.CompilerServices;

namespace AutomateDesign.Server.Data
{
    public class UserDao : BaseDao, IUserDao
    {
        public UserDao(DatabaseConnector connector) : base(connector) { }

        public void Create(User item)
        {
            using MySqlConnection connection = this.Connect();

            connection.ExecuteNonQuery(
                "INSERT INTO User (Email, Hash, Salt) VALUES (?, ?, ?)",
                item.Email, item.Password, 0
            );
        }

        public void Delete(int key)
        {
            string query = $"DELETE FROM User WHERE IdUser = {key}";

            using (MySqlConnection connection = Connection)
            {
                OpenConnection();
                ExecuteQuery(query);
            }
        }

        public IEnumerable<User> Read()
        {
            List<User> users = new List<User>();
            string query = "SELECT * FROM User";

            using (MySqlConnection connection = Connection)
            {              
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    User user = new User
                    {
                        IdUser = Convert.ToInt32(dataReader["IdUser"]),
                        Email = dataReader["Email"].ToString(),
                        Hash = dataReader["Hash"].ToString(),
                        Salt = dataReader["Salt"].ToString()
                    };
                    users.Add(user);
                }
                dataReader.Close();
                CloseConnection();                
            }
            return users;
        }

        public User Read(int key)
        {
            User user = null;
            string query = $"SELECT * FROM User WHERE IdUser = {key}";

            using (MySqlConnection connection = Connection)
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader.Read())
                {
                    user = new User
                    {
                        IdUser = Convert.ToInt32(dataReader["IdUser"]),
                        Email = dataReader["Email"].ToString(),
                        Hash = dataReader["Hash"].ToString(),
                        Salt = dataReader["Salt"].ToString()
                    };
                }
                dataReader.Close();                
            }
            return user;
        }

        public void Update(int key, User item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            string query = $"UPDATE User SET Email = '{item.Email}', Hash = '{item.Hash}', Salt = '{item.Salt}' WHERE IdUser = {key}";

            using (MySqlConnection connection = Connection)
            {
                OpenConnection();
                ExecuteQuery(query);
            }
        }
    }
}
