using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NomAutomate.Automate
{
    public abstract class Etat
    {
        public abstract string Nom { get; }

        /// <summary>
        /// La transition vers un nouvel Etat lié à un évènement
        /// </summary>
        /// <param name="e">l'évènement</param>
        /// <returns></returns>
        public abstract Etat Transition(Enum e);

        /// <summary>
        /// Déclenche l'action lié à un évènement
        /// </summary>
        /// <param name="e">l'évènement</param>
        public abstract void Action(Enum e);
    }
}