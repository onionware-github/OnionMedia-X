using System;
using System.Drawing;
using System.Globalization;
using Avalonia.Data.Converters;
using Color = Avalonia.Media.Color;

namespace OnionMedia.Avalonia.Converters;

sealed class HexToColorConverter : IValueConverter
{
    //Backend->UI
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string hex || hex == string.Empty) return null;
        try
        {
            var color = ColorTranslator.FromHtml(hex);
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }
        catch
        {
            return null;
        }
    }

    //UI->Backend
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is Color color
            ? ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B))
            : null;
    }
}