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
            this IServiceCollection services, Action<IMediatorConfiguration> configurationBuilder)
        {
            MediatorConfiguration configuration = new();
            configurationBuilder(configuration);

            var handlers = configuration.MessageHandlers.SelectMany(h =>
                MediatorReflectionUtil.GetHandlerMessageTypes(h).Select(m => (m, h)))
                .ToArray();

            services.AddTransient<IMediator>(sp => new Mediator(sp, handlers));
            return services;
        }
    }
}
