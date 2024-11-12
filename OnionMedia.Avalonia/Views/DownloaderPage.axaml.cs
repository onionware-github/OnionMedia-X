using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using OnionMedia.Avalonia.UserControls;
using OnionMedia.Core;
using OnionMedia.Core.Models;
using OnionMedia.Core.ViewModels;

namespace OnionMedia.Avalonia.Views;

public sealed partial class DownloaderPage : UserControl, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public YouTubeDownloaderViewModel? ViewModel => DataContext as YouTubeDownloaderViewModel;

    public DownloaderPage()
    {
        InitializeComponent();
        DataContext = App.DefaultServiceProvider.GetService<YouTubeDownloaderViewModel>();
        ((YouTubeDownloaderViewModel)DataContext).PropertyChanged += (o, e) =>
        {
            if (DataContext is not YouTubeDownloaderViewModel vm) return;
            if (e.PropertyName == nameof(YouTubeDownloaderViewModel.SelectedVideo))
            {
                var control = this.FindControl<TimeRangeSelector>("timeRangeSelector");
                if (control is null) return;
                IsItemSelected = vm.SelectedVideo is not null;
                control.UpdateIsReadOnly(!vm.QueueIsNotEmpty || !IsItemSelected);
                if (vm.SelectedVideo is not null)
                    control.UpdateTimeSpanGroup(vm.SelectedVideo.TimeSpanGroup);
            }

            if (e.PropertyName == nameof(YouTubeDownloaderViewModel.QueueIsEmpty))
            {
                if (vm.QueueIsEmpty) IsItemSelected = false;
                this.FindControl<TimeRangeSelector>("timeRangeSelector")
                    ?.UpdateIsReadOnly(!vm.QueueIsNotEmpty || !IsItemSelected);
            }
        };
    }

    private bool eventsHooked;
    protected override void OnLoaded()
    {
        base.OnLoaded();
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow.SizeChanged += UpdateSizeStyle;

        if (!eventsHooked)
        {
            App.MainWindow.SizeChanged += (_, _) => OnSizeChanged();
            ((YouTubeDownloaderViewModel)DataContext).PropertyChanged += async (_, _) =>
            {
                await Task.Delay(100);
                OnSizeChanged();
            };
            this.FindControl<ListBox>("searchResultsList").SizeChanged += (_, _) => OnSizeChanged();
            this.FindControl<ListBox>("videoQueue").SizeChanged += (_, _) => OnSizeChanged();
            eventsHooked = true;
        }
        
        OnSizeChanged();
        this.FindControl<TextBox>(nameof(videolink))?.Focus();
    }

    protected override void OnUnloaded()
    {
        base.OnUnloaded();
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow.SizeChanged -= UpdateSizeStyle;
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
        Dispatcher.UIThread.Post(() =>
            ((YouTubeDownloaderViewModel)DataContext).AddSearchedVideo.Execute(e.AddedItems[0]));
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
        HideParentFlyout(sender as Control);
    }

    public bool IsItemSelected
    {
        get => isItemSelected;
        set
        {
            if (isItemSelected == value) return;
            isItemSelected = value;
            PropertyChanged?.Invoke(this, new(nameof(IsItemSelected)));
        }
    }

    private bool isItemSelected;

    [GeneratedRegex(GlobalResources.URLREGEX)]
    private partial Regex UrlRegex();

    private void HideParentFlyout(Control element)
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

    private void VideoQueue_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        IsItemSelected = e.AddedItems?.Count > 0;
        if (DataContext is not YouTubeDownloaderViewModel vm) return;
        this.FindControl<TimeRangeSelector>("timeRangeSelector")
            ?.UpdateIsReadOnly(!vm.QueueIsNotEmpty || !IsItemSelected);
    }

    private void OpenDonationPage(object? sender, RoutedEventArgs e)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = GlobalResources.LocalDonationUrl,
            UseShellExecute = true
        });
    }
    
    private void OnSizeChanged()
    {
        var donationGrid = this.FindControl<Border>("donationGrid");
        var scrollViewer = this.FindControl<ScrollViewer>("scrollViewer");
        if (!AppSettings.Instance.ShowDonationBanner)
        {
            donationGrid.IsVisible = false;
            return;
        }

        // Check if ScrollViewer can scroll
        double windowHeight = this.Bounds.Height;
        if (scrollViewer.ScrollBarMaximum.Length == 0 && windowHeight - scrollViewer.DesiredSize.Height > donationGrid.Bounds.Height)
        {
            // If not, show donation banner
            donationGrid.IsVisible = true;
            return;
        }

        // Otherwise, hide banner and don't reserve screen space for it
        donationGrid.IsVisible = false;
    }
}