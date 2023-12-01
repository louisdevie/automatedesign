using AutomateDesign.Client.DependencyInjection;
using AutomateDesign.Client.Model.Logic;
using AutomateDesign.Client.Model.Network;
using AutomateDesign.Core.Documents;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace AutomateDesign.Client.ViewModel.Documents
{
    public class DocumentCollectionViewModel : ObservableCollection<DocumentBaseViewModel>
    {
        private Session? session;
        private IDocumentsClient client;

        /// <summary>
        /// Les informations sur le propriétaire des documents.
        /// </summary>
        public Session Session => this.session ?? throw new InvalidOperationException("La DocumentCollectionViewModel n'a pas encore été configurée.");

        /// <summary>
        /// Crée un nouveau modèle-vue représentant une collection de documents.
        /// </summary>
        /// <param name="session">Les informations sur le propriétaire des documents.</param>
        public DocumentCollectionViewModel()
        {
            this.client = DependencyContainer.Current.GetImplementation<IDocumentsClient>();
        }

        public void UseSession(Session session) => this.session = session;

        /// <summary>
        /// Recharge les automates.
        /// </summary>
        public async Task Reload()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.Clear();
                this.Add(new NewDocumentViewModel());
            });

            var pipeline = this.client.GetAllHeaders(this.Session);
            pipeline.OnHeaderReceived += this.DocumentHeaderReceived;

            await pipeline.ExecuteAsync();
        }

        private void DocumentHeaderReceived(DocumentHeader header)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.Add(new ExistingDocumentViewModel(header, this));
            });
        }

        /// <summary>
        /// Crée un nouvel automate et l'ajoute à la collection.
        /// </summary>
        /// <returns>L'automate créé.</returns>
        public ExistingDocumentViewModel NewDocument()
        {
            ExistingDocumentViewModel newDocument = ExistingDocumentViewModel.CreateEmptyDocument(this);
            this.Add(newDocument);
            return newDocument;
        }
    }
}