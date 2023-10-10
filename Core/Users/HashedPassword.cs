using AutomateDesign.Core.Random;
using Konscious.Security.Cryptography;
using System.Text;

namespace AutomateDesign.Core.Users
{
    /// <summary>
    /// Un mot de passe haché.
    /// </summary>
    public class HashedPassword
    {
        private byte[] hash;
        private string salt;

        /// <summary>
        /// Le hash du mot de passe.
        /// </summary>
        public byte[] Hash => hash;

        /// <summary>
        /// Le sel utilisé pour hacher le mot de passe.
        /// </summary>
        public string Salt => salt;

        /// <summary>
        /// Crée un <see cref="HashedPassword"/> à partir d'informations existantes.
        /// </summary>
        /// <param name="hash">Le hash du mot de passe.</param>
        /// <param name="salt">Le sel utilisé pour hacher le mot de passe.</param>
        public HashedPassword(byte[] hash, string salt)
        {
            this.hash = hash;
            this.salt = salt;
        }

        private static byte[] SaltAndHash(string password, string salt)
        {
            byte[] saltedPassword = Encoding.UTF8.GetBytes(password + salt);
            Argon2id argon2id = new(saltedPassword);
            argon2id.MemorySize = 19456;
            argon2id.Iterations = 2;
            argon2id.DegreeOfParallelism = 1;
            return argon2id.GetBytes(32);
        }

        /// <summary>
        /// Teste si un mot de passe correspond à celui-ci.
        /// </summary>
        /// <param name="password">Le mot de passe à tester.</param>
        /// <returns><see langword="true"/> si le mot de passe correspond, sinon <see langword="false"/>.</returns>
        public bool Match(string password)
        {
            return hash.SequenceEqual(SaltAndHash(password, salt));
        }

        /// <summary>
        /// Hache un mot de passe.
        /// </summary>
        /// <param name="password">Le mot de passe à hacher.</param>
        /// <returns>Le mot de passe haché avec un sel aléatoire.</returns>
        public static HashedPassword FromPassword(string password)
        {
            var rtg = new RandomTextGenerator(new CryptoRandomProvider());
            string salt = rtg.AlphaNumericString(32);
            return new HashedPassword(SaltAndHash(password, salt), salt);
        }
    }
}
