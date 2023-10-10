﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Core.Random
{
    internal interface IRandomProvider
    {
        /// <summary>
        /// Choisis un caractère aléatoire parmis une liste.
        /// </summary>
        /// <param name="allowed">La liste des caractères possibles.</param>
        /// <returns>Un caractère aléatoire.</returns>
        public char Pick(string allowed);
    }
}
