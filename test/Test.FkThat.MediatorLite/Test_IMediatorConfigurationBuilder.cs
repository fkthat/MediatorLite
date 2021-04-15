using System;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace FkThat.MediatorLite
{
    public class Test_IMediatorConfigurationBuilder
    {
        [Fact]
        public void AddHandlerGeneric_ShouldCallAddHandlerNonGeneric()
        {
            IMediatorConfigurationBuilder config = A.Fake<MediatorConfiguration>();
            A.CallTo(() => config.AddHandler(A<Type>._)).Returns(config);
            var r = config.AddHandler<Handler1>().AddHandler<Handler2>();
            r.Should().Be(config);
            A.CallTo(() => config.AddHandler(typeof(Handler1))).MustHaveHappened();
            A.CallTo(() => config.AddHandler(typeof(Handler2))).MustHaveHappened();
        }

        [Fact]
        public void AddHandlersFromAssembly_ShouldDiscoverHandlersInCurrentAssembly()
        {
            IMediatorConfigurationBuilder config = A.Fake<MediatorConfiguration>();
            A.CallTo(() => config.AddHandler(A<Type>._)).Returns(config);
            config.AddHandlersFromAssembly();
            A.CallTo(() => config.AddHandler(typeof(Handler1))).MustHaveHappened();
            A.CallTo(() => config.AddHandler(typeof(Handler2))).MustHaveHappened();
        }

        [Fact]
        public void AddHandlersFromAssembly_ShouldDiscoverHandlersInArbitraryAssembly()
        {
            IMediatorConfigurationBuilder config = A.Fake<MediatorConfiguration>();
            A.CallTo(() => config.AddHandler(A<Type>._)).Returns(config);
            config.AddHandlersFromAssembly(GetType().Assembly);
            A.CallTo(() => config.AddHandler(typeof(Handler1))).MustHaveHappened();
            A.CallTo(() => config.AddHandler(typeof(Handler2))).MustHaveHappened();
        }

        [Fact]
        public void AddHandlersFromAssembly_ShouldFilterHandlers()
        {
            IMediatorConfigurationBuilder config = A.Fake<MediatorConfiguration>();
            A.CallTo(() => config.AddHandler(A<Type>._)).Returns(config);
            config.AddHandlersFromAssembly(filter: t => t.Name == "Handler1");
            A.CallTo(() => config.AddHandler(typeof(Handler1))).MustHaveHappened();
            A.CallTo(() => config.AddHandler(typeof(Handler2))).MustNotHaveHappened();
        }

        public abstract class MediatorConfiguration : IMediatorConfigurationBuilder
        {
            public abstract IMediatorConfigurationBuilder AddHandler(Type type);
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
