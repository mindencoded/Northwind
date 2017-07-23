using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;

namespace S3K.RealTimeOnline.DataAccess.Tools
{
    public static class SqlDataReaderExtensions
    {
        public static TEntity ConvertToEntity<TEntity>(this SqlDataReader reader) where TEntity : class
        {
            var type = typeof(TEntity);
            var instance = (TEntity) Activator.CreateInstance(type);
            var properties = type.GetProperties();
            for (var i = 0; i < reader.FieldCount; i++)
            {
                var columnName = reader.GetName(i);
                var value = reader[i];

                /*
                var dotNetType = reader.GetFieldType(i);
                var sqlType = reader.GetDataTypeName(i);
                */

                if (value is DBNull) continue;

                foreach (var property in properties)
                    try
                    {
                        var attribute =
                            property.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();

                        if (attribute != null && columnName == attribute.Name || columnName == property.Name)
                        {
                            property.SetValue(instance, value);
                            break;
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
            }
            return instance;
        }
    }
}