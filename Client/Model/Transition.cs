using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model
{
    /// <summary>
    /// Structure des transitions de l'automate
    /// </summary>
    public class Transition : Objet
    {
        #region Attributs
        private int id;
        private State start;
        private State end;
        #endregion

        #region Properties
        public int Id { get => this.id; }
        public State Start { get => this.start; set => this.start = value; }
        public State End { get => this.end; set => this.end = value; }
        #endregion

        public Transition(int id) 
        { 
            this.id = id;
        }
    }
}
