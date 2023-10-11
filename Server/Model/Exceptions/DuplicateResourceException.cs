namespace AutomateDesign.Server.Model.Exceptions
{
    public class DuplicateResourceException : Exception
    {
        public DuplicateResourceException() : base() { }

        public DuplicateResourceException(string message) : base(message) { }
    }
}
