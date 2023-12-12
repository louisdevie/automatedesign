using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Core.Documents
{
    /// <summary>
    /// Les métadonnées d'un automate.
    /// </summary>
    public class DocumentHeader
    {
        private const int unsavedId = -1;
        private const string defaultDocumentName = "Automate sans nom";

        /// <summary>
        /// Le nom par défaut d'un document, utilisé quand un nouveau n'as pas encore de nom.
        /// </summary>
        public static string DefaultDocumentName => defaultDocumentName;


        private int id;
        private string name;
        private DateTime lastModification;

        /// <summary>
        /// L'id de l'automate.
        /// </summary>
        public int Id { get => this.id; set => this.id = Id; }

        /// <summary>
        /// Le nom donné à l'automate.
        /// </summary>
        public string Name { get => this.name; set => this.name = value; }

        /// <summary>
        /// La date de dernière modification.
        /// </summary>
        public DateTime LastModificationdate => lastModification;

        /// <summary>
        /// Le temps depuis la dernière modification.
        /// </summary>
        public TimeSpan TimeSinceLastModification => DateTime.Now - lastModification;


        public bool HasNeverBeenSaved => this.id == unsavedId;

        /// <summary>
        /// Crée des métadonnées pour un nouvel automate.
        /// </summary>
        public DocumentHeader()
        {
            this.id = unsavedId;
            this.name = DefaultDocumentName;
            this.lastModification = DateTime.Now;
        }

        /// <summary>
        /// Crée des métadonnées pour un automate existant.
        /// </summary>
        /// <param name="id">L'identifiant de l'automate.</param>
        /// <param name="name">Le nom à donner à l'automate.</param>
        /// <param name="lastModification">La date de dernière modification si l'automate existe déjà.</param>
        public DocumentHeader(int id, string name, DateTime? lastModification = null)
        {
            this.id = id;
            this.name = name;
            this.lastModification = lastModification ?? DateTime.Now;
        }

        /// <summary>
        /// Mets à jour la date de dernière modification.
        /// </summary>
        public void UpdateLastModificationDate()
        {
            this.lastModification = DateTime.Now;
        }
    }
}
