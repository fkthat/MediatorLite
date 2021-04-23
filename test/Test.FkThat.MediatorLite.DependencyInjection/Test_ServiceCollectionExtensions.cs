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
            var handler = A.Fake<H>();
            A.CallTo(() => handler.HandleMessageAsync(A<M>._)).Returns(Task.CompletedTask);
            ServiceCollection services = new();
            services.AddMediator();
            services.AddSingleton(handler);
            using var serviceProvider = services.BuildServiceProvider();
            var mediator = serviceProvider.GetRequiredService<IMediator>();
            M message = new();
            await mediator.SendMessageAsync(message);
            A.CallTo(() => handler.HandleMessageAsync(message)).MustHaveHappened();
        }

        // Message and Handler for testing

        public class M { }

        public abstract class H : IMessageHandler<M>
        {
            public abstract Task HandleMessageAsync(M message);
        }
    }
}
