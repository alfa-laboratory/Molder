namespace Molder.Infrastructures
{
    public static class StringPattern
    {
        public static string BRACES = @"\[([.\w]*)\]";
        public static string SEARCH = @"{{([^}]*)}}";
    }
}