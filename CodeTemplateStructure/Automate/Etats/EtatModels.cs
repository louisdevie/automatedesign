using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NomAutomate.Etats
{
    public class EtatX : Etat
    {
        public EtatX Transition(Enum e)
        {
            switch (e)
            {
                // Supprimer les cases innutiles et modifier leur action
                //cases

                default:
                    throw new UnexpectedEventException();
            }
        }

        public void Action(Enum e)
        {
            throw new NotImplementedException();
        }
    }
}