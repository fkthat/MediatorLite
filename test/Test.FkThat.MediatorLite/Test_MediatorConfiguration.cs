using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace FkThat.MediatorLite
{
    public class Test_MediatorConfiguration
    {
        [Fact]
        public void AddHandler_ShouldAddMessageHandlersAndDispatchers()
        {
            var messageDiscovery = A.Fake<IMessageDiscovery>();
            var messageCompiler = A.Fake<IMessageCompiler>();

            var handler1 = A.Fake<object>(options => options
                .Implements<IMessageHandler<Message0>>()
                .Implements<IMessageHandler<Message1>>()
                .Implements<IMessageHandler<Message2>>());

            var handler2 = A.Fake<object>(options => options
                .Implements<IMessageHandler<Message0>>()
                .Implements<IMessageHandler<Message3>>()
                .Implements<IMessageHandler<Message4>>());

            A.CallTo(() => messageDiscovery.GetHandlerMessageTypes(handler1.GetType()))
                .Returns(new[] { typeof(Message0), typeof(Message1), typeof(Message2) });

            A.CallTo(() => messageDiscovery.GetHandlerMessageTypes(handler2.GetType()))
                .Returns(new[] { typeof(Message0), typeof(Message3), typeof(Message4) });

            var f0 = A.Fake<Func<object, object, Task>>();
            var f1 = A.Fake<Func<object, object, Task>>();
            var f2 = A.Fake<Func<object, object, Task>>();
            var f3 = A.Fake<Func<object, object, Task>>();
            var f4 = A.Fake<Func<object, object, Task>>();

            A.CallTo(() => messageCompiler.GetMessageDispatchFunc(typeof(Message0))).Returns(f0);
            A.CallTo(() => messageCompiler.GetMessageDispatchFunc(typeof(Message1))).Returns(f1);
            A.CallTo(() => messageCompiler.GetMessageDispatchFunc(typeof(Message2))).Returns(f2);
            A.CallTo(() => messageCompiler.GetMessageDispatchFunc(typeof(Message3))).Returns(f3);
            A.CallTo(() => messageCompiler.GetMessageDispatchFunc(typeof(Message4))).Returns(f4);

            MediatorConfiguration sut = new(messageDiscovery, messageCompiler);

            var r = sut
                .AddHandler(handler1.GetType())
                .AddHandler(handler2.GetType());

            r.Should().Be(sut);

            sut.MessageHandlers.Should().BeEquivalentTo(
                (typeof(Message0), handler1.GetType()),
                (typeof(Message1), handler1.GetType()),
                (typeof(Message2), handler1.GetType()),
                (typeof(Message0), handler2.GetType()),
                (typeof(Message3), handler2.GetType()),
                (typeof(Message4), handler2.GetType()));

            sut.MessageDispatchers.Should().BeEquivalentTo(
                new Dictionary<Type, Func<object, object, Task>> {
                    { typeof(Message0), f0 },
                    { typeof(Message1), f1 },
                    { typeof(Message2), f2 },
                    { typeof(Message3), f3 },
                    { typeof(Message4), f4 }
                });
        }

        public class Message0 { }

        public class Message1 { }

        public class Message2 { }

        public class Message3 { }

        public class Message4 { }
    }
}
