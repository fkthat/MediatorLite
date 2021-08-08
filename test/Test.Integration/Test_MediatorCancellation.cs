using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FkThat.MediatorLite
{
    public class Test_MediatorCancellation
    {
        [Fact]
        public void SendMessageAsync_ShouldBeCancellable()
        {
            ServiceCollection services = new();
            services.AddMediator();
            services.AddTransient<Handler1>();
            services.AddTransient<Handler2>();
            using var container = services.BuildServiceProvider();
            var mediator = container.GetRequiredService<IMediator>();
            using CancellationTokenSource cancellationTokenSource = new();
            var cancellationToken = cancellationTokenSource.Token;
            var sendTask = mediator.SendMessageAsync(new Message(), cancellationToken);
            cancellationTokenSource.Cancel();
            FluentActions.Awaiting(() => sendTask).Should().Throw<OperationCanceledException>();
        }

        private class Message
        {
        }

        private class Handler1 : IMessageHandler<Message>
        {
            public async Task HandleMessageAsync(Message message, CancellationToken cancellationToken)
            {
                while (true)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await Task.Delay(500);
                }
            }
        }

        private class Handler2 : IMessageHandler<Message>
        {
            public async Task HandleMessageAsync(Message message, CancellationToken cancellationToken)
            {
                await Task.Delay(500);
            }
        }
    }
}
