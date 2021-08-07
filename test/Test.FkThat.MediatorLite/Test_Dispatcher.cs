using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using Xunit;

namespace FkThat.MediatorLite
{
    public class Test_Dispatcher
    {
        [Fact]
        public async Task DispatchAsync_ShouldDispatchMessage()
        {
            // Fake

            var h1 = A.Fake<H1>();
            var h2 = A.Fake<H2>();
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

            A.CallTo(() => serviceProvider.GetService(typeof(H1))).Returns(h1);
            A.CallTo(() => serviceProvider.GetService(typeof(H2))).Returns(h2);

            var discovery = A.Fake<IHandlerDiscovery>();
            var dispatchBuilder = A.Fake<IDispatchBuilder>();

            A.CallTo(() => discovery.FindMessageHandlers())
                .Returns(new[] {
                    (typeof(M0), typeof(H1)),
                    (typeof(M1), typeof(H1)),
                    (typeof(M2), typeof(H1)),
                    (typeof(M0), typeof(H2)),
                    (typeof(M3), typeof(H2)),
                    (typeof(M4), typeof(H2))
                });

            A.CallTo(() => dispatchBuilder.BuildDispatchFunc(typeof(M0)))
                .Returns((h, m) => ((IMessageHandler<M0>)h).HandleMessageAsync((M0)m));

            A.CallTo(() => dispatchBuilder.BuildDispatchFunc(typeof(M1)))
                .Returns((h, m) => ((IMessageHandler<M1>)h).HandleMessageAsync((M1)m));

            A.CallTo(() => dispatchBuilder.BuildDispatchFunc(typeof(M2)))
                .Returns((h, m) => ((IMessageHandler<M2>)h).HandleMessageAsync((M2)m));

            A.CallTo(() => dispatchBuilder.BuildDispatchFunc(typeof(M3)))
                .Returns((h, m) => ((IMessageHandler<M3>)h).HandleMessageAsync((M3)m));

            A.CallTo(() => dispatchBuilder.BuildDispatchFunc(typeof(M4)))
                .Returns((h, m) => ((IMessageHandler<M4>)h).HandleMessageAsync((M4)m));

            // Invoke

            Dispatcher testee = new(discovery, dispatchBuilder);

            M0 msg0 = new();
            M1 msg1 = new();
            M2 msg2 = new();
            M3 msg3 = new();
            M4 msg4 = new();
            using CancellationTokenSource cancellationTokenSource = new();
            var cancellationToken = cancellationTokenSource.Token;

            await testee.DispatchAsync(serviceProvider, msg0, cancellationToken);
            await testee.DispatchAsync(serviceProvider, msg1, cancellationToken);
            await testee.DispatchAsync(serviceProvider, msg2, cancellationToken);
            await testee.DispatchAsync(serviceProvider, msg3, cancellationToken);
            await testee.DispatchAsync(serviceProvider, msg4, cancellationToken);

            // Verify

            A.CallTo(() => h1.HandleMessageAsync(msg0)).MustHaveHappened();
            A.CallTo(() => h1.HandleMessageAsync(msg1)).MustHaveHappened();
            A.CallTo(() => h1.HandleMessageAsync(msg2)).MustHaveHappened();

            A.CallTo(() => h2.HandleMessageAsync(msg0)).MustHaveHappened();
            A.CallTo(() => h2.HandleMessageAsync(msg3)).MustHaveHappened();
            A.CallTo(() => h2.HandleMessageAsync(msg4)).MustHaveHappened();

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
        public async Task DispatchAsync_ShouldIgnoreUnknownMessage()
        {
            using CancellationTokenSource cancellationTokenSource = new();
            var cancellationToken = cancellationTokenSource.Token;

            // Fake

            var h1 = A.Fake<IMessageHandler<M0>>(options =>
                options.Implements<IMessageHandler<M0>>()
                    .Implements<IMessageHandler<M1>>()
                    .Implements<IMessageHandler<M2>>());

            var h2 = A.Fake<IMessageHandler<M0>>(options =>
                options.Implements<IMessageHandler<M0>>()
                    .Implements<IMessageHandler<M3>>()
                    .Implements<IMessageHandler<M4>>());

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

            var discovery = A.Fake<IHandlerDiscovery>();
            var dispatchBuilder = A.Fake<IDispatchBuilder>();

            A.CallTo(() => discovery.FindMessageHandlers())
                .Returns(new[] {
                    (typeof(M0), h1.GetType()),
                    (typeof(M1), h1.GetType()),
                    (typeof(M2), h1.GetType()),
                    (typeof(M0), h2.GetType()),
                    (typeof(M3), h2.GetType()),
                    (typeof(M4), h2.GetType())
                });

            A.CallTo(() => dispatchBuilder.BuildDispatchFunc(typeof(M1)))
                .Returns((h, m) => ((IMessageHandler<M1>)h).HandleMessageAsync((M1)m));

            A.CallTo(() => dispatchBuilder.BuildDispatchFunc(typeof(M1)))
                .Returns((h, m) => ((IMessageHandler<M1>)h).HandleMessageAsync((M1)m));

            A.CallTo(() => dispatchBuilder.BuildDispatchFunc(typeof(M1)))
                .Returns((h, m) => ((IMessageHandler<M1>)h).HandleMessageAsync((M1)m));

            A.CallTo(() => dispatchBuilder.BuildDispatchFunc(typeof(M1)))
                .Returns((h, m) => ((IMessageHandler<M1>)h).HandleMessageAsync((M1)m));

            A.CallTo(() => dispatchBuilder.BuildDispatchFunc(typeof(M1)))
                .Returns((h, m) => ((IMessageHandler<M1>)h).HandleMessageAsync((M1)m));

            A.CallTo(() => dispatchBuilder.BuildDispatchFunc(typeof(M1)))
                .Returns((h, m) => ((IMessageHandler<M1>)h).HandleMessageAsync((M1)m));

            // Invoke

            Dispatcher testee = new(discovery, dispatchBuilder);

            M5 msg5 = new();

            await testee.DispatchAsync(serviceProvider, msg5, cancellationToken);

            // Verify

            A.CallTo(h1).MustNotHaveHappened();
            A.CallTo(h2).MustNotHaveHappened();
        }

        public class M0 { }

        public class M1 { }

        public class M2 { }

        public class M3 { }

        public class M4 { }

        public class M5 { }

        public abstract class H1 : IMessageHandler<M0>, IMessageHandler<M1>, IMessageHandler<M2>
        {
            public abstract Task HandleMessageAsync(M0 message);

            public abstract Task HandleMessageAsync(M1 message);

            public abstract Task HandleMessageAsync(M2 message);
        }

        public abstract class H2 : IMessageHandler<M0>, IMessageHandler<M3>, IMessageHandler<M4>
        {
            public abstract Task HandleMessageAsync(M0 message);

            public abstract Task HandleMessageAsync(M3 message);

            public abstract Task HandleMessageAsync(M4 message);
        }
    }
}
