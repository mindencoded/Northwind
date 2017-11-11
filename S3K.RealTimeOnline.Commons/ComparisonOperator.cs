namespace S3K.RealTimeOnline.Commons
{
    public enum ComparisonOperator
    {
        EqualTo,
        NotEqualTo,
        LessThan,
        LessThanOrEqualTo,
        GreaterThan,
        GreaterThanOrEqualTo,
        Like,
        Contains,
        Between
    }

    public static class ComparisonOperatorExtension
    {
        public static string Value(this ComparisonOperator comparisonOperator)
        {
            switch (comparisonOperator)
            {
                case ComparisonOperator.EqualTo:
                    return "=";
                case ComparisonOperator.NotEqualTo:
                    return "<>";
                case ComparisonOperator.LessThan:
                    return "<";
                case ComparisonOperator.LessThanOrEqualTo:
                    return "<=";
                case ComparisonOperator.GreaterThan:
                    return ">";
                case ComparisonOperator.GreaterThanOrEqualTo:
                    return ">=";
                case ComparisonOperator.Like:
                    return "LIKE";
                case ComparisonOperator.Contains:
                    return "IN";
                case ComparisonOperator.Between:
                    return "BETWEEN";
                default:
                    return "=";
            }
        }
    }
}