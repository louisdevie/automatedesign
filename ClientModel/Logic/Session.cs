using AutomateDesign.Client.Model.Cryptography;

namespace AutomateDesign.Client.Model.Logic
{
    /// <summary>
    /// Les informations de session côté client. À ne pas confondre avec <see cref="Core.Users.Session"/>.
    /// </summary>
    public class Session
    {
        private string token;
        private int userId;
        private string userEmail;
        private byte[] userEncryptionKey;

        /// <summary>
        /// Le jeton servant à identifier la session.
        /// </summary>
        public string Token => this.token;

        /// <summary>
        /// L'identifiant de l'utilisateur connecté.
        /// </summary>
        public int UserId => this.userId;

        /// <summary>
        /// L'adresse mail de l'utilisateur connecté.
        /// </summary>
        public string UserEmail => this.userEmail;

        public byte[] UserEncryptionKey => this.userEncryptionKey;

        /// <summary>
        /// Crée une nouvelle session.
        /// </summary>
        /// <param name="token">Le jeton servant à identifier la session.</param>
        /// <param name="userId">L'identifiant de l'utilisateur connecté.</param>
        /// <param name="userEmail">L'adresse mail de l'utilisateur connecté.</param>
        public Session(string token, int userId, string userEmail, string userPassword)
        {
            this.token = token;
            this.userId = userId;
            this.userEmail = userEmail;
            this.userEncryptionKey = Pbkdf2KeyGenerator.GetKey(16, userPassword, userEmail);
        }
    }
}
