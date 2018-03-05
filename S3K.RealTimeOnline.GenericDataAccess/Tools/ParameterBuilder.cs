using System;
using System.Collections.Generic;
using S3K.RealTimeOnline.CommonUtils;

namespace S3K.RealTimeOnline.GenericDataAccess.Tools
{
    public class ParameterBuilder
    {
        public ParameterBuilder()
        {
            Comparison = Comparison.EqualTo;
            Condition = Condition.And;
        }

        public string ParameterName { get; set; }

        public string SourceColumn { get; set; }

        public Comparison Comparison { get; set; }

        public Condition Condition { get; set; }

        public object Value { get; set; }

        public override string ToString()
        {
            string parameterName = !ParameterName.StartsWith("@") ? '@' + ParameterName : ParameterName;

            if (Comparison == Comparison.Between)
            {
                return "[" + SourceColumn + "]" + " " + Comparison.Between + " " + parameterName + "1" +
                       " AND " +
                       parameterName + "2";
            }

            IList<string> parameters = new List<string>();
            Type valueType = Value != null ? Value.GetType() : typeof(string);
            if (Comparison == Comparison.Contains)
            {
                if (valueType.IsArray)
                {
                    object[] values = Value as object[];
                    if (values != null && values.Length > 0)
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            parameters.Add(parameterName + i);
                        }

                        return "[" + SourceColumn + "]" + " " + Comparison + " (" +
                               string.Join(", ", parameters) + ")";
                    }
                }

                return "[" + SourceColumn + "]" + " " + Comparison + " (" + parameterName + ")";
            }

            if (Comparison == Comparison.Like)
            {
                if (valueType.IsArray)
                {
                    object[] values = Value as object[];
                    if (values != null && values.Length > 0)
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            parameters.Add("[" + SourceColumn + "]" + " " + Comparison + " '%' + " +
                                           parameterName + i +
                                           " + '%'");
                        }

                        return "(" + string.Join(" OR ", parameters) + ")";
                    }
                }

                return Condition + " [" + SourceColumn + "]" + " " + Comparison + " '%' + " + parameterName + " + '%'";
            }

            return Condition + " [" + SourceColumn + "]" + " " + Comparison + " " + parameterName;
        }
    }
}