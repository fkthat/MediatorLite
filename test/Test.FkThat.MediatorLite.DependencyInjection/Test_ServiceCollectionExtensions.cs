using System;
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
    }
}
