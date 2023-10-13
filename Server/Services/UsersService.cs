using AutomateDesign.Core.Users;
using AutomateDesign.Protos;
using AutomateDesign.Server.Data;
using Grpc.Core;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Tls.Crypto;
using System;
using System.Text.RegularExpressions;

namespace AutomateDesign.Server.Services
{
    public class UsersService : Users.UsersBase
    {
        private IUserDao userDao;
        private IRegistrationDao registrationDao;
        private ISessionDao sessionDao;

        public UsersService(IUserDao userDao, IRegistrationDao registrationDao,ISessionDao sessionDao)
        {
            this.userDao = userDao;
            this.registrationDao = registrationDao;
            this.sessionDao = sessionDao;
        }

        public override Task<SignUpReply> SignUp(EmailAndPassword request, ServerCallContext context)
        {
            User newUser = new(request.Email, HashedPassword.FromPlain(request.Password));
            Registration registration = new(newUser);

            int userId = this.registrationDao.Create(registration).User.Id;

            return Task.FromResult(new SignUpReply { UserId = userId });
        }

        public override Task<Nothing> VerifyUser(VerificationRequest request, ServerCallContext context)
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

        public override Task<SignInReply> SignIn(EmailAndPassword request, ServerCallContext context)
        {
            User user = this.userDao.ReadByEmail(request.Email);

            if (!user.Password.Match(request.Password))
            {
                throw new RpcException(new Status(
                    StatusCode.InvalidArgument,
                    "Le mot de passe et le mail ne correspondent pas."
                ));
            }

            Session session = new Session(user);
            this.sessionDao.Create(session);

            return Task.FromResult(new SignInReply { UserId = user.Id, Token = session.Token });
        }

        public override Task<Nothing> ChangePassword(NewPassword request, ServerCallContext context)
        {
            User user = this.userDao.ReadByEmail(request.Email);
            if (request.Code != this.registrationDao.ReadById(user.Id).VerificationCode)
            {
                throw new RpcException(new Status(
                    StatusCode.InvalidArgument,
                    "Code de vérification incorrecte."
                ));
            }
            user.Password = HashedPassword.FromPlain(request.Password);
            return Task.FromResult(new Nothing());
        }        
    }
}
