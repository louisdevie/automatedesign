using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NomAutomate.Etats
{
    public class EtatX : Etat
    {
        public Etat Transition(Enum e)
        {
            Etats prochainEtat;
            switch (e)
            {
                // Supprimer les cases innutiles et modifier leur action
                //cases

                default:
                    prochainEtat = this;
            }
            return prochainEtat;
        }

        public void Action(Enum e)
        {
            throw new NotImplementedException();
        }
    }
}