using MySql.Data.MySqlClient;

namespace AutomateDesign.Server.Data
{
    public abstract class DatabaseConnection: IDisposable
    {
        private MySqlConnection connection;

        public MySqlConnection Connection { get => connection; }

        public DatabaseConnection(ConfigurationService configurationService)
        {
            var databaseSettings = configurationService.GetDatabaseSetting();

            string connectionString = $"SERVER={databaseSettings.Server};DATABASE={databaseSettings.Database};UID={databaseSettings.UserId};PASSWORD={databaseSettings.Password};";
            string ConnectionString = connectionString;

            connection = new MySqlConnection(ConnectionString);
        }

        /// <summary>
        /// Ouvrir la connexion à la base de données
        /// </summary>
        /// <returns></returns>
        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                // Gérer les erreurs de connexion ici
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Impossible de se connecter au serveur.");
                        break;
                    case 1045:
                        Console.WriteLine("Nom d'utilisateur ou mot de passe invalide.");
                        break;
                    default:
                        Console.WriteLine("Erreur de connexion à la base de données.");
                        break;
                }
                return false;
            }
        }

        /// <summary>
        /// Fermer la connexion à la base de données
        /// </summary>
        /// <returns></returns>
        public bool CloseConnection()
        {
            try
            {
                connection.Close(); 
                return true;
            }
            catch (MySqlException ex)
            {
                // Gérer les erreurs de fermeture de connexion ici
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Exécute une requête SQL
        /// </summary>
        /// <param name="query"></param>
        public void ExecuteQuery(string query)
        {
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); ;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (connection != null)
                {
                    connection.Dispose();
                    connection = null;
                }
            }
        }
    }
}

