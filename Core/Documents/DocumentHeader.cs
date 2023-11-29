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
        #region Attributs
        private string name;
        private DateTime lastModification;
        #endregion

        #region Properties
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
        #endregion

        /// <summary>
        /// Crée des métadonnées pour un automate.
        /// </summary>
        /// <param name="name">Le nom à donner à l'automate.</param>
        /// <param name="lastModification">La date de dernière modification si l'automate existe déjà.</param>
        public DocumentHeader(string name, DateTime? lastModification = null)
        {
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
