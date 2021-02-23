using Bogus;
using System;

namespace Molder.Generator.Models.Generators
{
    public interface IFakerGenerator
    {
        string Locale { get; set; }
        Faker Get();

        string Guid();

        DateTime Current();
        DateTime? GetDate(int day, int month, int year);
        DateTime? GetDate(int day, int month, int year, bool future = true, DateTime? refDate = null);
        DateTime? GetDateTime(int day, int month, int year, int hours, int minutes, int seconds, int milliseconds = 0);
        DateTime Between(DateTime? start = null, DateTime? end = null);
        DateTime Soon(int days = 1, DateTime? refDate = null);
        DateTime Future(int yearsToGoForward = 1, DateTime? refDate = null);
        DateTime Past(int yearsToGoBack = 1, DateTime? refDate = null);

        string Numbers(int len);
        string Chars(int len);
        string Phone(string format);
        string String(int len);
        string String2(int len);
        int IntFromTo(int min, int max);
        double DoubleFromTo(double min, double max, int limit);

        string Month();
        string Weekday();
        string Email(string provider);
        string Ip();
        string Url();
        string Sentence(int count);
        string Paragraphs(int min);
        string FirstName();
        string LastName();
        string FullName();
    }
}