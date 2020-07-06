using FluentAssertions;
using EvidentInstruction.Helpers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace EvidentInstruction.Tests
{
    /// <summary>
    /// Тесты проверки создания сообщений из List.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MessageTests
    {
        [Fact]
        public void CreateMessage_CorrectList_ReturnStr()
        {
            var list = new List<string>
            {
                "1", "2", "3"
            };

            var message = Message.CreateMessage(list);

            message.Should().ContainAll(list);
        }

        [Fact] 
        public void CreateMessage_OneElement_ReturnStr()
        {
            var list = new List<string>
            {
                "1"
            };
            var message = Message.CreateMessage(list);

            message.Should().ContainAll(list);
            message.Should().HaveLength(1);
        }

        [Fact]
        public void CreateMessage_EmptyList_ReturnNull()
        {
            var list = new List<string>();
            var message = Message.CreateMessage(list);

            message.Should().BeNull();
        }
    }
}
