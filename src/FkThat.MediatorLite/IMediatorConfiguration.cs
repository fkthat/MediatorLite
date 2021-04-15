using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FkThat.MediatorLite
{
    /// <summary>
    /// <c cref="Mediator"/> configuration.
    /// </summary>
    public interface IMediatorConfiguration
    {
        /// <summary>
        /// Maps message types to handler types.
        /// </summary>
        IReadOnlyCollection<(Type, Type)> MessageHandlers { get; }

        /// <summary>
        /// Maps message types to dispatch functions.
        /// </summary>
        IReadOnlyDictionary<Type, Func<object, object, Task>> MessageDispatchers { get; }
    }
}
