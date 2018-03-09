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

        public static string TrimEnd(this string target, string trimString)
        {
            string result = target;
            while (result.EndsWith(trimString))
            {
                result = result.Substring(0, result.Length - trimString.Length);
            }

            return result;
        }
    }
}