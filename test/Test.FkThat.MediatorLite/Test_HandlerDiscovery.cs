using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace FkThat.MediatorLite
{
    public class Test_HandlerDiscovery
    {
        [Fact]
        public void FindMessageHandlers_ShouldReturnMessageHandlerMapping()
        {
            var h1 = A.Fake<IMessageHandler<M0>>(opts =>
                opts.Implements<IMessageHandler<M1>>()
                    .Implements<IMessageHandler<M2>>())
                .GetType();

            var h2 = A.Fake<IMessageHandler<M0>>(opts =>
                opts.Implements<IMessageHandler<M3>>()
                    .Implements<IMessageHandler<M4>>())
                .GetType();

            HandlerDiscovery testee = new(new[] {
                typeof(H1),
                typeof(object), // not a handler - should be skipped
                typeof(H1), // doubled - should be skipped
                typeof(H2)
            });

            var result = testee.FindMessageHandlers();

            result.Should().BeEquivalentTo(
                (typeof(M0), typeof(H1)),
                (typeof(M1), typeof(H1)),
                (typeof(M2), typeof(H1)),
                (typeof(M0), typeof(H2)),
                (typeof(M3), typeof(H2)),
                (typeof(M4), typeof(H2)));
        }

        public class M0 { }

        public class M1 { }

        public class M2 { }

        public class M3 { }

        public class M4 { }

        public class H1 : IMessageHandler<M0>, IMessageHandler<M1>, IMessageHandler<M2>
        {
            public Task HandleMessageAsync(M0 message, CancellationToken cancellationToken) => throw new System.NotImplementedException();

            public Task HandleMessageAsync(M1 message, CancellationToken cancellationToken) => throw new System.NotImplementedException();

            public Task HandleMessageAsync(M2 message, CancellationToken cancellationToken) => throw new System.NotImplementedException();
        }

        public class H2 : IMessageHandler<M0>, IMessageHandler<M3>, IMessageHandler<M4>
        {
            public Task HandleMessageAsync(M0 message, CancellationToken cancellationToken) => throw new System.NotImplementedException();

            public Task HandleMessageAsync(M3 message, CancellationToken cancellationToken) => throw new System.NotImplementedException();

            public Task HandleMessageAsync(M4 message, CancellationToken cancellationToken) => throw new System.NotImplementedException();
        }
    }
}
