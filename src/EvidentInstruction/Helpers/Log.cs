using Serilog;

namespace EvidentInstruction.Helpers
{
    public static class Log
    {
        public static ILogger Logger = new LoggerConfiguration()
             .MinimumLevel.Verbose() 
             .WriteTo.ColoredConsole()
             .CreateLogger();
    }
}