using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace S3K.RealTimeOnline.GenericDomain
{
    public class EntityUtils
    {
        public static string JoinColumns<T>(bool useColumnAlias = false, bool includeRowNumber = false) where T : class
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            IList<string> columns = new List<string>();
            foreach (PropertyInfo property in properties)
            {
                ColumnAttribute attribute =
                    property.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
                if (attribute != null)
                {
                    string column = GetSchema<T>() + ".[" + GetTableName<T>() + "].[" + attribute.Name + "]";
                    if (useColumnAlias)
                    {
                        column = column + " AS '" + property.Name + "'";
                    }

                    columns.Add(column);
                }
            }

            if (includeRowNumber)
            {
                IList<string> keyNames = new List<string>();
                foreach (PropertyInfo property in properties)
                {
                    KeyAttribute attribute =
                        (KeyAttribute) Attribute.GetCustomAttribute(property, typeof(KeyAttribute));
                    if (attribute != null)
                    {
                        ColumnAttribute columnAttribute = properties.First(x => x.Name == property.Name)
                            .GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
                        if (columnAttribute != null)
                        {
                            keyNames.Add(GetSchema<T>() + ".[" + GetTableName<T>() + "].[" + columnAttribute.Name +
                                         "]");
                        }
                    }
                }

                columns.Add("ROW_NUMBER() OVER(ORDER BY " + string.Join(", ", keyNames) + ") AS 'RowNumber'");
            }

            return string.Join(", ", columns);
        }

        public static string JoinColumns<T>(IEnumerable<string> columns, bool useColumnAlias = false,
            bool includeRowNumber = false) where T : class
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            IList<string> columnAttributeList = properties
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
                    if (columnAttribute != null)
                    {
                        columnName = columnAttribute.Name;
                    }
                }
                else
                {
                    if (columnAttributeList.Contains(propertyName))
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
                    KeyAttribute attribute =
                        (KeyAttribute) Attribute.GetCustomAttribute(propertyInfo, typeof(KeyAttribute));
                    if (attribute != null)
                    {
                        ColumnAttribute columnAttribute = properties.First(x => x.Name == propertyInfo.Name)
                            .GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
                        if (columnAttribute != null)
                        {
                            keyNames.Add(GetSchema<T>() + ".[" + GetTableName<T>() + "].[" + columnAttribute.Name +
                                         "]");
                        }
                    }
                }

                columnList.Add("ROW_NUMBER() OVER(ORDER BY " + string.Join(", ", keyNames) + ") AS 'RowNumber'");
            }

            return string.Join(", ", columnList);
        }

        public static string SimpleJoinColumns<T>(bool useColumnAlias = false) where T : class
        {
            var type = typeof(T);
            var properties = type.GetProperties();
            var columns = new List<string>();
            foreach (var property in properties)
            {
                var attribute =
                    property.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();

                if (attribute != null)
                {
                    string column = "[" + attribute.Name + "]";
                    if (useColumnAlias)
                    {
                        column += " AS " + property.Name;
                    }

                    columns.Add(column);
                }
            }

            return string.Join(",", columns);
        }


        public static string SimpleJoinColumns<T>(IEnumerable<string> columns, bool useColumnAlias = false)
            where T : class
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            IList<string> columnAttributeList = properties
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
                    if (columnAttribute != null)
                    {
                        columnName = "[" + columnAttribute.Name + "]";
                    }
                }
                else
                {
                    if (columnAttributeList.Contains(propertyName))
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

        public static string SimpleJoinProperties<T>() where T : class
        {
            var type = typeof(T);
            var properties = type.GetProperties();
            var columns = new List<string>();
            foreach (var property in properties)
            {
                var attribute =
                    property.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();

                if (attribute != null)
                {
                    columns.Add("[" + property.Name + "]");
                }
            }

            return string.Join(",", columns);
        }

        public static string SimpleJoinProperties<T>(IEnumerable<string> columns)
            where T : class
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            IList<string> columnAttributeList = properties
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
                    if (columnAttribute != null)
                    {
                        columnName = "[" + propertyName + "]";
                    }
                }
                else
                {
                    if (columnAttributeList.Contains(propertyName))
                    {
                        columnName = "[" + UnderscoreCaseToTitleCase(propertyName) + "]";
                    }
                }

                if (columnName == null)
                {
                    continue;
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
                if (temp[0] is TableAttribute tableAttribute)
                    return tableAttribute.Name;
            }

            return type.Name;
        }

        public static string UnderscoreCaseToTitleCase(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            TextInfo info = CultureInfo.CurrentCulture.TextInfo;
            return info.ToTitleCase(value.ToLower().Replace("_", " ")).Replace(" ", string.Empty);
        }

        public static string GetColumnName<T>(string propertyName, bool extendedColumnName = false) where T : class
        {
            PropertyInfo[] propertyInfos = typeof(T).GetProperties();
            string columnName = null;
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (propertyInfo.Name != propertyName) continue;
                ColumnAttribute columnAttribute = propertyInfo.GetCustomAttributes
                    (typeof(ColumnAttribute), false).Cast<ColumnAttribute>().FirstOrDefault();
                if (columnAttribute == null) continue;
                columnName = columnAttribute.Name;
                break;
            }

            if (columnName == null) return null;

            if (extendedColumnName)
            {
                return GetSchema<T>() + ".[" + GetTableName<T>() + "].[" + columnName + "]";
            }

            return "[" + columnName + "]";
        }
    }
}