

using System.Collections.Generic;
using System.Reflection;
using S3K.RealTimeOnline.CommonUtils;

namespace S3K.RealTimeOnline.GenericDataAccess.Tools
{
    public static class QueryHelper
    {
        public static IList<ParameterBuilder> CreateParameterBuilderList(object conditions)
        {
            IList<ParameterBuilder> list = new List<ParameterBuilder>();
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
                    ParameterName = condition.Key,
                    Value = condition.Value,
                    Operator = Comparison.EqualTo,
                    SourceColumn = condition.Key.ToUnderscoreCase()
                };
                parameterBuilderList.Add(parameterBuilder);
            }

            return parameterBuilderList;
        }
    }
}
