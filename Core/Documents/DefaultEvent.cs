namespace AutomateDesign.Core.Documents
{
    /// <summary>
    /// L'évènement « autres », qui représente tous les évènements non précisés.
    /// </summary>
    public class DefaultEvent : IEvent
    {
        public int Order => 1_000_000_000; // traité en dernier

        public string DisplayName => "*";
    }
}
