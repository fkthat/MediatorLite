using System;
using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using FkThat.MediatorLite;
using FluentAssertions;
using Xunit;

namespace Microsoft.Extensions.DependencyInjection
{
    public class Test_ServiceCollectionExtensions
    {
        [Fact]
        public async Task AddMediator_ShouldRegisterAutoTypes()
        {
            ServiceCollection services = new();
            services.AddMediator();
            services.AddTransient<Handler>();

            // validate ServiceCollection contains required descriptors

            services.Should().Contain(d =>
                d.ServiceType == typeof(IMediatorConfiguration) &&
                d.Lifetime == ServiceLifetime.Singleton);

            services.Should().Contain(d =>
                d.ServiceType == typeof(IMediator) &&
                d.ImplementationType == typeof(Mediator) &&
                d.Lifetime == ServiceLifetime.Transient);

            // create configuration by ImplementationFactory from ServiceCollection

            var configurationFactory = services
                .Where(sd => sd.ServiceType == typeof(IMediatorConfiguration))
                .Select(sd => sd.ImplementationFactory)
                .First();

            using var serviceProvider = services.BuildServiceProvider();
            var configuration = (IMediatorConfiguration)configurationFactory(serviceProvider);

            // validate configuration contains Handler for Message
            configuration.MessageHandlers.Should().Contain((typeof(Message), typeof(Handler)));

            // validate configuration contains valid dispatcher
            var message = new Message();
            var handler = A.Fake<IMessageHandler<Message>>();
            A.CallTo(() => handler.HandleMessageAsync(message)).Returns(Task.CompletedTask);
            await configuration.MessageDispatchers[typeof(Message)](handler, message);
            A.CallTo(() => handler.HandleMessageAsync(message)).MustHaveHappened();
        }

        // Message and Handler for testing

        public class Message { }

        public class Handler : IMessageHandler<Message>
        {
            public Task HandleMessageAsync(Message message) => throw new NotImplementedException();
        }
    }
}
