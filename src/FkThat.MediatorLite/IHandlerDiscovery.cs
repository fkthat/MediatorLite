using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FkThat.MediatorLite
{
    /// <summary>
    /// Finds registered mesage handlers.
    /// </summary>
    public interface IHandlerDiscovery
    {
        /// <summary>
        /// Finds the registered message handlers.
        /// </summary>
        /// <returns>The sequence of (MessageType, HandlerType) pairs.</returns>
        public IEnumerable<(Type, Type)> FindMessageHandlers();
    }
}
