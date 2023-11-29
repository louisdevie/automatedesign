namespace AutomateDesign.Core.Documents
{
    /// <summary>
    /// Un état de l'automate.
    /// </summary>
    public class State
    {
        private int id;
        private string name;
        private Document document;
        public StateKind kind;

        /// <summary>
        /// L'identifiant de l'état.
        /// </summary>
        public int Id => this.id;

        /// <summary>
        /// Le nom de l'état.
        /// </summary>
        public string Name { get => this.name; set => this.name = value; }

        /// <summary>
        /// Les transitions qui partent de cet état.
        /// </summary>
        public IEnumerable<Transition> TransitionsFrom => this.document.Transitions.Where(t => t.Start == this);

        /// <summary>
        /// Les transitions qui arrivent à cet état.
        /// </summary>
        public IEnumerable<Transition> TransitionsTo => this.document.Transitions.Where(t => t.End == this);

        /// <summary>
        /// Le type d'état (s'il est final ou initial)
        /// </summary>
        public StateKind Kind
        {
            get => this.kind;
            set
            {
                this.kind = value;
                this.document.SetInitialState(this);
            }
        }

        /// <summary>
        /// Crée un nouvel état.
        /// </summary>
        /// <param name="document">Le document contenant l'état.</param>
        /// <param name="id">L'identifiant de l'état.</param>
        /// <param name="name">Le nom de l'état.</param>
        /// <param name="kind">Le type d'état.</param>
        public State(Document document, int id, string name, StateKind kind = StateKind.Normal)
        {
            this.document = document;
            this.id = id;
            this.name = name;
            this.kind = kind;
            if (this.kind == StateKind.Initial) this.document.SetInitialState(this);
        }

        public State() { }
    }
}
