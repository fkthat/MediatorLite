using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FkThat.MediatorLite;
using Xunit;

namespace Microsoft.Extensions.DependencyInjection
{
    public class Test_ServiceCollectionExtensions
    {
        [Fact]
        public async Task AddMediator_ShouldRegisterMediator()
        {
            using CancellationTokenSource cancellationTokenSource = new();
            var cancellationToken = cancellationTokenSource.Token;
            M message = new();
            var handler = A.Fake<H>();

            A.CallTo(() => handler.HandleMessageAsync(message, cancellationToken))
                .Returns(Task.CompletedTask);

            ServiceCollection services = new();
            services.AddMediator();
            services.AddSingleton(handler);
            using var serviceProvider = services.BuildServiceProvider();
            var mediator = serviceProvider.GetRequiredService<IMediator>();
            await mediator.SendMessageAsync(message, cancellationToken);

            A.CallTo(() => handler.HandleMessageAsync(message, cancellationToken))
                .MustHaveHappened();
        }

        // Message and Handler for testing

        public class M { }

        public abstract class H : IMessageHandler<M>
        {
            public abstract Task HandleMessageAsync(M message, CancellationToken cancellationToken);
        }
    }
}
