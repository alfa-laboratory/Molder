using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EvidentInstruction.Helpers;
using EvidentInstruction.Models.inerfaces;

namespace EvidentInstruction.Models
{
    public class FileProvider: IFileProvider
    {
        public bool CheckFileExtension(string filename)
        {
            string extension = System.IO.Path.GetExtension(filename);
            string extMustBeTxt = FileExtensions.TXT;
            bool result = (extension == extMustBeTxt) ? true : false;
            return result;
        }

        public bool Exist(string fullpath)
        {
            try
            {
                bool isExist = System.IO.File.Exists(fullpath);
                if (isExist) return true;
                else return false;
            }
            catch (FileNotFoundException e)
            {
                return false;
            }
        }

        public bool AppendAllText(string filename, string path, string content)
        {
            try
            {
                var fullPath = Path.Combine(path, filename);
                System.IO.File.AppendAllText(fullPath, content);
                if (System.IO.File.Exists(fullPath))
                {
                    Log.Logger.Information("Файл \"{filename}\" в директории \"{path}\" был создан ");
                    return true;
                }
                else return false;

            }
            catch (FileNotFoundException e)
            {
                Log.Logger.Warning("Файл \"{filename}\" в директории \"{path}\" не был создан по причине \"{e.Message}\" ");
                return false;
            }
        }

        public bool Create(string filename, string path, string content)
        {
            try
            {
                var fullpath = Path.Combine(path, filename);
                System.IO.File.Create(fullpath);
                if (System.IO.File.Exists(fullpath))
                {
                    Log.Logger.Information("Пустой файл \"{filename}\" в директории \"{fullpath}\" был создан ");
                    return true;
                }
                else return false;
            }
            catch (FileNotFoundException e)
            {
                Log.Logger.Warning("Пустой файл \"{filename}\" в директории \"{fullpath}\" не был создан по причине \"{e.Message}\" ");
                return false;
            }
        }

        public bool WriteAllText(string filename, string path, string content)
        {
            try
            {
                var fullpath = Path.Combine(path, filename);
                System.IO.File.WriteAllText(fullpath, content);
                if (System.IO.File.Exists(fullpath))
                {
                    Log.Logger.Warning("Файл \"{filename}\" в директории \"{fullpath}\" перезаписан");
                    return true;
                }
                else return false;
            }
            catch (FileNotFoundException e)
            {
                Log.Logger.Warning("Файл \"{filename}\" в директории \"{path}\" не был перезаписан по причине \"{e.Message}\" ");
                return false;
            }
        }

        public bool Delete(string fullpath)
        {
            try
            {
                System.IO.File.Delete(fullpath);
                if (System.IO.File.Exists(fullpath))
                {
                    Log.Logger.Information("Файл\"{fullpath}\" не был удален");
                    return false;
                }
                else
                {
                    Log.Logger.Information("Файл\"{fullpath}\" был был удален");
                    return true;
                }
            }
            catch (PathTooLongException e)
            {
                Log.Logger.Warning("Файл\"{fullpath}\" был не был удален из за оишбки \"{e.Message}\"");
                return false;
            }
            catch (IOException e)
            {
                Log.Logger.Warning("Файл\"{fullpath}\" был не был удален из за оишбки \"{e.Message}\"");
                return false;
            }
            catch (UnauthorizedAccessException e)
            {
                Log.Logger.Warning("Файл\"{fullpath}\" был не был удален из за оишбки \"{e.Message}\"");
                return false;
            }
        }
    }
}

