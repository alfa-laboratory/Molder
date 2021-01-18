using Bogus;
using System;

namespace Molder.Generator.Models.Providers.Interfaces
{
    public interface IBogusProvider
    {
        Faker Get();

        string Guid();
        DateTime Between(DateTime? start = null, DateTime? end = null);
        string Numbers(int len);
        string Chars(int len);
        string Phone(string format);
        string String2(int len);
        int IntFromTo(int min, int max);
        double DoubleFromTo(double min, double max, int limit);

        DateTime Soon(int days, DateTime? refDate);
        DateTime Future(int yearsToGoForward, DateTime? refDate);
        DateTime Past(int yearsToGoBack, DateTime? refDate);

        string Month();
        string Weekday();
        string Email(string domen);
        string Ip();
        string Url();
        string Sentence(int count);
        string Paragraphs(int min);
        string FirstName();
        string LastName();
        string FullName();
    }
}