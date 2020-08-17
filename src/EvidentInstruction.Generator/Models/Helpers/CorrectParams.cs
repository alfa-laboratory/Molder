using FluentAssertions;
using System.Collections.Generic;

namespace EvidentInstruction.Generator.Models
{
    public static class CorrectParams
    {
        public static void ForString(int len, string prefix, string postfix)
        {
            len.Should().BeGreaterThan(0, "Длина строки должна быть положительной.");
            len.Should().BeGreaterThan(prefix.Length + postfix.Length, "Постфикс и префикс в сумме длинее самой строки.");
        }

        public static void ForNumbers(int len, int min, int max)
        {
            if (len == 0 && min == 0 && max == 0)
            {
                len.Should().NotBe(0, "Невозможно создать число с нулевой длиной, которое больше и меньше 0.");
            }
            len.Should().BeGreaterOrEqualTo(0, "Длина строки не может быть отрицательной.");
            max.Should().BeGreaterOrEqualTo(min, "Максимальное значение не может быть меньше минимального.");
        }

        public static void ForNumbers(int limit, int len, double min, double max)
        {
            limit.Should().BeGreaterOrEqualTo(0, "Ограничение знаков после запятой не может быть отрицательным.");
            if (len == 0 && min == 0 && max == 0)
            {
                len.Should().NotBe(0, "Невозможно создать число с нулевой длиной, которое больше и меньше 0.");
            }
            len.Should().BeGreaterOrEqualTo(0, "Длина строки не может быть отрицательной.");
            max.Should().BeGreaterOrEqualTo(min, "Ограничение по максимуму не может быть меньше ограничения по минимуму.");
        }

        public static void ForLocale(string locale)
        {
            locale.Should().BeOneOf(new List<string>() { Constants.english, Constants.russian }
                , "Выберите русский или английский язык.");
        }
    }
}
