using System;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace EvidentInstruction.Generator.Models
{
    public  interface IGenerator
    {
        string GetRandomString(int len, string prefix);
        string GetRandomChars(int len, string prefix);
        string GetRandomNumbers(int len, string prefix);
        string GetGuid();
        string GetRandomPhone(string mask);
        DateTime? GetDate(int day, int month, int year);
        DateTime? GetTime(int hours, int minutes, int seconds, int milliseconds = 0);
        DateTime? GetDateTime(int day, int month, int year, int hours, int minutes, int seconds, int milliseconds = 0);
        DateTime GetCurrentDateTime();
        DateTime? GetOtherDate(int day, int month, int year, DateTime? date = null);
        DateTime? GetRandomDateTime(DateTime? start = null, DateTime? end = null);
    }
}

