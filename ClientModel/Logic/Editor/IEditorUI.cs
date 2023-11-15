using AutomateDesign.Core.Documents;
using System.Diagnostics.CodeAnalysis;

namespace AutomateDesign.Client.Model.Logic.Editor
{
    /// <summary>
    /// Peut demander des informations à l'utilisateur.
    /// </summary>
    public interface IEditorUI
    {
        /// <summary>
        /// Demande un nom pour un état.
        /// </summary>
        /// <param name="name">Le nom choisi par l'utilisateur, ou <see langword="null"/> si l'utilisateur à refusé.</param>
        /// <returns><see langword="true"/> si l'utilisateur a saisi un nom et confirmé, sinon <see langword="false"/>.</returns>
        bool PromptForStateName([NotNullWhen(true)] out string? name);

        /// <summary>
        /// Demande de choisir un évènement.
        /// </summary>
        /// <param name="evt">L'évènement choisi par l'utilisateur, ou <see langword="null"/> si l'utilisateur à refusé.</param>
        /// <returns><see langword="true"/> si l'utilisateur choisi un évènement et confirmé, sinon <see langword="false"/>.</returns>
        bool PromptForEvent([NotNullWhen(true)] out IEvent? evt);
    }
}
