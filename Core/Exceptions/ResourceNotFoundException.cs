using Grpc.Core;

namespace AutomateDesign.Core.Exceptions
{
    public class ResourceNotFoundException : RpcException
    {
        public ResourceNotFoundException(string message) : base(new Status(StatusCode.NotFound, message)) { }
    }
}
