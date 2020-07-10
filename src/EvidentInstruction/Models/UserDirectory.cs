using System;
using System.Collections.Generic;
using System.Text;

namespace EvidentInstruction.Models
{
    public class UserDirectory: IUserDirectory
    { 
        public string Get()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }
    }
}
