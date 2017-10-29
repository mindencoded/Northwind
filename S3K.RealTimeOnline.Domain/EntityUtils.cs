using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace S3K.RealTimeOnline.Domain
{
    public class EntityUtils
    {
        public static string JoinColumns<TEntity>() where TEntity : class
        {
            Type type = typeof(TEntity);
            PropertyInfo[] properties = type.GetProperties();
            IList<string> columns = new List<string>();
            foreach (PropertyInfo property in properties)
            {
                ColumnAttribute attribute =
                    property.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
                string propertyName = property.Name;
                string columnName = attribute != null ? attribute.Name : propertyName;
                columns.Add(GetSchema<TEntity>() + ".[" + GetTableName<TEntity>() + "].[" + columnName + "]");
            }
            if (columns.Count > 0)
                return string.Join(", ", columns);
            return null;
        }

        public static string JoinColumns<TEntity>(IList<string> columns, bool useAlias = false) where TEntity : class
        {
            Type type = typeof(TEntity);
            PropertyInfo[] typeProperties = type.GetProperties();
            IList<string> columnAttributes = typeProperties.Select(x => x.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault()?.Name).ToList();
            IList<string> columnQueryList = new List<string>();
            foreach (string item in columns)
            {
                string propertyName = item;
                string columnName = null;
                if (typeProperties.FirstOrDefault(x => x.Name == propertyName) != null)
                {
                    ColumnAttribute columnAttribute = typeProperties.First(x => x.Name == propertyName).GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
                    columnName = columnAttribute != null ? columnAttribute.Name : propertyName;
                }
                else
                {
                    if (columnAttributes.Contains(propertyName))
                    {
                        columnName = propertyName;
                        propertyName = UnderscoreCaseToTitleCase(propertyName);
                    }
                }

                if (columnName == null)
                {
                  continue;
                }

                string columnQuery = GetSchema<TEntity>() + ".[" + GetTableName<TEntity>() + "].[" + columnName + "]";
                if (useAlias)
                {
                    columnQuery += " AS '" + propertyName + "'";
                }
                columnQueryList.Add(columnQuery);
            }
            if (columnQueryList.Count > 0)
                return string.Join(", ", columnQueryList);
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

        private static string UnderscoreCaseToTitleCase(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            TextInfo info = CultureInfo.CurrentCulture.TextInfo;
            return info.ToTitleCase(value.ToLower().Replace("_", " ")).Replace(" ", string.Empty);
        }
    }
}