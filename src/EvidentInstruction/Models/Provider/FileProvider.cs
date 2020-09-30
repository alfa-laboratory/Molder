using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using EvidentInstruction.Helpers;
using EvidentInstruction.Infrastructures;
using EvidentInstruction.Models.Profider.Interfaces;

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
                return System.IO.File.Exists(fullpath);
            }
            catch (FileNotFoundException e)
            {
                Log.Logger.Warning($"File \"{fullpath}\" not found with error \"{e.Message}\"");
                return false;
            }
        }

        public bool AppendAllText(string filename, string path, string content)
        {
            try
            {
                var fullPath = Path.Combine(path, filename);
                System.IO.File.AppendAllText(fullPath, content);
                if (Exist(fullPath))
                {
                    Log.Logger.Information($"The file \"{filename}\" in the \"{path}\" directory has been created");
                    return true;
                }
                else return false;

            }
            catch (FileNotFoundException e)
            {
                Log.Logger.Warning($"The file \"{filename}\" in the \"{path}\" directory was not created due to \"{e.Message}\"");
                return false;
            }
        }

        public bool Create(string filename, string path, string content)
        {
            try
            {
                var fullpath = Path.Combine(path, filename);
                System.IO.File.Create(fullpath);
                if (Exist(fullpath))
                {
                    Log.Logger.Information($"An empty file \"{filename}\" in the \"{fullpath}\" directory has been created");
                    return true;
                }
                else return false;
            }
            catch (FileNotFoundException e)
            {
                Log.Logger.Warning($"An empty file \"{filename}\" in the \"{Path.Combine(path, filename)}\" directory was not created due to \"{e.Message}\"");
                return false;
            }
        }

        public bool WriteAllText(string filename, string path, string content)
        {
            try
            {
                var fullpath = Path.Combine(path, filename);
                System.IO.File.WriteAllText(fullpath, content);
                if (Exist(fullpath))
                {
                    Log.Logger.Warning($"The file \"{filename}\" in the \"{fullpath}\" directory has been overwritten");
                    return true;
                }
                else return false;
            }
            catch (FileNotFoundException e)
            {
                Log.Logger.Warning($"The file \"{filename}\" in the \"{path}\" directory was not overwritten due to \"{e.Message}\"");
                return false;
            }
        }

        public bool Delete(string fullpath)
        {
            try
            {
                System.IO.File.Delete(fullpath);
                if (Exist(fullpath))
                {
                    Log.Logger.Information($"The file \"{fullpath}\" has not been deleted");
                    return false;
                }
                else
                {
                    Log.Logger.Information($"The file \"{fullpath}\" has been deleted");
                    return true;
                }
            }
            catch (PathTooLongException e)
            {
                Log.Logger.Warning($"The file \"{fullpath}\" was not deleted due to an error \"{e.Message}\"");
                return false;
            }
            catch (IOException e)
            {
                Log.Logger.Warning($"The file \"{fullpath}\" was not deleted due to an error \"{e.Message}\"");
                return false;
            }
            catch (UnauthorizedAccessException e)
            {
                Log.Logger.Warning($"The file \"{fullpath}\" was not deleted due to an error \"{e.Message}\"");
                return false;
            }
        }

        public string ReadAllText(string filename, string path)
        {
            var fullpath = Path.Combine(path, filename);    
            try
            {
                var content = System.IO.File.ReadAllText(fullpath);
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

