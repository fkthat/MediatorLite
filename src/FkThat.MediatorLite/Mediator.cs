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
        private readonly MediatorConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mediator"/> class.
        /// </summary>
        /// <param name="serviceProvider">The <c cref="IServiceProvider"/>.</param>
        /// <param name="configuration">The configuration.</param>
        public Mediator(IServiceProvider serviceProvider, MediatorConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public Task SendMessageAsync(object message) =>
            _configuration.MessageDispatchers.TryGetValue(message.GetType(), out var dispatch)
                ? Task.WhenAll(_configuration.MessageHandlers
                    .Where(h => h.Item1 == message.GetType())
                    .Select(h => dispatch!(_serviceProvider.GetService(h.Item2), message)))
                : Task.CompletedTask;
    }
}
