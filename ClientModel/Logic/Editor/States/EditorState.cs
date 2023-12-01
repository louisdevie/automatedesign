using AutomateDesign.Client.Model.Logic.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model.Logic.Editor.States
{
    /// <summary>
    /// Un état dans lequel se trouve l'éditeur d'automates.
    /// </summary>
    public abstract class EditorState
    {
        /// <summary>
        /// Un message court décrivant l'état ou l'action attendue de la part de l'utilisateur.
        /// </summary>
        public abstract string StatusMessage { get; }

        /// <summary>
        /// Renvoie l'état suivant l'évènement <paramref name="evt"/>.
        /// </summary>
        /// <param name="evt">Un évènement arrivé.</param>
        /// <returns>Un état différent ou ce même état si rien ne change.</returns>
        public abstract EditorState Next(EditorEvent evt);

        /// <summary>
        /// Effectue la(les) action(s) nécessaires suite à l'évènement <paramref name="evt"/>
        /// </summary>
        /// <param name="evt">Un évènement arrivé.</param>
        /// <param name="ctx">Le contexte de l'éditeur au moment ou l'évènement est arrivé.</param>
        public abstract void Action(EditorEvent evt, EditorContext ctx);
    }
}
