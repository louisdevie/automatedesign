using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model
{
    public class Event : Objet
    {
        #region Attributs
        private Transition trigered;
        #endregion

        #region Porperties
        public Transition Trigered { get => this.trigered; set => this.trigered = value; }
        #endregion
    }
}
