using AutomateDesign.Core.Exceptions;
using MySql.Data.MySqlClient;

namespace AutomateDesign.Server.Data.MariaDb
{
    public class DatabaseConnector
    {
        private DatabaseSettings settings;

        public DatabaseConnector(DatabaseSettings settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Ouvre une connexion à la base de données
        /// </summary>
        /// <returns></returns>
        public MySqlConnection Connect()
        {
            try
            {
                string connectionString = $"SERVER={this.settings.Server};DATABASE={this.settings.Database};UID={this.settings.UserId};PASSWORD={this.settings.Password};";
                MySqlConnection connection = new(connectionString);
                connection.Open();
                return connection;
            }
            catch (MySqlException ex)
            {
                // Gérer les erreurs de connexion ici
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Impossible de se connecter au serveur de BDD.");
                        throw new DatabaseUnavailableException();
                    case 1045:
                        Console.WriteLine("Nom d'utilisateur ou mot de passe invalide.");
                        throw new DatabaseUnavailableException();
                    default:
                        Console.WriteLine("Erreur de connexion à la base de données.");
                        throw new DatabaseUnavailableException();
                }
            }
        }
    }
}

