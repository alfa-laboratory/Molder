using System;
using System.Data;

namespace AlfaBank.AFT.Core.Models.Web.Elements
{
    public class TableElement : Element
    {
        public TableElement(string name, string xpath) : base(name, xpath) { }

        public virtual DataTable GetTable()
        {
            throw new NotImplementedException();
        }
    }
}