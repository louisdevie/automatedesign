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

        public IEnumerable<EnumEvent> Events => this.enumEvents;

        public IEnumerable<Transition> Transitions => this.transitions;

        #endregion

        public Document()
        {
            this.states = new List<State>();
            this.enumEvents = new List<EnumEvent>();
            this.transitions = new List<Transition>();
        }

        /// <summary>
        /// Creer un etat a partir des parametres fournit et le renvoie
        /// </summary>
        /// <param name="name">le nom de l'etat</param>
        /// <param name="kind">niveau de l'etat (normal, initial, final)</param>
        /// <returns>l'etat creer</returns>
        public State CreateState(string name, StateKind kind = StateKind.NORMAL)
        {
            State item = new(this, this.states.Count, name, kind);
            this.states.Add(item);
            return item;
        }

        /// <summary>
        /// Creer un EnumEvent a partir du nom fournit en parametre
        /// </summary>
        /// <param name="name">le nom du EnumEvent</param>
        /// <returns>l'EnumEvent creer</returns>
        public EnumEvent CreateEnumEvent(string name)
        {
            EnumEvent evt = new(this.enumEvents.Count, name);
            this.enumEvents.Add(evt);
            return evt;
        }

        /// <summary>
        /// Creer une transition a partir des donnees fournit en parametre
        /// </summary>
        /// <param name="from">debut de la transition</param>
        /// <param name="to">fin de la transition</param>
        /// <param name="triggeredBy">nom de la transition</param>
        /// <returns>la transition creer</returns>
        public Transition CreateTransition(State from, State to, IEvent triggeredBy)
        {
            Transition trans = new(this.transitions.Count, from, to, triggeredBy);
            this.transitions.Add(trans);
            return trans;
        }
    }
}
