using System;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using Xunit;

namespace FkThat.MediatorLite
{
    public class Test_Mediator
    {
        [Fact]
        public async Task SendMessageAsync_ShouldCallDispatcher()
        {
            CancellationTokenSource cancellationTokenSource = new();
            var cancellationToken = cancellationTokenSource.Token;
            var serviceProvider = A.Fake<IServiceProvider>();
            var dispatcher = A.Fake<IDispatcher>();

            A.CallTo(() => dispatcher.DispatchAsync(A<IServiceProvider>._, A<object>._))
                .Returns(Task.CompletedTask);

            Mediator testee = new(serviceProvider, dispatcher);
            Message message = new();
            await testee.SendMessageAsync(message, cancellationToken);

            A.CallTo(() => dispatcher.DispatchAsync(serviceProvider, message)).MustHaveHappened();
        }

        public class Message { }
    }
}
