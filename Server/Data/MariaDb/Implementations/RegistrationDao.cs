using AutomateDesign.Core.Exceptions;
using AutomateDesign.Core.Users;

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
            using var connection = this.Connect();

            connection.ExecuteNonQuery(
                "REPLACE INTO `Registration`(`UserId`, `VerificationCode`, `Expiration`) VALUES (?, ?, ?)",
                registration.User.Id, registration.VerificationCode, registration.Expiration
            );

            return registration;
        }

        public void Delete(int userId)
        {
            using var connection = this.Connect();

            connection.ExecuteNonQuery("DELETE FROM `Registration` WHERE `UserId` = ?", userId);
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
                throw new ResourceNotFoundException("Aucune demande d'inscription trouvée pour cet utilisateur.");
            }
        }
    }
}
