using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using OnionMedia.Core;
using OnionMedia.Core.Services;
using OnionMedia.Core.ViewModels;
using ReactiveUI;

namespace OnionMedia.Avalonia.Views;

public sealed partial class DownloaderPage : UserControl, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public DownloaderPage()
    {
        InitializeComponent();
        DataContext = App.DefaultServiceProvider.GetService<YouTubeDownloaderViewModel>();
        ((YouTubeDownloaderViewModel)DataContext).PropertyChanged += (o, e) =>
        {
            if (e.PropertyName == nameof(YouTubeDownloaderViewModel.SelectedVideo))
                PropertyChanged?.Invoke(this, new(nameof(IsItemSelected)));
        };
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void VideoSearchList_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e?.AddedItems?.Count == 0) return;
		Dispatcher.UIThread.Post(() => ((YouTubeDownloaderViewModel)DataContext).AddSearchedVideo.Execute(e.AddedItems[0]));
    }

    private void Videolink_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        ((YouTubeDownloaderViewModel)DataContext).ValidUrl = UrlRegex().IsMatch(((TextBox)sender).Text);
        ((YouTubeDownloaderViewModel)DataContext).ClearResultsCommand.Execute(null);
    }

    private void Videolink_OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter || DataContext is not YouTubeDownloaderViewModel vm) return;
        vm.AddVideoCommand.Execute(vm.SearchTerm);
    }

    private void RemoveAllBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        ((YouTubeDownloaderViewModel)DataContext).RemoveAllCommand.Execute(null);
        HideParentFlyout(sender as IControl);
    }
    
    public bool IsItemSelected => (DataContext as YouTubeDownloaderViewModel)?.SelectedVideo is not null;

    [GeneratedRegex(GlobalResources.URLREGEX)]
    private partial Regex UrlRegex();
    
    private void HideParentFlyout(IControl element)
    {
        if (element is null) return;
        var parent = element.Parent;
        while (parent is not null)
        {
            if (parent is Popup flyout)
            {
                flyout.IsOpen = false;
                return;
            }
            parent = parent.Parent;
        }
    }
}