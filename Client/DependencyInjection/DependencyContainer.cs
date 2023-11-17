using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutomateDesign.Client.DependencyInjection
{
    /// <summary>
    /// Le conteneur d'injection de dépendances.
    /// </summary>
    public class DependencyContainer
    {
        private static DependencyContainer? current;

        public static DependencyContainer Current
        {
            get
            {
                current ??= new DependencyContainer();
                return current;
            }
        }

        private Dictionary<Type, ImplementationProvider> providers;

        private DependencyContainer()
        {
            this.providers = new();
        }

        /// <summary>
        /// Fournit une implémentation unique.
        /// </summary>
        /// <param name="service">Le type de service fourni.</param>
        /// <param name="implementation">L'implémentation à utiliser.</param>
        public void RegisterSingleton(Type service, object implementation)
        {
            this.providers.Add(service, new SingletonImplementationProvider(implementation));
        }

        /// <summary>
        /// Fournit une implémentation unique.
        /// </summary>
        /// <typeparam name="TService">Le type de service fourni.</typeparam>
        /// <param name="implementation">L'implémentation à utiliser.</param>
        public void RegisterSingleton<TService>(TService implementation)
        where TService : class
        {
            this.providers.Add(typeof(TService), new SingletonImplementationProvider(implementation));
        }

        /// <summary>
        /// Obtient l'implémentation d'un service.
        /// </summary>
        /// <param name="service">Le type de service voulu.</param>
        /// <returns>Une instance du type <paramref name="service"/>.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public object GetImplementation(Type service)
        {
            if (this.providers.TryGetValue(service, out ImplementationProvider? provider))
            {
                object impl = provider.GetImplementation();
                
                if (impl.GetType().IsAssignableTo(service))
                {
                    return impl;
                }
                else
                {
                    throw new InvalidOperationException($"The provider for {service.FullName} returned an implementation of type {impl.GetType().FullName}, which is not assignable to the former.");
                }
            }
            else
            {
                throw new InvalidOperationException($"No provider available for {service.FullName}.");
            }
        }

        /// <summary>
        /// Obtient l'implémentation d'un service.
        /// </summary>
        /// <typeparam name="TService">Le type de service voulu.</typeparam>
        /// <returns>Une instance du type <typeparamref name="TService"/>.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public TService GetImplementation<TService>()
        {
            if (this.providers.TryGetValue(typeof(TService), out ImplementationProvider? provider))
            {
                object impl = provider.GetImplementation();
                if (impl is TService service)
                {
                    return service;
                }
                else
                {
                    throw new InvalidOperationException($"The provider for {typeof(TService).FullName} returned an implementation of type {impl.GetType().FullName}, which is not assignable to the former.");
                }
            }
            else
            {
                throw new InvalidOperationException($"No provider available for {typeof(TService).FullName}.");
            }
        }
    }
}
