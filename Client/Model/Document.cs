using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model
{
    public class Document
    {
        #region Attributs
        private List<Objet> objets;
        #endregion

        #region Properties

        #endregion

        public Document() 
        { 
            objets = new List<Objet>();
        }


    }
}
