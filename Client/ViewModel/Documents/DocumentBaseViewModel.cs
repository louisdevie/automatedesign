using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.ViewModel.Documents
{
    /// <summary>
    /// Le modèle-vue de base pour les documents.
    /// </summary>
    public abstract class DocumentBaseViewModel : BaseViewModel
    {
        /// <summary>
        /// Le nom de l'automate.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Une représentation formatée du temps depuis la dernière modification.
        /// </summary>
        public abstract string TimeSinceLastModification { get; }
    }
}
