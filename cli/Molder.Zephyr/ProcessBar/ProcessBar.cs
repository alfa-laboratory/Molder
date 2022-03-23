using System;

namespace Molder.Zephyr.ProcessBar
{
    public static class ProcessBar
    {
        public static void Write(string text)
        {
            Write(text, true);
        }
        
        public static void Write(string text, bool isComplate)
        {
            Console.Write($"{text}");
        }

        public static void Done()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("DONE!");
            Console.ResetColor();
        }

        public static void Information(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($">>>{text}");
            Console.WriteLine();
            Console.ResetColor();
        }
        
        public static void Error(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"{Environment.NewLine}Error! {text}");
            Console.ResetColor();
        }
        
        public static void Warning(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"{Environment.NewLine}Warning! {text}");
            Console.ResetColor();
        }
    }
}