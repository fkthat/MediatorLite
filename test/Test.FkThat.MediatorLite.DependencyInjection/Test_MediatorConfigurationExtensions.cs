using System;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FkThat.MediatorLite.DependencyInjection
{
    public class Test_MediatorConfigurationExtensions
    {
        [Fact]
        public void AddHandlersFromAssembly_ShouldDiscoverHandlersInCurrentAssembly()
        {
            var config = A.Fake<IMediatorConfiguration>();
            A.CallTo(() => config.AddHandler(A<Type>._)).Returns(config);
            config.AddHandlersFromAssembly();
            A.CallTo(() => config.AddHandler(typeof(Handler1))).MustHaveHappened();
            A.CallTo(() => config.AddHandler(typeof(Handler2))).MustHaveHappened();
        }

        [Fact]
        public void AddHandlersFromAssembly_ShouldDiscoverHandlersInArbitraryAssembly()
        {
            var config = A.Fake<IMediatorConfiguration>();
            A.CallTo(() => config.AddHandler(A<Type>._)).Returns(config);
            config.AddHandlersFromAssembly(GetType().Assembly);
            A.CallTo(() => config.AddHandler(typeof(Handler1))).MustHaveHappened();
            A.CallTo(() => config.AddHandler(typeof(Handler2))).MustHaveHappened();
        }

        [Fact]
        public void AddHandlersFromAssembly_ShouldFilterHandlers()
        {
            var config = A.Fake<IMediatorConfiguration>();
            A.CallTo(() => config.AddHandler(A<Type>._)).Returns(config);
            config.AddHandlersFromAssembly(filter: t => t.Name == "Handler1");
            A.CallTo(() => config.AddHandler(typeof(Handler1))).MustHaveHappened();
            A.CallTo(() => config.AddHandler(typeof(Handler2))).MustNotHaveHappened();
        }
    }

    public class Handler1 : IMessageHandler<object>
    {
        public Task HandleMessageAsync(object message) => throw new NotImplementedException();
    }

    public class Handler2 : IMessageHandler<object>
    {
        public Task HandleMessageAsync(object message) => throw new NotImplementedException();
    }
}
