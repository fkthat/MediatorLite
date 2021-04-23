using System;
using System.Threading.Tasks;
using FakeItEasy;
using Xunit;

namespace FkThat.MediatorLite
{
    public class Test_Mediator
    {
        [Fact]
        public async Task SendMessageAsync_ShouldDispatchMessage()
        {
            // Fake

            var h1 = A.Fake<IMessageHandler<Message0>>(options =>
                options.Implements<IMessageHandler<Message1>>()
                    .Implements<IMessageHandler<Message2>>());

            var h2 = A.Fake<IMessageHandler<Message0>>(options =>
                options.Implements<IMessageHandler<Message3>>()
                    .Implements<IMessageHandler<Message4>>());

            var serviceProvider = A.Fake<IServiceProvider>();

            // Arrange

            A.CallTo(h1)
                .Where(c => c.Method.Name == "HandleMessageAsync")
                .WithReturnType<Task>()
                .Returns(Task.CompletedTask);

            A.CallTo(h2)
                .Where(c => c.Method.Name == "HandleMessageAsync")
                .WithReturnType<Task>()
                .Returns(Task.CompletedTask);

            A.CallTo(() => serviceProvider.GetService(h1.GetType())).Returns(h1);
            A.CallTo(() => serviceProvider.GetService(h2.GetType())).Returns(h2);

            // Invoke

            Mediator sut = new(serviceProvider, new[] { h1.GetType(), h2.GetType() });

            Message0 msg0 = new();
            Message1 msg1 = new();
            Message2 msg2 = new();
            Message1 msg3 = new();
            Message1 msg4 = new();

            await sut.SendMessageAsync(msg0);
            await sut.SendMessageAsync(msg1);
            await sut.SendMessageAsync(msg2);
            await sut.SendMessageAsync(msg3);
            await sut.SendMessageAsync(msg4);

            // Verify

            A.CallTo(h1)
                .Where(c => c.Method.Name == "HandleMessageAsync" && c.Arguments[0] == msg0)
                .MustHaveHappened();

            A.CallTo(h1)
                .Where(c => c.Method.Name == "HandleMessageAsync" && c.Arguments[0] == msg1)
                .MustHaveHappened();

            A.CallTo(h1)
                .Where(c => c.Method.Name == "HandleMessageAsync" && c.Arguments[0] == msg2)
                .MustHaveHappened();

            A.CallTo(h2)
                .Where(c => c.Method.Name == "HandleMessageAsync" && c.Arguments[0] == msg0)
                .MustHaveHappened();

            A.CallTo(h2)
                .Where(c => c.Method.Name == "HandleMessageAsync" && c.Arguments[0] == msg3)
                .MustHaveHappened();

            A.CallTo(h2)
                .Where(c => c.Method.Name == "HandleMessageAsync" && c.Arguments[0] == msg4)
                .MustHaveHappened();
        }

        [Fact]
        public async Task SendMessageAsync_ShouldIgnoreUnknownMessage()
        {
            // Fake

            var h1 = A.Fake<IMessageHandler<Message0>>(options =>
                options.Implements<IMessageHandler<Message1>>()
                    .Implements<IMessageHandler<Message2>>());

            var h2 = A.Fake<IMessageHandler<Message0>>(options =>
                options.Implements<IMessageHandler<Message3>>()
                    .Implements<IMessageHandler<Message4>>());

            var serviceProvider = A.Fake<IServiceProvider>();

            // Arrange

            A.CallTo(h1)
                .Where(c => c.Method.Name == "HandleMessageAsync")
                .WithReturnType<Task>()
                .Returns(Task.CompletedTask);

            A.CallTo(h2)
                .Where(c => c.Method.Name == "HandleMessageAsync")
                .WithReturnType<Task>()
                .Returns(Task.CompletedTask);

            A.CallTo(() => serviceProvider.GetService(h1.GetType())).Returns(h1);
            A.CallTo(() => serviceProvider.GetService(h2.GetType())).Returns(h2);

            // Invoke

            Mediator sut = new(serviceProvider, new[] { h1.GetType(), h2.GetType() });

            Message5 msg5 = new();

            await sut.SendMessageAsync(msg5);

            // Verify

            A.CallTo(h1)
                .Where(c => c.Method.Name == "HandleMessageAsync")
                .MustNotHaveHappened();

            A.CallTo(h2)
                .Where(c => c.Method.Name == "HandleMessageAsync")
                .MustNotHaveHappened();
        }

        public class Message0 { }

        public class Message1 { }

        public class Message2 { }

        public class Message3 { }

        public class Message4 { }

        public class Message5 { }
    }
}
