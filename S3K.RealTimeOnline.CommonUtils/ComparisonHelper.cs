using System;
using System.Collections.Generic;

namespace S3K.RealTimeOnline.CommonUtils
{
    public class ComparisonHelper
    {
        public static IDictionary<string, Comparison> Symbols = new Dictionary<string, Comparison>
        { 
            {"==", Comparison.EqualTo},
            {"<>", Comparison.NotEqualTo},
            {"!=", Comparison.NotEqualTo},
            {"<", Comparison.LessThan},
            {"<=", Comparison.LessThanOrEqualTo},
            {">", Comparison.GreaterThan},
            {">=", Comparison.GreaterThanOrEqualTo},
            {"*=", Comparison.Like},
            {"^=", Comparison.StartsWith},
            {"$=", Comparison.EndsWith},
            {"CONTAINS", Comparison.Contains},
            {"IN", Comparison.Contains},
            {"BETWEEN", Comparison.Between}
        };


        public static Comparison GetValueBySymbol(string symbol)
        {
            if (Symbols.ContainsKey(symbol))
            {
                return Symbols[symbol];
            }

            throw new Exception("The comparison command '" + symbol + "' does not exist.");
        }
    }
}