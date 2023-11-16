using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Core.Documents
{
    /// <summary>
    /// Une transition de l'automate.
    /// </summary>
    public class Transition
    {
        private int id;
        private State start;
        private State end;
        private Event triggeredBy;

        /// <summary>
        /// L'identifiant de la transition.
        /// </summary>
        public int Id => this.id;

        /// <summary>
        /// L'état de départ.
        /// </summary>
        public State Start { get => this.start; set => this.start = value; }

        /// <summary>
        /// L'état d'arrivée.
        /// </summary>
        public State End { get => this.end; set => this.end = value; }

        /// <summary>
        /// L'évènement qui déclenche cette transition.
        /// </summary>
        public Event TriggeredBy { get => this.triggeredBy; set => this.triggeredBy = value; }

        /// <summary>
        /// Crée une nouvelle transition.
        /// </summary>
        /// <param name="id">L'identifiant de la transition.</param>
        /// <param name="start">L'état de départ.</param>
        /// <param name="end">L'état d'arrivée.</param>
        /// <param name="triggeredBy">L'évènement qui déclenche cette transition.</param>
        public Transition(int id, State start, State end, Event triggeredBy)
        {
            this.id = id;
            this.start = start;
            this.end = end;
            this.triggeredBy = triggeredBy;
        }
    }
}
