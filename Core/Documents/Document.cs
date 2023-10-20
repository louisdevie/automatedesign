using System.Runtime.CompilerServices;

namespace AutomateDesign.Core.Documents
{
    public class Document
    {
        #region Attributs

        private List<State> states;
        private List<EnumEvent> enumEvents;
        private List<Transition> transitions;

        #endregion

        #region Properties

        public IEnumerable<State> States => this.states;

        public IEnumerable<IEvent> Events => this.enumEvents;

        public IEnumerable<Transition> Transitions => this.transitions;

        #endregion

        public Document()
        {
            this.states = new List<State>();
            this.enumEvents = new List<EnumEvent>();
            this.transitions = new List<Transition>();
        }

        public State CreateState(string name, StateKind kind = StateKind.NORMAL)
        {
            State item = new(this, this.states.Count, name, kind);
            this.states.Add(item);
            return item;
        }

        public EnumEvent CreateEnumEvent(string name)
        {
            EnumEvent evt = new(this.enumEvents.Count, name);
            this.enumEvents.Add(evt);
            return evt;
        }

        public Transition CreateTransition(State from, State to, IEvent triggeredBy)
        {
            Transition trans = new(this.transitions.Count, from, to, triggeredBy);
            this.transitions.Add(trans);
            return trans;
        }
    }
}
