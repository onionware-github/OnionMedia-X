using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;

namespace OnionMedia.Avalonia.Converters;

sealed class SampleRateComboBoxConverter : IValueConverter
{
    //Backend->UI
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (double.TryParse(value?.ToString(), out double d) && d == 0)
            return "auto";
        return value?.ToString();
    }

    //UI->Backend
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is "auto")
            return (double)0;
        return value;
    }
}