using System;
using System.Diagnostics.CodeAnalysis;

namespace Molder.Models.DateTimeHelpers
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
