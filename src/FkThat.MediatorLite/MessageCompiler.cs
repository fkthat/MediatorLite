using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FkThat.MediatorLite
{
    /// <summary>
    /// Creates a dispatch function for a message.
    /// </summary>
    public class MessageCompiler : IMessageCompiler
    {
        /// <summary>
        /// Gets the message dispatch function.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        public Func<object, object, Task> GetMessageDispatchFunc(Type messageType)
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
