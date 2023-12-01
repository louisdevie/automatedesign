using AutomateDesign.Client.Model.Logic;
using AutomateDesign.Client.Model.Network;
using AutomateDesign.Core.Documents;

namespace AutomateDesign.Client.ViewModel.Documents
{
    public class ExistingDocumentViewModel : DocumentBaseViewModel
    {
        private bool loaded;
        private bool hasUnsavedChanges;
        private Document document;
        private DocumentHeaderViewModel header;
        private DocumentCollectionViewModel parentCollection;
        private DocumentsClient documentsClient;

        /// <summary>
        /// Si l'automate est chargé ou non.
        /// </summary>
        public bool Loaded
        {
            get => this.loaded;
            set
            {
                this.loaded = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Si l'automate a été modifié depuis le dernier chargement/enregistrement.
        /// </summary>
        public bool HasUnsavedChanges
        {
            get => this.hasUnsavedChanges;
            private set
            {
                this.hasUnsavedChanges = value;
                this.NotifyPropertyChanged();
            }
        }

        public override string Name => this.header.Name;

        public override string TimeSinceLastModification => this.header.TimeSinceLastModification;

        /// <summary>
        /// Crée un modèle-vue à partir d'un automate.
        /// </summary>
        /// <param name="document">L'automate associé.</param>
        /// <param name="parentCollection">La collection qui contiendra ce modèle-vue.</param>
        public ExistingDocumentViewModel(Document document, DocumentCollectionViewModel parentCollection)
        {
            this.document = document;
            this.header = new(this.document.Header);
            this.parentCollection = parentCollection;
            this.loaded = false;
            this.hasUnsavedChanges = false;
            this.documentsClient = new DocumentsClient();
        }

        /// <summary>
        /// Crée un modèle-vue à partir de métadonnées.
        /// </summary>
        /// <param name="headerOnly">Les métadonnées de l'automate.</param>
        /// <param name="parentCollection">La collection qui contiendra ce modèle-vue.</param>
        public ExistingDocumentViewModel(DocumentHeader headerOnly, DocumentCollectionViewModel parentCollection)
        : this(new Document(headerOnly), parentCollection) { }

        /// <summary>
        /// Crée un modèle-vue pour un document vierge.
        /// </summary>
        /// <param name="parentCollection">La collection qui contiendra ce modèle-vue.</param>
        /// <returns></returns>
        public static ExistingDocumentViewModel CreateEmptyDocument(DocumentCollectionViewModel parentCollection)
        {
            return new(new Document(), parentCollection);
        }

        public void Load()
        {

        }

        public void Save()
        {

        }

        public void Unload()
        {

        }

        public void Delete() 
        {
            this.documentsClient.DeleteDocument(this.parentCollection.Session, document.Header.Id);
        }
    }
}
