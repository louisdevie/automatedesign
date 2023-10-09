using MySql.Data.MySqlClient;

namespace AutomateDesign.Server.Data
{
    public abstract class DatabaseConnection
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        public DatabaseConnection()
        {
            Initialize();
        }

        /// <summary>
        /// Initialise la connexion
        /// </summary>
        private void Initialize()
        {
            server = "127.0.0.1"; // Adresse IP serveur MySQL
            database = "sae_automate"; // Nom de votre la base de données
            uid = "automateUser"; // Utilisateur MySQL
            password = "automateUser!"; // Mot de passe MySQL

            string connectionString;
            connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";

            connection = new MySqlConnection(connectionString);
        }

        /// <summary>
        /// Ouvrir la connexion à la base de données
        /// </summary>
        /// <returns></returns>
        private bool OpenConnection()
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
        private bool CloseConnection()
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

    }
}

