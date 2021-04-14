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
        Task SendMessageAsync(object message);
    }
}
