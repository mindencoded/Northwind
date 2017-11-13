using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using S3K.RealTimeOnline.CommonUtils;

namespace S3K.RealTimeOnline.GenericDataAccess.Tools
{
    public class QueryBuilder : IQuery<IEnumerable<ExpandoObject>>
    {
        public QueryBuilder()
        {
            Page = 1;
            PageSize = 200;
        }

        public IList<string> Columns { get; set; }

        public IList<ParameterBuilder> Conditions { get; set; }

        public string OrderBy { get; set; }

        public int? Page { get; set; }

        public int? PageSize { get; set; }

        public void SetConditions(object conditions)
        {
            Conditions = new List<ParameterBuilder>();
            PropertyInfo[] propertyInfos = conditions.GetType().GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                ParameterBuilder parameterBuilder = new ParameterBuilder
                {
                    ParameterName = propertyInfo.Name,
                    Value = propertyInfo.GetValue(conditions),
                    Operator = Comparison.EqualTo,
                    SourceColumn = propertyInfo.Name.ToUnderscoreCase()
                };
                Conditions.Add(parameterBuilder);
            }
        }

        public void SetConditions(IDictionary<string, object> conditions)
        {
            Conditions = new List<ParameterBuilder>();
            foreach (KeyValuePair<string, object> condition in conditions)
            {
                ParameterBuilder parameterBuilder = new ParameterBuilder
                {
                    ParameterName = condition.Key,
                    Value = condition.Value,
                    Operator = Comparison.EqualTo,
                    SourceColumn = condition.Key.ToUnderscoreCase()
                };
                Conditions.Add(parameterBuilder);
            }
        }
    }
}

/*public static string CreateOrderByString<T>(IEnumerable<string> orderBy) where T : class
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
}*/