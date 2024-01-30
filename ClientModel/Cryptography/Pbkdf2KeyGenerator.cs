using System.Security.Cryptography;
using System.Text;

namespace AutomateDesign.Client.Model.Cryptography
{
    /// <summary>
    /// Un générateur de clé permettant d'utiliser l'algorithme PBKDF2 pour générer des clés cryptographiques.
    /// </summary>
    public class Pbkdf2KeyGenerator
    {
        /// <summary>
        /// Génère une clé cryptographique à partir d'un mot de passe.
        /// </summary>
        /// <param name="keySize">La taille voulue pour la clé.</param>
        /// <param name="password">Le mot de passe de l'utilisateur.</param>
        /// <param name="salt">Le sel à utiliser pour l'algorithme. Ce peut être n'importe quelle information constante sur l'utilisateur.</param>
        /// <returns></returns>
        public static byte[] GetKey(int keySize, string password, string salt)
        {
            Rfc2898DeriveBytes kdf = new(
                Encoding.UTF8.GetBytes(password),
                Encoding.UTF8.GetBytes(salt),
                4096,
                HashAlgorithmName.SHA256
            );

            return kdf.GetBytes(keySize);
        }
    }
}
