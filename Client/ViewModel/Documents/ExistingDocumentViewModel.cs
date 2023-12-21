using AutomateDesign.Client.Model.Logic.Editor;
using AutomateDesign.Client.Model.Network;
using AutomateDesign.Client.Model.Pipelines;
using AutomateDesign.Core.Documents;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AutomateDesign.Client.ViewModel.Documents
{
    /// <summary>
    /// Un <see cref="DocumentBaseViewModel"/> qui représente un document existant.
    /// </summary>
    public class ExistingDocumentViewModel : DocumentBaseViewModel, IModificationsObserver
    {
        private bool loaded;
        private bool hasUnsavedChanges;
        private Document document;
        private DocumentHeaderViewModel header;
        private DocumentCollectionViewModel parentCollection;
        private DocumentsClient documentsClient;
        private ObservableCollection<StateViewModel> states;
        private ObservableCollection<TransitionViewModel> transitions;
        private ObservableCollection<EventViewModel> events;
        private OnceAsyncCommand deleteCommand;

        /// <summary>
        /// L'en-tête du document.
        /// </summary>
        public DocumentHeaderViewModel Header => this.header;

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
        public bool HasUnsavedChanges => this.hasUnsavedChanges;

        public override string Name => this.header.Name;

        public override string TimeSinceLastModification => this.header.TimeSinceLastModification;

        /// <inheritdoc cref="Document.States"/>
        public ObservableCollection<StateViewModel> States => this.states;

        /// <inheritdoc cref="Document.Transitions"/>
        public ObservableCollection<TransitionViewModel> Transitions => this.transitions;

        /// <inheritdoc cref="Document.Events"/>
        public ObservableCollection<EventViewModel> Events => this.events;

        /// <summary>
        /// La classe métier présentée par ce modèle-vue.
        /// </summary>
        public Document Document => this.document;

        /// <summary>
        /// Une commande qui supprimme ce document quand invoquée, ce qui permet
        /// d'utiliser un binding avec la méthode <see cref="Delete"/>.
        /// </summary>
        public ICommand DeleteCommand => this.deleteCommand;

        /// <summary>
        /// Crée un modèle-vue à partir d'un automate. Le document résultant est considéré comme non chargé et non modifié.
        /// </summary>
        /// <param name="document">L'automate associé.</param>
        /// <param name="parentCollection">La collection qui contiendra ce modèle-vue.</param>
        public ExistingDocumentViewModel(Document document, DocumentCollectionViewModel parentCollection)
        {
            this.document = document;
            this.parentCollection = parentCollection;

            this.header = new DocumentHeaderViewModel(this.document.Header);
            this.header.PropertyChanged += this.HeaderPropertyChanged;

            this.loaded = false;
            this.hasUnsavedChanges = false;

            this.documentsClient = new DocumentsClient();

            this.states = new ObservableCollection<StateViewModel>();
            this.transitions = new ObservableCollection<TransitionViewModel>();
            this.events = new ObservableCollection<EventViewModel>();

            this.deleteCommand = new OnceAsyncCommand(this.Delete, canRetry: true);
        }

        /// <summary>
        /// Crée un modèle-vue pour un nouvel automate. Le document résultant est considéré comme chargé et non modifié.
        /// </summary>
        /// <param name="parentCollection">La collection qui contiendra ce modèle-vue.</param>
        public ExistingDocumentViewModel(DocumentCollectionViewModel parentCollection)
            : this(new Document(), parentCollection)
        {
            this.loaded = true;
        }

        /// <summary>
        /// Crée un modèle-vue à partir de métadonnées. Le document résultant est considéré comme non chargé et non modifié.
        /// </summary>
        /// <param name="headerOnly">Les métadonnées de l'automate.</param>
        /// <param name="parentCollection">La collection qui contiendra ce modèle-vue.</param>
        public ExistingDocumentViewModel(DocumentHeader headerOnly, DocumentCollectionViewModel parentCollection)
            : this(new Document(headerOnly), parentCollection)
        {
        }

        private void HeaderPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
            case nameof(DocumentHeaderViewModel.Name):
                this.NotifyPropertyChanged(nameof(Name));
                break;

            case nameof(DocumentHeaderViewModel.TimeSinceLastModification):
                this.NotifyPropertyChanged(nameof(TimeSinceLastModification));
                break;
            }
        }

        /// <summary>
        /// Charge l'automate contenu dans le document.
        /// </summary>
        public async Task Load(IPipelineProgress? progress = null)
        {
            var pipeline = this.documentsClient.GetDocument(this.parentCollection.Session, this.document.Id);

            pipeline.ReportProgressTo(progress);

            if (await pipeline.ExecuteAsync())
            {
                this.document = await pipeline.GetDocument();

                foreach (State state in this.document.States) this.states.Add(new StateViewModel(state));
                foreach (EnumEvent evt in this.document.Events) this.events.Add(new EventViewModel(evt));
                foreach (Transition transition in this.document.Transitions) this.transitions.Add(new TransitionViewModel(transition, this));
                
                this.loaded = true;
            }
        }

        /// <summary>
        /// Enregistre l'automate et l'en-tête.
        /// </summary>
        /// <param name="progress">Un objet à qui rapporter l'avancement de l'opération.</param>
        public async Task Save(IPipelineProgress? progress = null)
        {
            var pipeline = this.documentsClient.SaveDocument(this.parentCollection.Session, this.document);

            pipeline.ReportProgressTo(progress);

            if (await pipeline.ExecuteAsync())
            {
                this.document.Header.Id = await pipeline.GetNewDocumentId();
            }
        }

        /// <summary>
        /// Décharge l'automate contenu dans le document.
        /// </summary>
        public void Unload()
        {
            this.document.Clear();
            this.states.Clear();
            this.loaded = false;
        }

        /// <summary>
        /// Supprime ce document et l'enlève de la collection.
        /// </summary>
        public async Task Delete()
        {
            var result = MessageBox.Show(
                $"Êtes-vous sûr de vouloir supprimer « {this.Name} » ?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result != MessageBoxResult.Yes) throw new OperationCanceledException();

            await this.documentsClient.DeleteDocument(this.parentCollection.Session, this.document.Header.Id);
            Application.Current.Dispatcher.Invoke(() => { this.parentCollection.Remove(this); });
        }

        #region Implémentation de IModificationObserver
        /// <summary>
        /// Ajoute une transitionViewModel à partir de la transition fournit dans notre liste de transitions
        /// </summary>
        /// <param name="transition">la transition à ajouter</param>

        public void OnTransitionAdded(Transition transition)
        {
            this.transitions.Add(new TransitionViewModel(transition, this));
        }

        /// <summary>
        /// Ajoute un StateViewModel à partir de l'état fournit dans notre liste d'états
        /// </summary>
        /// <param name="state">l'état à ajouter</param>
        public void OnStateAdded(State state)
        {
            this.states.Add(new StateViewModel(state));
        }

        /// <summary>
        /// Ajoute un EventViewModel à partir de l'enumEvent fournit dans notre liste d'évènements
        /// </summary>
        /// <param name="enumEvent">l'évènement à ajouter</param>
        public void OnEnumEventAdded(EnumEvent enumEvent)
        {
            this.events.Add(new EventViewModel(enumEvent));
        }

        public void OnSubjectChanged(Document? document)
        {
            if (document is { } && document != this.document)
            {
                // on essaie de changer le document
                throw new InvalidOperationException(
                    "Un ExistingDocumentViewModel ne peut pas observer d'autre document que le sien.");
            }
        }

        /// <summary>
        /// Supprime une transitionViewModel à partir de la transition fournit de notre liste de transitions
        /// </summary>
        /// <param name="transition">la transition à supprimer</param>

        public void OnTransitionDeleted(Transition transition)
        {
            foreach (var item in this.transitions)
            {
                if (item.Equals(transition))
                {
                    this.transitions.Remove(item);
                }
            }
            this.transitions.Add(new TransitionViewModel(transition, this));
        }

        /// <summary>
        /// Supprime un StateViewModel à partir de l'état fournit de notre liste d'états
        /// </summary>
        /// <param name="state">l'état à supprimer</param>
        public void OnStateDeleted(State state)
        {
            foreach (var item in this.states)
            {
                if (item.Equals(state))
                {
                    this.states.Remove(item);
                }
            }
        }

        /// <summary>
        /// Supprime un EventViewModel à partir de l'enumEvent fournit de notre liste d'évènements
        /// </summary>
        /// <param name="enumEvent">l'évènement à supprimer</param>
        public void OnEnumEventDeleted(EnumEvent enumEvent)
        {
            foreach (var item in this.events)
            {
                if (item.Equals(enumEvent))
                {
                    this.events.Remove(item);
                }
            }
        }
        #endregion
    }
}