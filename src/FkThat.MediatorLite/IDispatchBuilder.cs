using System;
using System.Threading.Tasks;

namespace FkThat.MediatorLite
{
    /// <summary>
    /// Creates dispatch delegates.
    /// </summary>
    public interface IDispatchBuilder
    {
        /// <summary>
        /// Builds the dispatch function.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        Func<object, object, Task> BuildDispatchFunc(Type messageType);
    }
}
