using System.Data;

namespace AlfaBank.AFT.Core.Data.DataBase.DbObjects
{
    public class DbCommandParameter
    {
        public string Name { get; set; }
        public DbType DbType { get; set; }
        public object Value { get; set; }
    }
}
