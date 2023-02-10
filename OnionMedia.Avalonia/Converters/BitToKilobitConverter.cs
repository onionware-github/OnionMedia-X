using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace OnionMedia.Avalonia.Converters;

sealed class BitToKilobitConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (double.TryParse(value.ToString(), out double bit))
            return Math.Round(bit / 1000, 3);
        throw new ArgumentException("value is not a number.");
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (double.TryParse(value.ToString(), out double kbit))
            return (long)Math.Round(kbit * 1000, 0);
        throw new ArgumentException("value is not a number.");
    }
}