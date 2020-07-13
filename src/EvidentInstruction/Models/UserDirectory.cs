using System;
using System.Collections.Generic;
using System.Text;

namespace EvidentInstruction.Models
{
    public class UserDirectory: IDirectory
    { 
        public string Get()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }
    }
}
