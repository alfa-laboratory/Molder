using System;
using System.IO;
using System.Net;
using System.Threading;
using EvidentInstruction.Exceptions;
using EvidentInstruction.Helpers;


namespace EvidentInstruction.Models
{
    public class TextFile : IFile, IDisposable
    {
        private ThreadLocal<UserDirectory> _userDirectory { get; } = new ThreadLocal<UserDirectory>();
        public UserDirectory UserDirectory
        {
            get
            {
                if (_userDirectory != null) return _userDirectory.Value;
                return null;
            }
            set => _userDirectory.Value = value;
        }
        public TextFile()
        {
            UserDirectory = new UserDirectory();
        }
        private bool CheckFileExtension(string filename)
        {
            string extension = Path.GetExtension(filename);
            string extMustBeTxt = FileExtensions.txt;
            bool result = (extension == extMustBeTxt) ? true : false;
            return result;
        }
        public bool IsExist(string filename, string path)
        {
            string fullpath = Path.Combine(path, filename);
            bool result = (System.IO.File.Exists(fullpath)) ? true : false;
            return result;
        }
        public void DownloadFileByURL(string url, string filename, string pathToSave)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                Log.Logger.Warning("Отсутствует имя файла");
                throw new ArgumentException("Имя файла " + filename + " отсутствует");
            }
            else
            {
                bool isValidExtension = CheckFileExtension(filename);
                if (isValidExtension)
                {
                    try
                    {
                        using (var webclient = new WebClient())
                        {
                            string endPath = Path.Combine(pathToSave, filename);
                            webclient.DownloadFile(new Uri(url), endPath);
                            bool isExist = IsExist(filename, pathToSave);
                            if (isExist)
                            {
                                Log.Logger.Information("Файл " + filename + " скачан ");
                            }
                            else
                            {
                                Log.Logger.Warning("Файл " + filename + " не скачан");
                                throw new FileNotFoundException("Файл " + filename + " скачан");
                            }
                        }
                    }
                    catch (WebException e)
                    {
                        Log.Logger.Warning(e.Message);
                    }
                }
                else
                {
                    throw new ValidFileNameException("Проверьте, что файл " + filename + " имеет расширение .txt");
                    Log.Logger.Warning("Проверьте, что файл " + filename + " имеет расширение .txt");
                }
            }
        }
        public void Create(string filename, string content = null)
        {
            string path = UserDirectory.Get();
            bool isNull = string.IsNullOrWhiteSpace(filename);
            if (!isNull)
            {
                string fullPath = Path.Combine(path, filename);
                bool IsTxt = CheckFileExtension(filename);

                if (IsTxt)
                {
                    var exist = System.IO.File.Exists(fullPath);
                    if (!exist)
                    {
                        if (string.IsNullOrWhiteSpace(content))
                        {
                            try
                            {
                                System.IO.File.Create(fullPath);
                                Log.Logger.Information("Пустой файл был создан");
                            }
                            catch (FileNotFoundException e)
                            {
                                Log.Logger.Warning(e.Message);
                            }
                        }
                        else
                        {
                            try
                            {
                                System.IO.File.AppendAllText(fullPath, content);
                                Log.Logger.Information("Файл был создан");
                            }
                            catch (FileNotFoundException e)
                            {
                                Log.Logger.Warning(e.Message);
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            System.IO.File.WriteAllText(fullPath, content);
                            Log.Logger.Information("Файл был перезаписан");
                        }
                        catch (FileNotFoundException e)
                        {
                            Log.Logger.Warning(e.Message);
                        }
                    }
                }
                else
                {
                    throw new FileExtensionException("Файл " + filename + " не является текстовым файлом");
                    Log.Logger.Warning("Файл " + filename + " не является текстовым файлом");
                }
            }
            else
            {
                throw new NoFileNameException("Имя файла отсутствует");
                Log.Logger.Warning("Имя файла отсутствует");
            }
        }
        public void Delete(string filename, string path)
        {
            string fullpath = Path.Combine(path, filename);
            string notfullpath = fullpath.Replace("\\", "");

            if (string.IsNullOrWhiteSpace(fullpath))
            {
                Log.Logger.Warning("Путь до файла не указан");
                throw new ArgumentNullException("Путь до файла " + path + " введен не верно");
            }
            if (System.IO.File.Exists(fullpath)) System.IO.File.Delete(fullpath);
            else
            {
                Log.Logger.Warning("Файла по указанному пути не существует");
                throw new FileNotFoundException("Файла по указанному пути не существует");
            }

            if (IsExist(filename, path))
            {
                Log.Logger.Warning("Файл " + filename + "не был удален");
                throw new FileExistException("Файл " + filename + " не был удален");
            }
            else
            {
                Log.Logger.Information("Файл " + filename + " был удален");
                Console.WriteLine("Файл " + filename + "  был удален");
            }
        }
        public string Get()
        {
            throw new NotImplementedException();
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
