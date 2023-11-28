using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model.Cryptography
{
    /// <summary>
    /// Permet de (dé)chiffrer des données binaires de manière asynchrone.
    /// </summary>
    public interface IEncryptionMethod
    {
        /// <summary>
        /// Chiffre un flux de données binaires.
        /// </summary>
        /// <param name="data">Les données à chiffrer.</param>
        /// <returns>Les données chiffrées sous la forme d'un flux asynchrone.</returns>
        IAsyncEnumerable<byte[]> EncryptAsync(IAsyncEnumerable<byte[]> data);

        /// <summary>
        /// Déchiffre un flux de données binaires.
        /// </summary>
        /// <param name="data">Les données à déchiffrer.</param>
        /// <returns>Les données déchiffrées sous la forme d'un flux asynchrone.</returns>
        IAsyncEnumerable<byte[]> DecryptAsync(IAsyncEnumerable<byte[]> data);
    }
}
