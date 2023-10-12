using Grpc.Core;

namespace AutomateDesign.Core.Exceptions
{
    public class InvalidResourceException : RpcException
    {
        public InvalidResourceException(string message)
        : base(new Status(StatusCode.InvalidArgument, message)) { }
    }
}
