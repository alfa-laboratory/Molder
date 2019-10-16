using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AlfaBank.AFT.Core.Model.Common.Support
{
    public class FileSupport
    {
        private readonly string GlobalPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + $"{Path.DirectorySeparatorChar}Files{Path.DirectorySeparatorChar}";
        public void CreateTextFile(string filename, string path = null, string content = null)
        {
            var validPath = GetValidSystemPath(string.IsNullOrWhiteSpace(path) ? GlobalPath : path);

            var exists = Directory.Exists(validPath);
            if (!exists)
            {
                Directory.CreateDirectory(validPath);
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                File.Create(validPath + filename);
            }
            else
            {
                File.AppendAllText(validPath + filename, content);
            }
        }

        public List<string> GetAllFiles(string path = null)
        {
            var validPath = GetValidSystemPath(path);
            var exists = System.IO.Directory.Exists(validPath);
            if (!exists) return null;

            var files = Directory.GetFiles(validPath, "*.*", SearchOption.TopDirectoryOnly);

            return files.Select(Path.GetFileName).ToList();
        }

        public string GetValidFilepath(string filename, string path = null)
        {
            var allFiles = GetAllFiles(path);
            var file = allFiles.Select(f => f == filename).FirstOrDefault();
            if (!file) return null;

            var validPath = string.IsNullOrWhiteSpace(path) ? GlobalPath + filename : path + filename;

            return validPath;
        }

        public void DeleteFolder(string path = null)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                var exists = Directory.Exists(GlobalPath);
                if(!exists)
                {
                    Directory.Delete(GlobalPath, true);
                }
            }
            else
            {
                var exists = Directory.Exists(path);
                if (!exists)
                {
                    Directory.Delete(path);
                }
            }
        }
        private string GetValidSystemPath(string path = null)
        {
            var controlPath = path;
            if (string.IsNullOrWhiteSpace(path))
            {
                controlPath = GlobalPath;
            }

            return $"{controlPath}{Path.DirectorySeparatorChar}";
        }
    }
}
