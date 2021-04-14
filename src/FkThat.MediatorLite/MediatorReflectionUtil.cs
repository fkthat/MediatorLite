using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FkThat.MediatorLite
{
    /// <summary>
    /// Reflection utilities.
    /// </summary>
    public static class MediatorReflectionUtil
    {
        /// <summary>
        /// Gets the message types handled by a handler of a given type.
        /// </summary>
        /// <param name="handlerType">The type of the handler.</param>
        public static IEnumerable<Type> GetHandlerMessageTypes(Type handlerType) =>
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
