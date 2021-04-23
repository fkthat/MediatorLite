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
        /// Adds the <c cref="Mediator"/> to the <c cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <c cref="IServiceCollection"/>.</param>
        /// <param name="lifetime">The lifetime of the <c cref="Mediator"/>.</param>
        public static IServiceCollection AddMediator(
            this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            services.AddTransient<IHandlerDiscovery>(sp =>
                new HandlerDiscovery(services.Select(sd => sd.ServiceType)))
                .AddTransient<IDispatchBuilder, DispatchBuilder>()
                .AddSingleton<IDispatcher, Dispatcher>()
                .Add(ServiceDescriptor.Describe(typeof(IMediator), typeof(Mediator), lifetime));

            return services;
        }
    }
}
