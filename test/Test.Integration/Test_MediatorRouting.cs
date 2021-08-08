using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FkThat.MediatorLite;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Integration
{
    public class Test_MediatorRouting
    {
        [Fact]
        public async Task Mediator_ShouldRouteMessagesToHandlers()
        {
            IList<(Type, object)> log = new List<(Type, object)>();

            ServiceCollection services = new();

            services.AddSingleton(log);
            services.AddMediator();
            services.AddTransient<H1>();
            services.AddTransient<H2>();

            using var container = services.BuildServiceProvider();
            var mediator = container.GetRequiredService<IMediator>();

            M0 m0 = new();
            M1 m1 = new();
            M2 m2 = new();
            M3 m3 = new();
            M4 m4 = new();

            await mediator.SendMessageAsync(m0);
            await mediator.SendMessageAsync(m1);
            await mediator.SendMessageAsync(m2);
            await mediator.SendMessageAsync(m3);
            await mediator.SendMessageAsync(m4);

            log.Take(2).Should().Contain((typeof(H1), m0))
                .And.Contain((typeof(H2), m0));

            log.Skip(2).Should().Equal(
                (typeof(H1), m1),
                (typeof(H1), m2),
                (typeof(H2), m3),
                (typeof(H2), m4));
        }

        public class M0 { }

        public class M1 { }

        public class M2 { }

        public class M3 { }

        public class M4 { }

        public class H1 : IMessageHandler<M0>, IMessageHandler<M1>, IMessageHandler<M2>
        {
            private readonly IList<(Type, object)> _log;

            public H1(IList<(Type, object)> log)
            {
                _log = log;
            }

            public Task HandleMessageAsync(M0 message, CancellationToken cancellationToken) => LogAsync(message);

            public Task HandleMessageAsync(M1 message, CancellationToken cancellationToken) => LogAsync(message);

            public Task HandleMessageAsync(M2 message, CancellationToken cancellationToken) => LogAsync(message);

            private Task LogAsync(object message)
            {
                _log.Add((GetType(), message));
                return Task.CompletedTask;
            }
        }

        public class H2 : IMessageHandler<M0>, IMessageHandler<M3>, IMessageHandler<M4>
        {
            private readonly IList<(Type, object)> _log;

            public H2(IList<(Type, object)> log)
            {
                _log = log;
            }

            public Task HandleMessageAsync(M0 message, CancellationToken cancellationToken) => LogAsync(message);

            public Task HandleMessageAsync(M3 message, CancellationToken cancellationToken) => LogAsync(message);

            public Task HandleMessageAsync(M4 message, CancellationToken cancellationToken) => LogAsync(message);

            private Task LogAsync(object message)
            {
                _log.Add((GetType(), message));
                return Task.CompletedTask;
            }
        }
    }
}
