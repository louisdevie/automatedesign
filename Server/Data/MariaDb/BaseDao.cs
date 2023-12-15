    using MySql.Data.MySqlClient;

namespace AutomateDesign.Server.Data.MariaDb
{
    public class BaseDao
    {
        private DatabaseConnector connector;

        protected MySqlConnection Connect() => this.connector.Connect();

        public BaseDao(DatabaseConnector connector)
        {
            this.connector = connector;
        }
    }

    public static class MySqlQueryExtensions
    {
        private static MySqlCommand PrepareCommand(MySqlConnection connection, string query, object?[] parameters)
        {
            MySqlCommand cmd = new(query, connection);
            foreach (object? parameter in parameters)
            {
                cmd.Parameters.AddWithValue("", parameter);
            }
            return cmd;
        }

        /// <summary>
        /// Exécute une requête SQL sans résultat. 
        /// </summary>
        /// <param name="query">La requête SQL à exécuter.</param>
        /// <param name="parameters">Les paramètres de la requête.</param>
        public static int ExecuteNonQuery(this MySqlConnection connection, string query, params object?[] parameters)
        {
            return PrepareCommand(connection, query, parameters).ExecuteNonQuery();
        }

        /// <summary>
        /// Exécute une requête SQL avec résultat.
        /// </summary>
        /// <param name="query">La requête SQL à exécuter.</param>
        /// <param name="parameters">Les paramètres de la requête.</param>
        public static MySqlDataReader ExecuteReader(this MySqlConnection connection, string query, params object?[] parameters)
        {
            return PrepareCommand(connection, query, parameters).ExecuteReader();
        }

        /// <summary>
        /// Exécute une requête SQL avec un résultat unique.
        /// </summary>
        /// <param name="query">La requête SQL à exécuter.</param>
        /// <param name="parameters">Les paramètres de la requête.</param>
        public static object ExecuteScalar(this MySqlConnection connection, string query, params object?[] parameters)
        {
            return PrepareCommand(connection, query, parameters).ExecuteScalar();
        }

        /// <inheritdoc cref="ExecuteScalar(MySqlConnection, string, object?[])"/>
        public static T ExecuteScalar<T>(this MySqlConnection connection, string query, params object?[] parameters)
        {
            return (T)PrepareCommand(connection, query, parameters).ExecuteScalar();
        }

        /// <summary>
        /// Renvoie la cle primaire du dernier element ajoute a la bdd
        /// </summary>
        /// <param name="connection">la connexion a la bdd a utiliser</param>
        /// <returns>la cle primaire</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static int GetLastInsertId(this MySqlConnection connection)
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT LAST_INSERT_ID()";
            object id = cmd.ExecuteScalar();

            if (id is int intId) return intId;
            else if (id is long longId) return (int)longId;
            else if (id is uint uintId) return (int)uintId;
            else if (id is ulong ulongId) return (int)ulongId;
            else throw new InvalidOperationException($"Unhandled type {id.GetType()} returned by LAST_INSERT_ID.");
        }
    }
}
