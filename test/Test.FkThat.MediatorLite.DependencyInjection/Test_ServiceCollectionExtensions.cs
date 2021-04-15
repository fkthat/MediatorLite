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
        public void AddMediator_ShouldRegisterTypes()
        {
            ServiceCollection services = new();
            var configure = A.Fake<Action<IMediatorConfigurationBuilder>>();

            IMediatorConfiguration? config = null;
            A.CallTo(() => configure(A<IMediatorConfigurationBuilder>._))
                .Invokes(c => config = (IMediatorConfiguration?)c.Arguments[0]);

            services.AddMediator(config => configure(config));

            services.Should().Contain(d =>
                d.ServiceType == typeof(IMediatorConfiguration) &&
                d.ImplementationInstance == config &&
                d.Lifetime == ServiceLifetime.Singleton);

            services.Should().Contain(d =>
                d.ServiceType == typeof(IMediator) &&
                d.ImplementationType == typeof(Mediator) &&
                d.Lifetime == ServiceLifetime.Transient);
        }

        [Fact]
        public void AddMediator_ShouldRegisterAutoTypes()
        {
            ServiceCollection services = new();
            services.AddTransient<Handler>();
            services.AddMediator();

            services.Should().Contain(d =>
                d.ServiceType == typeof(IMediatorConfiguration) &&
                d.Lifetime == ServiceLifetime.Singleton);

            services.Should().Contain(d =>
                d.ServiceType == typeof(IMediator) &&
                d.ImplementationType == typeof(Mediator) &&
                d.Lifetime == ServiceLifetime.Transient);

            var configuration = services
                .Where(sd => sd.ServiceType == typeof(IMediatorConfiguration))
                .Select(sd => sd.ImplementationInstance)
                .Cast<IMediatorConfiguration>()
                .First();

            configuration.MessageHandlers.Should()
                .Contain((typeof(Message), typeof(Handler)));
        }

        public class Message { }

        public class Handler : IMessageHandler<Message>
        {
            public Task HandleMessageAsync(Message message) => throw new NotImplementedException();
        }
    }
}
