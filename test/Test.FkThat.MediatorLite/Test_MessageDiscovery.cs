using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace FkThat.MediatorLite
{
    public class Test_MessageDiscovery
    {
        [Fact]
        public void GetHandlerMessageTypes_ShouldReturnHandlerMessageTypes()
        {
            var handler = A.Fake<object>(options => options
                .Implements<IMessageHandler<Message1>>()
                .Implements<IMessageHandler<Message2>>());

            var sut = new MessageDiscovery();
            var r = sut.GetHandlerMessageTypes(handler.GetType());
            r.Should().BeEquivalentTo(typeof(Message1), typeof(Message2));
        }

        public class Message1 { }

        public class Message2 { }
    }
}
