using System.Threading.Tasks;
using FakeItEasy;
using Xunit;

namespace FkThat.MediatorLite
{
    public class Test_DispatchBuilder
    {
        [Fact]
        public async Task BuildDispatchFunc_ShouldReturnHandlerDelegate()
        {
            var handler = A.Fake<IMessageHandler<M>>();
            A.CallTo(() => handler.HandleMessageAsync(A<M>._)).Returns(Task.CompletedTask);
            DispatchBuilder testee = new();
            var result = testee.BuildDispatchFunc(typeof(M));
            M message = new();
            await result(handler, message);
            A.CallTo(() => handler.HandleMessageAsync(message)).MustHaveHappened();
        }

        public class M { }
    }
}
