using FluentAssertions;

namespace EvidentInstruction.Generator.Models
{
    public static class CorrectParams
    {
        public static void ForString(int len, string prefix, string postfix)
        {
            len.Should().BeGreaterThan(0, "Длина строки должна быть положительной.");
            len.Should().BeGreaterThan(prefix.Length + postfix.Length, "Постфикс и префикс в сумме длинее самой строки.");
        }
    }
}
