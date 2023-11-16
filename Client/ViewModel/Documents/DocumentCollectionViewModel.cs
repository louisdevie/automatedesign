using System.Collections.ObjectModel;

namespace AutomateDesign.Client.ViewModel.Documents
{
    public class DocumentCollectionViewModel : ObservableCollection<DocumentViewModel>
    {
        /// <summary>
        /// Recharge les automates.
        /// </summary>
        public void Reload()
        {
            this.Clear();
        }

        /// <summary>
        /// Crée un nouvel automate et l'ajoute à la collection.
        /// </summary>
        /// <returns>L'automate créé.</returns>
        public DocumentViewModel NewDocument()
        {
            DocumentViewModel newDocument = DocumentViewModel.CreateEmptyDocument(this);
            this.Add(newDocument);
            return newDocument;
        }
    }
}