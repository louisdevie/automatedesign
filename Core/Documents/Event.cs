namespace AutomateDesign.Core.Documents
{
    /// <summary>
    /// Un évènement de l'automate.
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// Le nom de l'évènement.
        /// </summary>
        public abstract string DisplayName { get; }

        /// <summary>
        /// La précédence de l'évènement (les évènements avec les plus petites valeurs doivent être traités en premier).
        /// </summary>
        public abstract int Order { get; }
    }
}
