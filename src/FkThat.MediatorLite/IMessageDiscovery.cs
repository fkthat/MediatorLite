using System;
using System.Collections.Generic;

namespace FkThat.MediatorLite
{
    /// <summary>
    /// Determines handler's message types.
    /// </summary>
    public interface IMessageDiscovery
    {
        /// <summary>
        /// Gets the message types for the handler.
        /// </summary>
        /// <param name="handlerType">Type of the handler.</param>
        IEnumerable<Type> GetHandlerMessageTypes(Type handlerType);
    }
}
