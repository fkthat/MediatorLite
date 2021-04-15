using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using Xunit;

namespace FkThat.MediatorLite
{
    public class Test_Mediator
    {
        [Fact]
        public async Task SendMessageAsync_ShouldDispatchMessage()
        {
            var h1 = A.Fake<IMessageHandler<Message1>>();
            var h2 = A.Fake<IMessageHandler<Message2>>();
            var configuration = A.Fake<IMediatorConfiguration>();

            // arrange message handlers
            A.CallTo(() => configuration.MessageHandlers).Returns(new[] {
                (typeof(Message1), h1.GetType()),
                (typeof(Message2), h2.GetType()),
            });

            // arrange message dispatchers
            A.CallTo(() => configuration.MessageDispatchers).Returns(
                new Dictionary<Type, Func<object, object, Task>> {
                    [typeof(Message1)] = (h, m) =>
                        ((IMessageHandler<Message1>)h).HandleMessageAsync((Message1)m),
                    [typeof(Message2)] = (h, m) =>
                        ((IMessageHandler<Message2>)h).HandleMessageAsync((Message2)m),
                });

            var serviceProvider = A.Fake<IServiceProvider>();
            A.CallTo(() => serviceProvider.GetService(h1.GetType())).Returns(h1);
            A.CallTo(() => serviceProvider.GetService(h2.GetType())).Returns(h2);

            Mediator sut = new(serviceProvider, configuration);
            Message1 msg1 = new();
            Message2 msg2 = new();
            await sut.SendMessageAsync(msg1);
            await sut.SendMessageAsync(msg2);

            // verify messages dispatched
            A.CallTo(() => h1.HandleMessageAsync(msg1)).MustHaveHappened();
            A.CallTo(() => h2.HandleMessageAsync(msg2)).MustHaveHappened();
        }

        public class Message1 { }

        public class Message2 { }
    }
}
