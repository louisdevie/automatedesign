using System.Text.Json;

namespace AutomateDesign.Server.Data
{
    public class ConfigurationService
    {
        /// <summary>
        /// Lit le fichier config.json
        /// </summary>
        /// <returns>format de donnés qui contient les données nécessaire à la connection BDD</returns>
        public DatabaseSetting GetDatabaseSetting()
        {
            // Lire le fichier JSON
            var json = File.ReadAllText("appsetting.json");

            // Désérialiser les données en objet
            return JsonSerializer.Deserialize<DatabaseSetting>(json);
        }
    }
}
