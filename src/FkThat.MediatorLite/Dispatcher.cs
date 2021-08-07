using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace FkThat.MediatorLite
{
    /// <summary>
    /// A dispatcher of messages.
    /// </summary>
    public class Dispatcher : IDispatcher
    {
        private readonly IReadOnlyCollection<(Type MessageType, Type HandlerType)> _handlers;
        private readonly IReadOnlyDictionary<Type, Func<object, object, Task>> _dispatch;

        /// <summary>
        /// Initializes a new instance of the <see cref="Dispatcher"/> class.
        /// </summary>
        /// <param name="discovery">The handler discovery service.</param>
        /// <param name="dispatchBuilder">The dispatch functions builder.</param>
        public Dispatcher(IHandlerDiscovery discovery, IDispatchBuilder dispatchBuilder)
        {
            _handlers = discovery.FindMessageHandlers().ToHashSet();

            _dispatch = _handlers.Select(h => h.MessageType).Distinct()
                .ToDictionary(mt => mt, mt => dispatchBuilder.BuildDispatchFunc(mt));
        }

        /// <summary>
        /// Dispatches message asynchronously using a service provider.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="message">The message.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public Task DispatchAsync(
            IServiceProvider serviceProvider,
            object message,
            CancellationToken cancellationToken) =>
            Task.WhenAll(_handlers
                .Where(h => h.MessageType == message.GetType())
                .Select(h => _dispatch[h.MessageType](
                    serviceProvider.GetService(h.HandlerType),
                    message)));
    }
}
