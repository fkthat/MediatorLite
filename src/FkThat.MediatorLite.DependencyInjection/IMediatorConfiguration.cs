using System;

namespace FkThat.MediatorLite
{
    /// <summary>
    /// <c cref="Mediator"/> configuration API.
    /// </summary>
    public interface IMediatorConfiguration
    {
        /// <summary>
        /// Adds the message handler.
        /// </summary>
        /// <param name="type">The message handler type.</param>
        IMediatorConfiguration AddHandler(Type type);

        /// <summary>
        /// Adds the message handler.
        /// </summary>
        /// <typeparam name="T">The message handler type.</typeparam>
        IMediatorConfiguration AddHandler<T>() where T : IMessageHandler;
    }
}
