using System;
using System.Linq;
using System.Reflection;

namespace FkThat.MediatorLite
{
    /// <summary>
    /// <c cref="Mediator"/> configuration API.
    /// </summary>
    public interface IMediatorConfiguration
    {
        /// <summary>
        /// Adds the message handler.
        /// </summary>
        /// <param name="type">The message handler type.</param>
        IMediatorConfiguration AddHandler(Type type);

        /// <summary>
        /// Adds the message handler.
        /// </summary>
        /// <typeparam name="T">The message handler type.</typeparam>
        IMediatorConfiguration AddHandler<T>() where T : IMessageHandler =>
            AddHandler(typeof(T));

        /// <summary>
        /// Adds the message handlers from assembly.
        /// </summary>
        /// <param name="assembly">The assembly. Default is the calling assembly.</param>
        /// <param name="filter">The handler class filter. Default is no filtering.</param>
        IMediatorConfiguration AddHandlersFromAssembly(
            Assembly? assembly = null, Func<Type, bool>? filter = null)
        {
            assembly ??= Assembly.GetCallingAssembly();
            filter ??= t => true;

            bool isHandlerInterface(Type i) => i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IMessageHandler<>);

            bool isHandler(Type t) => t.IsPublic && !t.IsAbstract &&
                t.GetInterfaces().Any(isHandlerInterface);

            var handlers = assembly.GetTypes().Where(isHandler);
            handlers = handlers.Where(filter);
            return handlers.Aggregate(this, (c, t) => c.AddHandler(t));
        }
    }
}
