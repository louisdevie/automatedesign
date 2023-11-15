using System;

namespace AutomateDesign.Client.DependencyInjection
{
    /// <summary>
    /// Fournis une implémentation quand elle est demandée en appelant une fonction.
    /// </summary>
    internal class LazyProvider : ImplementationProvider
    {
        private Func<object> factory;

        /// <summary>
        /// Crée un fournisseur de service qui appele une fonction pour crée des instances au besoin.
        /// </summary>
        /// <param name="factory">La fonction qui crée les instances.</param>
        public LazyProvider(Func<object> factory)
        {
            this.factory = factory;
        }

        public override object GetImplementation() => this.factory();
    }
}
