using AutomateDesign.Core.Exceptions;
using AutomateDesign.Core.Users;
using AutomateDesign.Protos;
using AutomateDesign.Server.Data;
using AutomateDesign.Server.Middleware.Authentication;
using AutomateDesign.Server.Model;
using Grpc.Core;

namespace AutomateDesign.Server.Services
{
    public class UsersService : Users.UsersBase
    {
        private IUserDao userDao;
        private IRegistrationDao registrationDao;
        private ISessionDao sessionDao;
        private IDocumentDao documentDao;
        private EmailSender emailSender;

        private static readonly string IUT_EMAIL_HOST = "iut-dijon.u-bourgogne.fr";

        public UsersService(IUserDao userDao, IRegistrationDao registrationDao, ISessionDao sessionDao, IDocumentDao automateDao, EmailSender emailSender)
        {
            this.userDao = userDao;
            this.registrationDao = registrationDao;
            this.sessionDao = sessionDao;
            this.documentDao = automateDao;
            this.emailSender = emailSender;           
        }

        public override Task<UserIdOnly> SignUp(EmailAndPassword request, ServerCallContext context)
        {
            User newUser = new(request.Email, HashedPassword.FromPlain(request.Password));

            if (newUser.Email.Host != IUT_EMAIL_HOST)
            {
                throw new InvalidResourceException("L'adresse mail doit être votre adresse IUT.");
            }

            try
            {
                User existingUser = this.userDao.ReadByEmail(request.Email);

                if (existingUser.IsVerified)
                {
                    throw new DuplicateResourceException("Cette adresse mail est déjà utilisée.");
                }

                // nouveau mot de passe, ancien id
                newUser = newUser.WithId(existingUser.Id);
                this.userDao.Update(newUser);
            }
            catch (ResourceNotFoundException)
            {
                newUser = this.userDao.Create(newUser);
            }

            Registration registration = new(newUser);
            this.registrationDao.Create(registration);

            Task.Run(() =>
            {
                this.emailSender.Send(
                    new VerificationEmail(
                        newUser.Email,
                        "Code de vérification AutomateDesign",
                        "sign_up_email.html",
                        registration.VerificationCode
                    )
                );
            });

            return Task.FromResult(new UserIdOnly { UserId = newUser.Id });
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

            this.registrationDao.Delete(registration.User.Id);

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

            if (!user.IsVerified)
            {
                throw new RpcException(new Status(
                    StatusCode.FailedPrecondition,
                    "Utilisateur non vérifié."
                ));
            }

            Session session = new Session(user);
            this.sessionDao.Create(session);

            return Task.FromResult(new SignInReply { Token = session.Token, UserId = user.Id });
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

                    this.registrationDao.Delete(user.Id);

                    break;
            }

            user.Password = HashedPassword.FromPlain(request.NewPassword);
            this.userDao.Update(user);

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

            this.registrationDao.Create(registration);

            Task.Run(() =>
            {
                this.emailSender.Send(
                    new VerificationEmail(
                        user.Email,
                        "Code de vérification AutomateDesign",
                        "reset_password_email.html",
                        registration.VerificationCode
                    )
                );
            });

            return Task.FromResult(new UserIdOnly { UserId = user.Id });
        }

        public override Task<Nothing> Disconnect(SessionUser request, ServerCallContext context)
        {
            sessionDao.Delete(request.Session);
            
            return Task.FromResult(new Nothing());
        }

        public override Task<Nothing> DeleteAutomate(AutomateId request, ServerCallContext context)
        {
            documentDao.Delete(context.RequireUser().Id, request.Id);

            return Task.FromResult(new Nothing());
        }

    }
}
