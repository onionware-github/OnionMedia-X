using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OnionMedia.Core;
using OnionMedia.Core.ViewModels;

namespace OnionMedia.Avalonia.Views;

public sealed partial class SettingsPage : UserControl
{
    public SettingsPage()
    {
        InitializeComponent();
        DataContext = App.DefaultServiceProvider.GetService<SettingsViewModel>();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void FilenameSuffix_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        string text = (sender as TextBox)?.Text ?? string.Empty;
        ((SettingsViewModel)DataContext).InvalidFilename = InvalidFileNameCharRegex().IsMatch(text);
    }

    [GeneratedRegex(GlobalResources.INVALIDFILENAMECHARACTERSREGEX)]
    private static partial Regex InvalidFileNameCharRegex();

}