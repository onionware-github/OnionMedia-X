using System;
using System.Linq;
using Avalonia.Markup.Xaml;
using OnionMedia.Core.Services;

namespace OnionMedia.Avalonia.Markup;

sealed class ResourceExtension : MarkupExtension
{
    private readonly IStringResourceService resLoader = new ServiceProvider().JsonStringResourceService;
    
    public ResourceExtension(string key)
    {
        Key = key;
    }
    
    public string Key { get; set; }
    
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (string.IsNullOrWhiteSpace(Key)) return string.Empty;
        string[] values = Key.Split('/').Where(p => !string.IsNullOrWhiteSpace(p)).ToArray();
        if (!values.Any()) return string.Empty;

        if (values.Length == 1)
        {
            return resLoader.GetLocalized(values[0]);
        }

        return resLoader.GetLocalized(values[1], values[0]);
    }
}