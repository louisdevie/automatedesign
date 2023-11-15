using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.DependencyInjection
{
    /// <summary>
    /// Fournis une implémentation pour un service.
    /// </summary>
    internal abstract class ImplementationProvider
    {
        /// <summary>
        /// Fournis une instance du service demandé.
        /// </summary>
        /// <returns>Une instance du service.</returns>
        public abstract object GetImplementation();
    }
}
