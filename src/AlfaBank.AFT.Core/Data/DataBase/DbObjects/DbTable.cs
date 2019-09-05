namespace AlfaBank.AFT.Core.Data.DataBase.DbObjects
{
    public class DbTable
    {
        public string Schema { get; set; }
        public string Name { get; set; }

        public string FullName => Schema + "." + Name;

        public string Description { get; set; }
        public DbTableColumns Columns { get; set; }
    }
}
