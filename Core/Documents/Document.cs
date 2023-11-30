using System.Runtime.CompilerServices;

namespace AutomateDesign.Core.Documents
{
    /// <summary>
    /// Représente un automate.
    /// </summary>
    public class Document
    {
        private DocumentHeader header;
        private List<State> states;
        private List<EnumEvent> enumEvents;
        private List<Transition> transitions;

        private State? initialState;

        /// <summary>
        /// Les métadonnées de l'automate.
        /// </summary>
        public DocumentHeader Header => this.header;

        /// <summary>
        /// Les différents états.
        /// </summary>
        public IEnumerable<State> States => this.states;

        /// <summary>
        /// Les différents évènements.
        /// </summary>
        public IEnumerable<EnumEvent> Events => this.enumEvents;

        /// <summary>
        /// Les transitions entre les états.
        /// </summary>
        public IEnumerable<Transition> Transitions => this.transitions;

        /// <summary>
        /// Crée un automate vierge.
        /// </summary>
        public Document()
        {
            this.header = new DocumentHeader("");
            this.states = new List<State>();
            this.enumEvents = new List<EnumEvent>();
            this.transitions = new List<Transition>();
        }

        /// <summary>
        /// Crée un automate vide à partir des métadonnées fournies.
        /// </summary>
        /// <param name="header"></param>
        public Document(DocumentHeader header)
        {
            this.header = header;
            this.states = new List<State>();
            this.enumEvents = new List<EnumEvent>();
            this.transitions = new List<Transition>();
        }

        /// <summary>
        /// Ajoutes des états existants à l'automate.
        /// </summary>
        /// <param name="states">Les état à rajouter.</param>
        public void AddStates(IEnumerable<State> states) => this.states.AddRange(states);

        /// <summary>
        /// Ajoutes des états existants à l'automate.
        /// </summary>
        /// <param name="states">Les état à rajouter.</param>
        public void AddEvents(IEnumerable<EnumEvent> events) => this.enumEvents.AddRange(events);

        /// <summary>
        /// Ajoutes des états existants à l'automate.
        /// </summary>
        /// <param name="states">Les état à rajouter.</param>
        public void AddTransitions(IEnumerable<Transition> transitions) => this.transitions.AddRange(transitions);

        /// <summary>
        /// Ajoute un état à l'automate.
        /// </summary>
        /// <param name="name">Le nom de l'état.</param>
        /// <param name="kind">Le type d'état.</param>
        /// <returns>Le nouvel état.</returns>
        public State CreateState(string name, StateKind kind = StateKind.Normal)
        {
            State item = new(this, this.states.Count, name, kind);
            this.states.Add(item);
            return item;
        }

        /// <summary>
        /// Gère un changement d'état initial.
        /// </summary>
        /// <param name="state"></param>
        public void SetInitialState(State state)
        {
            if (this.initialState is not null) this.initialState.Kind = StateKind.Normal;
            this.initialState = state;
        }

        /// <summary>
        /// Ajoute un évènement à l'automate.
        /// </summary>
        /// <param name="name">Le nom de l'évènement.</param>
        /// <returns>Le nouvel évènement.</returns>
        public EnumEvent CreateEnumEvent(string name)
        {
            EnumEvent evt = new(this.enumEvents.Count, name);
            this.enumEvents.Add(evt);
            return evt;
        }

        /// <summary>
        /// Ajoute une transition à l'automate.
        /// </summary>
        /// <param name="from">L'état de départ.</param>
        /// <param name="to">L'état d'arrivée.</param>
        /// <param name="triggeredBy">L'évènement qui déclenche cette transition.</param>
        /// <returns>La nouvelle transition.</returns>
        public Transition CreateTransition(State from, State to, Event triggeredBy)
        {
            Transition trans = new(this.transitions.Count, from, to, triggeredBy);
            this.transitions.Add(trans);
            return trans;
        }

        /// <summary>
        /// Recherche un état par son identifiant dans l'automate.
        /// </summary>
        /// <param name="id">L'identifiant recherché.</param>
        /// <returns>L'état trouvé ou <see langword="null"/>.</returns>
        public State? FindState(int id) => this.States.FirstOrDefault(state => state.Id == id);

        /// <summary>
        /// Recherche un évènement par son identifiant dans l'automate.
        /// </summary>
        /// <param name="id">L'identifiant recherché.</param>
        /// <returns>L'évèenemnt trouvé ou <see langword="null"/>.</returns>
        public EnumEvent? FindEnumEvent(int id) => this.Events.FirstOrDefault(evt => evt.Id == id);
    }
}
