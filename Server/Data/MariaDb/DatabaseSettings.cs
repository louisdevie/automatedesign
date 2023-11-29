namespace AutomateDesign.Server.Data.MariaDb
{
    public class DatabaseSettings
    {
        #region Properties
        public string Server { get; set; }
        public string Database { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        #endregion

        public DatabaseSettings()
        {
            Server = "localhost";
            Database = string.Empty;
            UserId = string.Empty;
            Password = string.Empty;
        }
    }
}