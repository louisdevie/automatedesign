using Grpc.Core;

namespace AutomateDesign.Core.Exceptions
{
    public class DatabaseUnavailableException : RpcException
    {
        public DatabaseUnavailableException()
        : base(new Status(StatusCode.Unavailable, "Impossible de se connecter à la base de données")) { }
    }
}
