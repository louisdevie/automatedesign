using AutomateDesign.Client.Model.Cryptography;
using AutomateDesign.Client.Model.Serialisation;
using AutomateDesign.Core.Documents;
using Grpc.Core;

namespace AutomateDesign.Client.Model.Pipelines
{
    /// <summary>
    /// La classe de base pour les opérations de réception et d'envoi.
    /// </summary>
    public abstract class Pipeline
    {
        private IDocumentSerialiser documentSerialiser;
        private IEncryptionMethod encryptionMethod;
        private IPipelineProgress? progress;

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
        /// <returns>Une tâche représentant l'opération qui termine avec <see langword="true"/> si l'opération à réussi.</returns>
        public async Task<bool> ExecuteAsync()
        {
            try
            {
                await this.DoExecuteAsync();

                this.progress?.Done();
                return true;
            }
            catch (Exception ex)
            {
                this.progress?.Failed($"Le document n'a pas pu être envoyé suite à une erreur innatendue. ({ex.GetType()}: {ex.Message})");
                return false;
            }
        }

        /// <summary>
        /// Implémentation interne de l'opération.
        /// </summary>
        /// <returns>Une tâche qui représente l'opération. Elle doit terminer quand tout est fini.</returns>
        protected abstract Task DoExecuteAsync();

        /// <summary>
        /// Indique que le progrès de l'opération doit être rapporté à un objet.
        /// </summary>
        /// <param name="progress">L'objet à qui rapporter l'avancement.</param>
        public void ReportProgressTo(IPipelineProgress? progress)
        {
            this.progress = progress;
        }
    }
}
