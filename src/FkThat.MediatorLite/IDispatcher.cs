using System;
using System.Threading.Tasks;

namespace FkThat.MediatorLite
{
    /// <summary>
    /// A dispatcher of messages.
    /// </summary>
    public interface IDispatcher
    {
        /// <summary>
        /// Dispatches message asynchronously using a service provider.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        Task DispatchAsync(IServiceProvider serviceProvider, object message);
    }
}
