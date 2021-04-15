using System.Threading.Tasks;
using FakeItEasy;
using Xunit;

namespace FkThat.MediatorLite
{
    public class Test_MessageCompiler
    {
        [Fact]
        public async Task GetMessageDispatchFunc_ShouldCreateDispatchFunc()
        {
            var handler = A.Fake<IMessageHandler<Message>>();
            MessageCompiler sut = new();
            var r = sut.GetMessageDispatchFunc(typeof(Message));
            Message message = new();
            await r(handler, message);
            A.CallTo(() => handler.HandleMessageAsync(message)).MustHaveHappened();
        }

        public class Message { }
    }
}
