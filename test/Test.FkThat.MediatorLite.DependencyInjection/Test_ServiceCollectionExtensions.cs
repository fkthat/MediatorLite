using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FkThat.MediatorLite.DependencyInjection
{
    public class Test_ServiceCollectionExtensions
    {
        [Fact]
        public async Task AddMediator_ShouldRegisterMediator()
        {
            ServiceCollection services = new();
            HashSet<(object, object)> calls = new();
            Handler1 h1 = new((h, m) => calls.Add((h, m)));
            Handler2 h2 = new((h, m) => calls.Add((h, m)));
            services.AddSingleton(c => h1);
            services.AddSingleton(c => h2);
            services.AddMediator(config => config.AddHandler<Handler1>().AddHandler<Handler2>());
            using var container = services.BuildServiceProvider();
            var mediator = container.GetRequiredService<IMediator>();
            Message0 m0 = new();
            Message1 m1 = new();
            Message2 m2 = new();
            Message3 m3 = new();
            Message4 m4 = new();
            await mediator.SendMessageAsync(m0);
            await mediator.SendMessageAsync(m1);
            await mediator.SendMessageAsync(m2);
            await mediator.SendMessageAsync(m3);
            await mediator.SendMessageAsync(m4);

            calls.Should().BeEquivalentTo(
                (h1, m0), (h1, m1), (h1, m2), (h2, m0), (h2, m3), (h2, m4));
        }

        private class Message0 { }

        private class Message1 { }

        private class Message2 { }

        private class Message3 { }

        private class Message4 { }

        private class Handler1 : IMessageHandler<Message0>, IMessageHandler<Message1>, IMessageHandler<Message2>
        {
            private readonly Action<object, object> _callback;

            public Handler1(Action<object, object> callback)
            {
                _callback = callback;
            }

            public Task HandleMessageAsync(object message)
            {
                _callback(this, message);
                return Task.CompletedTask;
            }

            public Task HandleMessageAsync(Message0 message) => HandleMessageAsync((object)message);

            public Task HandleMessageAsync(Message1 message) => HandleMessageAsync((object)message);

            public Task HandleMessageAsync(Message2 message) => HandleMessageAsync((object)message);
        }

        private class Handler2 : IMessageHandler<Message0>, IMessageHandler<Message3>, IMessageHandler<Message4>
        {
            private readonly Action<object, object> _callback;

            public Handler2(Action<object, object> callback)
            {
                _callback = callback;
            }

            public Task HandleMessageAsync(object message)
            {
                _callback(this, message);
                return Task.CompletedTask;
            }

            public Task HandleMessageAsync(Message0 message) => HandleMessageAsync((object)message);

            public Task HandleMessageAsync(Message3 message) => HandleMessageAsync((object)message);

            public Task HandleMessageAsync(Message4 message) => HandleMessageAsync((object)message);
        }
    }
}
