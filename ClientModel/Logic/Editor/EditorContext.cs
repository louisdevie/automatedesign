using AutomateDesign.Client.Model.Logic.Editor.States;
using AutomateDesign.Core.Documents;

namespace AutomateDesign.Client.Model.Logic.Editor
{
    /// <summary>
    /// Enveloppe un automate et gére les modifications qui lui sont apportées.
    /// </summary>
    public class EditorContext
    {
        #region State

        private EditorState state;

        private EditorState State
        {
            get => this.state;
            set
            {
                if (value != this.state)
                {
                    this.state = value;
                    this.EditorStateChanged?.Invoke(this.state);
                }
            }
        }

        public delegate void EditorStateChangedEventHandler(EditorState state);

        /// <summary>
        /// Déclenché quand l'état de l'éditeur change.
        /// </summary>
        public event EditorStateChangedEventHandler? EditorStateChanged;

        /// <summary>
        /// Gère un évènement d'édition.
        /// </summary>
        /// <param name="e"></param>
        public void HandleEvent(EditorEvent e)
        {
            this.State.Action(e, this);
            this.State = this.State.Next(e);
        }

        #endregion

        #region Observable

        private List<IModificationsObserver> observers;

        /// <summary>
        /// Commence à observer les modifications apportées à l'automate.
        /// </summary>
        /// <param name="observer">L'objet qui va observer les modifications.</param>
        public void AddModificationObserver(IModificationsObserver observer) => this.observers.Add(observer);

        /// <summary>
        /// Arrête d'observer les modifications apportées à l'automate.
        /// </summary>
        /// <param name="observer">L'objet qui observait les modifications.</param>
        public void RemoveModificationObserver(IModificationsObserver observer) => this.observers.Remove(observer);

        #endregion

        private Document document;
        private IEditorUI editorUI;
        private EditorMode mode;

        public Document Document => this.document;

        public IEditorUI EditorUI => this.editorUI;

        public EditorMode Mode { get => this.mode; set => this.mode = value; }

        /// <summary>
        /// Crée un nouveau contexte d'édition.
        /// </summary>
        /// <param name="document">L'automate à éditer.</param>
        /// <param name="editor">Un élément de l'IHM permettant de demander des informations à l'utilisateur.</param>
        public EditorContext(Document document, IEditorUI editor)
        {
            this.state = new ReadyState();
            this.document = document;
            this.editorUI = editor;
            this.observers = new();
            this.mode = EditorMode.Move;
        }

        /// <summary>
        /// Configure le contexte dans son état de départ.
        /// </summary>
        public void Initialize()
        {
            this.EditorStateChanged?.Invoke(this.state);
        }

        public void AddState()
        {
            if (this.editorUI.PromptForStateName(out string? name))
            {
                State state = this.document.CreateState(name);
                
            }
        }
    }
}

