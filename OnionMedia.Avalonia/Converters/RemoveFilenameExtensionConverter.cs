using System;
using System.Globalization;
using System.IO;
using Avalonia.Data.Converters;

namespace OnionMedia.Avalonia.Converters;

public class RemoveFilenameExtensionConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string path) return value;
        return Path.GetFileNameWithoutExtension(path);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}