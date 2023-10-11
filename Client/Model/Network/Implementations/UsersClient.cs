using AutomateDesign.Protos;
using Grpc.Net.Client;

namespace AutomateDesign.Client.Model.Network.Implementations
{
    internal class UsersClient : Users.UsersClient, IUsersClient
    {
        public UsersClient(GrpcChannel channel) : base(channel) { }

        public int SignUp(string email, string password)
        {
            SignUpReply reply = this.SignUp(
                new SignUpRequest
                {
                    Email = email,
                    Password = password
                }
            );

            return reply.UserId;
        }
    }
}
