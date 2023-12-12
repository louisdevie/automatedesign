using AutomateDesign.Core.Documents;
using Humanizer;
using System.Globalization;

namespace AutomateDesign.Client.ViewModel.Documents
{
    /// <summary>
    /// Le modèle-vue pour les métadonnées d'un automate.
    /// </summary>
    public class DocumentHeaderViewModel : BaseViewModel
    {
        private DocumentHeader documentHeader;

        /// <inheritdoc cref="DocumentHeader.Name"/>
        public string Name
        {
            get => this.documentHeader.Name;
            set
            {
                this.documentHeader.Name = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <inheritdoc cref="DocumentHeader.TimeSinceLastModification"/>
        public string TimeSinceLastModification
            => this.documentHeader.TimeSinceLastModification.Humanize(culture: new CultureInfo("fr-FR"));

        /// <summary>
        /// Crée un nouveau modèle-vue pour l'en-tête d'un document.
        /// </summary>
        /// <param name="documentHeader">L'en-tête à présenter.</param>
        public DocumentHeaderViewModel(DocumentHeader documentHeader)
        {
            this.documentHeader = documentHeader;
        }

        /// <inheritdoc cref="DocumentHeader.UpdateLastModificationDate"/>
        public void UpdateLastModificationDate()
        {
            this.documentHeader.UpdateLastModificationDate();
            this.NotifyPropertyChanged(nameof(TimeSinceLastModification));
        }
    }
}