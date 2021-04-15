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
            MediatorConfiguration configuration = new(
                new MessageDiscovery(), new MessageCompiler());

            configurationBuilder(configuration);
            services.AddTransient<IMediator>(sp => new Mediator(sp, configuration));
            return services;
        }
    }
}
