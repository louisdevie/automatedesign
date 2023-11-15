namespace AutomateDesign.Core.Documents
{
    /// <summary>
    /// Structure d'un State de l'automate
    /// </summary>
    public class State
    {
        #region Attributs

        private int id;
        private string name;
        private Document document;
        public StateKind kind;

        #endregion

        #region Properties

        public int Id => this.id;

        public string Name { get => this.name; set => this.name = value; }

        public IEnumerable<Transition> TransitionsFrom => this.document.Transitions.Where(t => t.Start == this);

        public IEnumerable<Transition> TransitionsTo => this.document.Transitions.Where(t => t.End == this);

        public StateKind Kind { get => this.kind; set => this.kind = value; }

        #endregion

        public State(Document document, int id, string name, StateKind kind = StateKind.NORMAL)
        {
            this.document = document;
            this.id = id;
            this.name = name;
            this.kind = kind;
        }

        public State() { }
    }
}
