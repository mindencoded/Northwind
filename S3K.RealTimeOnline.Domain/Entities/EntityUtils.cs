using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace S3K.RealTimeOnline.Domain.Entities
{
    public class EntityUtils
    {
        public static string JoinColumns<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity);
            var properties = type.GetProperties();
            var columns = new List<string>();
            var schema = GetSchema<TEntity>();
            var tableName = GetTableName<TEntity>();
            foreach (var property in properties)
                try
                {
                    var attribute =
                        property.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();

                    if (attribute != null)
                        columns.Add(schema + ".[" + tableName + "].[" + attribute.Name + "]");
                    else
                        columns.Add(schema + ".[" + tableName + "].[" + property.Name + "]");
                }
                catch (Exception)
                {
                    // ignored
                }

            if (columns.Count > 0)
                return string.Join(",", columns);
            return null;
        }

        public static string SimpleJoinColumns<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity);
            var properties = type.GetProperties();
            var columns = new List<string>();
            foreach (var property in properties)
                try
                {
                    var attribute =
                        property.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();

                    if (attribute != null)
                        columns.Add("[" + attribute.Name + "]");
                    else
                        columns.Add(property.Name);
                }
                catch (Exception)
                {
                    // ignored
                }

            if (columns.Count > 0)
                return string.Join(",", columns);
            return null;
        }

        public static string GetSchema<TEntity>() where TEntity : class
        {
            var schema = "";
            var type = typeof(TEntity);
            var attribute = type.GetCustomAttributes(true).OfType<SchemaAttribute>().FirstOrDefault();
            if (attribute != null)
                schema = attribute.Name;
            return schema;
        }

        public static string GetTableName<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity);

            var temp = type.GetCustomAttributes(
                typeof(TableAttribute),
                true);

            if (temp.Length > 0)
            {
                var tableAttribute = temp[0] as TableAttribute;
                if (tableAttribute != null)
                    return tableAttribute.Name;
            }

            return type.Name;
        }
    }
}