using AutomateDesign.Core.Documents;

namespace AutomateDesign.Client.Model.Logic.Editor
{
    /// <summary>
    /// Observe les modifications apportées à un automate.
    /// </summary>
    public interface IModificationsObserver
    {
        /// <summary>
        /// Déclenchée quand un état est ajouté à l'automate.
        /// </summary>
        /// <param name="state">Le nouvel état.</param>
        void OnStateAdded(State state);

        /// <summary>
        /// Déclenchée quand une transition est ajoutée à l'automate.
        /// </summary>
        /// <param name="transition">La nouvelle transition.</param>
        void OnTransitionAdded(Transition transition);

        /// <summary>
        /// Déclenchée quand un nouvel évènement est défini par l'utilisateur.
        /// </summary>
        /// <param name="evt">Le nouvel évènement.</param>
        void OnEnumEventAdded(EnumEvent evt);

        /// <summary>
        /// Déclenchée quand le document observé change.
        /// </summary>
        /// <param name="document">Le nouveau document observé ou <see langword="null"/> si plus aucun document n'est observé.</param>
        void OnSubjectChanged(Document? document);
    }
}
