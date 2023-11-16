namespace AutomateDesign.Core.Documents
{
    /// <summary>
    /// L'évènement « autres », qui représente tous les évènements non précisés.
    /// </summary>
    public class DefaultEvent : Event
    {
        public override int Order => 1_000_000_000; // traité en dernier

        public override string DisplayName => "*";
    }
}
