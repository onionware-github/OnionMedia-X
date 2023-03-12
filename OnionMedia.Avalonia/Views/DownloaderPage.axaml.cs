﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
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
    
    protected override void OnLoaded()
    {
        base.OnLoaded();
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow.SizeChanged += UpdateSizeStyle;
    }

    public bool SmallWindowStyle { get; set; }
    
    private void UpdateSizeStyle(object? sender, SizeChangedEventArgs e)
    {
        SmallWindowStyle = e.NewSize.Width < 850;
        PropertyChanged?.Invoke(this, new(nameof(SmallWindowStyle)));
        UpdateProgressBarPanelPosition(e.NewSize.Width);
    }

    void UpdateProgressBarPanelPosition(double newWidth)
    {
        var progressBarPanel = this.FindControl<StackPanel>("progressBarPanel");
        if (progressBarPanel is null) return;
        
        switch (newWidth)
        {
            case >= 1120:
                Grid.SetRow(progressBarPanel, 0);
                Grid.SetColumn(progressBarPanel, 1);
                Grid.SetColumnSpan(progressBarPanel, 1);
                break;
            
            case >= 849:
                Grid.SetRow(progressBarPanel, 1);
                Grid.SetColumn(progressBarPanel, 0);
                Grid.SetColumnSpan(progressBarPanel, 2);
                break;
            
            case >= 575:
                Grid.SetRow(progressBarPanel, 0);
                Grid.SetColumn(progressBarPanel, 1);
                Grid.SetColumnSpan(progressBarPanel, 1);
                break;
            
            default:
                Grid.SetRow(progressBarPanel, 1);
                Grid.SetColumn(progressBarPanel, 0);
                Grid.SetColumnSpan(progressBarPanel, 2);
                break;
        }
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