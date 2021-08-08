using System.Threading;
using System.Threading.Tasks;

namespace FkThat.MediatorLite
{
    /// <summary>
    /// Mediator.
    /// </summary>
    public interface IMediator
    {
        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="message">The message.</param>
        Task SendMessageAsync(object message) => SendMessageAsync(message, CancellationToken.None);

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task SendMessageAsync(object message, CancellationToken cancellationToken);
    }
}
