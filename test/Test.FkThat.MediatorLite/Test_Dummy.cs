using FluentAssertions;
using Xunit;

namespace FkThat.MediatorLite
{
    public class Test_Dummy
    {
        [Fact]
        public void Foo_ShouldReturnHello()
        {
            Dummy dummy = new();
            dummy.Foo.Should().Be("Hello");
        }
    }
}
