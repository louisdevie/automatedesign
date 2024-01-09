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
using AutomateDesign.Client.Model.Export;
using AutomateDesign.Client.Model.Export.CSharp;
using AutomateDesign.Client.Model.Export.Latex;

namespace AutomateDesign.Client.ViewModel.Documents
{
    /// <summary>
    /// Un <see cref="DocumentBaseViewModel"/> qui représente un document existant.
    /// </summary>
    public class ExistingDocumentViewModel : DocumentBaseViewModel, IModificationsObserver
    {
        // propriétés internes
        private bool loaded;
        private bool hasUnsavedChanges;
        
        // objets métier
        private Document document;
        private DocumentsClient documentsClient;
        private Exporter? exporters;
        
        // autres modèles-vue
        private DocumentHeaderViewModel header;
        private DocumentCollectionViewModel parentCollection;
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
            this.loaded = false;
            this.hasUnsavedChanges = false;
            
            this.document = document;
            this.documentsClient = new DocumentsClient();
            this.exporters += new LatexExporter();
            this.exporters += new CSharpExporter();

            this.header = new(this.document.Header);
            this.header.PropertyChanged += this.HeaderPropertyChanged;
            this.parentCollection = parentCollection;
            this.states = new(this.document.States.Select(s => new StateViewModel(s)));
            this.transitions = new(this.document.Transitions.Select(t => new TransitionViewModel(t, this)));
            this.events = new(this.document.Events.Select(e => new EventViewModel(e)).Append(new EventViewModel(new DefaultEvent())));
            
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

        /// <summary>
        /// Exporte ce document dans un format et un fichier spécifié.
        /// </summary>
        /// <param name="format">Le format à utiliser.</param>
        /// <param name="path">Le chemin du fichier dans lequel écrire.</param>
        public void Export(ExportFormat format, string path)
        {
             this.exporters.Export(this.document, format, path);
        }

        #region Implémentation de IModificationObserver

        public void OnTransitionAdded(Transition transition)
        {
            this.transitions.Add(new TransitionViewModel(transition, this));
        }

        public void OnStateAdded(State state)
        {
            this.states.Add(new StateViewModel(state));
        }

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

        #endregion
    }
}
