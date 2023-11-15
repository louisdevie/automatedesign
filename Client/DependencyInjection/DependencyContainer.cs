using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutomateDesign.Client.DependencyInjection
{
    /// <summary>
    /// Conteneur d'injection de dépendances.
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
        /// Fournis une implémentation unique.
        /// </summary>
        /// <param name="service">Le type de service fourni.</param>
        /// <param name="implementation">L'implémentation à utiliser.</param>
        public void Register(Type service, object implementation)
        {
            this.providers.Add(service, new InstanceProvider(implementation));
        }

        /// <summary>
        /// Fournis une implémentation unique.
        /// </summary>
        /// <typeparam name="TService">Le type de service fourni.</typeparam>
        /// <param name="implementation">L'implémentation à utiliser.</param>
        public void Register<TService>(TService implementation)
        where TService : class
        {
            this.providers.Add(typeof(TService), new InstanceProvider(implementation));
        }

        /// <summary>
        /// Fournis une implémentation en appelant le constructeur par défaut d'une classe.
        /// </summary>
        /// <param name="service">Le type de service fourni.</param>
        /// <param name="implementation">La classe à utiliser.</param>
        public void Register(Type service, Type implementation)
        {
            this.providers.Add(service, new DefaultConstructorProvider(implementation));
        }

        /// <summary>
        /// Fournis une implémentation en appelant le constructeur par défaut d'une classe.
        /// </summary>
        /// <typeparam name="TService">Le type de service fourni.</typeparam>
        /// <typeparam name="TImplementation">La classe à utiliser.</typeparam>
        public void Register<TService, TImplementation>()
        where TService : class
        where TImplementation : new()
        {
            this.providers.Add(typeof(TService), new DefaultConstructorProvider(typeof(TImplementation)));
        }

        /// <summary>
        /// Fournis une implémentation en appelant une fonction au besoin.
        /// </summary>
        /// <param name="service">Le type de service fourni.</param>
        /// <param name="factory">La fabrique à utiliser pour construire des implémentations.</param>
        public void Register(Type service, Func<object> factory)
        {
            this.providers.Add(service, new LazyProvider(factory));
        }

        /// <summary>
        /// Fournis une implémentation en appelant une fonction au besoin.
        /// </summary>
        /// <typeparam name="TService">Le type de service fourni.</typeparam>
        /// <param name="factory">La fabrique à utiliser pour construire des implémentations.</param>
        public void Register<TService>(Func<TService> factory)
        where TService : class
        {
            this.providers.Add(typeof(TService), new LazyProvider(factory));
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
