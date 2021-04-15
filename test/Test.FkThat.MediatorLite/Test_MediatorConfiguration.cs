using System;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace FkThat.MediatorLite
{
    public class Test_MediatorConfiguration
    {
        [Fact]
        public void AddHandler_ShouldAddMessageHandlers()
        {
            MediatorConfiguration sut = new();

            var r = sut
                .AddHandler(typeof(Handler1))
                .AddHandler(typeof(Handler2));

            r.Should().Be(sut);

            sut.MessageHandlers.Should().BeEquivalentTo(
                (typeof(Message0), typeof(Handler1)),
                (typeof(Message1), typeof(Handler1)),
                (typeof(Message2), typeof(Handler1)),
                (typeof(Message0), typeof(Handler2)),
                (typeof(Message3), typeof(Handler2)),
                (typeof(Message4), typeof(Handler2)));
        }

        [Fact]
        public async Task AddHandler_ShouldAddMessageDispatchers()
        {
            MediatorConfiguration sut = new();

            var r = sut
                .AddHandler(typeof(Handler1))
                .AddHandler(typeof(Handler2));

            r.Should().Be(sut);

            Message0 msg0 = new();
            Message1 msg1 = new();
            Message2 msg2 = new();
            Message3 msg3 = new();
            Message4 msg4 = new();

            var h0 = A.Fake<IMessageHandler<Message0>>();
            var h1 = A.Fake<IMessageHandler<Message1>>();
            var h2 = A.Fake<IMessageHandler<Message2>>();
            var h3 = A.Fake<IMessageHandler<Message3>>();
            var h4 = A.Fake<IMessageHandler<Message4>>();

            A.CallTo(() => h0.HandleMessageAsync(A<Message0>._)).Returns(Task.CompletedTask);
            A.CallTo(() => h1.HandleMessageAsync(A<Message1>._)).Returns(Task.CompletedTask);
            A.CallTo(() => h2.HandleMessageAsync(A<Message2>._)).Returns(Task.CompletedTask);
            A.CallTo(() => h3.HandleMessageAsync(A<Message3>._)).Returns(Task.CompletedTask);
            A.CallTo(() => h4.HandleMessageAsync(A<Message4>._)).Returns(Task.CompletedTask);

            await sut.MessageDispatchers[typeof(Message0)](h0, msg0);
            await sut.MessageDispatchers[typeof(Message1)](h1, msg1);
            await sut.MessageDispatchers[typeof(Message2)](h2, msg2);
            await sut.MessageDispatchers[typeof(Message3)](h3, msg3);
            await sut.MessageDispatchers[typeof(Message4)](h4, msg4);

            A.CallTo(() => h0.HandleMessageAsync(msg0)).MustHaveHappened();
            A.CallTo(() => h1.HandleMessageAsync(msg1)).MustHaveHappened();
            A.CallTo(() => h2.HandleMessageAsync(msg2)).MustHaveHappened();
            A.CallTo(() => h3.HandleMessageAsync(msg3)).MustHaveHappened();
            A.CallTo(() => h4.HandleMessageAsync(msg4)).MustHaveHappened();
        }

        public class Message0 { }

        public class Message1 { }

        public class Message2 { }

        public class Message3 { }

        public class Message4 { }

        private class Handler1 :
            IMessageHandler<Message0>,
            IMessageHandler<Message1>,
            IMessageHandler<Message2>
        {
            public Task HandleMessageAsync(Message0 message) => throw new NotImplementedException();

            public Task HandleMessageAsync(Message1 message) => throw new NotImplementedException();

            public Task HandleMessageAsync(Message2 message) => throw new NotImplementedException();
        }

        private class Handler2 :
            IMessageHandler<Message0>,
            IMessageHandler<Message3>,
            IMessageHandler<Message4>
        {
            public Task HandleMessageAsync(Message0 message) => throw new NotImplementedException();

            public Task HandleMessageAsync(Message3 message) => throw new NotImplementedException();

            public Task HandleMessageAsync(Message4 message) => throw new NotImplementedException();
        }

        private class NotHandler { }
    }
}
