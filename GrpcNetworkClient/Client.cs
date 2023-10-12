using Grpc.Net.Client;

namespace AutomateDesign.Client.Model.Network
{
    public class Client
    {
        private static readonly string serverUrl = "https://localhost:5001";

        public GrpcChannel OpenChannel() => GrpcChannel.ForAddress(serverUrl);
    }
}
