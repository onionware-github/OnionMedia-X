using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FluentAvalonia.Core;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;
using OnionMedia.Core.Models;

namespace OnionMedia.Avalonia.Views;

public partial class ShellPage : UserControl
{
    public ShellPage()
    {
        InitializeComponent();
        var settings = AppSettings.Instance;
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

    private NavigationViewItem? previousItem;
    
    private NavigationView navView => this.FindControl<NavigationView>("navigationView");
    private NavigationViewItem mediaItem => this.FindControl<NavigationViewItem>("MediaPageItem");
    private NavigationViewItem downloaderItem => this.FindControl<NavigationViewItem>("DownloaderPageItem");
    private Frame? frame => this.FindControl<Frame>("shellFrame");
}