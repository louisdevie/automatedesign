using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model.Logic.Exceptions
{
    /// <summary>
    /// Exception levée quand l'utilisateur essaie d'effectuer une actions avec une saisie invalide.
    /// </summary>
    public class InvalidInputsException : Exception
    {
        public string[] invalidInputs;

        /// <summary>
        /// Crée une nouvelle <see cref="InvalidInputsException"/> avec un message d'erreur par défaut.
        /// </summary>
        /// <param name="inputNames">Les champs qui posent problème.</param>
        public InvalidInputsException(params string[] inputNames) : base("Saisie invalide.")
        {
            this.invalidInputs = inputNames;
        }

        /// <summary>
        /// Crée une nouvelle <see cref="InvalidInputsException"/> avec un message d'erreur particulier.
        /// </summary>
        /// <param name="message">Un message décrivant le problème.</param>
        /// <param name="inputNames">Les champs qui posent problème.</param>
        public InvalidInputsException(string message, params string[] inputNames) : base(message)
        {
            this.invalidInputs = inputNames;
        }

        /// <summary>
        /// Regarde si un champ donné à causé un problème à l'origine de cette exception.
        /// </summary>
        /// <param name="inputName">Le nom du champ.</param>
        /// <returns><see langword="true"/> si le champ est problématique, sinon <see langword="false"/>.</returns>
        public bool IsInputInvalid(string inputName)
        {
            return this.invalidInputs.Contains(inputName);
        }
    }
}
