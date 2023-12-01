using AutomateDesign.Client.Model.Logic;
using System.Collections.ObjectModel;

namespace AutomateDesign.Client.ViewModel.Documents
{
    public class DocumentCollectionViewModel : ObservableCollection<DocumentBaseViewModel>
    {
        private Session session;

        /// <summary>
        /// Les informations sur le propriétaire des documents.
        /// </summary>
        public Session Session => this.session;

        /// <summary>
        /// Crée un nouveau modèle-vue représentant une collection de documents.
        /// </summary>
        /// <param name="session">Les informations sur le propriétaire des documents.</param>
        public DocumentCollectionViewModel(Session session)
        {
            this.session = session;
        }

        /// <summary>
        /// Recharge les automates.
        /// </summary>
        public void Reload()
        {
            this.Clear();
            this.Add(new NewDocumentViewModel());
        }

        /// <summary>
        /// Crée un nouvel automate et l'ajoute à la collection.
        /// </summary>
        /// <returns>L'automate créé.</returns>
        public DocumentBaseViewModel NewDocument()
        {
            DocumentBaseViewModel newDocument = ExistingDocumentViewModel.CreateEmptyDocument(this);
            this.Add(newDocument);
            return newDocument;
        }
    }
}