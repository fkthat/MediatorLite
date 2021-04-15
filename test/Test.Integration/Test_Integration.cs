using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FkThat.MediatorLite;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Test.Integration
{
    public class Test_Integration
    {
        [Fact]
        public async Task Run()
        {
            ServiceCollection services = new();
            MessageLog log = new();
            services.AddSingleton(log);
            services.AddTransient<Handler1>();
            services.AddTransient<Handler2>();

            services.AddMediator();

            using var serviceProvider = services.BuildServiceProvider();
            var mediator = serviceProvider.GetRequiredService<IMediator>();

            Message0 msg0 = new();
            Message1 msg1 = new();
            Message2 msg2 = new();
            Message3 msg3 = new();
            Message3 msg4 = new();

            await mediator.SendMessageAsync(msg0);
            await mediator.SendMessageAsync(msg1);
            await mediator.SendMessageAsync(msg2);
            await mediator.SendMessageAsync(msg3);
            await mediator.SendMessageAsync(msg4);

            log.Messages.Should().Equal(
                (typeof(Handler1), msg0),
                (typeof(Handler2), msg0),
                (typeof(Handler1), msg1),
                (typeof(Handler1), msg2),
                (typeof(Handler2), msg3),
                (typeof(Handler2), msg4));
        }

        [Fact]
        public async Task Run_Scoped()
        {
            ServiceCollection services = new();
            MessageLog log = new();
            services.AddSingleton(log);
            services.AddTransient<Handler1>();
            services.AddTransient<Handler2>();

            services.AddMediator();

            using var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            Message0 msg0 = new();
            Message1 msg1 = new();
            Message2 msg2 = new();
            Message3 msg3 = new();
            Message3 msg4 = new();

            await mediator.SendMessageAsync(msg0);
            await mediator.SendMessageAsync(msg1);
            await mediator.SendMessageAsync(msg2);
            await mediator.SendMessageAsync(msg3);
            await mediator.SendMessageAsync(msg4);

            log.Messages.Should().Equal(
                (typeof(Handler1), msg0),
                (typeof(Handler2), msg0),
                (typeof(Handler1), msg1),
                (typeof(Handler1), msg2),
                (typeof(Handler2), msg3),
                (typeof(Handler2), msg4));
        }

        public class Message0 { }

        public class Message1 { }

        public class Message2 { }

        public class Message3 { }

        public class Message4 { }

        public class MessageLog
        {
            public IList<(Type, object)> Messages { get; } = new List<(Type, object)>();
        }

        public class Handler1 :
            IMessageHandler<Message0>,
            IMessageHandler<Message1>,
            IMessageHandler<Message2>
        {
            private readonly MessageLog _log;

            public Handler1(MessageLog log)
            {
                _log = log;
            }

            public Task HandleMessageAsync(Message0 message) => OnMessage(message);

            public Task HandleMessageAsync(Message1 message) => OnMessage(message);

            public Task HandleMessageAsync(Message2 message) => OnMessage(message);

            private Task OnMessage(object message)
            {
                _log.Messages.Add((GetType(), message));
                return Task.CompletedTask;
            }
        }

        public class Handler2 :
            IMessageHandler<Message0>,
            IMessageHandler<Message3>,
            IMessageHandler<Message4>
        {
            private readonly MessageLog _log;

            public Handler2(MessageLog log)
            {
                _log = log;
            }

            public Task HandleMessageAsync(Message0 message) => OnMessage(message);

            public Task HandleMessageAsync(Message3 message) => OnMessage(message);

            public Task HandleMessageAsync(Message4 message) => OnMessage(message);

            private Task OnMessage(object message)
            {
                _log.Messages.Add((GetType(), message));
                return Task.CompletedTask;
            }
        }
    }
}
