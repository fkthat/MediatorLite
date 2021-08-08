using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using Xunit;

namespace FkThat.MediatorLite
{
    public class Test_DispatchBuilder
    {
        [Fact]
        public async Task BuildDispatchFunc_ShouldReturnHandlerDelegate()
        {
            using CancellationTokenSource cancellationTokenSource = new();
            var cancellationToken = cancellationTokenSource.Token;
            M message = new();
            var handler = A.Fake<IMessageHandler<M>>();

            A.CallTo(() => handler.HandleMessageAsync(message, cancellationToken))
                .Returns(Task.CompletedTask);

            DispatchBuilder testee = new();
            var result = testee.BuildDispatchFunc(typeof(M));
            await result(handler, message, cancellationToken);

            A.CallTo(() => handler.HandleMessageAsync(message, cancellationToken))
                .MustHaveHappened();
        }

        public class M { }
    }
}
