using AlfaBank.AFT.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AlfaBank.AFT.Core.Model.KeyValues
{
    public class KeyValues : KeyValues<object>
    {
        public KeyValues()
        {
        }

        public KeyValues(Dictionary<string, object> dictionary)
          : base(dictionary)
        {
        }

        public static object GetValueHierarchically(KeyValues kv, string path)
        {
            return GetValueHierarchically(path, kv, typeof(object));
        }

        public static KeyValues CreateFromDataRow(DataRow row, string parentProp = null)
        {
            var keyValues1 = new KeyValues();
            foreach(DataColumn column in row.Table.Columns)
            {
                if((parentProp ?? string.Empty).Length > 0 && !column.ColumnName.StartsWith(parentProp))
                {
                    continue;
                }

                if(parentProp == null)
                {
                    continue;
                }

                var strArray = column.ColumnName.Substring(parentProp.Length > 0 ? parentProp.Length + 1 : 0).Split('.');
                if(strArray.Length == 0 || keyValues1.ContainsKey(strArray[0]))
                {
                    continue;
                }

                keyValues1[strArray[0]] = null;
                if(strArray.Length > 1)
                {
                    var keyValues2 = keyValues1;
                    var index = strArray[0];
                    var row1 = row;
                    var parentProp1 = parentProp.Length <= 0 ? strArray[0] : string.Join(".", parentProp, strArray[0]);
                    var fromDataRow = CreateFromDataRow(row1, parentProp1);
                    keyValues2[index] = fromDataRow;
                    if(strArray[0].EndsWith("[]"))
                    {
                        keyValues1[strArray[0]] = new KeyValues[1]
                        {
                            (KeyValues)keyValues1[strArray[0]]
                        };
                    }
                }
                else if(row[column] == null || row[column] == DBNull.Value)
                {
                    keyValues1[strArray[0]] = column.DataType.GetDefault();
                }
                else
                {
                    keyValues1[strArray[0]] = row[column];
                }
            }

            return keyValues1;
        }

        private static object GetValueHierarchically(
          string path,
          object parent,
          Type outType)
        {
            if(string.IsNullOrWhiteSpace(path))
            {
                return null;
            }

            var aszLevels = path.Split('.');
            if(parent == null || aszLevels.Length < 1)
            {
                return parent;
            }

            var expectedArray = aszLevels[0].IndexOf('[') >= 0 && aszLevels[0].IndexOf(']') > aszLevels[0].IndexOf('[');
            var source = new List<object>();
            switch(parent)
            {
                case KeyValues values:
                {
                    var key = values.Keys.FirstOrDefault(k =>
                    {
                        if(expectedArray && k.StartsWith(aszLevels[0].Substring(0, aszLevels[0].IndexOf('[') + 1)))
                        {
                            return true;
                        }

                        if(!expectedArray)
                        {
                            return k == aszLevels[0];
                        }

                        return false;
                    });
                    if(!values.ContainsKey(key))
                    {
                        return null;
                    }

                    var obj1 = values[key];
                    if(expectedArray)
                    {
                        if(int.TryParse(aszLevels[0].Substring(aszLevels[0].IndexOf('[') + 1, aszLevels[0].IndexOf(']') - (aszLevels[0].IndexOf('[') + 1)).Trim(), out var result))
                        {
                            obj1 = (obj1 as object[])?[result];
                        }
                    }

                    if(!(obj1 is object[]))
                    {
                        obj1 = new object[1] { obj1 };
                    }

                    foreach(var parent1 in obj1 as object[])
                    {
                        var obj2 = aszLevels.Length != 1 ? GetValueHierarchically(string.Join(".", aszLevels.Skip(1).ToArray()), parent1, outType) : parent1;
                        if(obj2 is object[] objects)
                        {
                            source.AddRange(objects);
                        }
                        else
                        {
                            source.Add(obj2);
                        }
                    }

                    break;
                }

                case KeyValues[] valueses:
                {
                    foreach(var t in valueses)
                    {
                        var valueHierarchically = GetValueHierarchically(string.Join(".", aszLevels.Skip(1).ToArray()), t, outType);
                        if(valueHierarchically is object[] objects)
                        {
                            source.AddRange(objects);
                        }
                        else
                        {
                            source.Add(valueHierarchically);
                        }
                    }

                    break;
                }
            }

            if(outType.IsArray || (source.Count > 1 && outType == typeof(object)))
            {
                return source.ToArray();
            }

            if(source.Count == 1)
            {
                return source.First();
            }

            if(source.Count > 1)
            {
                throw new Exception("Expected a single value, but got an array");
            }

            return null;
        }
    }
}
