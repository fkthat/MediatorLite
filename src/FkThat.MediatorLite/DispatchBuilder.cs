using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FkThat.MediatorLite
{
    /// <summary>
    /// Creates dispatch delegates.
    /// </summary>
    public class DispatchBuilder : IDispatchBuilder
    {
        /// <summary>
        /// Builds the dispatch function.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        public Func<object, object, Task> BuildDispatchFunc(Type messageType)
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
            return r.Compile();
        }
    }
}
