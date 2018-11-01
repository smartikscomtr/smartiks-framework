using Smartiks.Framework.Text.Abstractions;
using System;

namespace Smartiks.Framework.Text
{
    public class ChangeTypeStringConverterService : IStringConverterService
    {
        public IConvertible FromString(string value, Type targetType, IFormatProvider formatProvider)
        {
            return (IConvertible)Convert.ChangeType(value, targetType, formatProvider);
        }

        public string ToString(IConvertible value, IFormatProvider formatProvider)
        {
            return (string)Convert.ChangeType(value, typeof(string), formatProvider);
        }
    }
}