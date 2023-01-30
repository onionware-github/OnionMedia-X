using System;
using System.Globalization;
using Avalonia.Data.Converters;
using OnionMedia.Core.Extensions;

namespace OnionMedia.Avalonia.Converters;

sealed class ResourceNameToTextConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string resourceName && resourceName.IsNullOrEmpty())
            return null;

        if (value is Enum enumtype)
            resourceName = enumtype.GetDisplayName();
        else
            return null;

        string resourcePath = parameter is string rPath && !rPath.IsNullOrEmpty() ? rPath : "Resources";
        return resourceName.GetLocalized(resourcePath);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}