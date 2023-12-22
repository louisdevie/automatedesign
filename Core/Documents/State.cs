namespace AutomateDesign.Core.Documents
{
    /// <summary>
    /// Un état de l'automate.
    /// </summary>
    public class State
    {
        private const float DEFAULT_SIZE = 100;
        
        private int id;
        private string name;
        private Document document;
        private StateKind kind;
        private Position position;

        /// <summary>
        /// L'identifiant de l'état.
        /// </summary>
        public int Id => this.id;

        /// <summary>
        /// Le nom de l'état.
        /// </summary>
        public string Name { get => this.name; set => this.name = value; }

        public Position Position { get => this.position; set => this.position = value; }

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
                if (this.kind == StateKind.Initial) this.document.SetInitialState(this);
            }
        }

        /// <summary>
        /// Crée un nouvel état.
        /// </summary>
        /// <param name="document">Le document contenant l'état.</param>
        /// <param name="id">L'identifiant de l'état.</param>
        /// <param name="name">Le nom de l'état.</param>
        /// <param name="kind">Le type d'état.</param>
        public State(Document document, int id, string name, Position position, StateKind kind = StateKind.Normal)
        {
            this.document = document;
            this.id = id;
            this.name = name;
            this.Kind = kind;
            this.position = position;
        }

        /// <summary>
        /// Crée une <see cref="Position"/> pour un état centré sur un point.
        /// </summary>
        /// <param name="x">L'abscisse voulue de l'état.</param>
        /// <param name="y">L'ordonnée voulue de l'état.</param>
        /// <returns>Une position avec la taille par défaut d'un état.</returns>
        public static Position CenteredAt(float x, float y)
        {
            return new Position(x - DEFAULT_SIZE / 2, y - DEFAULT_SIZE / 2, DEFAULT_SIZE, DEFAULT_SIZE);
        }
    }
}
