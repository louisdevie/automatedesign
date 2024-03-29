﻿namespace AutomateDesign.Client.DependencyInjection
{
    /// <summary>
    /// Fournis la même instance à chaque fois qu'elle est demandée.
    /// </summary>
    internal class SingletonImplementationProvider : ImplementationProvider
    {
        private object implementation;

        /// <summary>
        /// Crée un fournisseur de service qui renvoie toujours la même implémentation.
        /// </summary>
        /// <param name="implementation">L'implémentation du service.</param>
        public SingletonImplementationProvider(object implementation)
        {
            this.implementation = implementation;
        }

        public override object GetImplementation() => this.implementation;
    }
}
