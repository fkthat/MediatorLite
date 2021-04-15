using System;
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
    }
}
