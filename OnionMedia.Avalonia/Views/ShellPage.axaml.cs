using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentAvalonia.Core;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;
using OnionMedia.Core.Enums;
using OnionMedia.Core.Models;
using OnionMedia.Core.Services;
using OnionMedia.Core.ViewModels;

namespace OnionMedia.Avalonia.Views;

public partial class ShellPage : UserControl, INotifyPropertyChanged
{
    private MediaViewModel MediaViewModel { get; }
    private YouTubeDownloaderViewModel DownloaderViewModel { get; }
    private readonly IPCPower PcPowerService;
    
    private PCPowerOption desiredPowerOption;
    private bool executeOnError;
    private string _glyph;
    private IBrush _brush;
    
    private bool _shutdownTipIsOpen;
    private bool _showHeaderPowerBtn;
    private bool _executeOnError;
    private PCPowerOption _powerOption;

    public event PropertyChangedEventHandler PropertyChanged;
    
    public bool ShutdownTipIsOpen
    {
        get { return _shutdownTipIsOpen; }
        set { SetProperty(ref _shutdownTipIsOpen, value); }
    }

    public bool ShowHeaderPowerButton
    {
        get { return _showHeaderPowerBtn; }
        set { SetProperty(ref _showHeaderPowerBtn, value); }
    }

    public bool ExecuteOnError
    {
        get { return _executeOnError; }
        set { SetProperty(ref _executeOnError, value); }
    }

    public PCPowerOption SelectedPowerOption
    {
        get { return _powerOption; }
        set { SetProperty(ref _powerOption, value); }
    }
    
    public string Glyph
    {
        get { return _glyph; }
        set { SetProperty(ref _glyph, value); }
    }
    
    public IBrush Brush
    {
        get { return _brush; }
        set { SetProperty(ref _brush, value); }
    }
    
    public PCPowerOption[] PowerOptions { get; } = Enum.GetValues<PCPowerOption>().ToArray();
    
    
    public ShellPage()
    {
        InitializeComponent();
        var settings = AppSettings.Instance;
        PcPowerService = App.DefaultServiceProvider.GetService<IPCPower>();;
        MediaViewModel = App.DefaultServiceProvider.GetService<MediaViewModel>();;
        DownloaderViewModel = App.DefaultServiceProvider.GetService<YouTubeDownloaderViewModel>();
        MediaViewModel.ConversionDone += ReactToProcessFinish;
        DownloaderViewModel.DownloadDone += ReactToProcessFinish;
        SetPowerIcon();
        App.Current.ActualThemeVariantChanged += (_, _) => SetPowerIcon();
        navView.SelectedItem = (settings.StartPageType is StartPageType.LastOpened && settings.DownloaderPageIsOpen) || settings.StartPageType is StartPageType.DownloaderPage ? downloaderItem : mediaItem;
        previousItem = navView?.SelectedItem as NavigationViewItem;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void NavigationView_OnSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        if (Equals(e.SelectedItem, previousItem)) return;
        AppSettings.Instance.DownloaderPageIsOpen = false;
        if (Equals(e.SelectedItem, mediaItem))
        {
            frame.Navigate(typeof(MediaPage), null, new SlideNavigationTransitionInfo() {Effect = SlideNavigationTransitionEffect.FromBottom});
        }
        else if (Equals(e.SelectedItem, downloaderItem))
        {
            frame.Navigate(typeof(DownloaderPage), null, new SlideNavigationTransitionInfo() {Effect = SlideNavigationTransitionEffect.FromBottom});
            AppSettings.Instance.DownloaderPageIsOpen = true;
        }
        else
        {
            frame.Navigate(typeof(SettingsPage), null, new SlideNavigationTransitionInfo() {Effect = SlideNavigationTransitionEffect.FromBottom});
        }

        previousItem = e.SelectedItem as NavigationViewItem;
    }
    
    private void ShutdownBtn_OnClick(object sender, RoutedEventArgs e)
    {
        var flyout = this.FindControl<TeachingTip>("shutdownFlyout");
        flyout.PreferredPlacement = navView.DisplayMode is NavigationViewDisplayMode.Minimal ? TeachingTipPlacementMode.TopRight : TeachingTipPlacementMode.BottomLeft;
        ShutdownTipIsOpen = true;
        var selector = this.FindControl<ComboBox>("actionSelector");
        selector.IsDropDownOpen = true;
        selector.IsDropDownOpen = false;
    }
    
    private void ShutdownBtn_OnTap(object sender, TappedEventArgs e) => this.ShutdownBtn_OnClick(this, null);

    private void ShutdownFlyout_OnCloseButtonClick(TeachingTip sender, EventArgs args)
    {
        SelectedPowerOption = desiredPowerOption;
        ExecuteOnError = executeOnError;
        ShutdownTipIsOpen = false;
    }

    private void ShutdownFlyout_OnActionButtonClick(TeachingTip sender, EventArgs args)
    {
        desiredPowerOption = SelectedPowerOption;
        executeOnError = ExecuteOnError;
        ShutdownTipIsOpen = false;
        SetPowerIcon();
    }

    private void NavigationView_OnDisplayModeChanged(object? sender, NavigationViewDisplayModeChangedEventArgs e)
    {
        ShowHeaderPowerButton = navView.DisplayMode is NavigationViewDisplayMode.Minimal;
    }

    private void ReactToProcessFinish(object sender, bool errors)
    {
        if  (desiredPowerOption is PCPowerOption.None ||
             (errors && !executeOnError) ||
             (sender is MediaViewModel && DownloaderViewModel.DownloadFileCommand.IsRunning) ||
             (sender is YouTubeDownloaderViewModel && MediaViewModel.StartConversionCommand.IsRunning))
        {
            return;
        }

        switch (desiredPowerOption)
        {
            case PCPowerOption.Shutdown:
                PcPowerService.Shutdown();
                return;

            case PCPowerOption.Hibernate:
                PcPowerService.Hibernate();
                return;

            case PCPowerOption.Sleep:
                PcPowerService.Standby();
                return;
        }
    }
    
    private void SetPowerIcon()
    {
        switch (desiredPowerOption)
        {
            case PCPowerOption.Shutdown:
                Glyph = "\uE7E8";
                Brush = new SolidColorBrush(Colors.OrangeRed);
                return;

            case PCPowerOption.Hibernate:
                Glyph = "\uE823";
                Brush = new SolidColorBrush(Colors.DarkOrange);
                return;

            case PCPowerOption.Sleep:
                Glyph = "\uE708";
                Brush = new SolidColorBrush(Colors.DarkOrange);
                return;

            case PCPowerOption.None:
                Glyph = "\uE7E8";
                Brush = Application.Current.ActualThemeVariant == ThemeVariant.Dark ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Black);
                return;
        }
    }

    private bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
        if (Equals(field, value))
        {
            return false;
        }
        field = value;
        PropertyChanged?.Invoke(this, new(propertyName));
        return true;
    }

    private NavigationViewItem? previousItem;
    
    private NavigationView navView => this.FindControl<NavigationView>("navigationView");
    private NavigationViewItem mediaItem => this.FindControl<NavigationViewItem>("MediaPageItem");
    private NavigationViewItem downloaderItem => this.FindControl<NavigationViewItem>("DownloaderPageItem");
    private Frame? frame => this.FindControl<Frame>("shellFrame");
}