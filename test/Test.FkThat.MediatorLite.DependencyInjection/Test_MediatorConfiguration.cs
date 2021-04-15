using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace FkThat.MediatorLite.DependencyInjection
{
    public class Test_MediatorConfiguration
    {
        [Fact]
        public void AddHandler_ShouldValidateHandlerType()
        {
            MediatorConfiguration sut = new();
            sut.Invoking(s => s.AddHandler(typeof(NotHandler)))
                .Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddHandler_ShouldAddHandlerType()
        {
            MediatorConfiguration sut = new();

            var r = sut
                .AddHandler(typeof(Handler1))
                .AddHandler(typeof(Handler2));

            r.Should().Be(sut);
            sut.MessageHandlers.Should().BeEquivalentTo(typeof(Handler1), typeof(Handler2));
        }

        public class Handler1 : IMessageHandler
        {
            public Task HandleMessageAsync(object message) => throw new NotImplementedException();
        }

        public class Handler2 : IMessageHandler
        {
            public Task HandleMessageAsync(object message) => throw new NotImplementedException();
        }

        public class NotHandler { }
    }
}
