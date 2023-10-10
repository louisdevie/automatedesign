using AutomateDesign.Core.Users;
using AutomateDesign.Server.Data;
using MySql.Data.MySqlClient;
using System.Data;
using System.Runtime.CompilerServices;
using System.Security.Policy;

namespace AutomateDesign.Server.Data.Implementation
{
    public class UserDao : BaseDao, IUserDao
    {
        public UserDao(DatabaseConnector connector) : base(connector) { }

        public void Create(User item)
        {
            using MySqlConnection connection = Connect();

            connection.ExecuteNonQuery(
                "INSERT INTO User (Email, Hash, Salt) VALUES (?, ?, ?)",
                item.Email, item.Password.Hash, item.Password.Salt
            );
        }

        public void Delete(int key)
        {
            using MySqlConnection connection = Connect();
            
            connection.ExecuteNonQuery("DELETE FROM User WHERE IdUser = ?",key);
        }

        public IEnumerable<User> Read()
        {
            List<User> users = new List<User>();
            string query = "SELECT * FROM User";

            using MySqlConnection connection = Connect();
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    byte[] password = new byte[32];
                    dataReader.GetBytes("Hash", 0, password, 0, 32);


                    User user = new User(

                        dataReader.GetInt32("IdUser"),
                        dataReader.GetString("Email"),
                        new HashedPassword(password, dataReader.GetString("Salt"))
                    );
                    users.Add(user);
                }
                dataReader.Close();
            }
            return users;
        }

        public User Read(int key)
        {
            User user = null;
            string query = $"SELECT * FROM User WHERE IdUser = {key}";

            using MySqlConnection connection = Connect();
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader.Read())
                {
                    byte[] password = new byte[32];
                    dataReader.GetBytes("Hash", 0, password, 0, 32);
                    user = new User(

                        dataReader.GetInt32("IdUser"),
                        dataReader.GetString("Email"),
                        new HashedPassword(password, dataReader.GetString("Salt"))
                    );
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

            using MySqlConnection connection = Connect();
            {
                
                connection.ExecuteNonQuery("UPDATE User SET Email = '?', Hash = '?', Salt = '?' WHERE IdUser = ?",
                    item.Email,item.Password.Hash, item.Password.Salt,key);
            }
        }
    }
}
