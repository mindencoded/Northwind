using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.Serialization;
using S3K.RealTimeOnline.GenericDataAccess.Tools;

namespace S3K.RealTimeOnline.GenericDataAccess.QueryHandlers
{
    [DataContract]
    public class GenericSelectQuery : Query<IEnumerable<ExpandoObject>>
    {
        [DataMember] public IList<string> Columns { get; set; }

        [DataMember] public IList<ParameterBuilder> Conditions { get; set; }

        [DataMember] public string OrderBy { get; set; }
    }
}

/*
  public void BuildOrderBy(Dictionary<string, SortDirection> entries)
        {
            IList<string> orderBy = new List<string>();
            foreach (KeyValuePair<string, SortDirection> entry in entries)
            {
                orderBy.Add(entry.Key + " " + entry.Value.ToString().ToUpper());
            }
            OrderBy = string.Join(",", orderBy.ToArray());
        }
 */

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