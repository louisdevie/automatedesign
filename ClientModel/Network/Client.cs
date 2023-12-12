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
        private static readonly string serverUrl = "https://localhost:5001";

        /// <summary>
        /// Ouvre un canal de communication avec le serveur.
        /// </summary>
        /// <returns></returns>
        protected GrpcChannel OpenChannel() => GrpcChannel.ForAddress(serverUrl);

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
                    { "Authrorization", $"Bearer {session.Token}" }
                }
            );
        }
    }
}