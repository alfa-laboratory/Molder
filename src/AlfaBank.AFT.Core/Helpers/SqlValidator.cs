using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AlfaBank.AFT.Core.Helpers
{
    public static class SqlValidator
    {
        public static ICollection<string> IsSqlQueryValid(string sql)
        {
            var errors = new List<string>();
            var parser = new TSql140Parser(false);

            using(TextReader reader = new StringReader(sql))
            {
                parser.Parse(reader, out var parseErrors);
                if (parseErrors == null || parseErrors.Any())
                {
                    return errors;
                }

                errors = parseErrors.Select(e => e.Message).ToList();
                return errors;
            }
        }
    }
}
