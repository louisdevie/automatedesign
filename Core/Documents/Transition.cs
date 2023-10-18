using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Core.Documents
{
    /// <summary>
    /// Structure des transitions de l'automate
    /// </summary>
    public class Transition
    {
        #region Attributs

        private int id;
        private State start;
        private State end;
        private IEvent triggeredBy;

        #endregion

        #region Properties

        public int Id => this.id;

        public State Start { get => this.start; set => this.start = value; }

        public State End { get => this.end; set => this.end = value; }

        public IEvent TriggeredBy { get => this.triggeredBy; set => this.triggeredBy = value; }

        #endregion

        public Transition(int id, State start, State end, IEvent triggeredBy)
        {
            this.id = id;
            this.start = start;
            this.end = end;
            this.triggeredBy = triggeredBy;
        }
    }
}
