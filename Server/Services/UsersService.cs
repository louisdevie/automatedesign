﻿using AutomateDesign.Core.Users;
using AutomateDesign.Protos;
using AutomateDesign.Server.Data;
using Grpc.Core;

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

        public override Task<UserIdOnly> SignUp(EmailAndPassword request, ServerCallContext context)
        {
            User newUser = new(request.Email, HashedPassword.FromPlain(request.Password));
            Registration registration = new(newUser);

            int userId = this.registrationDao.Create(registration).User.Id;

            return Task.FromResult(new UserIdOnly { UserId = userId });
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

        public override Task<Nothing> ChangePassword(PasswordChangeRequest request, ServerCallContext context)
        {
            User user = this.userDao.ReadById(request.UserId);

            if (!user.IsVerified)
            {
                throw new RpcException(new Status(
                    StatusCode.FailedPrecondition,
                    "Utilisateur non vérifié."
                ));
            }

            switch (request.AuthenticationCase)
            {
                case PasswordChangeRequest.AuthenticationOneofCase.CurrentPassword:
                    if (!user.Password.Match(request.CurrentPassword))
                    {
                        throw new RpcException(new Status(
                            StatusCode.InvalidArgument,
                            "Le mot de passe actuel ne correspond pas."
                        ));
                    }

                    user.Password = HashedPassword.FromPlain(request.NewPassword);

                    // TODO: Ré-encrypter les documents de l'utilisateur !

                    break;

                case PasswordChangeRequest.AuthenticationOneofCase.SecretCode:
                    Registration registration = this.registrationDao.ReadById(user.Id);

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

                    user.Password = HashedPassword.FromPlain(request.NewPassword);

                    break;
            }

            return Task.FromResult(new Nothing());
        }

        public override Task<Nothing> CheckResetCode(VerificationRequest request, ServerCallContext context)
        {
            Registration registration = this.registrationDao.ReadById(request.UserId);

            if (!registration.User.IsVerified)
            {
                throw new RpcException(new Status(
                    StatusCode.FailedPrecondition,
                    "Utilisateur non vérifié."
                ));
            }

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

            return Task.FromResult(new Nothing());
        }

        public override Task<UserIdOnly> ResetPassword(PasswordResetRequest request, ServerCallContext context)
        {
            User user = this.userDao.ReadByEmail(request.Email);

            if (!user.IsVerified)
            {
                throw new RpcException(new Status(
                    StatusCode.FailedPrecondition,
                    "Utilisateur non vérifié."
                ));
            }

            Registration registration = new(user);

            int userId = this.registrationDao.Create(registration).User.Id;

            return Task.FromResult(new UserIdOnly { UserId = userId });
        }
    }
}
