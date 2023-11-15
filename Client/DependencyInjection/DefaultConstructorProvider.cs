using System;
using System.Reflection;

namespace AutomateDesign.Client.DependencyInjection
{
    /// <summary>
    /// Fournis une nouvelle instance à chaque fois qu'elle est demandée en appelant le constructeur par défaut.
    /// </summary>
    internal class DefaultConstructorProvider : ImplementationProvider
    {
        private ConstructorInfo constructor;

        /// <summary>
        /// Crée un fournisseur de service qui crée de nouvelles instances en appelant le constructeur par défaut.
        /// </summary>
        /// <param name="type">La classe de l'implémentation.</param>
        /// <exception cref="ArgumentException"></exception>
        public DefaultConstructorProvider(Type type)
        {
            constructor = type.GetConstructor(Array.Empty<Type>())
                ?? throw new ArgumentException(
                    $"Aucun constructeur par défaut trouvé pour le type {type.FullName}",
                    nameof(type)
                );
        }

        public override object GetImplementation() => this.constructor.Invoke(null);
    }
}
