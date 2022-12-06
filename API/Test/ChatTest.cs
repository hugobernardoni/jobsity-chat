using JobSity.Model.Helpers;
using Xunit;

namespace Test
{
    public class ChatTest
    {
        [Fact]
        public void Should_Be_A_Stock_Command_Message()
        {
            var result = ChatHelper.IsCommand("/stock=aapl.us");
            Assert.True(result);
        }

        [Fact]
        public void Should_Be_A_Unknown_Command_Message()
        {
            var result = ChatHelper.IsCommand("/stock=1");
            Assert.True(result);
        }

        [Fact]
        public void Should_Be_A_Commom_Message()
        {
            var result = ChatHelper.IsCommand("Hello JobSity");
            Assert.False(result);
        }

        [Fact]
        public void Should_Be_A_Valid_Command_Message()
        {
            var result = ChatHelper.GetValidCommandMessage("/stock=aapl.us");
            Assert.Equal("aapl.us", result);
        }
       
    }
}
