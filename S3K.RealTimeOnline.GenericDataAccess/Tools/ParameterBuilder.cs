using System;
using System.Collections.Generic;
using S3K.RealTimeOnline.Commons;

namespace S3K.RealTimeOnline.GenericDataAccess.Tools
{
    public class ParameterBuilder
    {
        public ParameterBuilder()
        {
            Operator = Comparison.EqualTo;
        }

        public string ParameterName { get; set; }

        public string SourceColumn { get; set; }

        public Comparison Operator { get; set; }

        public object Value { get; set; }

        public override string ToString()
        {
            string parameterName = !ParameterName.StartsWith("@") ? '@' + ParameterName : ParameterName;
          
            if (Operator == Comparison.Between)
            {
                return "[" + SourceColumn + "]" + " " + Comparison.Between.Value() + " " + parameterName + "1" + " AND " +
                       parameterName + "2";
            }
            IList<string> parameters = new List<string>();
            Type valueType = Value != null ? Value.GetType() : typeof(string);
            if (Operator == Comparison.Contains)
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
                        return "[" + SourceColumn + "]" + " " + Operator.Value() + " (" + string.Join(", ", parameters) + ")";
                    }
                }

                return "[" + SourceColumn + "]" + " " + Operator.Value() + " (" + parameterName + ")";
            }

            if (Operator == Comparison.Like)
            {
                if (valueType.IsArray)
                {
                    object[] values = Value as object[];
                    if (values != null && values.Length > 0)
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            parameters.Add("[" + SourceColumn + "]" + " " + Operator.Value() + " '%' + " + parameterName + i +
                                           " + '%'");
                        }
                        return "(" + string.Join(" OR ", parameters) + ")";
                    }
                }
                return "[" + SourceColumn + "]" + " " + Operator.Value() + " '%' + " + parameterName + " + '%'";
            }

            return "[" + SourceColumn + "]" + " " + Operator.Value() + " " + parameterName;
        }
    }
}