using AutomateDesign.Client.DependencyInjection;
using AutomateDesign.Client.Model.Logic;
using AutomateDesign.Client.Model.Network;
using AutomateDesign.Core.Documents;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace AutomateDesign.Client.ViewModel.Documents
{
    /// <summary>
    /// Représente l'ensemble des automates d'un utilisateur.
    /// </summary>
    public class DocumentCollectionViewModel : ObservableCollection<DocumentBaseViewModel>
    {
        private Session? session;
        private IDocumentsClient client;

        /// <summary>
        /// Les informations sur le propriétaire des documents.
        /// </summary>
        public Session Session
        {
            get
            {
                if (this.session == null)
                {
                    throw new InvalidOperationException("La DocumentCollectionViewModel n'a pas encore été configurée.");
                }
                return this.session;
            }
            set
            {
                this.session = value;
            }
        }

        /// <summary>
        /// Crée un nouveau modèle-vue représentant une collection de documents.
        /// </summary>
        public DocumentCollectionViewModel()
        {
            this.client = DependencyContainer.Current.GetImplementation<IDocumentsClient>();
        }

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
            ExistingDocumentViewModel newDocument = new(this);
            this.Add(newDocument);
            return newDocument;
        }
    }
}