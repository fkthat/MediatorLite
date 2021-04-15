using System;
using System.Threading.Tasks;

namespace FkThat.MediatorLite
{
    /// <summary>
    /// Creates a dispatch function for a message.
    /// </summary>
    public interface IMessageCompiler
    {
        /// <summary>
        /// Gets the message dispatch function.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        Func<object, object, Task> GetMessageDispatchFunc(Type messageType);
    }
}
