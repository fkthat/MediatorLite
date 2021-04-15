using System;
using System.Linq;
using System.Reflection;
using FkThat.MediatorLite;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to <c cref="FkThat.MediatorLite.IMediatorConfiguration"/>.
    /// </summary>
    public static class MediatorConfigurationExtensions
    {
        /// <summary>
        /// Adds the message handlers from assembly.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="assembly">The assembly. Default is the calling assembly.</param>
        /// <param name="filter">The handler class filter. Default is no filtering.</param>
        public static IMediatorConfiguration AddHandlersFromAssembly(
            this IMediatorConfiguration configuration,
            Assembly? assembly = null,
            Func<Type, bool>? filter = null)
        {
            assembly ??= Assembly.GetCallingAssembly();
            filter ??= t => true;

            bool isHandlerInterface(Type i) => i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IMessageHandler<>);

            bool isHandler(Type t) => t.IsPublic && !t.IsAbstract &&
                t.GetInterfaces().Any(isHandlerInterface);

            var handlers = assembly.GetTypes().Where(isHandler);
            handlers = handlers.Where(filter);
            return handlers.Aggregate(configuration, (c, t) => c.AddHandler(t));
        }
    }
}
