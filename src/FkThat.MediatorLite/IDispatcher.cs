using System;
using System.Threading;
using System.Threading.Tasks;

namespace FkThat.MediatorLite
{
    /// <summary>
    /// The dispatcher of messages.
    /// </summary>
    public interface IDispatcher
    {
        /// <summary>
        /// Dispatches message asynchronously using a service provider.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="message">The message.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task DispatchAsync(
            IServiceProvider serviceProvider, object message, CancellationToken cancellationToken);
    }
}
