using System;

namespace Smartiks.Framework.Text.Abstractions
{
    public interface IStringConverterService
    {
        IConvertible FromString(string value, Type targetType, IFormatProvider formatProvider);

        string ToString(IConvertible value, IFormatProvider formatProvider);
    }
}