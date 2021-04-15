using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FkThat.MediatorLite
{
    /// <summary>
    /// Configuration of <c cref="Mediator"/>.
    /// </summary>
    public class MediatorConfiguration : IMediatorConfiguration
    {
        private readonly HashSet<Type> _handlers = new();

        /// <summary>
        /// Gets the collection of message handlers.
        /// </summary>
        public IReadOnlyCollection<Type> MessageHandlers => _handlers;

        /// <summary>
        /// Adds the message handler.
        /// </summary>
        /// <param name="type">The message handler type.</param>
        public IMediatorConfiguration AddHandler(Type type)
        {
            if (!typeof(IMessageHandler).IsAssignableFrom(type))
            {
                throw new ArgumentException(
                    $"The {type} doesn't implement {typeof(IMessageHandler)}",
                    nameof(type));
            }

            _handlers.Add(type);
            return this;
        }
    }
}
