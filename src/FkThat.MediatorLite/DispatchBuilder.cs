using System;
using System.Linq.Expressions;
using System.Threading;
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
        public Func<object, object, CancellationToken, Task> BuildDispatchFunc(Type messageType)
        {
            // (h, m, ct) => ((IMessageHandler<TMsg>)h).HandleMessageAsync((TMsg)m, ct);

            var handlerType = typeof(IMessageHandler<>).MakeGenericType(messageType);

            var handleMethod = handlerType.GetMethod(
                "HandleMessageAsync", new[] { messageType, typeof(CancellationToken) });

            var h = Expression.Parameter(typeof(object));
            var m = Expression.Parameter(typeof(object));
            var ct = Expression.Parameter(typeof(CancellationToken));
            var handler = Expression.Convert(h, handlerType);
            var message = Expression.Convert(m, messageType);
            var body = Expression.Call(handler, handleMethod, message, ct);

            var lambda = Expression.Lambda<Func<object, object, CancellationToken, Task>>(
                body, h, m, ct);

            return lambda.Compile();
        }
    }
}
