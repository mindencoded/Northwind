using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace S3K.RealTimeOnline.CommonUtils
{
    public static class StringExtensions
    {
        public static bool Like(this string toSearch, string toFind)
        {
            return new Regex(
                @"\A" + new Regex(@"\.|\$|\^|\{|\[|\(|\||\)|\*|\+|\?|\\").Replace(toFind, ch => @"\" + ch)
                    .Replace('_', '.').Replace("%", ".*") + @"\z", RegexOptions.Singleline).IsMatch(toSearch);
        }

        public static string ToUnderscoreCase(this string str)
        {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString()))
                .ToUpper();
        }

        public static string ToTitleCase(this string str)
        {
            TextInfo txtInfo = CultureInfo.CurrentCulture.TextInfo;
            return txtInfo.ToTitleCase(str).Replace('_', ' ').Replace(" ", String.Empty);
            //return txtInfo.ToTitleCase(s.ToLower());
        }

        public static string TrimStart(this string target, string trimString)
        {
            string result = target;
            while (result.StartsWith(trimString))
            {
                result = result.Substring(trimString.Length);
            }
            return result;
        }

        public static string TrimEnd(this string input, string suffixToRemove, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {

            if (input != null && suffixToRemove != null
                              && input.EndsWith(suffixToRemove, comparisonType))
            {
                return input.Substring(0, input.Length - suffixToRemove.Length);
            }
            return input;
        }

        /// <summary>
        /// Get string value between [first] a and [last] b.
        /// </summary>
        public static string Between(this string value, string a, string b)
        {
            int posA = value.IndexOf(a, StringComparison.OrdinalIgnoreCase);
            int posB = value.LastIndexOf(b, StringComparison.OrdinalIgnoreCase);
            if (posA == -1)
            {
                return "";
            }
            if (posB == -1)
            {
                return "";
            }
            int adjustedPosA = posA + a.Length;
            if (adjustedPosA >= posB)
            {
                return "";
            }
            return value.Substring(adjustedPosA, posB - adjustedPosA);
        }

        /// <summary>
        /// Get string value after [first] a.
        /// </summary>
        public static string Before(this string value, string a)
        {
            int posA = value.IndexOf(a, StringComparison.OrdinalIgnoreCase);
            if (posA == -1)
            {
                return "";
            }
            return value.Substring(0, posA);
        }

        /// <summary>
        /// Get string value after [last] a.
        /// </summary>
        public static string After(this string value, string a)
        {
            int posA = value.LastIndexOf(a, StringComparison.OrdinalIgnoreCase);
            if (posA == -1)
            {
                return "";
            }
            int adjustedPosA = posA + a.Length;
            if (adjustedPosA >= value.Length)
            {
                return "";
            }
            return value.Substring(adjustedPosA);
        }

    }
}