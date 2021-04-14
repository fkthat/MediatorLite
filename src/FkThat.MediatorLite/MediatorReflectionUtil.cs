using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        /// <summary>
        /// Builds the dispatch expression for the given message type.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <exception cref="NotImplementedException"></exception>
        public static Expression<Func<object, object, Task>> BuildDispatch(Type messageType)
        {
            // (h, m) => ((IMessageHandler<TMsg>)h).HandleMessageAsync((TMsg)m);
            var handlerType = typeof(IMessageHandler<>).MakeGenericType(messageType);
            var handleMethod = handlerType.GetMethod("HandleMessageAsync", new[] { messageType });
            var h = Expression.Parameter(typeof(object));
            var m = Expression.Parameter(typeof(object));
            var x = Expression.Convert(h, handlerType);
            var y = Expression.Convert(m, messageType);
            var z = Expression.Call(x, handleMethod, y);
            var r = Expression.Lambda<Func<object, object, Task>>(z, h, m);
            return r;
        }
    }
}
