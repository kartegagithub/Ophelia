using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace Ophelia.Data.EntityFramework
{
    public static class IDataRecordExtensions
    {
        public static T Field<T>(this IDataRecord record, string name)
        {
            Guard.ArgumentNullException(record, "record");
            Guard.ArgumentNullException(name, "name");
            return Field<T>(record, record.GetOrdinal(name));
        }

        public static T Field<T>(this IDataRecord record, int ordinal)
        {
            Guard.ArgumentNullException(record, "record");
            object value = record.IsDBNull(ordinal) ? null : record.GetValue(ordinal);
            return (T)value;
        }

        public static ReadOnlyCollection<string> GetFieldNames(this IDataRecord record)
        {
           var fieldNames = new List<string>(record.FieldCount);
            fieldNames.AddRange(Enumerable.Range(0, record.FieldCount)
                .Select(i => record.GetName(i)));
            return fieldNames.AsReadOnly();
        }

        public static bool ValidateFieldNames(this IDataRecord record)
        {
            var fieldNames = record.GetFieldNames();
            if (fieldNames.Count != record.FieldCount ||
                fieldNames.Where((fieldName, ordinal) => record.GetName(ordinal) != fieldName)
                .Any())
            {
                return false;
            }
            return true;
        }
    }
}
