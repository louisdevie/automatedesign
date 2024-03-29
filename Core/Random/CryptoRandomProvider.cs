﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Core.Random
{
    /// <summary>
    /// Générateur adapté à une utilisation cryptographique basé sur <see cref="RandomNumberGenerator"/>.
    /// </summary>
    public class CryptoRandomProvider : IRandomProvider
    {
        /// <summary>
        /// Crée un générateur de nombre aléatoire sécurisé pour la cryptographie.
        /// </summary>
        public CryptoRandomProvider() { }

        public uint FourDigitCode()
        {
            return (uint)RandomNumberGenerator.GetInt32(0, 10000);
        }

        public char Pick(string allowed)
        {
            return allowed[RandomNumberGenerator.GetInt32(allowed.Length)];
        }
    }
}
