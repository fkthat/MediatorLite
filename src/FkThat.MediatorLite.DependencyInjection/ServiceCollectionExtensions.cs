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
        /// Adds the mediator to the <c cref="IServiceCollection"/>. This method should be called
        /// only after all handlers registration.
        /// </summary>
        /// <param name="services">The <c cref="IServiceCollection"/>.</param>
        public static IServiceCollection AddMediator(this IServiceCollection services)
        {
            // is a message handler interface
            bool isHandlerInterface(Type t) =>
                t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IMessageHandler<>);

            // is a message handler class
            bool isHandlerType(Type t) => t.GetInterfaces().Any(isHandlerInterface);

            // filter registered types
            var handlers = services.Select(sd => sd.ServiceType).Where(isHandlerType);

            // configuration function
            void configure(IMediatorConfigurationBuilder config) =>
                handlers.Aggregate(config, (c, h) => c.AddHandler(h));

            return services.AddMediator(configure);
        }
    }
}
