using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FkThat.MediatorLite
{
    /// <summary>
    /// Mediator
    /// </summary>
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly HashSet<(Type, Type)> _handlers;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mediator"/> class.
        /// </summary>
        /// <param name="serviceProvider">The <span class="code"></span> to resolve handlers.</param>
        /// <param name="handlers">Maps message types to handler types.</param>
        public Mediator(IServiceProvider serviceProvider, IEnumerable<(Type, Type)> handlers)
        {
            _serviceProvider = serviceProvider;
            _handlers = new(handlers);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mediator"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="handlers">Maps message types to handler types.</param>
        public Mediator(IServiceProvider serviceProvider, params (Type, Type)[] handlers)
            : this(serviceProvider, (IEnumerable<(Type, Type)>)handlers)
        {
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public Task SendMessageAsync(object message) => Task.WhenAll(
            _handlers.Where(h => h.Item1 == message.GetType())
                .Select(h => _serviceProvider.GetService(h.Item2))
                .Where(h => h != null).Cast<IMessageHandler>()
                .Select(h => h.HandleMessageAsync(message)));
    }
}
