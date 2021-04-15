using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace FkThat.MediatorLite.DependencyInjection
{
    public class Test_IMediatorConfiguration
    {
        [Fact]
        public void AddHandlerGeneric_ShouldCallAddHandlerNonGeneric()
        {
            IMediatorConfiguration config = A.Fake<MediatorConfiguration>();
            A.CallTo(() => config.AddHandler(A<Type>._)).Returns(config);
            var r = config.AddHandler<Handler1>().AddHandler<Handler2>();
            r.Should().Be(config);
            A.CallTo(() => config.AddHandler(typeof(Handler1))).MustHaveHappened();
            A.CallTo(() => config.AddHandler(typeof(Handler2))).MustHaveHappened();
        }

        [Fact]
        public void AddHandlersFromAssembly_ShouldDiscoverHandlersInCurrentAssembly()
        {
            IMediatorConfiguration config = A.Fake<MediatorConfiguration>();
            A.CallTo(() => config.AddHandler(A<Type>._)).Returns(config);
            config.AddHandlersFromAssembly();
            A.CallTo(() => config.AddHandler(typeof(Handler1))).MustHaveHappened();
            A.CallTo(() => config.AddHandler(typeof(Handler2))).MustHaveHappened();
        }

        [Fact]
        public void AddHandlersFromAssembly_ShouldDiscoverHandlersInArbitraryAssembly()
        {
            IMediatorConfiguration config = A.Fake<MediatorConfiguration>();
            A.CallTo(() => config.AddHandler(A<Type>._)).Returns(config);
            config.AddHandlersFromAssembly(GetType().Assembly);
            A.CallTo(() => config.AddHandler(typeof(Handler1))).MustHaveHappened();
            A.CallTo(() => config.AddHandler(typeof(Handler2))).MustHaveHappened();
        }

        [Fact]
        public void AddHandlersFromAssembly_ShouldFilterHandlers()
        {
            IMediatorConfiguration config = A.Fake<MediatorConfiguration>();
            A.CallTo(() => config.AddHandler(A<Type>._)).Returns(config);
            config.AddHandlersFromAssembly(filter: t => t.Name == "Handler1");
            A.CallTo(() => config.AddHandler(typeof(Handler1))).MustHaveHappened();
            A.CallTo(() => config.AddHandler(typeof(Handler2))).MustNotHaveHappened();
        }

        public abstract class MediatorConfiguration : IMediatorConfiguration
        {
            public abstract IMediatorConfiguration AddHandler(Type type);
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
