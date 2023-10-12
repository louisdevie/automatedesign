namespace AutomateDesign.Core.Exceptions
{
    public class DuplicateResourceException : Exception
    {
        public DuplicateResourceException() : base() { }

        public DuplicateResourceException(string message) : base(message) { }
    }
}
