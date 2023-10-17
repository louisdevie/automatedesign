using Grpc.Core;

namespace AutomateDesign.Core.Exceptions
{
    public class DuplicateResourceException : RpcException
    {
        public DuplicateResourceException(string message)
        : base(new Status(StatusCode.AlreadyExists, message)) { }
    }
}
