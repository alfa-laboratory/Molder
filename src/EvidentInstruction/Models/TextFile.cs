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
        protected bool CheckFileExtension(string filename)
        {
            string extension = Path.GetExtension(filename);
            string extMustBeTxt = FileExtensions.TXT;
            bool result = (extension == extMustBeTxt) ? true : false;
            return result;
        }
        public bool IsExist(string filename, string path)
        {
            if (string.IsNullOrWhiteSpace(path)) path = UserDirectory.Get();
            string fullpath = Path.Combine(path, filename);
            bool result = (System.IO.File.Exists(fullpath)) ? true : false;
            return result;
        }
        public void DownloadFileByURL(string url, string filename, string pathToSave)
        {
            if (string.IsNullOrWhiteSpace(pathToSave)) pathToSave = UserDirectory.Get();
            if (string.IsNullOrWhiteSpace(filename))
            {
                Log.Logger.Warning("Имя файла отсутствует");
                throw new ArgumentException("Имя файла отсутствует");
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
                                Log.Logger.Information("Файл  \"{0}\" был скачан в директорию \"{1}\" ", filename, pathToSave);
                            }
                            else
                            {
                                Log.Logger.Warning("Файл \"{0}\" не скачан",filename);
                                throw new FileNotFoundException("Файл \"{0}\" не скачан", filename);
                            }
                        }
                    }
                    catch (WebException e)
                    {
                        Log.Logger.Warning("Файл \"{0}\" не скачан по ошибке {1}", filename,e.Message);
                    }
                }
                else
                {
                    throw new FileExtensionException("Проверьте, что файл \"{0}\" имеет расширение {1} ", filename, FileExtensions.TXT);
                    Log.Logger.Warning("Проверьте, что файл \"{0}\"  имеет расширение {1}",filename, FileExtensions.TXT);
                }
            }
        }
        public void Create(string filename,string path, string content = null)
        {
            if (string.IsNullOrWhiteSpace(path)) path = UserDirectory.Get();
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
                                Log.Logger.Information("Пустой файл \"{0}\" был создан в директории \"{1}\" ",filename, path);
                            }
                            catch (FileNotFoundException e)
                            {
                                Log.Logger.Warning("Пустой файл \"{0}\" не был создан с ошибкой \"{1}\"", filename,e.Message);
                            }
                        }
                        else
                        {
                            try
                            {
                                System.IO.File.AppendAllText(fullPath, content);
                                Log.Logger.Information("Файл \"{0}\" был создан в директории \"{1}\" ", filename, path);
                            }
                            catch (FileNotFoundException e)
                            {
                                Log.Logger.Warning("Пустой файл \"{0}\" не был создан с ошибкой \"{1}\"", filename, e.Message);
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            System.IO.File.WriteAllText(fullPath, content);
                            Log.Logger.Information("Файл \"{0}\" в директории \"{1}\" был перезаписан ", filename, path);
                        }
                        catch (FileNotFoundException e)
                        {
                            Log.Logger.Warning("Файл \"{0}\" не был перезаписан с ошибкой \"{1}\"", filename, e.Message);
                        }
                    }
                }
                else
                {
                    throw new FileExtensionException("Файл \"{0}\" не является текстовым файлом", filename);
                    Log.Logger.Warning("Файл \"{0}\" не является текстовым файлом", filename);
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
            if (string.IsNullOrWhiteSpace(path)) path = UserDirectory.Get();
            string fullpath = Path.Combine(path, filename);
            string notfullpath = fullpath.Replace("\\", "");
            if (string.IsNullOrWhiteSpace(filename))
            {
                Log.Logger.Warning("Имя файла отсутствует");
                throw new NoFileNameException("Имя файла отсутствует");
            }
            if (string.IsNullOrWhiteSpace(fullpath))
            {
                Log.Logger.Warning("Путь до файла \"{0}\"  введен не верно", filename);
                throw new ArgumentNullException("Путь до файла \"{0}\"  введен не верно", filename);
            }
            if (System.IO.File.Exists(fullpath)) System.IO.File.Delete(fullpath);
            else
            {
                Log.Logger.Warning("Файла \"{0}\" по указанному пути \"{1}\"  не существует",filename, path);
                throw new FileExistException("Файла \"{0}\" по указанному пути \"{1}\"  не существует", filename, path);
            }

            if (IsExist(filename, path))
            {
                Log.Logger.Warning("Файл \"{0}\" по указанному пути \"{1}\"  не  был удален", filename, path);
                throw new FileExistException("Файл \"{0}\" по указанному пути \"{1}\"  не  был удален", filename, path);
            }
            else
            {
                Log.Logger.Information("Файл \"{0}\" по указанному пути \"{1}\"   был успешно удален", filename, path);
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
