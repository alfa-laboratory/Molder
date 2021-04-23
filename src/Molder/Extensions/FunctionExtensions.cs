using Microsoft.Extensions.Logging;
using Molder.Helpers;
using System;

namespace Molder.Extensions
{
    public static partial class ParseFunctions
    {
        public static object parseInt(string str)
        {
            try
            {
                return int.Parse(str);
            }catch(FormatException ex)
            {
                Log.Logger().LogWarning($"Parsing string to int return an error {ex.Message}.");
                return str;
            }
        }


        public static object parseLong(string str)
        {
            try
            {
                return long.Parse(str);
            }
            catch (FormatException ex)
            {
                Log.Logger().LogWarning($"Parsing string to long return an error {ex.Message}.");
                return str;
            }
        }

        public static object parseDouble(string str)
        {
            try
            {
                return double.Parse(str);
            }
            catch (FormatException ex)
            {
                Log.Logger().LogWarning($"Parsing string to double return an error {ex.Message}.");
                return str;
            }
        }

        public static object parseBool(string str)
        {
            try
            {
                return bool.Parse(str);
            }
            catch (FormatException ex)
            {
                Log.Logger().LogWarning($"Parsing string to bool return an error {ex.Message}.");
                return str;
            }
        }
    }
}