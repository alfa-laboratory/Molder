using System;

namespace EvidentInstruction.Generator.Models
{
    public  interface IGenerator
    {
        string GetRandomString(int len, string prefix, string postfix, bool isEng = true);
        string GetRandomChars(int len, string prefix, string postfix, bool isEng = true);
        string GetRandomStringNumbers(int len, string prefix, string postfix);
        int GetRandomInt(int len = 0, int min = 0, int max = 0);
        double GetRandomDouble(int limit, int len = 0, double min = 0, double max = 0);
        string GetGuid();
        string GetRandomPhone(string mask);
        DateTime? GetDate(int day, int month, int year);
        DateTime? GetTime(int hours, int minutes, int seconds, int milliseconds = 0);
        DateTime? GetDateTime(int day, int month, int year, int hours, int minutes, int seconds, int milliseconds = 0);
        DateTime GetCurrentDateTime();
        DateTime? GetOtherDate(int day, int month, int year, DateTime? date = null);
        DateTime? GetRandomDateTime(DateTime? start = null, DateTime? end = null);
        string GetMonth(bool isEng = true);
        string GetWeekday(bool isEng = true);
        string GetEmail(string provider = "gmail.com");
        string GetIp();
        string GetUrl();
        string GetSentence(int count, bool isEng = true);
        string GetParagraph(int min, bool isEng = true);
        string GetFirstName(bool isEng = true);
        string GetLastName(bool isEng = true);
        string GetFullName(bool isEng = true);
    }
}

