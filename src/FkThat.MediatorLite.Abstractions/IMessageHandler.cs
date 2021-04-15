using System.Threading.Tasks;

namespace FkThat.MediatorLite
{
    /// <summary>
    /// Message handler.
    /// </summary>
    /// <typeparam name="T">The message type.</typeparam>
    public interface IMessageHandler<T>
    {
        /// <summary>
        /// Handles the message.
        /// </summary>
        /// <param name="message">The message.</param>
        Task HandleMessageAsync(T message);
    }
}
