using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AlfaBank.AFT.Core.Data.DataBase.DbObjects
{
    public class DbTableColumns : System.Dynamic.DynamicObject, IEnumerable<DbTableColumn>
    {
        private readonly List<DbTableColumn> _columns;

        public DbTableColumns(List<DbTableColumn> columns)
        {
            _columns = columns ?? new List<DbTableColumn>();
        }

        // DynamicObject
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _columns.Select(c => c.Name).Distinct().ToList();
        }

        // DynamicObject
        public override bool TryGetMember(System.Dynamic.GetMemberBinder binder, out object result)
        {
            result = null;

            foreach (var t in _columns)
            {
                if(t.Name != binder.Name)
                {
                    continue;
                }

                if(!t.Type.IsInstanceOfType(binder.ReturnType))
                {
                    throw new ArgumentException(
                        $"Property '{binder.Name}' is not an instance of '{binder.ReturnType.FullName}'!");
                }

                if(t.Value == null && Nullable.GetUnderlyingType(binder.ReturnType) == null)
                {
                    throw new ArgumentNullException($"Property '{binder.Name}' cannot be null!");
                }

                result = _columns.Single(c => c.Name == binder.Name).Value;
                return true;
            }

            throw new ArgumentNullException($"Property '{binder.Name}' does not exist!");
        }

        // DynamicObject
        public override bool TrySetMember(System.Dynamic.SetMemberBinder binder, object value)
        {
            for(var i = 0; i < _columns.Count; i++)
            {
                if(_columns[i].Name != binder.Name)
                {
                    continue;
                }

                if(!_columns[i].Type.IsInstanceOfType(binder.ReturnType))
                {
                    throw new ArgumentException(
                        $"Property '{binder.Name}' is not an instance of '{binder.ReturnType.FullName}'!");
                }

                if(value == null && Nullable.GetUnderlyingType(binder.ReturnType) == null)
                {
                    throw new ArgumentNullException($"Property '{binder.Name}' cannot be null!");
                }

                dynamic col = value;
                _columns[i] = col;
                return true;
            }

            throw new ArgumentNullException($"Property '{binder.Name}' does not exist!");
        }

        // IEnumerable<>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // IEnumerable<>
        public IEnumerator<DbTableColumn> GetEnumerator()
        {
            return _columns.GetEnumerator();
        }
    }
}
