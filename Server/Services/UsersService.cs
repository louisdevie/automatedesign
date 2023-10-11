using AutomateDesign.Core.Users;
using AutomateDesign.Protos;
using AutomateDesign.Server.Data;
using Grpc.Core;

namespace AutomateDesign.Server.Services
{
    public class UsersService : Users.UsersBase
    {
        private IRegistrationDao registrationDao;

        public UsersService(IRegistrationDao registrationDao)
        {
            this.registrationDao = registrationDao;
        }

        public override Task<Nothing> SignUp(SignUpRequest request, ServerCallContext context)
        {
            User newUser = new(request.Email, HashedPassword.FromPlain(request.Password));
            Registration registration = new(newUser);

            this.registrationDao.Create(registration);

            return Task.FromResult(new Nothing());
        }
    }
}
