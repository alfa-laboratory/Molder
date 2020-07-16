using System;
using System.IO;
using System.Net;
using System.Threading;
using EvidentInstruction.Exceptions;
using EvidentInstruction.Helpers;
using EvidentInstruction.Models.inerfaces;

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
        public string Filename { get; set; }
        public string Path { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }

        public IFileProvider FileProvider = new FileProvider();
        public IPathProvider PathProvider = new PathProvider();
        public IWebProvider WebProvider = new WebProvider();
        public TextFile()
        {
            UserDirectory = new UserDirectory();
        }
        protected bool CheckFileExtension(string filename)
        {
            string extension = System.IO.Path.GetExtension(filename);
            string extMustBeTxt = FileExtensions.TXT;
            bool result = (extension == extMustBeTxt) ? true : false;
            return result;
        }
        public bool IsExist(string filename, string path)
        {
            if (string.IsNullOrWhiteSpace(path)) path = UserDirectory.Get();
            string fullpath = System.IO.Path.Combine(path, filename);
            bool result = (System.IO.File.Exists(fullpath)) ? true : false;
            return result;
        }
        public bool DownloadFile(string url, string filename, string pathToSave)
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
                    throw new ValidFileNameException("Проверьте, что файл \"{filename}\"  имеет расширение .txt");
                    Log.Logger.Warning("Проверьте, что файл \"{filename}\"  имеет расширение .txt");
                }
            }
        }

        public bool Create(string filename, string path, string content = null)
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
                    throw new FileExtensionException("Файл " + filename + " не является текстовым файлом");
                }
            }
            else
            {
                throw new NoFileNameException("Имя файла отсутствует");
            }
        }

        public bool Delete(string filename, string path)
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
                throw new FileExistException("Файла  в директории \"{0}\" не существует", path);
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
