using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using Xunit;

namespace FkThat.MediatorLite.Abstractions
{
    public class Test_IMediator
    {
        [Fact]
        public async Task SendMessageAsync_ShouldCallSendMessageAsyncWithDefaultToken()
        {
            IMediator testee = A.Fake<Mediator>();
            A.CallTo(() => testee.SendMessageAsync(A<object>._)).CallsBaseMethod();
            object msg = new();
            await testee.SendMessageAsync(msg);
            A.CallTo(() => testee.SendMessageAsync(msg, CancellationToken.None)).MustHaveHappened();
        }

        public abstract class Mediator : IMediator
        {
            public abstract Task SendMessageAsync(
                object message, CancellationToken cancellationToken);
        }
    }
}
