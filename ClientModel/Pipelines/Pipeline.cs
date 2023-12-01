using AutomateDesign.Client.Model.Cryptography;
using AutomateDesign.Client.Model.Serialisation;
using AutomateDesign.Core.Documents;

namespace AutomateDesign.Client.Model.Pipelines
{
    /// <summary>
    /// La classe de base pour les opérations de réception et d'envoi.
    /// </summary>
    public abstract class Pipeline
    {
        private IDocumentSerialiser documentSerialiser;
        private IEncryptionMethod encryptionMethod;

        /// <summary>
        /// Crée un nouveau pipeline composé d'un sérialiseur et d'un chiffreur.
        /// </summary>
        /// <param name="documentSerialiser">La classe à utiliser pour (dé)sérialiser les données.</param>
        /// <param name="encryptionMethod">La classe à utiliser pour (dé)chiffrer les données.</param>
        public Pipeline(IDocumentSerialiser documentSerialiser, IEncryptionMethod encryptionMethod)
        {
            this.documentSerialiser = documentSerialiser;
            this.encryptionMethod = encryptionMethod;
        }

        protected IDocumentSerialiser DocumentSerialiser => this.documentSerialiser;

        protected IEncryptionMethod EncryptionMethod => this.encryptionMethod;

        /// <summary>
        /// Effectue l'opération.
        /// </summary>
        /// <returns>Une tâche représentant l'opération en cours.</returns>
        public abstract Task ExecuteAsync();
    }
}
