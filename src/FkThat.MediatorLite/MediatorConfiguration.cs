using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FkThat.MediatorLite
{
    /// <summary>
    /// <c cref="Mediator"/> configuration.
    /// </summary>
    public class MediatorConfiguration : IMediatorConfiguration, IMediatorConfigurationBuilder
    {
        private readonly HashSet<(Type, Type)> _messageHandlers = new();
        private readonly Dictionary<Type, Func<object, object, Task>> _messageDispatchers = new();
        private readonly IMessageDiscovery _messageDiscovery;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediatorConfiguration"/> class.
        /// </summary>
        /// <param name="messageDiscovery">The message discovery.</param>
        public MediatorConfiguration(IMessageDiscovery messageDiscovery)
        {
            _messageDiscovery = messageDiscovery;
        }

        /// <summary>
        /// Maps message types to handler types.
        /// </summary>
        public IReadOnlyCollection<(Type, Type)> MessageHandlers => _messageHandlers;

        /// <summary>
        /// Maps message types to dispatch functions.
        /// </summary>
        public IReadOnlyDictionary<Type, Func<object, object, Task>> MessageDispatchers =>
            _messageDispatchers;

        /// <summary>
        /// Adds the message handler.
        /// </summary>
        /// <param name="handlerType">The message handler type.</param>
        public IMediatorConfigurationBuilder AddHandler(Type handlerType)
        {
            foreach (var msgType in _messageDiscovery.GetHandlerMessageTypes(handlerType))
            {
                _messageHandlers.Add((msgType, handlerType));

                if (!_messageDispatchers.ContainsKey(msgType))
                {
                    _messageDispatchers.Add(msgType, BuildDispatch(msgType).Compile());
                }
            }

            return this;
        }

        private static Expression<Func<object, object, Task>> BuildDispatch(Type messageType)
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
