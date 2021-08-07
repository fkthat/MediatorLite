using System;
using System.Threading;
using System.Threading.Tasks;

namespace FkThat.MediatorLite
{
    /// <summary>
    /// Mediator
    /// </summary>
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDispatcher _dispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mediator"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="dispatcher">The message dispatcher.</param>
        public Mediator(IServiceProvider serviceProvider, IDispatcher dispatcher)
        {
            _serviceProvider = serviceProvider;
            _dispatcher = dispatcher;
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public Task SendMessageAsync(object message, CancellationToken cancellationToken) =>
            _dispatcher.DispatchAsync(_serviceProvider, message);
    }
}
