using System;
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
            var serviceProvider = A.Fake<IServiceProvider>();
            A.CallTo(() => serviceProvider.GetService(typeof(Handler1))).Returns(h1);
            A.CallTo(() => serviceProvider.GetService(typeof(Handler2))).Returns(h2);

            Mediator sut = new(
                serviceProvider,
                (typeof(Message1), typeof(Handler1)),
                (typeof(Message2), typeof(Handler2)));

            Message1 msg1 = new();
            Message2 msg2 = new();
            await sut.SendMessageAsync(msg1);
            await sut.SendMessageAsync(msg2);
            A.CallTo(() => h1.HandleMessageAsync((object)msg1)).MustHaveHappened();
            A.CallTo(() => h2.HandleMessageAsync((object)msg2)).MustHaveHappened();
        }

        public class Message1 { }

        public class Message2 { }

        public class Handler1 { }

        public class Handler2 { }
    }
}
