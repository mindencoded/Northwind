using System;
using System.Collections.Generic;

namespace Northwind.Shared
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
            {"$=", Comparison.EndsWith}
        };

        public static IDictionary<string, Comparison> Sentences = new Dictionary<string, Comparison>
        {
            {"STARTSWITH", Comparison.StartsWith},
            {"ENDSWHIT", Comparison.EndsWith},
            {"CONTAINS", Comparison.Contains},
            {"IN", Comparison.Contains},
            {"BETWEEN", Comparison.Between},
            {"LIKE", Comparison.Like}
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