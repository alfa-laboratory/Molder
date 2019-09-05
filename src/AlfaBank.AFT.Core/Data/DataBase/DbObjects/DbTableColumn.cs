using System;

namespace AlfaBank.AFT.Core.Data.DataBase.DbObjects
{
    public class DbTableColumn
    {
        public DbTable Table { get; set; }

        public string Name { get; set; }
        public string DisplayName { get; set; }

        public string Description { get; set; }

        public int Ordinal { get; set; }

        public bool IsIdentity { get; set; }

        public string DbTypeName { get; set; }
        public Type DbType { get; set; }
        public Type Type { get; set; }

        public string DefaultValueDb { get; set; }
        public bool DefaultValueSpecified { get; set; }
        public dynamic DefaultValue { get; set; }

        public bool ValueSpecified { get; set; }
        public dynamic Value { get; set; }
    }
}
