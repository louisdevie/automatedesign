using AutomateDesign.Core.Users;
using AutomateDesign.Protos;
using AutomateDesign.Server.Data;
using Grpc.Core;

namespace AutomateDesign.Server.Services
{
    public class UsersService : Users.UsersBase
    {
        private IUserDao userDao;
        private IRegistrationDao registrationDao;

        public UsersService(IUserDao userDao, IRegistrationDao registrationDao)
        {
            this.userDao = userDao;
            this.registrationDao = registrationDao;
        }

        public override Task<SignUpReply> SignUp(SignUpRequest request, ServerCallContext context)
        {
            User newUser = new(request.Email, HashedPassword.FromPlain(request.Password));
            Registration registration = new(newUser);

            int userId = this.registrationDao.Create(registration).User.Id;

            return Task.FromResult(new SignUpReply { UserId = userId });
        }

        public override Task<Nothing> Verify(VerificationRequest request, ServerCallContext context)
        {
            Registration registration = this.registrationDao.ReadById(request.UserId);

            if (registration.Expired)
            {
                throw new RpcException(new Status(
                    StatusCode.FailedPrecondition,
                    "Le code de vérification est expiré."
                ));
            }

            if (request.SecretCode != registration.VerificationCode)
            {
                throw new RpcException(new Status(
                    StatusCode.InvalidArgument,
                    "Le code de vérification est incorrect."
                ));
            }

            registration.User.IsVerified = true;
            this.userDao.Update(registration.User);

            return Task.FromResult(new Nothing());
        }
    }
}
