using Grpc.Net.Client;
using System;
using System.Windows.Navigation;

namespace AutomateDesign.Client.Model.Network.Implementations
{
    internal class Services
    {
        #region Singleton

        private static Services? current;
        
        public static Services Current
        {
            get
            {
                if (current is null)
                {
                    current = new Services();
                }
                return current;
            }
        }

        #endregion

        private string serverUrl;

        private Services()
        {
            this.serverUrl = "https://localhost:5001";
        }

        public GrpcChannel OpenChannel() => GrpcChannel.ForAddress(this.serverUrl);
    }
}
