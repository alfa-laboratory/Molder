using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using EvidentInstruction.Helpers;
using EvidentInstruction.Models.Interfaces;

namespace EvidentInstruction.Models
{
    [ExcludeFromCodeCoverage]
    public class FileProvider: IFileProvider
    {
        public bool CheckFileExtension(string filename)
        {
            string extension = Path.GetExtension(filename);
            string extMustBeTxt = FileExtensions.TXT;
            bool result = (extension == extMustBeTxt) ? true : false;
            return result;
        }

        public bool Exist(string fullpath)
        {
            try
            {
                bool isExist = File.Exists(fullpath);
                if (isExist) return true;
                else return false;
            }
            catch (FileNotFoundException e)
            {
                Log.Logger.Warning($"Файл \"{fullpath}\" не найден с ошибкой \"{e.Message}\" ");
                return false;
            }
        }

        public bool AppendAllText(string filename, string path, string content)
        {
            try
            {
                var fullPath = Path.Combine(path, filename);
                File.AppendAllText(fullPath, content);
                if (Exist(fullPath))
                {
                    Log.Logger.Information($"Файл \"{filename}\" в директории \"{path}\" был создан ");
                    return true;
                }
                else return false;

            }
            catch (FileNotFoundException e)
            {
                Log.Logger.Warning($"Файл \"{filename}\" в директории \"{path}\" не был создан по причине \"{e.Message}\" ");
                return false;
            }
        }

        public bool Create(string filename, string path, string content)
        {
            try
            {
                var fullpath = Path.Combine(path, filename);
                File.Create(fullpath);
                if (Exist(fullpath))
                {
                    Log.Logger.Information($"Пустой файл \"{filename}\" в директории \"{fullpath}\" был создан ");
                    return true;
                }
                else return false;
            }
            catch (FileNotFoundException e)
            {
                Log.Logger.Warning($"Пустой файл \"{filename}\" в директории \"{Path.Combine(path, filename)}\" не был создан по причине \"{e.Message}\" ");
                return false;
            }
        }

        public bool WriteAllText(string filename, string path, string content)
        {
            try
            {
                var fullpath = Path.Combine(path, filename);
                File.WriteAllText(fullpath, content);
                if (Exist(fullpath))
                {
                    Log.Logger.Warning($"Файл \"{filename}\" в директории \"{fullpath}\" перезаписан");
                    return true;
                }
                else return false;
            }
            catch (FileNotFoundException e)
            {
                Log.Logger.Warning($"Файл \"{filename}\" в директории \"{path}\" не был перезаписан по причине \"{e.Message}\" ");
                return false;
            }
        }

        public bool Delete(string fullpath)
        {
            try
            {
                File.Delete(fullpath);
                if (Exist(fullpath))
                {
                    Log.Logger.Information($"Файл\"{fullpath}\" не был удален");
                    return false;
                }
                else
                {
                    Log.Logger.Information($"Файл\"{fullpath}\" был был удален");
                    return true;
                }
            }
            catch (PathTooLongException e)
            {
                Log.Logger.Warning($"Файл\"{fullpath}\" был не был удален из за оишбки \"{e.Message}\"");
                return false;
            }
            catch (IOException e)
            {
                Log.Logger.Warning($"Файл\"{fullpath}\" был не был удален из за оишбки \"{e.Message}\"");
                return false;
            }
            catch (UnauthorizedAccessException e)
            {
                Log.Logger.Warning($"Файл\"{fullpath}\" был не был удален из за оишбки \"{e.Message}\"");
                return false;
            }
        }

        public string ReadAllText(string filename, string path)
        {
            var fullpath = Path.Combine(path, filename);    
            try
            {
                var content = File.ReadAllText(fullpath);
                Log.Logger.Information($"File \" {fullpath} \" has been read.");

                return content;
            }
            catch(FileNotFoundException e)
            {
                Log.Logger.Warning($"File \"{fullpath}\" not found. \" {e.Message}\" ");
                return null;
            }
            catch(PathTooLongException e)
            {
                Log.Logger.Warning($"File \"{fullpath}\" not found. \" {e.Message}\" ");
                return null;
            }
            catch(IOException e)
            {
                Log.Logger.Warning($"File \"{fullpath}\" not found. \" {e.Message}\" ");
                return null;
            }
            
        }
    }
}

