using System.Collections.ObjectModel;

namespace AutomateDesign.Client.ViewModel.Documents
{
    public class DocumentCollectionViewModel : ObservableCollection<DocumentBaseViewModel>
    {
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