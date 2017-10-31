using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace S3K.RealTimeOnline.Domain
{
    public class EntityUtils
    {
        public static string JoinColumns<T>(bool includeRowNumber = false) where T : class
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            IList<string> columns = new List<string>();
            foreach (PropertyInfo property in properties)
            {
                ColumnAttribute attribute =
                    property.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
                string columnName = attribute != null ? attribute.Name : property.Name;
                columns.Add(GetSchema<T>() + ".[" + GetTableName<T>() + "].[" + columnName + "]");
            }

            if (includeRowNumber)
            {
                IList<string> keyNames = new List<string>();
                foreach (PropertyInfo propertyInfo in properties)
                {
                    KeyAttribute attribute = (KeyAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(KeyAttribute));
                    if (attribute != null)
                    {
                        ColumnAttribute columnAttribute = properties.First(x => x.Name == propertyInfo.Name)
                            .GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
                        string keyName = columnAttribute != null ? columnAttribute.Name : propertyInfo.Name;
                        keyNames.Add(GetSchema<T>() + ".[" + GetTableName<T>() + "].[" + keyName + "]");
                    }
                }
                columns.Add("ROW_NUMBER() OVER(ORDER BY " + string.Join(", ", keyNames) + ") AS 'RowNumber'");
            }
            return string.Join(", ", columns);
        }

        public static string JoinColumns<T>(IList<string> columns, bool useColumnAlias = false,
            bool includeRowNumber = false) where T : class
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            IList<string> columnAttributes = properties
                .Select(x => x.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault()?.Name).ToList();
            IList<string> columnList = new List<string>();
            foreach (string item in columns)
            {
                string propertyName = item;
                string columnName = null;
                if (properties.FirstOrDefault(x => x.Name == propertyName) != null)
                {
                    ColumnAttribute columnAttribute = properties.First(x => x.Name == propertyName)
                        .GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
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

                string columnQuery = GetSchema<T>() + ".[" + GetTableName<T>() + "].[" + columnName + "]";
                if (useColumnAlias)
                {
                    columnQuery += " AS '" + propertyName + "'";
                }
                columnList.Add(columnQuery);
            }

            if (includeRowNumber)
            {
                IList<string> keyNames = new List<string>();
                foreach (PropertyInfo propertyInfo in properties)
                {
                    KeyAttribute attribute = (KeyAttribute) Attribute.GetCustomAttribute(propertyInfo, typeof(KeyAttribute));
                    if (attribute != null)
                    {
                        ColumnAttribute columnAttribute = properties.First(x => x.Name == propertyInfo.Name)
                            .GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
                        string keyName = columnAttribute != null ? columnAttribute.Name : propertyInfo.Name;
                        keyNames.Add(GetSchema<T>() + ".[" + GetTableName<T>() + "].[" + keyName + "]");
                    }
                }
                columnList.Add("ROW_NUMBER() OVER(ORDER BY " + string.Join(", ", keyNames) + ") AS 'RowNumber'");
            }
            return string.Join(", ", columnList);
        }

        public static string SimpleJoinColumns<T>() where T : class
        {
            var type = typeof(T);
            var properties = type.GetProperties();
            var columns = new List<string>();
            foreach (var property in properties)
            {
                var attribute =
                    property.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();

                if (attribute != null)
                    columns.Add("[" + attribute.Name + "]");
                else
                    columns.Add(property.Name);

            }
            return string.Join(",", columns);
        }

        public static string SimpleJoinColumns<T>(IList<string> columns, bool useColumnAlias = false) where T : class
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            IList<string> columnAttributes = properties
                .Select(x => x.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault()?.Name).ToList();
            IList<string> columnList = new List<string>();
            foreach (string item in columns)
            {
                string propertyName = item;
                string columnName = null;
                if (properties.FirstOrDefault(x => x.Name == propertyName) != null)
                {
                    ColumnAttribute columnAttribute = properties.First(x => x.Name == propertyName)
                        .GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
                    columnName = columnAttribute != null ? "[" + columnAttribute.Name + "]" : "[" + propertyName + "]";
                }
                else
                {
                    if (columnAttributes.Contains(propertyName))
                    {
                        columnName = "[" + propertyName + "]";
                        propertyName = UnderscoreCaseToTitleCase(propertyName);
                    }
                }

                if (columnName == null)
                {
                    continue;
                }

                if (useColumnAlias)
                {
                    columnName += " AS '" + propertyName + "'";
                }
                columnList.Add(columnName);
            }        
            return string.Join(", ", columnList);
        }

        public static string GetSchema<T>() where T : class
        {
            var schema = "";
            var type = typeof(T);
            var attribute = type.GetCustomAttributes(true).OfType<SchemaAttribute>().FirstOrDefault();
            if (attribute != null)
                schema = attribute.Name;
            return schema;
        }

        public static string GetTableName<T>() where T : class
        {
            var type = typeof(T);

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