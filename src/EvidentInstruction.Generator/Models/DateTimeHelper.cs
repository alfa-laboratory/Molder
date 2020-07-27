using System;
using System.Diagnostics.CodeAnalysis;

namespace EvidentInstruction.Generator.Models
{
    [ExcludeFromCodeCoverage]
    public class DateTimeHelper : IDateTimeHelper
    {
        public DateTime GetDateTimeNow()
        {
            return DateTime.Now;
        }
    }
}
