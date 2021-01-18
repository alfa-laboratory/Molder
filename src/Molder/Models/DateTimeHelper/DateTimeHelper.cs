using Molder.Models.DateTimeHelpers.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Molder.Models
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
