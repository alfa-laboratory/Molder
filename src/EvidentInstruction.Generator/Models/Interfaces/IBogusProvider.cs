using System;
using System.Collections.Generic;
using System.Text;

namespace EvidentInstruction.Generator.Models
{
    interface IBogusProvider
    {
        string Guid();
        DateTime Between(DateTime? start = null, DateTime? end = null);
        int IntNumbers(int len);
        int IntFromTo(int min, int max);
        double DoubleNumbers(int len, int limit);
        double DoubleFromTo(double min, double max, int limit);
        string Phone(string format = "(###)###-####");
        string String(int len);
        string RuString(int len);
        string Chars(int len);
        string RuChars(int len);
        string Month();
        string Weekday();
        string Email(string domen);
        string Ip();
        string Url();
        string Sentence(int count);
        string Paragraph(int min);
        string FirstName();
        string LastName();
        string FullName();
    }
}
