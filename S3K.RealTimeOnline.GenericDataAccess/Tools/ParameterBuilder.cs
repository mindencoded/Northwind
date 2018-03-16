using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

        public string PropertyName { get; set; }

        public string SourceColumn { get; set; }

        public Comparison Comparison { get; set; }

        public Condition Condition { get; set; }

        public bool IsStartGroup { get; set; }

        public bool IsEndGroup { get; set; }

        public object Value { get; set; }

        public override string ToString()
        {
            string parameterName = !PropertyName.StartsWith("@") ? '@' + PropertyName : PropertyName;

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

        public string StartGroup()
        {
            return IsStartGroup ? "(" : "";
        }

        public string EndGroup()
        {
            return IsEndGroup ? ")" : "";
        }

        public static ParameterBuilder Create(string filter, Condition condition)
        {
            ParameterBuilder parameter = new ParameterBuilder
            {
                Condition = condition
            };

            if (filter.StartsWith("("))
            {
                filter = filter.TrimStart("(");
                parameter.IsStartGroup = true;
            }

            foreach (KeyValuePair<string, Comparison> sentence in ComparisonHelper.Sentences)
            {
                if (filter.ToUpper().StartsWith(sentence.Key))
                {
                    if (filter.Trim().EndsWith("))"))
                    {
                        filter = filter.TrimEnd(")");
                        parameter.IsEndGroup = true;
                    }

                    parameter.Comparison = sentence.Value;

                    string[] values = Regex.Match(filter, @"\(([^)]*)\)").Groups[1].Value.Split(';');

                    parameter.PropertyName = values[0].Trim();
                    parameter.Value = values[1].TrimEnd(")").Trim('\'').Split(',', '~');
                    return parameter;
                }
            }

            foreach (KeyValuePair<string, Comparison> symbol in ComparisonHelper.Symbols)
            {
                string[] values =
                    filter.Split(new[] {symbol.Key}, StringSplitOptions.RemoveEmptyEntries);

                if (values.Length > 1)
                {
                    parameter.Comparison = symbol.Value;
                    parameter.PropertyName = values[0].Trim();
                    values[1] = values[1].Trim();
                    if (values[1].EndsWith(")"))
                    {
                        values[1] = values[1].TrimEnd(")");
                        parameter.IsEndGroup = true;
                    }

                    parameter.Value = values[1].Trim('\'');
                    return parameter;
                }
            }

            return null;
        }
    }
}