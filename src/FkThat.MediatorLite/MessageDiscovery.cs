using System;
using System.Collections.Generic;
using System.Linq;

namespace FkThat.MediatorLite
{
    /// <summary>
    /// Determines handler's message types.
    /// </summary>
    public class MessageDiscovery : IMessageDiscovery
    {
        /// <summary>
        /// Gets the message types for the handler.
        /// </summary>
        /// <param name="handlerType">Type of the handler.</param>
        public IEnumerable<Type> GetHandlerMessageTypes(Type handlerType) =>
            handlerType.GetInterfaces()
                .Where(itf => itf.IsGenericType)
                .Select(itf => new {
                    InterfaceType = itf,
                    GenericTypeDefinition = itf.GetGenericTypeDefinition()
                })
                .Where(itf => itf.GenericTypeDefinition == typeof(IMessageHandler<>))
                .SelectMany(itf => itf.InterfaceType.GetGenericArguments());
    }
}
