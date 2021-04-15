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
            A.CallTo(() => serviceProvider.GetService(h1.GetType())).Returns(h1);
            A.CallTo(() => serviceProvider.GetService(h2.GetType())).Returns(h2);
            MediatorConfiguration configuration = new();
            configuration.AddHandler(h1.GetType());
            configuration.AddHandler(h2.GetType());
            Mediator sut = new(serviceProvider, configuration);
            Message1 msg1 = new();
            Message2 msg2 = new();
            await sut.SendMessageAsync(msg1);
            await sut.SendMessageAsync(msg2);
            A.CallTo(() => h1.HandleMessageAsync(msg1)).MustHaveHappened();
            A.CallTo(() => h2.HandleMessageAsync(msg2)).MustHaveHappened();
        }

        public class Message1 { }

        public class Message2 { }
    }
}
