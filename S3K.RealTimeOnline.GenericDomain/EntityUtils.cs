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
                    KeyAttribute attribute =
                        (KeyAttribute) Attribute.GetCustomAttribute(propertyInfo, typeof(KeyAttribute));
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
                    columnName = columnAttribute != null ? columnAttribute.Name : propertyName;
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
                    columnName = columnAttribute != null ? "[" + columnAttribute.Name + "]" : "[" + propertyName + "]";
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

        public static string UnderscoreCaseToTitleCase(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            TextInfo info = CultureInfo.CurrentCulture.TextInfo;
            return info.ToTitleCase(value.ToLower().Replace("_", " ")).Replace(" ", string.Empty);
        }

        public static string CreateOrderByString<T>(IEnumerable<string> orderBy) where T : class
        {
            if (orderBy == null)
            {
                return null;
            }
            PropertyInfo[] properties = typeof(T).GetProperties();
            string[] propertyNames = properties.Select(x => x.Name).ToArray();
            IList<string> columnAttributeList = properties
                .Select(x => x.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault()?.Name).ToList();
            IList<string> columnNames = new List<string>();
            foreach (string orderItem in orderBy)
            {
                string propertyName;
                string direction = null;
                string columnName = null;
                if (orderItem.Split(' ').Length > 1)
                {
                    propertyName = orderItem.Split(' ')[0];
                    string ascOrDesc = orderItem.Split(' ')[1].ToUpper();
                    if (ascOrDesc.Equals("DESC") || ascOrDesc.Equals("ASC"))
                    {
                        direction = ascOrDesc;
                    }
                }
                else
                {
                    propertyName = orderItem;
                }

                if (propertyNames.Contains(propertyName))
                {
                    PropertyInfo propertyInfo = typeof(T).GetProperty(propertyName);
                    if (propertyInfo != null)
                    {
                        ColumnAttribute columnAttribute = propertyInfo.GetCustomAttributes(false)
                            .OfType<ColumnAttribute>()
                            .FirstOrDefault();
                        if (columnAttribute != null)
                        {
                            columnName = "[" + columnAttribute.Name + "]";
                        }
                        else
                        {
                            columnName = "[" + propertyName + "]";
                        }
                    }
                }
                else
                {
                    if (columnAttributeList.Contains(propertyName))
                    {
                        columnName = propertyName;
                    }
                }

                if (columnName != null)
                {
                    if (direction != null)
                    {
                        columnName += ' ' + direction;
                    }

                    columnNames.Add(columnName);
                }
            }
            return string.Join(", ", columnNames);
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
                if (columnAttribute != null)
                {
                    columnName = columnAttribute.Name;
                    break;
                }
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