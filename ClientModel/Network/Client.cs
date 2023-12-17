using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using AutomateDesign.Client.Model.Logic;
using Grpc.Core;
using Grpc.Net.Client;

namespace AutomateDesign.Client.Model.Network
{
    /// <summary>
    /// La classe de base pour les clients gRPC.
    /// </summary>
    public class Client
    {
        private static readonly string ServerUrl = "https://10.128.120.128:5001";

        private GrpcChannel? channel;

        /// <summary>
        /// Un canal de communication avec le serveur.
        /// </summary>
        protected GrpcChannel Channel
        {
            get
            {
                if (this.channel == null)
                {
                    var handler = new HttpClientHandler();
                    handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                    this.channel = GrpcChannel.ForAddress(ServerUrl, new GrpcChannelOptions { HttpHandler = handler });
                }

                return this.channel;
            }
        }

        /// <summary>
        /// Crée un objet contenant des métadonnées pour authentifer une requête.
        /// </summary>
        /// <param name="session">Les informations sur l'utilisateur.</param>
        /// <returns>Des <see cref="CallOptions"/> avec les informations nécessaires.</returns>
        protected static CallOptions CallOptionsFromSession(Session session)
        {
            return new CallOptions(
                new Metadata
                {
                    { "Authorization", $"Bearer {session.Token}" }
                }
            );
        }
    }
}