using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBISCCMWeb.Utility
{
    public static class DataTableX
    {
        public static IEnumerable<dynamic> AsDynamicEnumerable(this DataTable table)
        {
            // Validate argument here..

            return table.AsEnumerable().Select(row => new DynamicRow(row));
        }

        public sealed class DynamicRow : DynamicObject
        {
            private readonly DataRow _row;

            internal DynamicRow(DataRow row) { _row = row; }

            // Interprets a member-access as an indexer-access on the 
            // contained DataRow.
            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                var retVal = _row.Table.Columns.Contains(binder.Name);
                result = retVal ? _row[binder.Name] : null;
                return retVal;
            }

            public object GetMemberValue(string colName)
            {
                object retVal;
                if (_row.Table.Columns.Contains(colName) && _row[colName] != DBNull.Value)
                    retVal = _row[colName];
                else
                    retVal = null;
                return retVal;
            }

            public TypeCode GetTypeCode(string colName)
            {
                return Type.GetTypeCode(_row.Table.Columns[colName].DataType);
            }
        }
    }
}