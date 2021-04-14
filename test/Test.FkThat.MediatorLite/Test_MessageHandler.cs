using System.Threading.Tasks;
using FakeItEasy;
using Xunit;

namespace FkThat.MediatorLite
{
    public class Test_MessageHandler
    {
        [Fact]
        public async Task HandleMessageAsync_ShouldDispatchMessage()
        {
            Message1 msg1 = new();
            Message2 msg2 = new();
            Message3 msg3 = new();
            var h = A.Fake<Handler>();
            A.CallTo(() => h.HandleMessageAsync(msg1)).Returns(Task.CompletedTask);
            A.CallTo(() => h.HandleMessageAsync(msg2)).Returns(Task.CompletedTask);
            await h.HandleMessageAsync((object)msg1);
            await h.HandleMessageAsync((object)msg2);
            await h.HandleMessageAsync((object)msg3);
            A.CallTo(() => h.HandleMessageAsync(msg1)).MustHaveHappened();
            A.CallTo(() => h.HandleMessageAsync(msg2)).MustHaveHappened();
        }

        public class Message1 { }

        public class Message2 { }

        public class Message3 { }

        public abstract class Handler : MessageHandler<Handler>,
            IMessageHandler<Message1>, IMessageHandler<Message2>
        {
            public abstract Task HandleMessageAsync(Message1 message);

            public abstract Task HandleMessageAsync(Message2 message);
        }
    }
}
