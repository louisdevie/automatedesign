namespace AutomateDesign.Core.Documents
{
    public interface IEvent
    {
        public string DisplayName { get; }

        public int Order { get; }
    }
}
