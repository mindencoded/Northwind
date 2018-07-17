using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Northwind.Shared;

namespace Northwind.WebRole.Tools
{
    public static class QueryHelper
    {
        public static IList<ParameterBuilder> CreateParameterBuilderList(object conditions)
        {
            IList<ParameterBuilder> list = new List<ParameterBuilder>();
            PropertyInfo[] propertyInfos = conditions.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                ParameterBuilder parameterBuilder = new ParameterBuilder
                {
                    PropertyName = propertyInfo.Name,
                    Value = propertyInfo.GetValue(conditions),
                    Comparison = Comparison.EqualTo,
                    SourceColumn = propertyInfo.Name.ToUnderscoreCase()
                };
                list.Add(parameterBuilder);
            }

            return list;
        }

        public static IList<ParameterBuilder> CreateParameterBuilderList(IDictionary<string, object> conditions)
        {
            IList<ParameterBuilder> parameterBuilderList = new List<ParameterBuilder>();
            foreach (KeyValuePair<string, object> condition in conditions)
            {
                ParameterBuilder parameterBuilder = new ParameterBuilder
                {
                    PropertyName = condition.Key,
                    Value = condition.Value,
                    Comparison = Comparison.EqualTo,
                    SourceColumn = condition.Key.ToUnderscoreCase()
                };
                parameterBuilderList.Add(parameterBuilder);
            }

            return parameterBuilderList;
        }


        public static string BuildOrderBy(Dictionary<string, OrderBy> entries)
        {
            IList<string> orderBy = new List<string>();
            foreach (KeyValuePair<string, OrderBy> entry in entries)
            {
                orderBy.Add(entry.Key + " " + entry.Value.ToString().ToUpper());
            }

            return string.Join(",", orderBy.ToArray());
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
                .Select(x => x.GetCustomAttributes(true).OfType<ColumnAttribute>().FirstOrDefault()?.Name).ToList();
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
                        ColumnAttribute columnAttribute = propertyInfo.GetCustomAttributes(true)
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
    }
}