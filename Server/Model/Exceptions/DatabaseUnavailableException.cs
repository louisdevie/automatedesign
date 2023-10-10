namespace AutomateDesign.Server.Model.Exceptions
{
    public class DatabaseUnavailableException : Exception
    {
        public DatabaseUnavailableException() : base() { }

        public DatabaseUnavailableException(string message) : base(message) { }
    }
}
