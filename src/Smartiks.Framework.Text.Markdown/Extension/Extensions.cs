using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Smartiks.Framework.Text.Markdown.Extension
{
    public static class Extensions
    {
        public static string Join(this IEnumerable<string> collection, char separator)
        {
            return String.Join(separator, collection);
        }

        public static string Replace(this string str, string pattern, MatchEvaluator evaluator)
        {
            return Regex.Replace(str, pattern, evaluator);
        }

        public static string ReplaceLineByLine(this string str, string pattern, MatchEvaluator evaluator)
        {
            return str
                .Split('\n')
                .Select(line => Regex.Replace(line, pattern, evaluator))
                .Join('\n');
        }
    }
}