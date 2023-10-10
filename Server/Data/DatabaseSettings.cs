namespace AutomateDesign.Server.Data
{
    public class DatabaseSettings
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }

        public DatabaseSettings()
        {
            Server = "localhost";
            Database = String.Empty;
            UserId = String.Empty;
            Password = String.Empty;
        }
    }
}