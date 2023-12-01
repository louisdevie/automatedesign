using AutomateDesign.Client.Model.Logic;
using Grpc.Core;
using Grpc.Net.Client;

namespace AutomateDesign.Client.Model.Network
{
    public class Client
    {
        private static readonly string serverUrl = "https://localhost:5001";

        public GrpcChannel OpenChannel() => GrpcChannel.ForAddress(serverUrl);

        protected static CallOptions CallOptionsFromSession(Session session)
        {
            return new CallOptions(
                new Metadata
                {
                    { "Authrorization", $"Bearer {session.Token}" }
                }
            );
        }
    }
}
