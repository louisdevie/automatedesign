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
        public void AddModificationObserver(IModificationsObserver observer)
        {
            this.observers.Add(observer);
            observer.OnSubjectChanged(this.document);
        }

        /// <summary>
        /// Arrête d'observer les modifications apportées à l'automate.
        /// </summary>
        /// <param name="observer">L'objet qui observait les modifications.</param>
        public void RemoveModificationObserver(IModificationsObserver observer)
        {
            this.observers.Remove(observer);
            observer.OnSubjectChanged(null);
        }

        private void NotifyStateAdded(State state)
        {
            foreach (var observer in this.observers) observer.OnStateAdded(state);
        }

        private void NotifyTransitionAdded(Transition transition)
        {
            foreach (var observer in this.observers) observer.OnTransitionAdded(transition);
        }

        private void NotifyEnumEventAdded(EnumEvent evt)
        {
            foreach (var observer in this.observers) observer.OnEnumEventAdded(evt);
        }

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

        /// <summary>
        /// Finalise l'ajout d'un état.
        /// </summary>
        /// <param name="initialPosition">La position de l'état dans le diagramme.</param>
        /// <returns>L'état nouvellement créé si l'utilisateur à confirmé l'opération.</returns>
        public State? AddState(Position? initialPosition)
        {
            if (this.editorUI.PromptForStateName(out string? name))
            {
                State state = this.document.CreateState(name, initialPosition ?? default);
                this.NotifyStateAdded(state);
                return state;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Finalise l'ajout d'une transition.
        /// </summary>
        /// <param name="start">L'état de départ de la transition.</param>
        /// <param name="end">L'état d'arrivée de la transition.</param>
        /// <returns>La transition nouvellement crée si l'utilisateur à confirmé l'opération.</returns>
        internal Transition? AddTransition(State start, State end)
        {
            if (this.editorUI.PromptForEvent(out IEvent? evt))
            {
                Transition transition = this.document.CreateTransition(start, end, evt);
                this.NotifyTransitionAdded(transition);
                return transition;
            }
            else
            {
                return null;
            }
        }

        public EnumEvent AddEnumEvent(string name)
        {
            EnumEvent evt = this.document.CreateEnumEvent(name);
            this.NotifyEnumEventAdded(evt);
            return evt;
        }
    }
}

