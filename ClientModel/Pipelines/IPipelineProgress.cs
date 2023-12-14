using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model.Pipelines
{
    /// <summary>
    /// Représente la progression d'une opération.
    /// </summary>
    public interface IPipelineProgress
    {
        /// <summary>
        /// Indique que l'opération est terminée.
        /// </summary>
        public void Done();

        /// <summary>
        /// Indique que l'opération a échouée.
        /// </summary>
        /// <param name="reason">Une phrase décrivant le problème.</param>
        public void Failed(string reason);
    }
}
