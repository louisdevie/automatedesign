using AutomateDesign.Core.Users;
using AutomateDesign.Server.Model.Exceptions;

namespace AutomateDesign.Server.Data.MariaDb.Implementations
{
    public class RegistrationDao : BaseDao, IRegistrationDao
    {
        private IUserDao userDao;

        public RegistrationDao(DatabaseConnector connector) : base(connector)
        {
            this.userDao = new UserDao(connector);
        }

        public Registration Create(Registration registration)
        {
            User existingUser;
            try
            {
                existingUser = this.userDao.ReadByEmail(registration.User.Email.Address);
            }
            catch (ResourceNotFoundException)
            {
                existingUser = this.userDao.Create(registration.User);
            }

            using var connection = this.Connect();

            connection.ExecuteNonQuery(
                "REPLACE INTO `Registration`(`IdUser`, `VerificationCode`, `Expiration`) VALUES (?, ?, ?)",
                existingUser.Id, registration.VerificationCode, registration.Expiration
            );

            return registration.WithUser(existingUser);
        }

        public void Delete(int userId)
        {
            using var connection = this.Connect();

            connection.ExecuteNonQuery("DELETE FROM `Registration` WHERE `IdUser` = ?", userId);
        }

        public Registration ReadById(int userId)
        {
            using var connection = this.Connect();

            using var result = connection.ExecuteReader(
                "SELECT `VerificationCode`, `Expiration` FROM `Registration` WHERE `UserId` = ?",
                userId
            );

            if (result.Read())
            {
                return new Registration(
                    result.GetUInt32(0),
                    result.GetDateTime(1),
                    this.userDao.ReadById(userId)
                );
            }
            else
            {
                throw new ResourceNotFoundException();
            }
        }
    }
}
