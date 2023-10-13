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
            User user;
            try
            {
                user = this.userDao.ReadByEmail(registration.User.Email.Address);

                if (user.IsVerified)
                {
                    throw new DuplicateResourceException("Cette adresse mail est déjà utilisée.");
                }

                // nouveau mot de passe, ancien id
                user = registration.User.WithId(user.Id);
                this.userDao.Update(user);
            }
            catch (ResourceNotFoundException)
            {
                user = this.userDao.Create(registration.User);
            }

            using var connection = this.Connect();

            connection.ExecuteNonQuery(
                "REPLACE INTO `Registration`(`UserId`, `VerificationCode`, `Expiration`) VALUES (?, ?, ?)",
                user.Id, registration.VerificationCode, registration.Expiration
            );

            return registration.WithUser(user);
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
