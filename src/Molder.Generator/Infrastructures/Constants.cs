using System;

namespace Molder.Generator.Infrastructures
{
    public static class Constants
    {
        public static readonly DateTime START_DATETIME = new(1900, 01, 01);
        public static readonly DateTime LAST_DATETIME = new(2100, 12, 31);

        public static char NUMBER_CHAR = '#';
        public static char LETTER_CHAR = '?';
        public static char RANDOM_CHAR = '*';

        public const string PHONE_FORMAT = "(###)###-####";

        public const string DEFAULT_LOCALE = "en";
    }
}