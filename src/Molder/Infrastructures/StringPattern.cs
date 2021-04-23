namespace Molder.Infrastructures
{
    public static class StringPattern
    {
        public static string BRACES = @"\[([.\w]*)\]";
        public static string SEARCH = @"{{([^}]*)}}";
        public static string METHOD = @"^(?<method>\w+)(\((?<parameters>[^)]*)\))$";

        public static string MethodPlaceholder = "method";
        public static string ParametersPlaceholder = "parameters";
    }
}