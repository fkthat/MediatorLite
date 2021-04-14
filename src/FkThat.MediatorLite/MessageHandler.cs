using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FkThat.MediatorLite
{
    /// <summary>
    /// Abstract base message handler.
    /// </summary>
    /// <typeparam name="T">Actual message handler type.</typeparam>
    public abstract class MessageHandler<T> : IMessageHandler
    {
        private static readonly IReadOnlyDictionary<Type, Func<object, object, Task>> _dispatch =
            MediatorReflectionUtil.GetHandlerMessageTypes(typeof(T))
                .ToDictionary(t => t, t => MediatorReflectionUtil.BuildDispatch(t).Compile());

        /// <summary>
        /// Handles the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public Task HandleMessageAsync(object message) =>
            _dispatch.TryGetValue(message.GetType(), out var func)
                ? func(this, message) : Task.CompletedTask;
    }
}
