using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Smartiks.Framework.Data.Web
{
    public static class SquareBracketedKeyValuePairNormalizer
    {
        public static IDictionary<string, StringValues> Normalize(IEnumerable<KeyValuePair<string, StringValues>> values)
        {
            return values.ToDictionary(v => Regex.Replace(v.Key, @"\[([\p{L}_.]*?[\p{L}_.]+?[\p{L}\p{N}_.]*?)\]", ".$1"), v => v.Value, StringComparer.OrdinalIgnoreCase);
        }
    }
}