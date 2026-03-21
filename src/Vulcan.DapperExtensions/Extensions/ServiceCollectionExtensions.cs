using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Vulcan.DapperExtensions.ORMapping;
using Vulcan.DapperExtensions.ORMapping.Configuration;

namespace Vulcan.DapperExtensions.Extensions
{
    /// <summary>
    /// Extension methods for configuring Vulcan ORM with dependency injection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Vulcan ORM services to the dependency injection container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configureOptions">Optional action to configure Vulcan options.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddVulcan(
            this IServiceCollection services,
            Action<VulcanOptions> configureOptions = null)
        {
            var options = new VulcanOptions();
            configureOptions?.Invoke(options);

            // Register EntityConfigurationStore as singleton
            var configurationStore = new EntityConfigurationStore();
            services.AddSingleton<IEntityConfigurationStore>(configurationStore);

            // Set the global configuration store for EntityReflect
            EntityReflect.SetConfigurationStore(configurationStore);

            // Auto-register configurations from assemblies
            if (options.AutoScanAssemblies.Any())
            {
                RegisterConfigurations(configurationStore, options.AutoScanAssemblies);
            }

            return services;
        }

        /// <summary>
        /// Adds Vulcan ORM services with automatic configuration scanning.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="assemblies">Assemblies to scan for IEntityConfiguration implementations.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddVulcan(
            this IServiceCollection services,
            params Assembly[] assemblies)
        {
            return services.AddVulcan(options =>
            {
                options.AutoScanAssemblies.AddRange(assemblies);
            });
        }

        /// <summary>
        /// Registers all IEntityConfiguration implementations from the specified assemblies.
        /// </summary>
        private static void RegisterConfigurations(
            EntityConfigurationStore store,
            List<Assembly> assemblies)
        {
            var configurationType = typeof(IEntityConfiguration<>);

            foreach (var assembly in assemblies)
            {
                var configurationTypes = assembly.GetTypes()
                    .Where(t => !t.IsAbstract && !t.IsInterface)
                    .Where(t => t.GetInterfaces()
                        .Any(i => i.IsGenericType && 
                                  i.GetGenericTypeDefinition() == configurationType));

                foreach (var type in configurationTypes)
                {
                    var configInstance = Activator.CreateInstance(type);

                    // Get the entity type from the interface
                    var entityType = type.GetInterfaces()
                        .First(i => i.IsGenericType && 
                                    i.GetGenericTypeDefinition() == configurationType)
                        .GetGenericArguments()[0];

                    // Call RegisterConfiguration<T> via reflection
                    var registerMethod = typeof(EntityConfigurationStore)
                        .GetMethod(nameof(EntityConfigurationStore.RegisterConfiguration))
                        .MakeGenericMethod(entityType);

                    registerMethod.Invoke(store, new[] { configInstance });
                }
            }
        }
    }

    /// <summary>
    /// Configuration options for Vulcan ORM.
    /// </summary>
    public class VulcanOptions
    {
        /// <summary>
        /// Assemblies to scan for IEntityConfiguration implementations.
        /// </summary>
        public List<Assembly> AutoScanAssemblies { get; } = new List<Assembly>();
    }
}
