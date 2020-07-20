using System;
using EvidentInstruction.Exceptions;
using EvidentInstruction.Helpers;
using EvidentInstruction.Models.Interfaces;
namespace EvidentInstruction.Models
{
    public class TextFile : IFile, IDisposable
    {
        public IDirectory UserDirectory { get; set; } = new UserDirectory();
        public string Filename { get; set; }
        public string Path { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }

        public IFileProvider FileProvider = new FileProvider();
        public IPathProvider PathProvider = new PathProvider();
        public IWebProvider WebProvider = new WebProvider();
        
        public bool IsExist(string filename, string path = null)
        {
            if (string.IsNullOrWhiteSpace(path)) path = UserDirectory.Get();
            string fullpath = PathProvider.Combine(path, filename);
            bool result = (FileProvider.Exist(fullpath)) ? true : false;
            return result;
        }
        public bool DownloadFile(string url, string filename, string pathToSave = null)
        {
            if (string.IsNullOrWhiteSpace(pathToSave)) pathToSave = UserDirectory.Get();
            if (string.IsNullOrWhiteSpace(filename))
            {
                Log.Logger.Warning("Имя файла отсутствует");
                throw new ArgumentException("Имя файла отсутствует");
            }
            else
            {
                bool isValidExtension = FileProvider.CheckFileExtension(filename);
                if (isValidExtension)
                {
                    if (WebProvider.Download(url, pathToSave, filename)) return true;
                    else return false;
                }
                else
                {
                    throw new ValidFileNameException($"Проверьте, что файл \"{filename}\"  имеет расширение .txt");
                    Log.Logger.Warning($"Проверьте, что файл \"{filename}\"  имеет расширение .txt");
                }
            }
        }

        public bool Create(string filename, string path = null, string content = null)
        {
            if (string.IsNullOrWhiteSpace(path)) path = UserDirectory.Get();
            bool isNull = string.IsNullOrEmpty(filename);
            if (!isNull)
            {
                string fullPath = System.IO.Path.Combine(path, filename);
                bool IsTxt = FileProvider.CheckFileExtension(filename);

                if (IsTxt)
                {
                    var exist = FileProvider.Exist(fullPath);
                    if (!exist)
                    {
                        if (string.IsNullOrWhiteSpace(content))
                        {
                            if (FileProvider.Create(filename, path, content)) return true;
                            else return false;

                        }
                        else
                        {
                            if (FileProvider.AppendAllText(filename, path, content)) return true;
                            else return false;
                        }
                    }
                    else
                    {
                        if (FileProvider.WriteAllText(filename, path, content)) return true;
                        else return false;
                    }

                }
                else
                {
                    throw new FileExtensionException($"Файл \"{filename}\" не является текстовым файлом");
                }
            }
            else
            {
                throw new NoFileNameException("Имя файла отсутствует");
            }
        }

        public bool Delete(string filename, string path = null)
        {
            if (string.IsNullOrWhiteSpace(path)) path = UserDirectory.Get();
            string fullpath = PathProvider.Combine(path, filename);
            if (string.IsNullOrWhiteSpace(filename))
            {
                Log.Logger.Warning("Имя файла отсутствует");
                throw new NoFileNameException("Имя файла отсутствует");
            }

            if (FileProvider.Exist(fullpath))
            {
                FileProvider.Delete(fullpath);
                return true;
            }
            else
            {
                Log.Logger.Warning("Файла по указанному пути не существует");
                throw new FileExistException("Файла в директории  не существует");
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
