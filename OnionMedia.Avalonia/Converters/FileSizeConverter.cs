using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace OnionMedia.Avalonia.Converters;

sealed class FileSizeConverter : IValueConverter
{
    static readonly string[] sizeunits = { "B", "KB", "MB", "GB" };

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not long size && (value == null || !long.TryParse(value.ToString(), out size)))
            throw new ArgumentException("value is not from type long.");

        double formattedSize = size;
        int index;
        for (index = 0; index < sizeunits.Length && size >= 1000; index++, size /= 1000)
            formattedSize /= 1000;

        return $"{Math.Round(formattedSize, 2)} {sizeunits[index]}";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}