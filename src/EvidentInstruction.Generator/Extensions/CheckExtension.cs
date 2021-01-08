using FluentAssertions;

namespace EvidentInstruction.Generator.Extensions
{
    public static class CheckExtension
    {
        public static void Check(this int length)
        {
            length.Should().BeGreaterThan(0, "the length must be positive");
        }

        public static void Check(this int length, string prefix, string postfix)
        {
            length.Should().BeGreaterThan(prefix.Length + postfix.Length, "postfix and/or prefix are longer than the string itself");
        }

        public static void Limit(int limit)
        {
            limit.Should().BeGreaterThan(0, "the decimal place limit cannot be negative");
        }

        public static void BeGreaterThan(this int max, int min)
        {
            max.Should().BeGreaterThan(min, "the maximum value cannot be less than the minimum");
        }

        public static void BeGreaterThan(this double max, double min)
        {
            max.Should().BeGreaterThan(min, "the maximum value cannot be less than the minimum");
        }

        public static void Numbers(this int length, int min, int max)
        {
            length.Check();
            max.BeGreaterThan(min);
        }

        public static void Doubles(this int length, double min, double max, int limit)
        {
            length.Check();
            max.BeGreaterThan(min);
            Limit(limit);
        }
    }
}