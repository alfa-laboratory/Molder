using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using EvidentInstruction.Helpers;
using EvidentInstruction.Models.Interfaces;

namespace EvidentInstruction.Models
{
    [ExcludeFromCodeCoverage]
    public class WebProvider: IWebProvider
    {
        public bool Download(string url, string pathToSave, string filename)
        {
            try
            {
                using (var webclient = new WebClient())
                {
                    string endPath = new EvidentInstruction.Models.TextFile().PathProvider.Combine(pathToSave, filename);
                    webclient.DownloadFile(new Uri(url), endPath);
                    bool isExist = new EvidentInstruction.Models.TextFile().IsExist(filename, pathToSave);
                    if (isExist)
                    {
                        Log.Logger.Warning( $"Файл  \"{filename}\" был скачан в директорию \"{endPath}\" ");
                        return true;
                    }
                    else
                    {
                        Log.Logger.Warning($"Файл \"{filename}\" не скачан");
                        throw new FileNotFoundException($"Файл \"{filename}\" не скачан");
                    }
                }
            }

            catch (WebException e)
            {
                Log.Logger.Error($"Файл \"{filename}\" не скачан из за ошибки \"{e.Message}\"");
                return false;
            }
        }
    }
}
