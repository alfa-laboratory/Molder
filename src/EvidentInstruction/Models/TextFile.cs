using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using EvidentInstruction.Exceptions;
using EvidentInstruction.Helpers;

namespace EvidentInstruction.Models
{
    public class TextFile : IFile
    {

        private readonly UserDirectory userDirectory;

        public TextFile()
        {
            userDirectory = new UserDirectory();
        }
        public bool CheckFileExistence()
        {
            throw new NotImplementedException();
        }

        private bool CheckFileName(string filename)
        {
            bool result = string.IsNullOrEmpty(filename) ? false : true;
            return result;
        }

        private bool CheckFileExtension(string filename)
        {
            string extension = Path.GetExtension(filename);
            string extMustBeTxt = ".txt";
            bool result = (extension == extMustBeTxt) ? true : false;
            return result;
        }

        private string GetBinDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }
        private bool IsFileExist(string filename)
        {
            string binDirectory = GetBinDirectory();
            string path = Path.Combine(binDirectory, filename);
            bool result = (System.IO.File.Exists(path)) ? true : false;
            return result;
        }
        public void DownloadFileByURL(string url, string filename)
        {
            bool isValidName = CheckFileName(filename);
            bool isValidExtension = CheckFileExtension(filename);
            if ((isValidExtension) && (isValidName))
            {
                using (var webclient = new WebClient())
                {
                    webclient.DownloadFile(url, filename);
                    bool isExist = IsFileExist(filename);
                    if (isExist)
                    {
                        Log.Logger.Warning("Файл " + filename + " скачан ");
                    }
                    else
                    {
                        Log.Logger.Warning("Файл " + filename + " не скачан");
                        throw new FileNotFoundException("Файл " + filename + " скачан");
                    }
                }
            }
            else
            {
                throw new ValidFileNameException("Проверьте, что файл имеет имя и его расширение .txt");
                Log.Logger.Warning("Проверьте, что файл имеет имя и его расширение .txt");
            }
        }

        public void Create(string filename, string content = null)
        {
            string path = userDirectory.Get();
            bool isNull = string.IsNullOrEmpty(filename);
            if (!isNull)
            {
                string fullPath = Path.Combine(path, filename);
                string extension = Path.GetExtension(fullPath);
                string extMustBeTxt = ".txt";
                if (extension == extMustBeTxt)
                {
                    var exist = System.IO.File.Exists(fullPath);
                    if (!exist)
                    {
                        if (string.IsNullOrWhiteSpace(content))
                        {
                            try
                            {
                                System.IO.File.Create(fullPath);
                                Log.Logger.Warning("Пустой файл был создан")
                            }
                            catch (FileNotFoundException e)
                            {
                                Log.Logger.Warning("Файл не был создан");
                            }

                        }
                        else
                        {
                            try
                            {
                                System.IO.File.AppendAllText(fullPath, content);
                                Log.Logger.Warning("Файл был создан");
                            }
                            catch (FileNotFoundException e)
                            {
                                Log.Logger.Warning("Файл не был создан");
                            }

                        }
                    }
                    else
                    {
                        try
                        {
                            System.IO.File.WriteAllText(fullPath, content);
                            Log.Logger.Warning("Файл был перезаписан");
                        }
                        catch (FileNotFoundException e)
                        {
                            Log.Logger.Warning("Файл не был создан");
                        }

                    }

                }
                else
                {
                    throw new FileExtensionException("Файл " + filename + " не является текстовым файлом");
                }
            }
            else
            {
                throw new NoFileNameException("Имя файла отсутствует");
            }
        }


        public void Delete()
        {
            throw new NotImplementedException();
        }

        public string Get()
        {
            throw new NotImplementedException();
        }
    }
}
