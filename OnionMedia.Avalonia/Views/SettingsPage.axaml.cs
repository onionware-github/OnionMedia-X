using System.ComponentModel;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;
using OnionMedia.Core;
using OnionMedia.Core.Models;
using OnionMedia.Core.ViewModels;

namespace OnionMedia.Avalonia.Views;

public sealed partial class SettingsPage : UserControl, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public SettingsPage()
    {
        InitializeComponent();
        DataContext = App.DefaultServiceProvider.GetService<SettingsViewModel>();
    }

    public bool SmallWindowStyle { get; set; }

    private void UpdateSizeStyle(object? sender, SizeChangedEventArgs e)
    {
        SmallWindowStyle = e.NewSize.Width < 900;
        PropertyChanged?.Invoke(this, new(nameof(SmallWindowStyle)));
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    protected override void OnLoaded()
    {
        base.OnLoaded();
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow.SizeChanged += UpdateSizeStyle;
    }

    private void FilenameSuffix_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        string text = (sender as TextBox)?.Text ?? string.Empty;
        ((SettingsViewModel)DataContext).InvalidFilename = InvalidFileNameCharRegex().IsMatch(text);
    }

    [GeneratedRegex(GlobalResources.INVALIDFILENAMECHARACTERSREGEX)]
    private static partial Regex InvalidFileNameCharRegex();

    private void FilenameSuffix_OnLostFocus(object? sender, RoutedEventArgs e)
    {
        string text = (sender as TextBox)?.Text ?? string.Empty;
        AppSettings.Instance.ConvertedFilenameSuffix = text;
    }

    private void FilenameSuffix_OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter) return;
        string text = (sender as TextBox)?.Text ?? string.Empty;
        AppSettings.Instance.ConvertedFilenameSuffix = text;
    }
}