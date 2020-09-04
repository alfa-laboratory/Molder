using EvidentInstruction.Models.DateTimeHelpers.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;

namespace EvidentInstruction.Models
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
