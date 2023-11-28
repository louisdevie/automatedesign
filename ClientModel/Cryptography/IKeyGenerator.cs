using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model.Cryptography
{
    /// <summary>
    /// Permets de générer une clé de taille fixe à partir d'un mot de passe.
    /// </summary>
    public interface IKeyGenerator
    {
        /// <summary>
        /// Génère une clé de taille fixe qu'un utilisateur peut utiliser pour (dé)chiffer ses automates.
        /// </summary>
        /// <param name="keySize">La taille de clé voulue.</param>
        /// <param name="password">Le mot de passe de l'utilisateur.</param>
        /// <param name="salt">Un sel cryptographique associé à l'utilisateur.</param>
        /// <returns></returns>
        byte[] GetKey(int keySize, string password, string salt);
    }
}
