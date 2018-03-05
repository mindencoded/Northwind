namespace S3K.RealTimeOnline.CommonUtils
{
    public enum Comparison
    {
        EqualTo,
        NotEqualTo,
        LessThan,
        LessThanOrEqualTo,
        GreaterThan,
        GreaterThanOrEqualTo,
        Like,
        Contains,
        Between,
        StartsWith,
        EndsWith
    }

    public static class ComparisonExtension
    {
        public static string ToString(this Comparison comparison)
        {
            switch (comparison)
            {
                case Comparison.EqualTo:
                    return "=";
                case Comparison.NotEqualTo:
                    return "<>";
                case Comparison.LessThan:
                    return "<";
                case Comparison.LessThanOrEqualTo:
                    return "<=";
                case Comparison.GreaterThan:
                    return ">";
                case Comparison.GreaterThanOrEqualTo:
                    return ">=";
                case Comparison.Like:
                    return "LIKE";
                case Comparison.Contains:
                    return "IN";
                case Comparison.Between:
                    return "BETWEEN";
                default:
                    return null;
            }
        }
    }
}