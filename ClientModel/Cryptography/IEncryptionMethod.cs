using AutomateDesign.Core.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Channels;
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
        /// <param name="input">Le canal d'ou arrivent les données à chiffrer.</param>
        /// <param name="output">Le canal dans lequel envoyer les données chiffrées.</param>
        /// <returns>Une tâche représentant l'opération.</returns>
        Task EncryptAsync(DocumentChannelReader input, DocumentChannelWriter output);

        /// <summary>
        /// Déchiffre un flux de données binaires.
        /// </summary>
        /// <param name="input">Le canal d'ou arrivent les données à déchiffrer.</param>
        /// <param name="output">Le canal dans lequel envoyer les données déchiffrées.</param>
        /// <returns>Une tâche représentant l'opération.</returns>
        Task DecryptAsync(DocumentChannelReader input, DocumentChannelWriter output);

        /// <summary>
        /// Déchiffre des parties de document sans suivre une structure particulière.
        /// </summary>
        /// <param name="input">Le canal d'ou arrivent les données à déchiffrer.</param>
        /// <param name="output">Le canal dans lequel envoyer les données déchiffrées.</param>
        /// <returns>Une tâche représentant l'opération.</returns>
        Task DecryptUnstructuredAsync(ChannelReader<byte[]> input, ChannelWriter<byte[]> output);
    }
}
