using Molder.Generator.Infrastructures;
using Molder.Generator.Models.Generators;

namespace Molder.Generator.Extensions
{
    public static partial class GenerationFunctions
    {
        public static string currentDateTime(string format = "dd-MM-yyyy")
        {
            return new FakerGenerator().Current().ToString(format);
        }

        public static string futureDateTime(string format = "dd-MM-yyyy")
        {
            return new FakerGenerator().Future().ToString(format);
        }

        public static string pastDateTime(string format = "dd-MM-yyyy")
        {
            return new FakerGenerator().Past().ToString(format);
        }

        public static string randomDateTime(string format = "dd-MM-yyyy")
        {
            return new FakerGenerator().Between().ToString(format);
        }

        public static string randomInt(string len = "15")
        {
            return new FakerGenerator().Numbers(int.Parse(len));
        }

        public static string randomChars(string len = "15")
        {
            return new FakerGenerator().Chars(int.Parse(len));
        }

        public static string randomString(string len = "15")
        {
            return new FakerGenerator().String(int.Parse(len));
        }

        public static string randomPhone(string format = Constants.PHONE_FORMAT)
        {
            return new FakerGenerator().Phone(format);
        }
    }
}