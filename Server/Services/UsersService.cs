using AutomateDesign.Protos;
using Grpc.Core;

namespace AutomateDesign.Server.Services
{
    public class UsersService : Users.UsersBase
    {
        public UsersService()
        {
        }

        public override Task<Null> SignUp(SignUpRequest request, ServerCallContext context)
        {
            Console.WriteLine($"inscription : email = {request.Email}, password = {request.Password}");
            return Task.FromResult(new Null());
        }
    }
}
