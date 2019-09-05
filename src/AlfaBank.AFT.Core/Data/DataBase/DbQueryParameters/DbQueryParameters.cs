using System.Collections.Generic;

namespace AlfaBank.AFT.Core.Data.DataBase.DbQueryParameters
{
    public class DbQueryParameters
    {
        public IEnumerable<string> Tables { get; set; }
        public IEnumerable<string> Columns { get; set; }
        public string Conditions { get; set; }
        public int LineLimit { get; set; }
        public int LineSkip { get; set; }
    }
}
