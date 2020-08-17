using System;

namespace EvidentInstruction.Generator.Models
{
    public  interface IGenerator
    {

        string GetRandomString(int len, string prefix, string postfix, string locale);

        string GetRandomChars(int len, string prefix, string postfix, string locale);

        string GetRandomStringNumbers(int len, string prefix, string postfix);

        int GetRandomInt(int len = 0, int min = 0, int max = 0);

        double GetRandomDouble(int limit, int len = 0, double min = 0, double max = 0);

        string GetGuid();

        string GetRandomPhone(string mask = null);

        DateTime? GetDate(int day, int month, int year);

        DateTime? GetTime(int hours, int minutes, int seconds, int milliseconds = 0);

        DateTime? GetDateTime(int day, int month, int year, int hours, int minutes, int seconds, int milliseconds = 0);
        DateTime GetCurrentDateTime();
        DateTime? GetOtherDate(int day, int month, int year, DateTime? date = null);
        DateTime? GetRandomDateTime(DateTime? start = null, DateTime? end = null);

        string GetMonth(string locale);

        string GetWeekday(string locale);

        string GetEmail(string domen = "gmail.com");

        string GetIp();

        string GetUrl();
        string GetSentence(int count, string locale);
        string GetParagraph(int min, string locale);

        string GetFirstName(string locale);

        string GetLastName(string locale);

        string GetFullName(string locale);
    }
}

