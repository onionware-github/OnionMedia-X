using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace OnionMedia.Avalonia.Converters;

sealed class EqualityConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value?.Equals(parameter);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}