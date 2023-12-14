using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NomAutomate.Automate.Etats
{
    public class EtatX : Etat
    {
        public EtatX Transition(Enum e)
        {
            // Ici la transition à généré à partir de l'automate
        }

        public void Action(Enum e)
        {
            throw new NotImplementedException();
        }
    }
}