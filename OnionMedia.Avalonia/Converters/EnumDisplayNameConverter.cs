using System;
using System.Globalization;
using Avalonia.Data.Converters;
using OnionMedia.Core.Extensions;

namespace OnionMedia.Avalonia.Converters;
sealed class EnumDisplayNameConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value));

        if (value is Enum enumtype)
            return enumtype.GetDisplayName();
        return string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}