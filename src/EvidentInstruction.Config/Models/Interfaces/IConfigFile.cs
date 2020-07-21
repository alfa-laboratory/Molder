using System.Configuration;
using System.IO;

namespace EvidentInstruction.Config.Models.Interfaces
{
    interface IConfigFile
    {

        //они должны быть перегружены 
        string GetFilePath();
        bool IsFileExists();
        string ReadJsonFile();



    }
}
