using System;
using System.Collections.Generic;
using System.Linq;

namespace FkThat.MediatorLite
{
    /// <summary>
    /// Finds registered message handlers.
    /// </summary>
    public class HandlerDiscovery : IHandlerDiscovery
    {
        private readonly IEnumerable<Type> _types;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerDiscovery"/> class.
        /// </summary>
        /// <param name="types">Types to scan.</param>
        public HandlerDiscovery(IEnumerable<Type> types)
        {
            _types = types;
        }

        /// <summary>
        /// Finds the message handlers.
        /// </summary>
        /// <returns>The sequence of (MessageType, HandlerType) pairs.</returns>
        public IEnumerable<(Type, Type)> FindMessageHandlers() =>
            _types.Distinct()
                .SelectMany(t =>
                    t.GetInterfaces()
                        .Where(it =>
                            it.IsGenericType &&
                            it.GetGenericTypeDefinition() == typeof(IMessageHandler<>))
                        .Select(it => it.GetGenericArguments().First())
                        .Select(mt => (mt, t)));
    }
}
