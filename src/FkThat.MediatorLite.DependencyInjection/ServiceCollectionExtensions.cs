using System;
using System.Linq;
using FkThat.MediatorLite;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for <c cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the mediator to the <c cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <c cref="IServiceCollection"/>.</param>
        /// <param name="configurationBuilder">Configures the <c cref="Mediator"/>.</param>
        [Obsolete("This method is obsolete and will be removed. Use parameterless overload.")]
        public static IServiceCollection AddMediator(
            this IServiceCollection services,
            Action<IMediatorConfigurationBuilder> configurationBuilder)
        {
            MessageDiscovery discovery = new();
            MessageCompiler compiler = new();
            MediatorConfiguration configuration = new(discovery, compiler);
            configurationBuilder(configuration);
            services.AddSingleton<IMediatorConfiguration>(configuration);
            services.AddTransient<IMediator, Mediator>();
            return services;
        }

        /// <summary>
        /// Adds the mediator to the <c cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <c cref="IServiceCollection"/>.</param>
        /// <param name="lifetime">The lifetime of the <c cref="Mediator"/>.</param>
        public static IServiceCollection AddMediator(
            this IServiceCollection services, ServiceLifetime lifetime)
        {
            services.AddSingleton(sp => (IMediatorConfiguration)services
                .Select(sd => sd.ServiceType)
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(IMessageHandler<>)))
                .Aggregate(
                    (IMediatorConfigurationBuilder)new MediatorConfiguration(
                        new MessageDiscovery(), new MessageCompiler()),
                    (c, h) => c.AddHandler(h)));

            services.Add(ServiceDescriptor.Describe(typeof(IMediator), typeof(Mediator), lifetime));
            return services;
        }

        /// <summary>
        /// Adds the mediator with the transient lifetime to the <c cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <c cref="IServiceCollection"/>.</param>
        public static IServiceCollection AddMediator(this IServiceCollection services) =>
            services.AddMediator(ServiceLifetime.Transient);
    }
}
