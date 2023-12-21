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
                // Vérifier les cases et modifier leur action si nécessaire
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