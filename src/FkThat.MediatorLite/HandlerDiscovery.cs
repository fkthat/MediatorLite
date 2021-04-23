using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FkThat.MediatorLite
{
    /// <summary>
    /// Finds registered mesage handlers.
    /// </summary>
    public class HandlerDiscovery : IHandlerDiscovery
    {
        private readonly IEnumerable<Type> _handlers;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerDiscovery"/> class.
        /// </summary>
        /// <param name="handlers">The known handler types.</param>
        public HandlerDiscovery(IEnumerable<Type> handlers)
        {
            _handlers = handlers;
        }

        /// <summary>
        /// Finds the message handlers.
        /// </summary>
        /// <returns>The sequence of (MessageType, HandlerType) pairs.</returns>
        public IEnumerable<(Type, Type)> FindMessageHandlers() =>
            _handlers.Distinct()
                .SelectMany(ht =>
                    ht.GetInterfaces()
                        .Where(it =>
                            it.IsGenericType &&
                            it.GetGenericTypeDefinition() == typeof(IMessageHandler<>))
                        .Select(it => it.GetGenericArguments().First())
                        .Select(mt => (mt, ht)));
    }
}
