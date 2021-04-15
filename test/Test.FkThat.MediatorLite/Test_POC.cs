using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FkThat.MediatorLite
{
    public class Test_POC
    {
        [Fact]
        public void GetService_ShouldReturnNullForInvalidRegistration()
        {
            ServiceCollection services = new();
            services.AddTransient<Handler>();
            using var serviceProvider = services.BuildServiceProvider();
            var handlerType = typeof(IMessageHandler<>).MakeGenericType(typeof(Message));
            var r = serviceProvider.GetService(handlerType);
            r.Should().BeNull();
        }

        public class Message { }

        public class Handler : IMessageHandler<Message>
        {
            public Task HandleMessageAsync(Message message) => throw new NotImplementedException();
        }
    }
}
