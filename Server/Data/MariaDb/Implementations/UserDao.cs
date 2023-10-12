using AutomateDesign.Core.Users;
using AutomateDesign.Core.Exceptions;
using MySql.Data.MySqlClient;
using System.Data;

namespace AutomateDesign.Server.Data.MariaDb.Implementations
{
    public class UserDao : BaseDao, IUserDao
    {
        public UserDao(DatabaseConnector connector) : base(connector) { }

        public User Create(User user)
        {
            using MySqlConnection connection = Connect();

            connection.ExecuteNonQuery(
                "INSERT INTO User (Email, PasswordHash, PasswordSalt, IsVerified) VALUES (?, ?, ?, ?)",
                user.Email, user.Password.Hash, user.Password.Salt, user.IsVerified
            );

            return user.WithId(connection.GetLastInsertId());
        }

        private static User Hydrate(MySqlDataReader result)
        {
            byte[] password = new byte[32];
            result.GetBytes("PasswordHash", 0, password, 0, 32);

            return new User(
                result.GetInt32("UserId"),
                result.GetString("Email"),
                new HashedPassword(password, result.GetString("PasswordSalt")),
                result.GetBoolean("IsVerified")
            );
        }

        public User ReadById(int id)
        {
            using MySqlConnection connection = Connect();

            using var result = connection.ExecuteReader(
                "SELECT * FROM `User` WHERE `UserId` = ?", id
            );

            if (result.Read())
            {
                return Hydrate(result);
            }
            else
            {
                throw new ResourceNotFoundException();
            }
        }

        public User ReadByEmail(string address)
        {
            using MySqlConnection connection = Connect();

            using var result = connection.ExecuteReader(
                "SELECT * FROM `User` WHERE `Email` = ?", address
            );

            if (result.Read())
            {
                return Hydrate(result);
            }
            else
            {
                throw new ResourceNotFoundException();
            }
        }

        public void Update(User user)
        {
            using MySqlConnection connection = Connect();
            
            connection.ExecuteNonQuery(
                "UPDATE `User` SET `PasswordHash` = ?, `PasswordSalt` = ?, `IsVerified` = ? WHERE `UserId` = ?",
                user.Password.Hash, user.Password.Salt, user.IsVerified, user.Id
            );
        }

        public void Delete(int id)
        {
            using MySqlConnection connection = Connect();

            connection.ExecuteNonQuery("DELETE FROM `User` WHERE `UserId` = ?", id);
        }
    }
}
