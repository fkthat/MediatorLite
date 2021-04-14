using System;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace FkThat.MediatorLite
{
    public class Test_MediatorReflectionUtil
    {
        [Fact]
        public void GetHandlerMessageTypes_ShouldReturnListOfTypes()
        {
            var r = MediatorReflectionUtil.GetHandlerMessageTypes(typeof(Handler));
            r.Should().BeEquivalentTo(typeof(Message1), typeof(Message2), typeof(Message3));
        }

        [Fact]
        public async Task BuildDispatch_ShouldReturnDispatchExpression()
        {
            var msg = new Message1();
            var h = A.Fake<IMessageHandler<Message1>>();
            A.CallTo(() => h.HandleMessageAsync(msg)).Returns(Task.CompletedTask);
            var func = MediatorReflectionUtil.BuildDispatch(typeof(Message1)).Compile();
            await func(h, msg);
            A.CallTo(() => h.HandleMessageAsync(msg)).MustHaveHappened();
        }

        public class Message1 { }

        public class Message2 { }

        public class Message3 { }

        public class Handler :
            IMessageHandler<Message1>,
            IMessageHandler<Message2>,
            IMessageHandler<Message3>
        {
            public Task HandleMessageAsync(object message) => throw new NotImplementedException();

            public Task HandleMessageAsync(Message1 message) => throw new NotImplementedException();

            public Task HandleMessageAsync(Message2 message) => throw new NotImplementedException();

            public Task HandleMessageAsync(Message3 message) => throw new NotImplementedException();
        }
    }
}
