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
            if (string.IsNullOrWhiteSpace(path))
            {
                path = UserDirectory.Get();
            }

            var fullpath = PathProvider.Combine(path, filename);

            return FileProvider.Exist(fullpath) ? true : false;
        }

        public bool DownloadFile(string url, string filename, string pathToSave = null)
        {
            if (string.IsNullOrWhiteSpace(pathToSave))
            {
                pathToSave = UserDirectory.Get();
            }

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
                    Log.Logger.Warning($"Проверьте, что файл \"{filename}\" имеет расширение .txt");
                    throw new ValidFileNameException($"Проверьте, что файл \"{filename}\" имеет расширение .txt");
                }
            }
        }

        public bool Create(string filename, string path = null, string content = null)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                path = UserDirectory.Get();
            }

            bool isNull = string.IsNullOrEmpty(filename);
            if (!isNull)
            {
                bool IsTxt = FileProvider.CheckFileExtension(filename);

                if (IsTxt)
                {
                    var exist = IsExist(filename, path);
                    if (!exist)
                    {
                        if (string.IsNullOrWhiteSpace(content))
                        {
                            return FileProvider.Create(filename, path, content);

                        }
                        else
                        {
                            return FileProvider.AppendAllText(filename, path, content);
                        }
                    }
                    else
                    {
                        return FileProvider.WriteAllText(filename, path, content);
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
            if (string.IsNullOrWhiteSpace(path))
            {
                path = UserDirectory.Get();
            }

            if (string.IsNullOrWhiteSpace(filename))
            {
                Log.Logger.Warning("Имя файла отсутствует");
                throw new NoFileNameException("Имя файла отсутствует");
            }

            if (IsExist(filename, path))
            {
                var fullpath = PathProvider.Combine(path, filename);
                return FileProvider.Delete(fullpath);
            }
            else
            {
                Log.Logger.Warning($"Файла \"{filename}\" в директории \"{path}\" не существует");
                throw new FileExistException($"Файла \"{filename}\" в директории \"{path}\" не существует");
            }
        }

        public string GetContent(string filename, string path = null)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                path = UserDirectory.Get();
            }
            
            if (string.IsNullOrWhiteSpace(filename))
            {
                Log.Logger.Warning("Имя файла отсутствует");
                throw new NoFileNameException("Имя файла отсутствует");
            }

            if (IsExist(filename, path))
            {
                return FileProvider.ReadAllText(filename, path);
            }
            else
            {
                Log.Logger.Warning($"Файла \"{filename}\" в директории \"{path}\" не существует");
                throw new FileExistException($"Файла \"{filename}\" в директории \"{path}\" не существует");
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
