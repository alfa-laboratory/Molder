using System;

namespace EvidentInstruction.Generator.Models
{
    public static class Constants
    {
        public const string russian = "ru";
        public const string english = "en";
        public const string digits = "0123456789";
        public const string chars = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm";
        public const string ruChars = "ЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮйцукенгшщзхъфывапролджэячсмитьбю";
        public static DateTime startDate = new DateTime(1900, 01, 01);
        public static DateTime endDate = new DateTime(2100, 12, 31);
        public const string phoneMask = "+7##########";
    }
}
