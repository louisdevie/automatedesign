using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Core.Random
{
    public interface IRandomProvider
    {
        /// <summary>
        /// Génère un nombre aléatoire à 4 chiffres (en décimal).
        /// </summary>
        /// <returns>Le nombre généré.</returns>
        public uint FourDigitCode();

        /// <summary>
        /// Choisis un caractère aléatoire parmis une liste.
        /// </summary>
        /// <param name="allowed">La liste des caractères possibles.</param>
        /// <returns>Un caractère aléatoire.</returns>
        public char Pick(string allowed);
    }
}
