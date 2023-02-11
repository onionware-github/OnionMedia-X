﻿using System.ComponentModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using OnionMedia.Core.Models;
using OnionMedia.Core.ViewModels;
using ReactiveUI;

namespace OnionMedia.Avalonia.Views;

public partial class MediaPage : UserControl, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public MediaPage()
    {
        InitializeComponent();
        DataContext = App.DefaultServiceProvider.GetService<MediaViewModel>();
    }

    protected override void OnLoaded()
    {
        base.OnLoaded();
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow.SizeChanged += UpdateSizeStyle;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public bool SmallWindowStyle { get; set; }

    private void UpdateSizeStyle(object? sender, SizeChangedEventArgs e)
    {
        SmallWindowStyle = e.NewSize.Width < 750;
        PropertyChanged?.Invoke(this, new(nameof(SmallWindowStyle)));
    }
    
    private void HideParentFlyout(IControl element)
    {
        if (element is null) return;
        var parent = element.Parent;
        while (parent is not null)
        {
            if (parent is Flyout flyout)
            {
                flyout.Hide();
                return;
            }
            parent = parent.Parent;
        }
    }
    
    private void Close_Flyout(object? sender, RoutedEventArgs e)
    {
        HideParentFlyout(sender as IControl);
    }
    
    private void ToggleButton_Click(object sender, RoutedEventArgs e) => ((ToggleButton)sender).IsChecked = true;

    private void Framerates_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e?.AddedItems?.Count is 0 || e.AddedItems[0] is not double) return;
        ((MediaViewModel)DataContext).SetFramerateCommand.Execute((double)e.AddedItems[0]);
        HideParentFlyout(sender as IControl);
    }

    private void Resolutions_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e?.AddedItems?.Count is 0 || e.AddedItems[0] is not Resolution) return;
        ((MediaViewModel)DataContext).SetResolutionCommand.Execute((Resolution)e.AddedItems[0]);
        HideParentFlyout(sender as IControl);
    }
}